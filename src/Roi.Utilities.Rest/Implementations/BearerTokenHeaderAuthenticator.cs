namespace Roi.Utilities.Rest.Implementations
{
    public class BearerTokenHeaderAuthenticator : IAuthenticator
    {
        private string _bearerToken;
        private string _headerName;

        public BearerTokenHeaderAuthenticator(string headerName, string bearerToken)
        {
            _bearerToken = bearerToken;
            _headerName = headerName;
        }

        public void Authenticate(IRestRequestHeaderHelper headerHelper)
        {
            headerHelper.AddHeader(_headerName, $"Bearer {_bearerToken}");
        }
    }
}
