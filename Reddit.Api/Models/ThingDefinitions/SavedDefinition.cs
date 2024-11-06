﻿using Reddit.Api.Models.Requests;

namespace Reddit.Api.Models.ThingDefinitions
{
    public class SavedDefinition : ThingDefinition
    {
        public override Enum? DefaultSort => null;

        public override bool FilteredByDefault => false;

        public override ApiEndpointDefinition EndpointDefinition => new("user/%USER%/saved");

        public override ThingKind Kind => ThingKind.Listing;

        public SavedDefinition() : base("Saved", '\0')
        {
        }
    }
}