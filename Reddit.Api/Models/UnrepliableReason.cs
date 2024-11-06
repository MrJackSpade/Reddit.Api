using System.Runtime.Serialization;

namespace Reddit.Api.Models
{
    public enum UnrepliableReason
    {
        [EnumMember(Value = null)]
        None = 0,

        [EnumMember(Value = "NEAR_BLOCKER")]
        NearBlocker = 1,

        [EnumMember(Value = "NEAR_BLOCKED")]
        NearBlocked = 2,
    }
}