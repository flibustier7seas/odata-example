# OData Web API bugs

## Deep expand not working

Issue: https://github.com/OData/WebApi/issues/226

Problem: When expanding anything more than 3 levels the JSON output does not return this information.

Request: ```http://localhost:63000/v1/Users(2)/Orders?$expand=OrderPositions($expand=Products($expand=Parameters))```

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
    "@odata.context": "http://localhost:63000/v1/$metadata#Collection(ODataExample.Model.Order)",
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

### Solution

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
    "@odata.context": "http://localhost:63000/v1/$metadata#Users(2)/Orders",
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

## Function with ReturnsCollectionFromEntitySet throw Exception

Issue: https://github.com/OData/WebApi/issues/255

Request: ```http://localhost:63000/v1/Users(1)/FilterOrdersByName(name='Order')```

```csharp
builder.EntityType<User>()
    .Function("FilterOrdersByName")
    .ReturnsCollectionFromEntitySet<Order>("Orders")
    .Parameter<string>("name").Required();
```

```csharp
[HttpGet]
[ODataRoute("Users({key})/FilterOrdersByName(name={name})")]
public IQueryable<Order> FilterOrdersByName(long key, string name)
{
    return query.GetUsers()
        .Where(x => x.Id == key)
        .SelectMany(x => x.Orders)
        .Where(x => x.Name.Contains(name));
}
```

```json
{
    "error": {
        "code": "",
        "message": "An error has occurred.",
        "innererror": {
            "message": "The Path property in ODataMessageWriterSettings.ODataUri must be set when writing contained elements.",
            "type": "Microsoft.OData.ODataException",
            "stacktrace": "   в Microsoft.OData.ODataWriterCore.EnterScope(WriterState newState, ODataItem item)\r\n   в Microsoft.OData.ODataWriterCore.WriteStartNestedResourceInfoImplementation(ODataNestedResourceInfo nestedResourceInfo)\r\n   в Microsoft.OData.ODataWriterCore.WriteStart(ODataNestedResourceInfo nestedResourceInfo)\r\n   в Microsoft.AspNet.OData.Formatter.Serialization.ODataResourceSerializer.WriteNavigationLinks(IEnumerable`1 navigationProperties, ResourceContext resourceContext, ODataWriter writer)\r\n   в Microsoft.AspNet.OData.Formatter.Serialization.ODataResourceSerializer.WriteResource(Object graph, ODataWriter writer, ODataSerializerContext writeContext, IEdmTypeReference expectedType)\r\n   в Microsoft.AspNet.OData.Formatter.Serialization.ODataResourceSerializer.WriteObjectInline(Object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)\r\n   в Microsoft.AspNet.OData.Formatter.Serialization.ODataResourceSetSerializer.WriteResourceSet(IEnumerable enumerable, IEdmTypeReference resourceSetType, ODataWriter writer, ODataSerializerContext writeContext)\r\n   в Microsoft.AspNet.OData.Formatter.Serialization.ODataResourceSetSerializer.WriteObjectInline(Object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)\r\n   в Microsoft.AspNet.OData.Formatter.Serialization.ODataResourceSetSerializer.WriteObject(Object graph, Type type, ODataMessageWriter messageWriter, ODataSerializerContext writeContext)\r\n   в Microsoft.AspNet.OData.Formatter.ODataOutputFormatterHelper.WriteToStream(Type type, Object value, IEdmModel model, ODataVersion version, Uri baseAddress, MediaTypeHeaderValue contentType, IWebApiUrlHelper internaUrlHelper, IWebApiRequestMessage internalRequest, IWebApiHeaders internalRequestHeaders, Func`2 getODataMessageWrapper, Func`2 getEdmTypeSerializer, Func`2 getODataPayloadSerializer, Func`1 getODataSerializerContext)\r\n   в Microsoft.AspNet.OData.Formatter.ODataMediaTypeFormatter.WriteToStreamAsync(Type type, Object value, Stream writeStream, HttpContent content, TransportContext transportContext, CancellationToken cancellationToken)\r\n--- Конец трассировка стека из предыдущего расположения, где возникло исключение ---\r\n   в System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\r\n   в System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   в System.Runtime.CompilerServices.TaskAwaiter.GetResult()\r\n   в System.Web.Http.Owin.HttpMessageHandlerAdapter.<BufferResponseContentAsync>d__13.MoveNext()"
        }
    }
}
```

### Solution

```csharp
[HttpGet]
[ODataRoute("Users({key})/FilterOrdersByName(name={name})")]
public IQueryable<Order> FilterOrdersByName(long key, string name)
{
    var path = Request.GetPathHandler().Parse("http://localhost/odata/", $"Users({key})/Orders", Request.GetRequestContainer());
    Request.ODataProperties().Path = path;

    return query.GetUsers()
        .Where(x => x.Id == key)
        .SelectMany(x => x.Orders)
        .Where(x => x.Name.Contains(name));
}
```
