using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace Roi.Utilities.Rest
{
    public class RoiRestClient
    {
        protected RestClient InternalRestClient { get; }

        public RoiRestClient(string baseUrl) : this(baseUrl, null)
        {
        }

        public RoiRestClient(string baseUrl, IAuthenticator authenticator) :
            this(baseUrl, authenticator, false)
        {

        }

        public RoiRestClient(string baseUrl, IAuthenticator authenticator, bool useAdditionalTlsOrSslSecurity)
        {
            InternalRestClient = new RestClient(baseUrl);

            if (authenticator != null)
            {
                var authenticatorTranslator = new AuthenticatorTranslator(authenticator);
                //need to create a concrete class that implements Rest Sharp's IAuthenticator interface, but uses our autheticator instance that was passed into this method
                InternalRestClient.Authenticator = authenticatorTranslator;
            }
            if (useAdditionalTlsOrSslSecurity)
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 |
                                                       (SecurityProtocolType)768 |
                                                       (SecurityProtocolType)3072;
            }
        }

        public RoiRestClientResponse<TReturnedEntity> GetSingle<TReturnedEntity>(
            string resourceRelativePath) where TReturnedEntity : new()
        {
            return GetSingle<TReturnedEntity>(resourceRelativePath, null);
        }

        public RoiRestClientResponse<TReturnedEntity> GetSingle<TReturnedEntity>(
            string resourceRelativePath, string rootElementName) where TReturnedEntity : new()
        {
            var request = GetBasicRequest(
                resourceRelativePath,
                Method.GET,
                DataFormat.Json);

            if (!string.IsNullOrEmpty(rootElementName))
            {
                request.RootElement = rootElementName;
            }
            var response = InternalRestClient.Execute<TReturnedEntity>(request);

            var restClientResponse = new RoiRestClientResponse<TReturnedEntity>();

            if (response.ResponseStatus == ResponseStatus.Error) //TODO: what about other status enums?
            {
                restClientResponse.Success = false;
                restClientResponse.ErrorMessage = response.ErrorMessage;
                restClientResponse.Content = response.Content;
            }
            else
            {
                restClientResponse.Success = true;
                restClientResponse.ReturnedObject = response.Data;
                //                foreach (var parameter in response.Headers)
                //                {
                //look through the headers for ResourceUri - location to the newly created object
                //                    restClientResponse.ResourceUri = null;
                //                    var absoluteUri = response.Headers.Location.AbsoluteUri;
                //                    var lastForwardSlashLocation = absoluteUri.LastIndexOf("/");
                //                    var parsedId =
                //                        absoluteUri.Substring(
                //                            lastForwardSlashLocation + 1,
                //                            absoluteUri.Length - lastForwardSlashLocation - 1);
                //                    restClientResponse.ResourceParsedId = parsedId;
                //                }

            }
            restClientResponse.HttpStatusCode = (int)response.StatusCode;
            return restClientResponse;
        }

        public RoiRestClientResponse<List<TReturnedEntity>> GetMany<TReturnedEntity>(
            string resourceRelativePath, Dictionary<string, string> queryStringParameters, string rootElementName)
            where TReturnedEntity : new()
        {
            var request = GetBasicRequest(
                resourceRelativePath,
                Method.GET,
                DataFormat.Json);

            if (!string.IsNullOrEmpty(rootElementName))
            {
                request.RootElement = rootElementName;
            }
            if (queryStringParameters?.Count > 0)
            {
                foreach (var queryStringParameter in queryStringParameters)
                {
                    request.AddQueryParameter(queryStringParameter.Key, queryStringParameter.Value);

                }
            }
            var response = InternalRestClient.Execute<List<TReturnedEntity>>(request);

            var restClientResponse = new RoiRestClientResponse<List<TReturnedEntity>>();

            if (response.ResponseStatus == ResponseStatus.Error) //TODO: what about other status enums?
            {
                restClientResponse.Success = false;
                restClientResponse.ErrorMessage = response.ErrorMessage;
                restClientResponse.Content = response.Content;
            }
            else
            {
                restClientResponse.Success = true;
                restClientResponse.ReturnedObject = response.Data;
            }
            restClientResponse.HttpStatusCode = (int)response.StatusCode;
            return restClientResponse;

        }

        public RoiRestClientResponse<List<TReturnedEntity>> GetMany<TReturnedEntity>(
            string resourceRelativePath, string rootElementName) where TReturnedEntity : new()
        {
            return GetMany<TReturnedEntity>(resourceRelativePath, null, rootElementName);
        }

        public RoiRestClientResponse<TReturnedEntity> Post<TReturnedEntity>(
            string resourceRelativePath, object resourceToCreate, string rootElement)
            where TReturnedEntity : class, new()
        {
            var request = GetBasicRequest(
                resourceRelativePath,
                Method.POST,
                DataFormat.Json);

            request.AddBody(resourceToCreate);

            if (!string.IsNullOrEmpty(rootElement))
            {
                request.RootElement = rootElement;
            }
            var response = InternalRestClient.Execute<TReturnedEntity>(request);

            var restClientResponse = new RoiRestClientResponse<TReturnedEntity>();

            if (response.ResponseStatus == ResponseStatus.Error) //TODO: what about other status enums?
            {
                restClientResponse.Success = false;
                restClientResponse.ErrorMessage = response.ErrorMessage;
                restClientResponse.Content = response.Content;
            }
            else
            {
                restClientResponse.ReturnedObject = response.Data;
                restClientResponse.Success = true;
                restClientResponse.ReturnedObject = response.Data;
            }

            restClientResponse.HttpStatusCode = (int)response.StatusCode;
            return restClientResponse;
        }

        public RoiRestClientResponse<List<TReturnedEntity>> Post<TReturnedEntity>(
            string resourceRelativePath, Dictionary<string, string> postBodyParameters)
            where TReturnedEntity : class, new()
        {

            var request = GetBasicRequest(
                resourceRelativePath,
                Method.POST,
                null);

            foreach (var postBodyParameter in postBodyParameters)
            {
                request.AddParameter(postBodyParameter.Key, postBodyParameter.Value);
            }

            var response = InternalRestClient.Execute<List<TReturnedEntity>>(request);

            var restClientResponse = new RoiRestClientResponse<List<TReturnedEntity>>();

            if (response.ResponseStatus == ResponseStatus.Error) //TODO: what about other status enums?
            {
                restClientResponse.Success = false;
                restClientResponse.ErrorMessage = response.ErrorMessage;
                restClientResponse.Content = response.Content;
            }
            else
            {
                restClientResponse.ReturnedObject = response.Data;
                restClientResponse.Success = true;
                restClientResponse.ReturnedObject = response.Data;
            }

            restClientResponse.HttpStatusCode = (int)response.StatusCode;
            return restClientResponse;
        }

        private static RestRequest GetBasicRequest(string resourceRelativePath, Method httpMethod,
            DataFormat? dataFormat)
        {
            var request = new RestRequest(httpMethod);
            request.Resource = resourceRelativePath;
            if (dataFormat != null)
            {
                request.RequestFormat = dataFormat.Value;
            }
            return request;
        }

    }

    internal class AuthenticatorTranslator : RestSharp.Authenticators.IAuthenticator
    {
        public IAuthenticator Authenticator { get; set; }
        public IRestClient Client { get; set; }
        public IRestRequest Request { get; set; }

        public AuthenticatorTranslator(IAuthenticator authenticator)
        {
            Authenticator = authenticator;
        }

        public void Authenticate(RestSharp.IRestClient client, RestSharp.IRestRequest request)
        {
            var clientTranslator = new RestClientTranslator(client);
            var requestTranslator = new RestRequestTranslator(request);
            this.Authenticator.Authenticate(clientTranslator, requestTranslator);
        }
    }

    internal class RestClientTranslator : IRestClient
    {
        public RestSharp.IRestClient RestClient { get; set; }

        public RestClientTranslator(RestSharp.IRestClient restClient)
        {
            RestClient = restClient;
        }
    }


    internal class RestRequestTranslator : IRestRequest
    {
        public RestSharp.IRestRequest RestRequest { get; set; }

        public RestRequestTranslator(RestSharp.IRestRequest restRequest)
        {
            RestRequest = restRequest;
        }

        public void AddHeader(string headerName, string headerValue)
        {
            throw new NotImplementedException();
        }
    }
}
