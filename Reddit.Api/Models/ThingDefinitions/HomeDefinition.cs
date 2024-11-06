using Reddit.Api.Models.Requests;

namespace Reddit.Api.Models.ThingDefinitions
{
    public class HomeDefinition : SubRedditDefinition
    {
        public override string DisplayName => "Home";

        public override ApiEndpointDefinition EndpointDefinition => new("");

        public HomeDefinition() : base("")
        {
        }
    }
}