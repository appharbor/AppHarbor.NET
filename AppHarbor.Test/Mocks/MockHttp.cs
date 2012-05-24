using System;
using System.IO;
using System.Text;
using RestSharp;

namespace AppHarbor.Test.Mocks
{
    public class SampleDataMockHttp : MockHttp, IHttp
    {
        public SampleDataMockHttp()
            : base()
        {
        }

        HttpResponse IHttp.Delete()
        {
            var url = this.Url.LocalPath;
            switch (url)
            {
                case "/applications/:application":
                case "/applications/:application/collaborators/5":
                case "/applications/:application/configurationvariables/5":
                case "/applications/:application/hostnames/5":
                case "/applications/:application/servicehooks/5":
                    {
                        return CreateHttpResponse(System.Net.HttpStatusCode.NoContent, null);
                    }
                default:
                    {
                        return CreateHttpResponse(System.Net.HttpStatusCode.NotFound, null);
                    }
            }
        }

        HttpResponse IHttp.Get()
        {
            var url = this.Url.LocalPath;
            switch (url)
            {
                case "/applications":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetApplications.json"));
                    }
                case "/applications/:application":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetApplication.json"));
                    }
                case "/applications/:application/collaborators":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetCollaborators.json"));
                    }
                case "/applications/:application/collaborators/5":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetCollaborator.json"));
                    }
                case "/applications/:application/configurationvariables":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetConfigurationVariables.json"));
                    }
                case "/applications/:application/configurationvariables/5":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetConfigurationVariable.json"));
                    }
                case "/applications/:application/hostnames":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetHostnames.json"));
                    }
                case "/applications/:application/hostnames/5":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetHostname.json"));
                    }
                case "/applications/:application/servicehooks":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetServiceHooks.json"));
                    }
                case "/applications/:application/servicehooks/5":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetServiceHook.json"));
                    }
                case "/applications/:application/builds":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetBuilds.json"));
                    }
                case "/applications/:application/builds/5":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetBuild.json"));
                    }
                case "/applications/:application/builds/5/tests":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetTests.json"));
                    }
                case "/applications/:application/builds/5/tests/3.1":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetTest.json"));
                    }
                case "/applications/:application/errors":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetErrors.json"));
                    }
                case "/applications/:application/errors/5":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetError.json"));
                    }
                case "/user":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\GetUser.json"));
                    }
                default:
                    {
                        return CreateHttpResponse(System.Net.HttpStatusCode.NotFound, null);
                    }
            }
        }

        HttpResponse IHttp.Post()
        {
            var url = this.Url.LocalPath;
            switch (url)
            {
                case "/applications":
                    {
                        return CreateHttpResponse(System.Net.HttpStatusCode.Created, "https://appharbor.com/applications/:application");
                    }
                case "/applications/:application/collaborators":
                    {
                        return CreateHttpResponse(System.Net.HttpStatusCode.Created, "https://appharbor.com/applications/:application/collaborators/5");
                    }
                case "/applications/:application/configurationvariables":
                    {
                        return CreateHttpResponse(System.Net.HttpStatusCode.Created, "https://appharbor.com/applications/:application/configurationvariables/5");
                    }
                case "/applications/:application/hostnames":
                    {
                        return CreateHttpResponse(System.Net.HttpStatusCode.Created, "https://appharbor.com/applications/:application/hostnames/5");
                    }
                case "/applications/:application/servicehooks":
                    {
                        return CreateHttpResponse(System.Net.HttpStatusCode.Created, "https://appharbor.com/applications/:application/servicehooks/5");
                    }
                default:
                    {
                        return CreateHttpResponse(System.Net.HttpStatusCode.BadRequest, null);
                    }
            }
        }

        HttpResponse IHttp.Put()
        {
            var url = this.Url.LocalPath;
            switch (url)
            {
                case "/applications/:application":
                case "/applications/:application/collaborators/5":
                case "/applications/:application/configurationvariables/5":
                    {
                        return CreateHttpResponse(System.Net.HttpStatusCode.OK, null);
                    }
                default:
                    {
                        return CreateHttpResponse(System.Net.HttpStatusCode.NotFound, null);
                    }
            }
        }

    }

    public class EmptyListDataMockHttp : MockHttp, IHttp
    {
        HttpResponse IHttp.Get()
        {
            var url = this.Url.LocalPath;
            switch (url)
            {
                case "/applications":
                case "/applications/:application/collaborators":
                case "/applications/:application/configurationvariables":
                case "/applications/:application/hostnames":
                case "/applications/:application/servicehooks":
                case "/applications/:application/builds":
                case "/applications/:application/builds/5/tests":
                case "/applications/:application/errors":
                    {
                        return CreateGetResponse(this.Url, System.Net.HttpStatusCode.OK, Path.Combine(BaseLocation, @"Data\EmptyArray.json"));
                    }
                default:
                    {
                        return CreateHttpResponse(System.Net.HttpStatusCode.NotFound, null);
                    }
            }
        }
    }

    public class ExistingDataMockHttp : MockHttp, IHttp
    {
        HttpResponse IHttp.Post()
        {
            var url = this.Url.LocalPath;
            switch (url)
            {
                case "/applications":
                case "/applications/:application/collaborators":
                case "/applications/:application/configurationvariables":
                case "/applications/:application/hostnames":
                case "/applications/:application/servicehooks":
                    {
                        return CreateHttpResponse(System.Net.HttpStatusCode.Conflict, null);
                    }
                default:
                    {
                        return CreateHttpResponse(System.Net.HttpStatusCode.BadRequest, null);
                    }
            }
        }
    }

    public abstract class MockHttp : Http, IHttp
    {
        public const string BaseURL = "http://unit.test/";

        public MockHttp()
            : base()
        {
            BaseLocation = Util.GetCurrentBasePath();
        }

        public string BaseLocation { get; private set; }

        protected HttpResponse CreateHttpResponse(System.Net.HttpStatusCode statusCode, string location)
        {
            var encoding = Encoding.UTF8;
            var response = new HttpResponse()
            {
                StatusCode = statusCode,
                ContentEncoding = "UTF8",
                ContentLength = 0,
                RawBytes = encoding.GetBytes(""),
                ContentType = "application/json",
            };

            if (location != null)
            {
                response.Headers.Add(new HttpHeader()
                {
                    Name = "Location",
                    Value = location
                });
            }

            return response;
        }

        protected HttpResponse CreateGetResponse(Uri url, System.Net.HttpStatusCode statusCode, string fileName)
        {
            var encoding = Encoding.UTF8;
            var content = File.ReadAllText(fileName, encoding);
            var response = new HttpResponse()
            {
                ResponseUri = url,
                ContentEncoding = "UTF8",
                StatusCode = statusCode,
                ContentLength = content.Length,
                RawBytes = encoding.GetBytes(content),
                ContentType = "application/json",
            };

            return response;
        }

        HttpResponse IHttp.Delete()
        {
            return CreateHttpResponse(System.Net.HttpStatusCode.NotFound, null);
        }

        HttpResponse IHttp.Get()
        {
            return CreateHttpResponse(System.Net.HttpStatusCode.NotFound, null);
        }

        HttpResponse IHttp.Post()
        {
            return CreateHttpResponse(System.Net.HttpStatusCode.NotFound, null);
        }

        HttpResponse IHttp.Put()
        {
            return CreateHttpResponse(System.Net.HttpStatusCode.NotFound, null);
        }
    }
}
