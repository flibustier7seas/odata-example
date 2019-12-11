# [OData Bug] Deep expand not working

Package: Microsoft.AspNet.OData.7.2.3

Problem: When expanding anything more than 3 levels the JSON output does not return this information.

Issue: [MaxExpansionDepth = 0 appears to have no effect](https://github.com/OData/WebApi/issues/1065)

Request: `http://localhost:63773/ODataExample/Users(2)?$expand=Orders($expand=OrderPositions($expand=Products($expand=Parameters)))`

```csharp
builder.EntitySet<User>("Users");
builder.EntitySet<Order>("Orders");
builder.EntitySet<Product>("Products");

builder.EntityType<User>()
       .Expand(10, nameof(User.Orders))
       // The property will be marked as containment navigation property
       // and expanding anything more than 3 levels the JSON output does not return information
       .ContainsMany(x => x.Orders);

builder.EntityType<Order>()
       .Expand(10, nameof(Order.OrderPositions));

builder.EntityType<OrderPosition>()
       .Expand(10, nameof(OrderPosition.Products));

builder.EntityType<Product>()
       .Expand(10, nameof(Product.Parameters));
```

Response:

```json
{
    "@odata.context": "http://localhost:63773/odataexample/$metadata#Users/$entity",
    "Id": 2,
    "Name": "User_2",
    "Orders@odata.context": "http://localhost:63773/odataexample/$metadata#Users(2)/Orders",
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
                            "Name": "Product_2111"
                        }
                    ]
                }
            ]
        }
    ]
}
```

## Solution

Mark all expanded properties as **ContainsTarget**.

```csharp
builder.EntityType<Order>()
       .Expand(10, nameof(Order.OrderPositions))
       .ContainsMany(x => x.OrderPositions);

builder.EntityType<OrderPosition>()
       .Expand(10, nameof(OrderPosition.Products))
       .ContainsMany(x => x.Products);
```

Response:

```json
{
    "@odata.context": "http://localhost:63773/odataexample/$metadata#Users/$entity",
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
```

## Related issues

-   [Deep expand not working](https://github.com/OData/WebApi/issues/226)
