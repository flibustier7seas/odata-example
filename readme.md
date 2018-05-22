## Deep expand not working

Problem: When expanding anything more than 3 levels the JSON output does not return this information.

Request: ```http://localhost:63773/ODataExample/Users(2)/Orders?$expand=OrderPositions($expand=Products($expand=Parameters))```

Metadata:
```xml
<EntityType Name="User">
    <Key>
        <PropertyRef Name="Id"/>
    </Key>
    <Property Name="Id" Type="Edm.Int64" Nullable="false"/>
    <Property Name="Name" Type="Edm.String"/>
    <NavigationProperty Name="Orders" Type="Collection(ODataExample.Model.Order)"/>
</EntityType>
```
Response:
```json
{
    "@odata.context": "http://localhost:63773/odataexample/$metadata#Users(2)/Orders",
    "value": [{
        "Id": 21,
        "Name": "Order_21",
        "OrderPositions": [{
            "Id": 211,
            "Name": "OrderPosition_211",
            "Products": [{
                "Id": 2111,
                "Name": "Product_2111"
            }]
        }]
    }]
}
```

## Solution

All expanded property should be marked as containment navigation property.
```csharp
    builder.EntityType<User>()
        .Expand(10, nameof(User.Orders))
        .ContainsMany(x => x.Orders);

    builder.EntityType<Order>()
        .Expand(10, nameof(Order.OrderPositions))
        .ContainsMany(x => x.OrderPositions);

    builder.EntityType<OrderPosition>()
        .Expand(nameof(OrderPosition.Products))
        .ContainsMany(x => x.Products);

    builder.EntityType<Product>()
        .Expand(nameof(Product.Parameters))
        .ContainsMany(x => x.Parameters);
```
Metadata:
```xml
<EntityType Name="User">
    <Key>
        <PropertyRef Name="Id"/>
    </Key>
    <Property Name="Id" Type="Edm.Int64" Nullable="false"/>
    <Property Name="Name" Type="Edm.String"/>
    <NavigationProperty Name="Orders" Type="Collection(ODataExample.Model.Order)" ContainsTarget="true"/>
</EntityType>
```
Response:
```json
{
    "@odata.context": "http://localhost:63773/odataexample/$metadata#Orders",
    "value": [{
        "Id": 21,
        "Name": "Order_21",
        "OrderPositions": [{
            "Id": 211,
            "Name": "OrderPosition_211",
            "Products": [{
                "Id": 2111,
                "Name": "Product_2111",
                "Parameters": [{
                    "Id": 21111,
                    "Name": "Parameter_21111",
                    "Value": "Value_21111"
                }]
            }]
        }]
    }]
}
```

