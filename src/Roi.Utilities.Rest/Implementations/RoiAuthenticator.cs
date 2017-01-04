namespace Roi.Utilities.Rest.Implementations
{
    public class RoiAuthenticator : IAuthenticator
    {
        private string _bearerToken;
        private static readonly string AuthorizationHeaderName = "Authorization";

        public RoiAuthenticator(string bearerToken)
        {
            _bearerToken = bearerToken;
        }

        public void Authenticate(IRestRequestHeaderHelper headerHelper)
        {
            headerHelper.AddHeader(AuthorizationHeaderName, $"Bearer {_bearerToken}");
        }
    }
}
