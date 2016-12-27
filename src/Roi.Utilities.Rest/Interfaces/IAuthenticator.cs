namespace Roi.Utilities.Rest
{
    public interface IAuthenticator
    {
        void Authenticate(IRestClientTranslator client, IRestRequestHeaderHelper request);
    }

    public interface IRestClientTranslator
    {
        
    }

    public interface IRestRequestHeaderHelper
    {
        void AddHeader(string headerName, string headerValue);
    }
}
