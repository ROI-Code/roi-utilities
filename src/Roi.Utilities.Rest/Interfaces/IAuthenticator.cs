namespace Roi.Utilities.Rest
{
    public interface IAuthenticator
    {
        void Authenticate(IRestClientTranslator client, IRestRequestTranslator request);
    }

    public interface IRestClientTranslator
    {
        
    }

    public interface IRestRequestTranslator
    { }
}
