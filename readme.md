## [OData Bug] Function with ReturnsCollectionFromEntitySet throw Exception

Issue: [[Bug] Functions returning entity types with contained properties throw exception](https://github.com/OData/WebApi/issues/255)

Request: `http://localhost:63773/ODataExample/Users/ByOrderName(name='Order_1')?$expand=Orders($expand=OrderPositions($expand=Products($expand=Parameters)))`

```csharp
builder.EntitySet<User>("Users");

builder.EntityType<User>()
       .Expand(10, nameof(User.Orders))
       .ContainsMany(x => x.Orders);

builder.EntityType<User>().Collection
       .Function("ByOrderName")
       .ReturnsCollectionFromEntitySet<User>("Users")
       .Parameter<string>("name").Required();

builder.EntityType<Order>()
       .Expand(10, nameof(Order.OrderPositions))
       .ContainsMany(x => x.OrderPositions);

builder.EntityType<OrderPosition>()
       .Expand(10, nameof(OrderPosition.Products))
       .ContainsMany(x => x.Products);

builder.EntityType<Product>()
        .Expand(10, nameof(Product.Parameters));
```

Response:

```json
{
    "error": {
        "code": "",
        "message": "An error has occurred.",
        "innererror": {
            "message": "The Path property in ODataMessageWriterSettings.ODataUri must be set when writing contained elements.",
            "type": "Microsoft.OData.ODataException",
            "stacktrace": "   at Microsoft.OData.ODataWriterCore.EnterScope(WriterState newState, ODataItem item)\r\n   at Microsoft.OData.ODataWriterCore.WriteStartNestedResourceInfoImplementation(ODataNestedResourceInfo nestedResourceInfo)\r\n   at Microsoft.OData.ODataWriterCore.WriteStart(ODataNestedResourceInfo nestedResourceInfo)\r\n   at Microsoft.AspNet.OData.Formatter.Serialization.ODataResourceSerializer.WriteExpandedNavigationProperties(IDictionary`2 navigationPropertiesToExpand, ResourceContext resourceContext, ODataWriter writer)\r\n   at Microsoft.AspNet.OData.Formatter.Serialization.ODataResourceSerializer.WriteResource(Object graph, ODataWriter writer, ODataSerializerContext writeContext, IEdmTypeReference expectedType)\r\n   at Microsoft.AspNet.OData.Formatter.Serialization.ODataResourceSerializer.WriteObjectInline(Object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)\r\n   at Microsoft.AspNet.OData.Formatter.Serialization.ODataResourceSetSerializer.WriteResourceSet(IEnumerable enumerable, IEdmTypeReference resourceSetType, ODataWriter writer, ODataSerializerContext writeContext)\r\n   at Microsoft.AspNet.OData.Formatter.Serialization.ODataResourceSetSerializer.WriteObjectInline(Object graph, IEdmTypeReference expectedType, ODataWriter writer, ODataSerializerContext writeContext)\r\n   at Microsoft.AspNet.OData.Formatter.Serialization.ODataResourceSetSerializer.WriteObject(Object graph, Type type, ODataMessageWriter messageWriter, ODataSerializerContext writeContext)\r\n   at Microsoft.AspNet.OData.Formatter.ODataOutputFormatterHelper.WriteToStream(Type type, Object value, IEdmModel model, ODataVersion version, Uri baseAddress, MediaTypeHeaderValue contentType, IWebApiUrlHelper internaUrlHelper, IWebApiRequestMessage internalRequest, IWebApiHeaders internalRequestHeaders, Func`2 getODataMessageWrapper, Func`2 getEdmTypeSerializer, Func`2 getODataPayloadSerializer, Func`1 getODataSerializerContext)\r\n   at Microsoft.AspNet.OData.Formatter.ODataMediaTypeFormatter.WriteToStreamAsync(Type type, Object value, Stream writeStream, HttpContent content, TransportContext transportContext, CancellationToken cancellationToken)\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task task)\r\n   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)\r\n   at System.Web.Http.Owin.HttpMessageHandlerAdapter.<BufferResponseContentAsync>d__13.MoveNext()"
        }
    }
}
```

## Solution

Change `Request.ODataProperties().Path`:

```csharp
[HttpGet]
[ODataRoute("Users/ByOrderName(name={name})")]
public IQueryable<User> ByOrderName( string name)
{
    var path = Request.GetPathHandler().Parse("http://localhost/odata/", "Users", Request.GetRequestContainer());
    Request.ODataProperties().Path = path;

    return _query.GetUsers()
                 .Where(user => user.Orders.Any(order => order.Name.Contains(name)));
}
```

Response:

```json
{
    "@odata.context": "http://localhost:63773/odataexample/$metadata#Users(Orders(OrderPositions(Products(Parameters()))))",
    "value": [
        {
            "Id": 1,
            "Name": "User_1",
            "Orders": [
                {
                    "Id": 11,
                    "Name": "Order_11",
                    "OrderPositions": [
                        {
                            "Id": 111,
                            "Name": "OrderPosition_111",
                            "Products": [
                                {
                                    "Id": 1111,
                                    "Name": "Product_1111",
                                    "Parameters": [
                                        {
                                            "Id": 11111,
                                            "Name": "Parameter_11111",
                                            "Value": "Value_11111"
                                        },
                                        {
                                            "Id": 11112,
                                            "Name": "Parameter_11112",
                                            "Value": "Value_11112"
                                        }
                                    ]
                                },
                                {
                                    "Id": 1112,
                                    "Name": "Product_1112",
                                    "Parameters": [
                                        {
                                            "Id": 11121,
                                            "Name": "Parameter_11121",
                                            "Value": "Value_11121"
                                        },
                                        {
                                            "Id": 11122,
                                            "Name": "Parameter_11122",
                                            "Value": "Value_11122"
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    "Id": 12,
                    "Name": "Order_12",
                    "OrderPositions": []
                },
                {
                    "Id": 13,
                    "Name": "Order_13",
                    "OrderPositions": []
                }
            ]
        }
    ]
}
```