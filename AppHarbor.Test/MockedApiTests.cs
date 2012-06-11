using System;
using System.Linq;
using AppHarbor.Model;
using AppHarbor.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace AppHarbor.Test
{
	[TestClass]
	public class MockedApiTests
	{
		private static AppHarborClient Api;
		private static AppHarborClient EmptyListDataApi;
		private static AppHarborClient ExistingDataDataApi;
		private static string ApplicationID;

		private class MockedAppHarborClient : AppHarborClient
		{
			public MockedAppHarborClient(AuthInfo authInfo, RestClient restClient)
				: base(authInfo, restClient)
			{
			}
		}

		[ClassInitialize]
		public static void InitTest(TestContext context)
		{
			var authInfo = new AuthInfo("unittest");

			var client = new RestSharp.RestClient(MockHttp.BaseUrl);
			client.HttpFactory = new RestSharp.SimpleFactory<SampleDataMockHttp>();
			Api = new MockedAppHarborClient(authInfo, client);

			var clientEmptyListData = new RestSharp.RestClient(MockHttp.BaseUrl);
			clientEmptyListData.HttpFactory = new RestSharp.SimpleFactory<EmptyListDataMockHttp>();
			EmptyListDataApi = new MockedAppHarborClient(authInfo, clientEmptyListData);

			var clientExistingData = new RestSharp.RestClient(MockHttp.BaseUrl);
			clientExistingData.HttpFactory = new RestSharp.SimpleFactory<ExistingDataMockHttp>();
			ExistingDataDataApi = new MockedAppHarborClient(authInfo, clientExistingData);

			ApplicationID = ":application";
		}

		[TestMethod]
		public void Create_Non_Existing_Application()
		{
			var createResult = Api.CreateApplication(ApplicationID);
			Assert.IsNotNull(createResult);
			Assert.AreEqual(CreateStatus.Created, createResult.Status);
			Assert.AreEqual(ApplicationID, createResult.Id);
			Assert.AreEqual("https://appharbor.com/applications/:application", createResult.Location.AbsoluteUri);
		}

		[TestMethod]
		public void Try_Create_Existing_Application()
		{
			var createResult = ExistingDataDataApi.CreateApplication(ApplicationID);
			Assert.IsNotNull(createResult);
			Assert.AreEqual(CreateStatus.AlreadyExists, createResult.Status);
			Assert.AreEqual(null, createResult.Id);
			Assert.AreEqual(null, createResult.Location);
		}

		[TestMethod]
		public void Create_Non_Existing_Collaborator()
		{
			var createResult = Api.CreateCollaborator(ApplicationID, "some@mail.com", CollaboratorType.Collaborator);
			Assert.IsNotNull(createResult);
			Assert.AreEqual(CreateStatus.Created, createResult.Status);
			Assert.AreEqual("5", createResult.Id);
			Assert.AreEqual("https://appharbor.com/applications/:application/collaborators/5", createResult.Location.AbsoluteUri);
		}

		[TestMethod]
		public void Try_Create_Existing_Collaborator()
		{
			var createResult = ExistingDataDataApi.CreateCollaborator(ApplicationID, "some@mail.com", CollaboratorType.Collaborator);
			Assert.IsNotNull(createResult);
			Assert.AreEqual(CreateStatus.AlreadyExists, createResult.Status);
			Assert.IsNull(createResult.Id);
			Assert.AreEqual(null, createResult.Location);
		}

		[TestMethod]
		public void Create_Non_Existing_ConfigurationVariable()
		{
			var createResult = Api.CreateConfigurationVariable(ApplicationID, "somekey", "somevalue");
			Assert.IsNotNull(createResult);
			Assert.AreEqual(CreateStatus.Created, createResult.Status);
			Assert.AreEqual("5", createResult.Id);
			Assert.AreEqual("https://appharbor.com/applications/:application/configurationvariables/5", createResult.Location.AbsoluteUri);
		}

		[TestMethod]
		public void Try_Create_Existing_ConfigurationVariable()
		{
			var createResult = ExistingDataDataApi.CreateConfigurationVariable(ApplicationID, "somekey", "somevalue");
			Assert.IsNotNull(createResult);
			Assert.AreEqual(CreateStatus.AlreadyExists, createResult.Status);
			Assert.IsNull(createResult.Id);
			Assert.AreEqual(null, createResult.Location);
		}

		[TestMethod]
		public void Create_Non_Existing_Hostname()
		{
			var createResult = Api.CreateHostname(ApplicationID, "somehostname.com");
			Assert.IsNotNull(createResult);
			Assert.AreEqual(CreateStatus.Created, createResult.Status);
			Assert.AreEqual("5", createResult.Id);
			Assert.AreEqual("https://appharbor.com/applications/:application/hostnames/5", createResult.Location.AbsoluteUri);
		}

		[TestMethod]
		public void Try_Create_Existing_Hostname()
		{
			var createResult = ExistingDataDataApi.CreateHostname(ApplicationID, "somehostname.com");
			Assert.IsNotNull(createResult);
			Assert.AreEqual(CreateStatus.AlreadyExists, createResult.Status);
			Assert.IsNull(createResult.Id);
			Assert.AreEqual(null, createResult.Location);
		}

		[TestMethod]
		public void Create_Non_Existing_ServiceHook()
		{
			var createResult = Api.CreateServicehook(ApplicationID, "http://someurl.com");
			Assert.IsNotNull(createResult);
			Assert.AreEqual(CreateStatus.Created, createResult.Status);
			Assert.AreEqual("5", createResult.Id);
			Assert.AreEqual("https://appharbor.com/applications/:application/servicehooks/5", createResult.Location.AbsoluteUri);
		}

		[TestMethod]
		public void Try_Create_Existing_ServiceHook()
		{
			var createResult = ExistingDataDataApi.CreateServicehook(ApplicationID, "http://someurl.com");
			Assert.IsNotNull(createResult);
			Assert.AreEqual(CreateStatus.AlreadyExists, createResult.Status);
			Assert.IsNull(createResult.Id);
			Assert.AreEqual(null, createResult.Location);
		}

		[TestMethod]
		public void Edit_Existing_Application()
		{
			var edited = Api.EditApplication(ApplicationID, new Application
			{
				Name = "SomeName"
			});
			Assert.IsTrue(edited);
		}

		[TestMethod]
		public void Edit_Non_Existing_Application()
		{
			var edited = Api.EditApplication(":notexistsapplication", new Application
			{
				Name = "SomeName"
			});
			Assert.IsFalse(edited);
		}

		[TestMethod]
		public void Edit_Existing_Collaborator()
		{
			var edited = Api.EditCollaborator(ApplicationID, new Collaborator
			{
				Id = "5",
				Role = CollaboratorType.Collaborator,
			});
			Assert.IsTrue(edited);
		}

		[TestMethod]
		public void Edit_Non_Existing_Collaborator()
		{
			var edited = Api.EditCollaborator(ApplicationID, new Collaborator
			{
				Id = "6",
				Role = CollaboratorType.Collaborator,
			});
			Assert.IsFalse(edited);
		}

		[TestMethod]
		public void Edit_Existing_ConfigurationVariable()
		{
			var edited = Api.EditConfigurationVariable(ApplicationID, new ConfigurationVariable
			{
				Id = "5",
				Key = "somekey",
				Value = "somevalue",
			});
			Assert.IsTrue(edited);
		}

		[TestMethod]
		public void Edit_Non_Existing_ConfigurationVariable()
		{
			var edited = Api.EditConfigurationVariable(ApplicationID, new ConfigurationVariable
			{
				Id = "6",
				Key = "somekey",
				Value = "somevalue",
			});
			Assert.IsFalse(edited);
		}

		[TestMethod]
		public void Delete_Existing_Application()
		{
			var deleted = Api.DeleteApplication(ApplicationID);
			Assert.IsTrue(deleted);
		}

		[TestMethod]
		public void Delete_Non_Existing_Application()
		{
			var deleted = Api.DeleteApplication(":notexistsapplication");
			Assert.IsFalse(deleted);
		}

		[TestMethod]
		public void Delete_Existing_Collaborator()
		{
			var deleted = Api.DeleteCollaborator(ApplicationID, "5");
			Assert.IsTrue(deleted);
		}

		[TestMethod]
		public void Delete_Non_Existing_Collaborator()
		{
			var deleted = Api.DeleteCollaborator(ApplicationID, "6");
			Assert.IsFalse(deleted);
		}

		[TestMethod]
		public void Delete_Existing_ConfigurationVariable()
		{
			var deleted = Api.DeleteConfigurationVariable(ApplicationID, "5");
			Assert.IsTrue(deleted);
		}

		[TestMethod]
		public void Delete_Non_Existing_ConfigurationVariable()
		{
			var deleted = Api.DeleteConfigurationVariable(ApplicationID, "6");
			Assert.IsFalse(deleted);
		}

		[TestMethod]
		public void Delete_Existing_Hostname()
		{
			var deleted = Api.DeleteHostname(ApplicationID, "5");
			Assert.IsTrue(deleted);
		}

		[TestMethod]
		public void Delete_Non_Existing_Hostname()
		{
			var deleted = Api.DeleteHostname(ApplicationID, "6");
			Assert.IsFalse(deleted);
		}

		[TestMethod]
		public void Delete_Existing_Servicehook()
		{
			var deleted = Api.DeleteServicehook(ApplicationID, "5");
			Assert.IsTrue(deleted);
		}

		[TestMethod]
		public void Delete_Non_Existing_Servicehook()
		{
			var deleted = Api.DeleteServicehook(ApplicationID, "6");
			Assert.IsFalse(deleted);
		}

		[TestMethod]
		public void Get_Non_Empty_Application_List()
		{
			var items = Api.GetApplications();
			Assert.IsNotNull(items);
			Assert.IsTrue(items.Count() == 1);

			var item = items.Single();
			Assert.AreEqual(item.Url, "https://appharbor.com/applications/:application");
		}

		[TestMethod]
		public void Get_Empty_Application_List()
		{
			var items = EmptyListDataApi.GetApplications();
			Assert.IsNotNull(items);
			Assert.IsTrue(!items.Any());
		}

		[TestMethod]
		public void Get_Existing_Application()
		{
			var item = Api.GetApplication(ApplicationID);
			Assert.IsNotNull(item);
			Assert.AreEqual(item.Name, "Hello World");
			Assert.AreEqual(item.Slug, "helloworld");
		}

		[TestMethod]
		public void Get_Null_For_Non_Existing_Application()
		{
			var item = Api.GetApplication(":nonexistingapplication");
			Assert.IsNull(item);
		}

		[TestMethod]
		public void Get_Non_Empty_Collaborator_List()
		{
			var items = Api.GetCollaborators(ApplicationID);

			Assert.IsNotNull(items);
			Assert.IsTrue(items.Count() == 1);

			var item = items.Single();
			Assert.AreEqual(item.Id, "5");
			Assert.AreEqual(item.Role, CollaboratorType.Collaborator);
			Assert.AreEqual(item.Url, "https://appharbor.com/applications/:application/collaborators/5");

			Assert.IsNotNull(item.User);
			Assert.AreEqual(item.User.Id, "WseYku+3w4HAMf+dIq854Qw/2Qc=");
			Assert.AreEqual(item.User.Name, "test");
		}

		[TestMethod]
		public void Get_Empty_Collaborator_List()
		{
			var items = EmptyListDataApi.GetCollaborators(ApplicationID);

			Assert.IsNotNull(items);
			Assert.IsTrue(!items.Any());
		}

		[TestMethod]
		public void Get_Existing_Collaborator()
		{
			var item = Api.GetCollaborator(ApplicationID, "5");
			Assert.AreEqual(item.Id, "5");
			Assert.AreEqual(item.Role, CollaboratorType.Collaborator);
			Assert.AreEqual(item.Url, "https://appharbor.com/applications/:application/collaborators/5");

			Assert.IsNotNull(item.User);
			Assert.AreEqual(item.User.Id, "WseYku+3w4HAMf+dIq854Qw/2Qc=");
			Assert.AreEqual(item.User.Name, "test");
		}

		[TestMethod]
		public void Get_Null_For_Non_Existing_Collaborator()
		{
			var item = Api.GetCollaborator(ApplicationID, "6");
			Assert.IsNull(item);
		}

		[TestMethod]
		public void Get_Non_Empty_ConfigurationVariable_List()
		{
			var items = Api.GetConfigurationVariables(ApplicationID);

			Assert.IsNotNull(items);
			Assert.IsTrue(items.Count() == 1);

			var item = items.Single();
			Assert.AreEqual(item.Id, "5");
			Assert.AreEqual(item.Key, "foo");
			Assert.AreEqual(item.Value, "bar");
			Assert.AreEqual(item.Url, "https://appharbor.com/applications/:application/configurationvariables/5");
		}

		[TestMethod]
		public void Get_Empty_ConfigurationVariable_List()
		{
			var items = EmptyListDataApi.GetConfigurationVariables(ApplicationID);

			Assert.IsNotNull(items);
			Assert.IsTrue(!items.Any());
		}

		[TestMethod]
		public void Get_Existing_ConfigurationVariable()
		{
			var item = Api.GetConfigurationVariable(ApplicationID, "5");
			Assert.AreEqual(item.Id, "5");
			Assert.AreEqual(item.Key, "foo");
			Assert.AreEqual(item.Value, "bar");
			Assert.AreEqual(item.Url, "https://appharbor.com/applications/:application/configurationvariables/5");
		}

		[TestMethod]
		public void Get_Null_For_Non_Existing_ConfigurationVariable()
		{
			var item = Api.GetConfigurationVariable(ApplicationID, "6");
			Assert.IsNull(item);
		}

		[TestMethod]
		public void Get_Non_Empty_Hostname_List()
		{
			var items = Api.GetHostnames(ApplicationID);

			Assert.IsNotNull(items);
			Assert.IsTrue(items.Count() == 1);

			var item = items.Single();
			Assert.AreEqual(item.Id, "5");
			Assert.AreEqual(item.Value, "example.org");
			Assert.AreEqual(item.Canonical, false);
			Assert.AreEqual(item.Url, "https://appharbor.com/applications/:application/hostnames/5");
		}

		[TestMethod]
		public void Get_Empty_Hostname_List()
		{
			var items = EmptyListDataApi.GetHostnames(ApplicationID);

			Assert.IsNotNull(items);
			Assert.IsTrue(!items.Any());
		}

		[TestMethod]
		public void Get_Existing_Hostname()
		{
			var item = Api.GetHostname(ApplicationID, "5");
			Assert.AreEqual(item.Id, "5");
			Assert.AreEqual(item.Value, "example.org");
			Assert.AreEqual(item.Canonical, false);
			Assert.AreEqual(item.Url, "https://appharbor.com/applications/:application/hostnames/5");
		}

		[TestMethod]
		public void Get_Null_For_Non_Existing_Hostname()
		{
			var item = Api.GetHostname(ApplicationID, "6");
			Assert.IsNull(item);
		}

		[TestMethod]
		public void Get_Non_Empty_Servicehook_List()
		{
			var items = Api.GetServicehooks(ApplicationID);

			Assert.IsNotNull(items);
			Assert.IsTrue(items.Count() == 1);

			var item = items.Single();
			Assert.AreEqual(item.Id, "5");
			Assert.AreEqual(item.Value, "http://www.example.org");
			Assert.AreEqual(item.Url, "https://appharbor.com/applications/:application/servicehooks/5");
		}

		[TestMethod]
		public void Get_Empty_Servicehook_List()
		{
			var items = EmptyListDataApi.GetServicehooks(ApplicationID);

			Assert.IsNotNull(items);
			Assert.IsTrue(!items.Any());
		}

		[TestMethod]
		public void Get_Existing_Servicehook()
		{
			var item = Api.GetServicehook(ApplicationID, "5");
			Assert.AreEqual(item.Id, "5");
			Assert.AreEqual(item.Value, "http://www.example.org");
			Assert.AreEqual(item.Url, "https://appharbor.com/applications/:application/servicehooks/5");
		}

		[TestMethod]
		public void Get_Null_For_Non_Existing_Servicehook()
		{
			var item = Api.GetServicehook(ApplicationID, "6");
			Assert.IsNull(item);
		}

		[TestMethod]
		public void Get_Non_Empty_Build_List()
		{
			var items = Api.GetBuilds(ApplicationID);

			Assert.IsNotNull(items);
			Assert.IsTrue(items.Count() == 1);

			var item = items.Single();
			Assert.AreEqual("5", item.Id);
			Assert.AreEqual("Succeeded", item.Status);
			Assert.AreEqual(new DateTime(2012, 02, 28, 13, 36, 30), item.Created);
			Assert.AreEqual(new DateTime(2012, 02, 28, 13, 37, 30), item.Deployed);
			Assert.AreEqual("https://appharbor.com/applications/:application/builds/:build/download", item.DownloadUrl);
			Assert.AreEqual("https://appharbor.com/applications/:application/builds/:build/tests", item.TestsUrl);
			Assert.AreEqual("https://appharbor.com/applications/:application/builds/5", item.Url.AbsoluteUri);

			Assert.IsNotNull(item.Commit);
			Assert.AreEqual("9f0fcea24cbb441c3ed848af3ccc5061d69cc7db", item.Commit.Id);
			Assert.AreEqual("foo", item.Commit.Message);
		}

		[TestMethod]
		public void Get_Empty_Build_List()
		{
			var items = EmptyListDataApi.GetBuilds(ApplicationID);

			Assert.IsNotNull(items);
			Assert.IsTrue(!items.Any());
		}

		[TestMethod]
		public void Get_Existing_Build()
		{
			var item = Api.GetBuild(ApplicationID, "5");
			Assert.AreEqual("5", item.Id);
			Assert.AreEqual("Succeeded", item.Status);
			Assert.AreEqual(new DateTime(2012, 02, 28, 13, 36, 30), item.Created);
			Assert.AreEqual(new DateTime(2012, 02, 28, 13, 37, 30), item.Deployed);
			Assert.AreEqual("https://appharbor.com/applications/:application/builds/:build/download", item.DownloadUrl);
			Assert.AreEqual("https://appharbor.com/applications/:application/builds/:build/tests", item.TestsUrl);
			Assert.AreEqual("https://appharbor.com/applications/:application/builds/5", item.Url.AbsoluteUri);

			Assert.IsNotNull(item.Commit);
			Assert.AreEqual("9f0fcea24cbb441c3ed848af3ccc5061d69cc7db", item.Commit.Id);
			Assert.AreEqual("foo", item.Commit.Message);
		}

		[TestMethod]
		public void Get_Null_For_Non_Existing_Build()
		{
			var item = Api.GetBuild(ApplicationID, "6");
			Assert.IsNull(item);
		}

		[TestMethod]
		public void Get_Non_Empty_Test_List_For_Existing_Build()
		{
			var items = Api.GetTests(ApplicationID, "5");

			Assert.IsNotNull(items);
			Assert.AreEqual(2, items.Count());

			var first = items.First();
			Assert.AreEqual("1", first.Id);
			Assert.AreEqual("FooTest", first.Name);
			Assert.AreEqual("Failed", first.Status);
			Assert.AreEqual("Group", first.Kind);
			Assert.AreEqual(TimeSpan.Parse("00:00:03.3852434"), first.Duration);

			Assert.IsNotNull(first.Tests);
			Assert.AreEqual(2, first.Tests.Count);

			var item = first.Tests[0];
			Assert.AreEqual("1.1", item.Id);
			Assert.AreEqual("CanCreateFoo", item.Name);
			Assert.AreEqual("Failed", item.Status);
			Assert.AreEqual("Test", item.Kind);
			Assert.AreEqual(TimeSpan.FromTicks(156002), item.Duration);

			item = first.Tests[1];
			Assert.AreEqual("1.2", item.Id);
			Assert.AreEqual("CanDeleteFoo", item.Name);
			Assert.AreEqual("Passed", item.Status);
			Assert.AreEqual("Test", item.Kind);
			Assert.AreEqual(TimeSpan.FromTicks(468006), item.Duration);

			var second = items.Skip(1).First();
			Assert.IsNotNull(second.Tests);

			Assert.AreEqual("2", second.Id);
			Assert.AreEqual("BarTest", second.Name);
			Assert.AreEqual("Passed", second.Status);
			Assert.AreEqual("Group", second.Kind);
			Assert.AreEqual(TimeSpan.FromTicks(1028526).Add(new TimeSpan(0, 0, 4)), second.Duration);

			Assert.IsNotNull(second.Tests);
			Assert.AreEqual(2, second.Tests.Count);

			item = second.Tests[0];
			Assert.AreEqual("2.1", item.Id);
			Assert.AreEqual("CanCreateBar", item.Name);
			Assert.AreEqual("Passed", item.Status);
			Assert.AreEqual("Test", item.Kind);
			Assert.AreEqual(TimeSpan.FromTicks(1248016), item.Duration);

			item = second.Tests[1];
			Assert.AreEqual("2.2", item.Id);
			Assert.AreEqual("CanDeleteBar", item.Name);
			Assert.AreEqual("Skipped", item.Status);
			Assert.AreEqual("Test", item.Kind);
			Assert.AreEqual(TimeSpan.FromTicks(936012), item.Duration);
		}

		[TestMethod]
		public void Get_Empty_Test_List_For_Existing_Build()
		{
			var items = EmptyListDataApi.GetTests(ApplicationID, "5");

			Assert.IsNotNull(items);
			Assert.IsTrue(!items.Any());
		}

		[TestMethod]
		public void Get_Existing_Test()
		{
			var item = Api.GetTest(ApplicationID, "5", "3.1");
			Assert.AreEqual("3.1", item.Id);
			Assert.AreEqual("ShouldReturnFooWhenBarIsCalled", item.Name);
			Assert.AreEqual("Passed", item.Status);
			Assert.AreEqual("Test", item.Kind);
			Assert.AreEqual(item.Duration, TimeSpan.FromTicks(312004));

			Assert.IsNotNull(item.Tests);
			Assert.AreEqual(0, item.Tests.Count);
		}

		[TestMethod]
		public void Get_Null_For_Non_Existing_Test()
		{
			var item = Api.GetTest(ApplicationID, "5", "6");
			Assert.IsNull(item);
		}

		[TestMethod]
		public void Get_Null_Test_List_For_Non_Existing_Build()
		{
			var item = Api.GetTest(ApplicationID, "6", "6");
			Assert.IsNull(item);
		}

		[TestMethod]
		public void Get_Errors()
		{
			var items = Api.GetErrors(ApplicationID);

			Assert.IsNotNull(items);
			Assert.IsTrue(items.Count() == 1);

			var item = items.Single();
			Assert.AreEqual("5", item.Id);
			Assert.AreEqual("foo", item.Commit_Id);
			Assert.AreEqual(new DateTime(2012, 03, 05, 15, 01, 11), item.Date);
			Assert.AreEqual("/", item.Request_Path);
			Assert.AreEqual("An unhandled exception has occurred.", item.Message);
			Assert.AreEqual("https://appharbor.com/applications/:application/errors/5", item.Url.AbsoluteUri);

			Assert.IsNotNull(item.Exception);
			Assert.AreEqual("at FooController.Show(Bar bar)", item.Exception.Stack_Trace);
			Assert.AreEqual("Object reference not set to an instance of an object.", item.Exception.Message);
			Assert.AreEqual("System.NullReferenceException", item.Exception.Type);
			Assert.AreEqual(null, item.Exception.Inner_Exception);
		}

		[TestMethod]
		public void Get_Existing_Error()
		{
			var item = Api.GetError(ApplicationID, "5");
			Assert.AreEqual("5", item.Id);
			Assert.AreEqual("foo", item.Commit_Id);
			Assert.AreEqual(new DateTime(2012, 03, 05, 15, 01, 42), item.Date);
			Assert.AreEqual("/", item.Request_Path);
			Assert.AreEqual("An unhandled exception has occurred.", item.Message);
			Assert.AreEqual("https://appharbor.com/applications/:application/errors/5", item.Url.AbsoluteUri);

			Assert.IsNotNull(item.Exception);
			Assert.AreEqual("at FooController.Show(Bar bar)", item.Exception.Stack_Trace);
			Assert.AreEqual("Object reference not set to an instance of an object.", item.Exception.Message);
			Assert.AreEqual("System.NullReferenceException", item.Exception.Type);
			Assert.AreEqual(null, item.Exception.Inner_Exception);
		}

		[TestMethod]
		public void Get_Null_For_Non_Existing_Error()
		{
			var item = Api.GetError(ApplicationID, "6");
			Assert.IsNull(item);
		}

		[TestMethod]
		public void GetUserTest()
		{
			var item = Api.GetUser();
			Assert.AreEqual(item.Id, "0#WseYku+3w4HAMf+dIq854Qw/2Qc=");
			Assert.AreEqual(item.Username, "foo");

			Assert.IsNotNull(item.Email_Addresses);
			Assert.AreEqual(2, item.Email_Addresses.Count);
			Assert.AreEqual("foo@example.com", item.Email_Addresses[0]);
			Assert.AreEqual("bar@example.com", item.Email_Addresses[1]);
		}
	}
}
