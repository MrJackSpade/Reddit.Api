﻿using System.Runtime.Serialization;

namespace Reddit.Api.Models.Api
{
    public enum CollapsedReasonKind
    {
        [EnumMember(Value = null)]
        None,

        [EnumMember(Value = "DELETED")]
        Deleted = 1,

        [EnumMember(Value = "LOW_SCORE")]
        LowScore = 2,

        [EnumMember(Value = "BLOCKED_AUTHOR")]
        BlockedAuthor = 3
    }
}