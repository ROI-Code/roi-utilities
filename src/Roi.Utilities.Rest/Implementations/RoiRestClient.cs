using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;

namespace Roi.Utilities.Rest
{
    public class RoiRestClient : IRestClient //TODO: make Get and GetMany methods call into the the same private method
    {

        public static DataFormat DataFormat { get; set; }

        protected RestClient InternalRestClient { get; }

        public RoiRestClient(string baseUrl, ResponseFormat responseFormat) : this(baseUrl, responseFormat, null)
        {
        }

        public RoiRestClient(string baseUrl, ResponseFormat responseFormat, IAuthenticator authenticator) :
            this(baseUrl, authenticator, responseFormat, false)
        {

        }

        public RoiRestClient(string baseUrl, IAuthenticator authenticator, ResponseFormat responseFormat, bool useAdditionalTlsOrSslSecurity)
        {
            InternalRestClient = new RestClient(baseUrl);

            switch (responseFormat)
            {
                case ResponseFormat.Json:
                    DataFormat = DataFormat.Json;
                    break;
                case ResponseFormat.Xml:
                    DataFormat = DataFormat.Xml;
                    break;
                case ResponseFormat.Csv:
                    // TODO: figure out what to do for CSV output format
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(responseFormat), responseFormat, null);
            }

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
            return GetSingleExecute<TReturnedEntity>(resourceRelativePath, null);
        }

        public RoiRestClientResponse<TReturnedEntity> GetSingle<TReturnedEntity>(
            string resourceRelativePath, string rootElementName) where TReturnedEntity : new()
        {
            return GetSingleExecute<TReturnedEntity>(resourceRelativePath, rootElementName);
        }

