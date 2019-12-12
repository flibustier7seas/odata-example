## [OData Bug] Deep expand not working

Issue: [Deep expand not working](https://github.com/OData/WebApi/issues/226)

Problem: When expanding anything more than 3 levels the JSON output does not return this information.

Edm model:

```csharp
builder.EntitySet<User>("Users");

builder.EntityType<User>()
       .Filter(QueryOptionSetting.Allowed, nameof(User.Name))
       .Expand(10, nameof(User.Orders));

builder.EntityType<Order>()
       .Expand(10, nameof(Order.OrderPositions));

builder.EntityType<OrderPosition>()
       .Expand(10, nameof(OrderPosition.Products));

builder.EntityType<Product>()
       .Expand(10, nameof(Product.Parameters));
```

Request: `http://localhost:63773/ODataExample/Users?$expand=Orders($expand=OrderPositions($expand=Products($expand=Parameters)))&$filter=Name in ('User_2')`

Response:

```json
{
    "@odata.context": "http://localhost:63773/odataexample/$metadata#Users(Orders(OrderPositions(Products(Parameters()))))",
    "value": [
        {
            "Id": 2,
            "Name": "User_2",
            "Orders": [
                {
                    "Id": 21,
                    "Name": "Order_21",
                    "OrderPositions": [
                        {
                            "Id": 211,
                            "Name": "OrderPosition_211"
                        }
                    ]
                }
            ]
        }
    ]
}
```

Request: `http://localhost:63773/ODataExample/Users(2)/Orders?$expand=OrderPositions($expand=Products($expand=Parameters))`

Response:

```json
{
    "@odata.context": "http://localhost:63773/odataexample/$metadata#Collection(ODataExample.Order)",
    "value": [
        {
            "Id": 21,
            "Name": "Order_21",
            "OrderPositions": [
                {
                    "Id": 211,
                    "Name": "OrderPosition_211"
                }
            ]
        }
    ]
}
```

## Solution

Mark all expanded properties as ContainsTarget.

```csharp
builder.EntityType<User>()
       .Filter(QueryOptionSetting.Allowed, nameof(User.Name))
       .Expand(10, nameof(User.Orders))
       .ContainsMany(x => x.Orders);

builder.EntityType<Order>()
       .Expand(10, nameof(Order.OrderPositions))
       .ContainsMany(x => x.OrderPositions);

builder.EntityType<OrderPosition>()
       .Expand(10, nameof(OrderPosition.Products))
       .ContainsMany(x => x.Products);
```

Request: `http://localhost:63773/ODataExample/Users?$expand=Orders($expand=OrderPositions($expand=Products($expand=Parameters)))&$filter=Name in ('User_2')`

Response:

```json
{
    "@odata.context": "http://localhost:63773/odataexample/$metadata#Users(Orders(OrderPositions(Products(Parameters()))))",
    "value": [
        {
            "Id": 2,
            "Name": "User_2",
            "Orders": [
                {
                    "Id": 21,
                    "Name": "Order_21",
                    "OrderPositions": [
                        {
                            "Id": 211,
                            "Name": "OrderPosition_211",
                            "Products": [
                                {
                                    "Id": 2111,
                                    "Name": "Product_2111",
                                    "Parameters": [
                                        {
                                            "Id": 21111,
                                            "Name": "Parameter_21111",
                                            "Value": "Value_21111"
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ]
        }
    ]
}
```

Request: `http://localhost:63773/ODataExample/Users(2)/Orders?$expand=OrderPositions($expand=Products($expand=Parameters))`

Response:

```json
{
    "@odata.context": "http://localhost:63773/odataexample/$metadata#Users(2)/Orders(OrderPositions(Products(Parameters())))",
    "value": [
        {
            "Id": 21,
            "Name": "Order_21",
            "OrderPositions": [
                {
                    "Id": 211,
                    "Name": "OrderPosition_211",
                    "Products": [
                        {
                            "Id": 2111,
                            "Name": "Product_2111",
                            "Parameters": [
                                {
                                    "Id": 21111,
                                    "Name": "Parameter_21111",
                                    "Value": "Value_21111"
                                }
                            ]
                        }
                    ]
                }
            ]
        }
    ]
}
```
