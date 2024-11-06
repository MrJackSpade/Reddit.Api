using System.Net;

namespace Reddit.Api.Exceptions
{
    public class RemoteException(string url, string content, HttpStatusCode httpStatusCode) : Exception(content)
    {
        public HttpStatusCode HttpStatusCode { get; } = httpStatusCode;

        public string Url { get; } = url;
    }
}