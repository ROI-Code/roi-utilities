namespace Roi.Utilities.Rest
{
    public interface IAuthenticator
    {
        void Authenticate(IRestRequestHeaderHelper headerHelper);
    }

    public interface IRestRequestHeaderHelper
    {
        void AddHeader(string headerName, string headerValue);
    }
}
