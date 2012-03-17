# AppHarbort.NET - A .NET client for the [AppHarbor API][1]

### License: Apache License 2.0

### Features

* Supports the full AppHarbor API support
* Fully managed API
* Handles oAuth token

```csharp
// create an Api instance with the token obtained from oAuth
var api = new AppHarborApi(new AuthInfo()
{
	Token = "token obtained via oAuth"
});

// get a list of all applications
List<Application> applications = api.GetApplications();

foreach(var application in applications)
{
	Console.WrileLine(string.Format("Application name: {0}, Url: {1}", application.Name, application.Url));	
}
```

  [1]: http://support.appharbor.com/kb/api/api-overview