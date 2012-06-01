using System;
using System.Linq;
using AppHarbor.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppHarbor.Test
{
	/// <summary>
	/// To run these tests you need to specify:
	/// - A valid AppHarbor Access Token 
	/// - A valid collaborator email
	/// Afterwards remove the Ignore attribute from the class.
	/// </summary>
	[TestClass]
	public class IntegrationTests
	{
		/// <summary>
		/// This will be dynamically set to a unique name
		/// </summary>
		private static string ApplicationID = null;

		/// <summary>
		/// Needs to be a valid Access Token
		/// </summary>
		private static string AccessToken = null;

		/// <summary>
		/// Needs to be an Email address that is registered at AppHarbor 
		/// but not the email address of the current account holder
		/// </summary>
		private static string CollaboratorEmail = null;

		private static AppHarborApi Api;

		[ClassInitialize]
		public static void TestInit(TestContext context)
		{
			if (string.IsNullOrWhiteSpace(AccessToken))
			{
				Assert.Inconclusive("Please specify a valid AccessToken");
			}

			// zzzintegration + first 20 chracters of newly created guid
			// this should result in a fairly unique applicationid
			ApplicationID = "zzzintegration" + Guid.NewGuid()
				.ToString("N")
				.ToLower()
				.Substring(0, 20);

			Api = new AppHarborApi(new AuthInfo()
			{
				AccessToken = AccessToken,
			});
		}

		private void EnsureApplication(string name)
		{
			var application = Api.GetApplication(ApplicationID);
			if (application != null)
			{
				return;
			}

			Api.DeleteApplication(ApplicationID);
			var result = Api.CreateApplication(ApplicationID, null);
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Id);
			Assert.AreEqual(CreateStatus.Created, result.Status);
		}

		[TestMethod]
		[Priority(10)]
		public void Create_Get_Edit_Delete_Application()
		{
			var result = Api.CreateApplication(ApplicationID, null);
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Id);
			Assert.AreEqual(CreateStatus.Created, result.Status);

			var application = Api.GetApplication(result.Id);
			Assert.IsNotNull(application);
			Assert.AreEqual(result.Id, application.Slug);
			Assert.AreEqual(result.Id, application.Name);
			Assert.AreEqual("amazon-web-services::us-east-1", application.RegionIdentitfier);

			application.Name = result.Id + "u";
			var updated = Api.EditApplication(result.Id, application);
			Assert.IsTrue(updated);

			application = Api.GetApplication(result.Id);
			Assert.IsNotNull(application);
			Assert.AreEqual(result.Id, application.Slug);
			Assert.AreEqual(result.Id + "u", application.Name);
			Assert.AreEqual("amazon-web-services::us-east-1", application.RegionIdentitfier);

			var deleted = Api.DeleteApplication(ApplicationID);
			Assert.IsTrue(deleted);

			application = Api.GetApplication(result.Id);
			Assert.IsNull(application);
		}

		//[Ignore]
		[TestMethod]
		public void Create_Get_Edit_Delete_Collaborator()
		{
			if (string.IsNullOrWhiteSpace(CollaboratorEmail))
			{
				Assert.Inconclusive("Please specify a valid CollaboratorEmail, if you don't have one just uncomment the Ignore attribue");
			}

			EnsureApplication(ApplicationID);

			var result = Api.CreateCollaborator(ApplicationID, CollaboratorEmail, Model.CollaboratorType.Collaborator);
			Assert.IsNotNull(result);
			Assert.AreNotEqual(0, result.Id);
			Assert.AreEqual(CreateStatus.Created, result.Status);

			var item = Api.GetCollaborator(ApplicationID, result.Id);
			Assert.IsNotNull(item);
			Assert.AreEqual(result.Id, item.Id);
			Assert.AreEqual(CollaboratorType.Collaborator, item.Role);

			item.Role = CollaboratorType.Administrator;
			var updated = Api.EditCollaborator(ApplicationID, item);
			Assert.IsTrue(updated);

			item = Api.GetCollaborator(ApplicationID, result.Id);
			Assert.IsNotNull(item);
			Assert.AreEqual(result.Id, item.Id);
			Assert.AreEqual(CollaboratorType.Administrator, item.Role);

			Api.DeleteCollaborator(ApplicationID, result.Id);
		}

		[TestMethod]
		public void Create_Get_Edit_Delete_ConfigurationVariable()
		{
			EnsureApplication(ApplicationID);

			var result = Api.CreateConfigurationVariable(ApplicationID, "somekey", "somevalue");
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Id);
			Assert.AreEqual(CreateStatus.Created, result.Status);

			var item = Api.GetConfigurationVariable(ApplicationID, result.Id);
			Assert.IsNotNull(item);
			Assert.AreEqual(result.Id, item.Id);
			Assert.AreEqual("somekey", item.Key);
			Assert.AreEqual("somevalue", item.Value);

			item.Key = "somekeyu";
			item.Value = "somevalueu";
			var updated = Api.EditConfigurationVariable(ApplicationID, item);
			Assert.IsTrue(updated);

			item = Api.GetConfigurationVariable(ApplicationID, result.Id);
			Assert.IsNotNull(item);
			Assert.AreEqual(result.Id, item.Id);
			Assert.AreEqual("somekeyu", item.Key);
			Assert.AreEqual("somevalueu", item.Value);

			Api.DeleteConfigurationVariable(ApplicationID, result.Id);
		}

		[TestMethod]
		public void Create_Get_Delete_Hostname()
		{
			EnsureApplication(ApplicationID);

			var result = Api.CreateHostname(ApplicationID, "some345345n4534host.com");
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Id);
			Assert.AreEqual(CreateStatus.Created, result.Status);

			var item = Api.GetHostname(ApplicationID, result.Id);
			Assert.IsNotNull(item);
			Assert.AreEqual(result.Id, item.Id);
			Assert.AreEqual("some345345n4534host.com", item.Value);

			Api.DeleteHostname(ApplicationID, result.Id);
		}

		[TestMethod]
		public void Create_Get_Delete_Servicehook()
		{
			EnsureApplication(ApplicationID);

			var result = Api.CreateServicehook(ApplicationID, "http://somehost.com");
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Id);
			Assert.AreEqual(CreateStatus.Created, result.Status);

			var item = Api.GetServicehook(ApplicationID, result.Id);
			Assert.IsNotNull(item);
			Assert.AreEqual(result.Id, item.Id);
			Assert.AreEqual("http://somehost.com", item.Value);

			Api.DeleteServicehook(ApplicationID, result.Id);
		}

		[ClassCleanup]
		public static void TestCleanupAttribute()
		{
			// remove all applications that start with: zzzintegration
			var applications = Api.GetApplications();
			foreach (var item in applications
				.Where(i => i.Slug != null && i.Slug.StartsWith("zzzintegration")))
			{
				Api.DeleteApplication(item.Slug);
			}
		}
	}
}
