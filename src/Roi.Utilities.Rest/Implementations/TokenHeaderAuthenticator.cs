namespace Roi.Utilities.Rest.Implementations
{
    public class TokenHeaderAuthenticator : IAuthenticator
    {
        private string _bearerToken;
        private string _headerName;
        private string _tokenName;

        public TokenHeaderAuthenticator(string tokenName, string headerName, string bearerToken)
        {
            _bearerToken = bearerToken;
            _headerName = headerName;
            _tokenName = tokenName;
        }

        public void Authenticate(IRestRequestHeaderHelper headerHelper)
        {
            headerHelper.AddHeader(_headerName, $"{_tokenName} {_bearerToken}");
        }
    }
}
