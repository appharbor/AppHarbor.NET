# AppHarbor.NET - A .NET client for the [AppHarbor API][1]

### License: Apache License 2.0
### Updates for now via [@ntziolis][2]

### Features

* Full support for the AppHarbor API (2012.03.21)
* Managed API Access (currently .NET 3.5, could be made .NET 2.0 compatible if requested)
* Implements AppHarbor OAuth header: `Authorization: BEARER :access_token`
* Sample Web Application with AppHarbor OAUth sample
* Unit tested

### Usage

```csharp
// create an Api instance with the token obtained from oAuth
var api = new AppHarborApi(new AuthInfo()
{
	AccessToken = "token obtained via oAuth"
});

// get a list of all applications
IList<Application> applications = api.GetApplications();

foreach (var application in applications)
{
	Console.WriteLine(string.Format("Application name: {0}, Url: {1}", application.Name, application.Url));
}
```

### Sample Web Application Project

* Basic OAuth Access Token retrieval implementation
* more to come

### Todo

* More unit Tests
* Expand sample web application

  [1]: http://support.appharbor.com/kb/api/api-overview
  [2]: https://twitter.com/ntziolis