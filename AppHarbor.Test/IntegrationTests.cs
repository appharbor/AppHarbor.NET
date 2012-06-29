﻿using System;
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
		private static string ApplicationSlug;

		/// <summary>
		/// Needs to be a valid Access Token
		/// </summary>
		private static string AccessToken;

		/// <summary>
		/// Needs to be an Email address that is registered at AppHarbor 
		/// but not the email address of the current account holder
		/// </summary>
		private static string CollaboratorEmail;

		private static AppHarborClient Api;

		[ClassInitialize]
		public static void TestInit(TestContext context)
		{
			if (string.IsNullOrWhiteSpace(AccessToken))
			{
				Assert.Inconclusive("Please specify a valid AccessToken");
			}

			// zzzintegration + first 20 chracters of newly created guid
			// this should result in a fairly unique applicationSlug
			ApplicationSlug = "zzzintegration" + Guid.NewGuid()
				.ToString("N")
				.ToLower()
				.Substring(0, 20);

			Api = new AppHarborClient(new AuthInfo(AccessToken));
		}

		private void EnsureApplication()
		{
			var application = Api.GetApplication(ApplicationSlug);
			if (application != null)
			{
				return;
			}

			Api.DeleteApplication(ApplicationSlug);
			var result = Api.CreateApplication(ApplicationSlug, null);
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Id);
			Assert.AreEqual(CreateStatus.Created, result.Status);
		}

		[TestMethod]
		[Priority(10)]
		public void Create_Get_Edit_Delete_Application()
		{
			var result = Api.CreateApplication(ApplicationSlug, null);
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Id);
			Assert.AreEqual(CreateStatus.Created, result.Status);

			var application = Api.GetApplication(result.Id);
			Assert.IsNotNull(application);
			Assert.AreEqual(result.Id, application.Slug);
			Assert.AreEqual(result.Id, application.Name);
			Assert.AreEqual("amazon-web-services::us-east-1", application.RegionIdentifier);

			application.Name = result.Id + "u";
			var updated = Api.EditApplication(result.Id, application);
			Assert.IsTrue(updated);

			application = Api.GetApplication(result.Id);
			Assert.IsNotNull(application);
			Assert.AreEqual(result.Id, application.Slug);
			Assert.AreEqual(result.Id + "u", application.Name);
			Assert.AreEqual("amazon-web-services::us-east-1", application.RegionIdentifier);

			var deleted = Api.DeleteApplication(ApplicationSlug);
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

			EnsureApplication();

			var result = Api.CreateCollaborator(ApplicationSlug, CollaboratorEmail, Model.CollaboratorType.Collaborator);
			Assert.IsNotNull(result);
			Assert.AreNotEqual(0, result.Id);
			Assert.AreEqual(CreateStatus.Created, result.Status);

			var item = Api.GetCollaborator(ApplicationSlug, result.Id);
			Assert.IsNotNull(item);
			Assert.AreEqual(result.Id, item.Id);
			Assert.AreEqual(CollaboratorType.Collaborator, item.Role);

			item.Role = CollaboratorType.Administrator;
			var updated = Api.EditCollaborator(ApplicationSlug, item);
			Assert.IsTrue(updated);

			item = Api.GetCollaborator(ApplicationSlug, result.Id);
			Assert.IsNotNull(item);
			Assert.AreEqual(result.Id, item.Id);
			Assert.AreEqual(CollaboratorType.Administrator, item.Role);

			Api.DeleteCollaborator(ApplicationSlug, result.Id);
		}

		[TestMethod]
		public void Create_Get_Edit_Delete_ConfigurationVariable()
		{
			EnsureApplication();

			var result = Api.CreateConfigurationVariable(ApplicationSlug, "somekey", "somevalue");
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Id);
			Assert.AreEqual(CreateStatus.Created, result.Status);

			var item = Api.GetConfigurationVariable(ApplicationSlug, result.Id);
			Assert.IsNotNull(item);
			Assert.AreEqual(result.Id, item.Id);
			Assert.AreEqual("somekey", item.Key);
			Assert.AreEqual("somevalue", item.Value);

			item.Key = "somekeyu";
			item.Value = "somevalueu";
			var updated = Api.EditConfigurationVariable(ApplicationSlug, item);
			Assert.IsTrue(updated);

			item = Api.GetConfigurationVariable(ApplicationSlug, result.Id);
			Assert.IsNotNull(item);
			Assert.AreEqual(result.Id, item.Id);
			Assert.AreEqual("somekeyu", item.Key);
			Assert.AreEqual("somevalueu", item.Value);

			Api.DeleteConfigurationVariable(ApplicationSlug, result.Id);
		}

		[TestMethod]
		public void Create_Get_Delete_Hostname()
		{
			EnsureApplication();

			var result = Api.CreateHostname(ApplicationSlug, "some345345n4534host.com");
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Id);
			Assert.AreEqual(CreateStatus.Created, result.Status);

			var item = Api.GetHostname(ApplicationSlug, result.Id);
			Assert.IsNotNull(item);
			Assert.AreEqual(result.Id, item.Id);
			Assert.AreEqual("some345345n4534host.com", item.Value);

			Api.DeleteHostname(ApplicationSlug, result.Id);
		}

		[TestMethod]
		public void Create_Get_Delete_Servicehook()
		{
			EnsureApplication();

			var result = Api.CreateServicehook(ApplicationSlug, "http://somehost.com");
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Id);
			Assert.AreEqual(CreateStatus.Created, result.Status);

			var item = Api.GetServicehook(ApplicationSlug, result.Id);
			Assert.IsNotNull(item);
			Assert.AreEqual(result.Id, item.Id);
			Assert.AreEqual("http://somehost.com", item.Value);

			Api.DeleteServicehook(ApplicationSlug, result.Id);
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
