# OData Web API bugs

## Optional parameter not working for function with single argument

```csharp
[ODataRoute("Users/GetFirstName()")]
public string GetFirstName()
{
    return GetFirstName("Unknown");
}

[ODataRoute("Users/GetFirstName(firstName={firstName})")]
public string GetFirstName(string firstName)
{
    return firstName;
}
```

```csharp
var getFirstName = builder.EntityType<User>().Collection
    .Function("GetFirstName")
    .Returns<string>();

getFirstName.Parameter<string>("firstName").Optional().HasDefaultValue("Unknown");
```

### Request
`http://localhost:63000/v1/Users/GetFirstName()`

### Response

Status: `500 Internal Server Error`
```
Server Error in '/' Application.

The path template 'Users/GetFirstName()' on the action 'GetFirstName' in controller 'Users' is not a valid OData path template. Bad Request - Error in query syntax.

Description: An unhandled exception occurred during the execution of the current web request. Please review the stack trace for more information about the error and where it originated in the code.

Exception Details: System.InvalidOperationException: The path template 'Users/GetFirstName()' on the action 'GetFirstName' in controller 'Users' is not a valid OData path template. Bad Request - Error in query syntax.
```
