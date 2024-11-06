﻿using System.Runtime.Serialization;

namespace Reddit.Api.Models
{
    public enum UpvoteState
    {
        [EnumMember(Value = null)]
        None,

        [EnumMember(Value = "true")]
        Upvote,

        [EnumMember(Value = "false")]
        Downvote
    }
}