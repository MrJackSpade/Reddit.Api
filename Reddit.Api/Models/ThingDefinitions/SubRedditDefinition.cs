using Reddit.Api.Models.Requests;

namespace Reddit.Api.Models.ThingDefinitions
{
    public class SubRedditDefinition(string name) : ThingDefinition(name, 'r')
    {
        public override Enum DefaultSort => ApiPostSort.Hot;

        public override ApiEndpointDefinition EndpointDefinition => new("r/" + Name);

        public override ThingKind Kind => ThingKind.Subreddit;
    }
}