        private RoiRestClientResponse<TReturnedEntity> GetSingleExecute<TReturnedEntity>(
            string resourceRelativePath, string rootElementName) where TReturnedEntity : new()
        {
            var request = GetBasicRequest(resourceRelativePath, Method.GET);

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
            }
            restClientResponse.HttpStatusCode = (int)response.StatusCode;
            return restClientResponse;
        }

        public RoiRestClientResponse<List<TReturnedEntity>> GetMany<TReturnedEntity>(
            string resourceRelativePath, string rootElementName) where TReturnedEntity : class, new()
        {

            return GetManyExecute<TReturnedEntity>(resourceRelativePath, null, rootElementName, null);
        }

        public RoiRestClientResponse<List<TReturnedEntity>> GetMany<TReturnedEntity>(
            string resourceRelativePath, Dictionary<string, string> queryStringParameters, string rootElementName)
            where TReturnedEntity : class, new()
        {
            return GetManyExecute<TReturnedEntity>(resourceRelativePath, queryStringParameters, rootElementName, null);
        }

        public RoiRestClientResponse<List<TReturnedEntity>> GetMany<TReturnedEntity>(
            string resourceRelativePath, Dictionary<string, string> queryStringParameters, Func<string, object> deserializeFromHttpResponse)
            where TReturnedEntity : class, new()
        {
            return GetManyExecute<TReturnedEntity>(resourceRelativePath, queryStringParameters, null, deserializeFromHttpResponse);
        }

        private RoiRestClientResponse<List<TReturnedEntity>> GetManyExecute<TReturnedEntity>(
            string resourceRelativePath, Dictionary<string, string> queryStringParameters, string rootElementName,
            Func<string, object> deserializeFromHttpResponse)
            where TReturnedEntity : class, new()
        {
            var request = GetBasicRequest(resourceRelativePath, Method.GET);

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
            restClientResponse.Content = response.Content;

            if (response.ResponseStatus == ResponseStatus.Error) //TODO: what about other status enums?
            {
                restClientResponse.Success = false;
                restClientResponse.ErrorMessage = response.ErrorMessage;
            }
            else
            {
                restClientResponse.Success = true;
            }
            if (response.Data == null)
            {
                if (deserializeFromHttpResponse != null)
                {
                    var deserializedContent = deserializeFromHttpResponse(restClientResponse.Content);
                    var typedObject = deserializedContent as List<TReturnedEntity>;
                    restClientResponse.ReturnedObject = typedObject;
                }
            }
            else
            {
                restClientResponse.ReturnedObject = response.Data;
            }

            restClientResponse.HttpStatusCode = (int)response.StatusCode;
            return restClientResponse;
        }

        public RoiRestClientResponse<TReturnedEntity> GetManyXml<TReturnedEntity>(
            string resourceRelativePath, string rootElementName) where TReturnedEntity : new()
        {

            return GetManyXmlExecute<TReturnedEntity>(resourceRelativePath, null, rootElementName);
        }

        public RoiRestClientResponse<TReturnedEntity> GetManyXml<TReturnedEntity>(
            string resourceRelativePath, Dictionary<string, string> queryStringParameters, string rootElementName)
            where TReturnedEntity : new()
        {
            return GetManyXmlExecute<TReturnedEntity>(resourceRelativePath, queryStringParameters, rootElementName);
        }

        private RoiRestClientResponse<TReturnedEntity> GetManyXmlExecute<TReturnedEntity>(
            string resourceRelativePath, Dictionary<string, string> queryStringParameters, string rootElementName)
            where TReturnedEntity : new()
        {
            var request = GetBasicRequest(resourceRelativePath, Method.GET);

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
            }
            restClientResponse.HttpStatusCode = (int)response.StatusCode;
            return restClientResponse;
        }

        public RoiRestClientResponse<TReturnedEntity> Post<TReturnedEntity>(
            string resourceRelativePath, object resourceToCreate, string rootElement)
            where TReturnedEntity : class, new()
        {
            return PostExecute<TReturnedEntity>(resourceRelativePath, resourceToCreate, rootElement);
        }

        private RoiRestClientResponse<TReturnedEntity> PostExecute<TReturnedEntity>(
            string resourceRelativePath, object resourceToCreate, string rootElement)
            where TReturnedEntity : class, new()
        {

            var request = GetBasicRequest(resourceRelativePath, Method.POST);

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

        public RoiRestClientResponse<TReturnedEntity> Post<TReturnedEntity>(
            string resourceRelativePath, Dictionary<string, string> postBodyParameters)
            where TReturnedEntity : class, new()
        {
            return PostExecute<TReturnedEntity>(resourceRelativePath, postBodyParameters);
        }

        private RoiRestClientResponse<TReturnedEntity> PostExecute<TReturnedEntity>(
            string resourceRelativePath, Dictionary<string, string> postBodyParameters)
            where TReturnedEntity : class, new()
        {

            var request = GetBasicRequest(resourceRelativePath, Method.POST);

            foreach (var postBodyParameter in postBodyParameters)
            {
                request.AddParameter(postBodyParameter.Key, postBodyParameter.Value);
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
                restClientResponse.Content = response.Content;
            }

            restClientResponse.HttpStatusCode = (int)response.StatusCode;
            return restClientResponse;
        }

        private static RestRequest GetBasicRequest(string resourceRelativePath, Method httpMethod)
        {
            var request = new RestRequest(httpMethod)
            {
                Resource = resourceRelativePath,
                RequestFormat = DataFormat
            };
            return request;

        }

        private class AuthenticatorTranslator : RestSharp.Authenticators.IAuthenticator
        {
            private IAuthenticator RoiAuthenticator { get; set; }

            public AuthenticatorTranslator(IAuthenticator roiAuthenticator)
            {
                RoiAuthenticator = roiAuthenticator;
            }

            public void Authenticate(RestSharp.IRestClient restSharpRestClient, RestSharp.IRestRequest restSharpRestRequest)
            {
                var clientTranslator = new RestClientTranslator(restSharpRestClient);
                var requestTranslator = new RestRequestTranslator(restSharpRestRequest);
                RoiAuthenticator.Authenticate(clientTranslator, requestTranslator);
            }
        }     
    }

    internal class RestRequestTranslator : IRestRequestTranslator
    {
        private RestSharp.IRestRequest RestSharpRestRequest { get; set; }


        public RestRequestTranslator(RestSharp.IRestRequest restSharpRestRequest)
        {
            RestSharpRestRequest = restSharpRestRequest;
        }

        public void AddHeader(string headerName, string headerValue)
        {
            RestSharpRestRequest.AddHeader(headerName, headerValue);
        }
    }

    internal class RestClientTranslator : IRestClientTranslator
    {
        private RestSharp.IRestClient RestSharpRestClient { get; set; }

        public RestClientTranslator(RestSharp.IRestClient restSharpRestClient)
        {
            RestSharpRestClient = restSharpRestClient;
        }
    }
}
