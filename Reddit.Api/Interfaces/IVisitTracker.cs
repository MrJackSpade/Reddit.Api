using Reddit.Api.Models.Api;

namespace Reddit.Api.Interfaces
{
    public interface IVisitTracker
    {
        bool HasVisited(ApiThing thing);

        void Visit(ApiThing thing);
    }
}