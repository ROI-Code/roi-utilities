using System;
using System.Collections.Generic;

namespace Roi.Utilities.Rest
{
    public interface IRestClient
    {
        RoiRestClientResponse<TReturnedEntity> GetSingle<TReturnedEntity>(
            ResponseFormat responseFormat, string resourceRelativePath) where TReturnedEntity : new();

        RoiRestClientResponse<TReturnedEntity> GetSingle<TReturnedEntity>(
            ResponseFormat responseFormat, string resourceRelativePath, string rootElementName)
            where TReturnedEntity : new();

        RoiRestClientResponse<List<TReturnedEntity>> GetMany<TReturnedEntity>(
            ResponseFormat responseFormat, string resourceRelativePath, string rootElementName)
            where TReturnedEntity : class, new();

        RoiRestClientResponse<List<TReturnedEntity>> GetMany<TReturnedEntity>(
            ResponseFormat responseFormat, string resourceRelativePath, Dictionary<string, string> queryStringParameters, 
            string rootElementName)
            where TReturnedEntity : class, new();

        RoiRestClientResponse<List<TReturnedEntity>> GetMany<TReturnedEntity>(
            ResponseFormat responseFormat, string resourceRelativePath, Dictionary<string, string> queryStringParameters, 
            Func<string, object> deserializeFromHttpResponse)
            where TReturnedEntity : class, new();

        RoiRestClientResponse<TReturnedEntity> GetManyXml<TReturnedEntity>(
            ResponseFormat responseFormat, string resourceRelativePath, string rootElementName)
            where TReturnedEntity : new();

        RoiRestClientResponse<TReturnedEntity> GetManyXml<TReturnedEntity>(
            ResponseFormat responseFormat, string resourceRelativePath, Dictionary<string, string> queryStringParameters, string rootElementName)
            where TReturnedEntity : new();

        RoiRestClientResponse<TReturnedEntity> Post<TReturnedEntity>(
            ResponseFormat responseFormat, string resourceRelativePath, object resourceToCreate)
            where TReturnedEntity : class, new();

        RoiRestClientResponse<TReturnedEntity> Post<TReturnedEntity>(
            ResponseFormat responseFormat, string resourceRelativePath, object resourceToCreate, string rootElement)
            where TReturnedEntity : class, new();

        RoiRestClientResponse<TReturnedEntity> Post<TReturnedEntity>(
            ResponseFormat responseFormat, string resourceRelativePath, Dictionary<string, string> postBodyParameters)
            where TReturnedEntity : class, new();
    }
}
