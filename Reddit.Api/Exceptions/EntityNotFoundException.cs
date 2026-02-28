using System.Net;

namespace Reddit.Api.Exceptions
{
    internal class EntityNotFoundException : RemoteException
    {
        public EntityNotFoundException(string url, string content) : base(url, content, HttpStatusCode.NotFound)
        {
        }
    }
}
