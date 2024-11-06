using Reddit.Api.Models.Api;

namespace Reddit.Api.Interfaces
{
    public interface IMore
    {
        List<string> ChildNames { get; }

        int? Count { get; }

        ApiThing Parent { get; }
    }
}