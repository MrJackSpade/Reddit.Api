using System.Runtime.Serialization;

namespace Reddit.Api.Models
{
    public enum InboxSort
    {
        [EnumMember(Value = null)]
        Undefined = 0,

        [EnumMember(Value = "inbox")]
        Inbox = 1,

        [EnumMember(Value = "unread")]
        Unread = 2,

        [EnumMember(Value = "sent")]
        Sent = 3
    }
}