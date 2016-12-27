using System;

namespace Roi.Utilities.Rest
{
    public class RoiRestClientResponse
    {
        public bool Success { get; set; }
        public Uri ResourceUri { get; set; }
        public string ResourceParsedId { get; set; }
        public int HttpStatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public string Content { get; set; }
//        public List<Cookie> returnedCookies { get; set; }
    }

    public class RoiRestClientResponse<T> : RoiRestClientResponse
    {
        public T ReturnedObject { get; set; }
    }
}
