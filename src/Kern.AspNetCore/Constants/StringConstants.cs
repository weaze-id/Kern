namespace Kern.AspNetCore.Constants;

public class StringConstants
{
    public const string SWAGGER_DESCRIPTION =
@"# Introduction
The {{Title}} API follows the general patterns of REST.
You can use the resources of a {{Title}} by making HTTPS requests to URLs that represent those resources. You can find description of all the endpoints here.

# Getting Started
To test and explore {{Title}} API you can use Postman.
Postman is a powerful HTTP client for testing RESTful APIs by displaying requests and responses in manageable formats.

These are steps you need to take to start testing {{Title}} API via Postman:
- Download and install Postman. You can get it here: [https://www.getpostman.com](https://www.getpostman.com)
- Get [{{Title}} API Postman Collection](/swagger/{{Version}}/swagger.json) and import it into Postman.
- Define variables used in postman collection. 
    For example, in {{Title}} production environment for {{Title}} API {{Version}} you should define baseUrl variable.
    It is useful to configure variables in postman environments so you will not have to redefine the values for each request manually.
    [Learn more about Postman environments](https://learning.postman.com/docs/sending-requests/managing-environments/).
- Configure the Postman Authorization header. Each request to {{Title}} API should contain a token in request header.
    You can read more about ways to get the token in the Authorization section of the documentation.
- Once you complete these steps you are ready to make calls to {{Title}} API via Postman.

# Authorization
An application credential is any piece of information that identifies, authenticates, or authorizes an application in some way.

{{Title}} API provide one authorization method:
- Bearer token

## Bearer token
Bearer tokens are a simple way to make calls to the API.
You can get your bearer token by hitting login endpoint.
For every call to the API you must include your access token in the Authorization header:

```
    Authorization: Bearer eyJhbGciOiJIUzI1NiI...
```
";
}