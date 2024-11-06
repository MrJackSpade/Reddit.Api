using Reddit.Api.Models.Requests;

namespace Reddit.Api.Models.ThingDefinitions
{
    public class UserDefinition(string name) : ThingDefinition(name, 'u')
    {
        public override Enum DefaultSort => UserProfileSort.New;

        public override bool FilteredByDefault => false;

        public override ApiEndpointDefinition EndpointDefinition => new($"/user/{Name}");

        public override ThingKind Kind => ThingKind.Account;
    }
}