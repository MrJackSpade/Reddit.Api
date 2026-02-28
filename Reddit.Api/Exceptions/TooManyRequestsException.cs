using System.Net;

namespace Reddit.Api.Exceptions
{
    internal class TooManyRequestsException : RemoteException
    {
        public TooManyRequestsException(string url, string content) : base(url, content, HttpStatusCode.TooManyRequests)
        {
        }
    }
}
