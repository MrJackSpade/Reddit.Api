using System.Runtime.Serialization;

namespace Reddit.Api.Models.Api
{
    public enum DistinguishedKind
    {
        [EnumMember(Value = null)]
        None,

        [EnumMember(Value = "moderator")]
        Moderator,

        [EnumMember(Value = "admin")]
        Admin
    }
}