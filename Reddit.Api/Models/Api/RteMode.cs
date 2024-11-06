using System.Runtime.Serialization;

namespace Reddit.Api.Models.Api
{
    public enum RteMode
    {
        [EnumMember(Value = null)]
        Undefined,

        [EnumMember(Value = "markdown")]
        Markdown
    }
}