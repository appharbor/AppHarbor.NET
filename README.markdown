# AppHarbor.NET - A .NET client for the [AppHarbor API][1]

### License: Apache License 2.0

### Twitter
*  News and updates for AppHarbor.NET: [@AppHarborNET][2]
*  In addition you can also follow my personal account: [@ntziolis][3]

### Features

* Full support for the AppHarbor API (2012.03.21)
* Managed API Access (currently .NET 3.5, could be made .NET 2.0 compatible if requested)
* Implements AppHarbor OAuth header: `Authorization: BEARER :access_token`
* Sample web application with AppHarbor OAuth sample
* Unit tested

### Usage

#### Create Api instance

```csharp
// create an Api instance with the token obtained from oAuth
var api = new AppHarborApi(new AuthInfo()
{
	AccessToken = "token obtained via oAuth"
});
```

#### Get list of AppHarbor applications

```csharp
// get a list of all applications
var applications = api.GetApplications();

foreach (var application in applications)
{
	Console.WriteLine(string.Format("Application name: {0}, Url: {1}", 
		application.Name, application.Url));
}
```

#### Create new AppHarbor applications

```csharp
// creating always returns a CreateResult
// which has a Status, ID, Location
var createResult = api.CreateApplication("New Application Name", null);

// based on the Status decide on what todo
switch (createResult.Status)
{
	case CreateStatus.Created:
		{
			var newID = createResult.ID;
			var newURL = createResult.Location;

			// get actual application object via the api
			var newApplication = api.GetApplication(newID);

			// more code
			break;
		}
	case CreateStatus.AlreadyExists:
	case CreateStatus.Undefined:
		{
			// handle
			break;
		}
	default:
		break;
}
```

### Sample Web Application Project

* Basic OAuth Access Token retrieval implementation
* more to come

### Todo

* More unit tests
* Add integration tests
* Add XML Comments for public methods
* Expand sample web application

  [1]: http://support.appharbor.com/kb/api/api-overview
  [2]: https://twitter.com/AppHarborNET
  [3]: https://twitter.com/ntziolis 