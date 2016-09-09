﻿namespace Roi.Utilities.Rest
{
    public interface IAuthenticator
    {
        void Authenticate(IRestClient client, IRestRequest request);
    }
}
