using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Reddit.Api.Exceptions
{
    internal class EntityNotFoundException : RemoteException
    {
        public EntityNotFoundException(string url, string content) : base(url, content, HttpStatusCode.NotFound)
        {
        }
    }
}
