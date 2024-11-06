using Reddit.Api.Models.Requests;

namespace Reddit.Api.Models.ThingDefinitions
{
    public class MessageDefinition() : ThingDefinition("Messages", '\0')
    {
        public override Enum DefaultSort => InboxSort.Undefined;

        public override bool FilteredByDefault => false;

        public override ApiEndpointDefinition EndpointDefinition => new("message");

        public override ThingKind Kind => ThingKind.Message;
    }
}