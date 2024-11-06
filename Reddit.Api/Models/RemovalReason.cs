using System.Runtime.Serialization;

namespace Reddit.Api.Models
{
    public enum RemovalReason
    {
        [EnumMember(Value = null)]
        None,

        [EnumMember(Value = "legal")]
        Legal
    }
}