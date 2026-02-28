using System.Text.Json.Serialization;

namespace Reddit.Api.Models.Enums
{
    /// <summary>
    /// Type of moderation action.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ModActionType>))]
    public enum ModActionType
    {
        [JsonStringEnumMemberName("banuser")]
        BanUser,

        [JsonStringEnumMemberName("unbanuser")]
        UnbanUser,

        [JsonStringEnumMemberName("removelink")]
        RemoveLink,

        [JsonStringEnumMemberName("approvelink")]
        ApproveLink,

        [JsonStringEnumMemberName("removecomment")]
        RemoveComment,

        [JsonStringEnumMemberName("approvecomment")]
        ApproveComment,

        [JsonStringEnumMemberName("addmoderator")]
        AddModerator,

        [JsonStringEnumMemberName("removemoderator")]
        RemoveModerator,

        [JsonStringEnumMemberName("addcontributor")]
        AddContributor,

        [JsonStringEnumMemberName("removecontributor")]
        RemoveContributor,

        [JsonStringEnumMemberName("editmoderator")]
        EditModerator,

        [JsonStringEnumMemberName("editcontributor")]
        EditContributor,

        [JsonStringEnumMemberName("muteuser")]
        MuteUser,

        [JsonStringEnumMemberName("unmuteuser")]
        UnmuteUser,

        [JsonStringEnumMemberName("locking")]
        Locking,

        [JsonStringEnumMemberName("unlocking")]
        Unlocking,

        [JsonStringEnumMemberName("sticky")]
        Sticky,

        [JsonStringEnumMemberName("unsticky")]
        Unsticky,

        [JsonStringEnumMemberName("setsubredditflags")]
        SetSubredditFlags,

        [JsonStringEnumMemberName("wikibanned")]
        WikiBanned,

        [JsonStringEnumMemberName("wikicontributor")]
        WikiContributor,

        [JsonStringEnumMemberName("wikiunbanned")]
        WikiUnbanned,

        [JsonStringEnumMemberName("wikipagelisted")]
        WikiPageListed,

        [JsonStringEnumMemberName("wikipageunlisted")]
        WikiPageUnlisted,

        [JsonStringEnumMemberName("modmail_mute")]
        ModmailMute,

        [JsonStringEnumMemberName("modmail_unmute")]
        ModmailUnmute,

        [JsonStringEnumMemberName("spamlink")]
        SpamLink,

        [JsonStringEnumMemberName("spamcomment")]
        SpamComment,

        [JsonStringEnumMemberName("editflair")]
        EditFlair,

        [JsonStringEnumMemberName("distinguish")]
        Distinguish,

        [JsonStringEnumMemberName("marknsfw")]
        MarkNsfw,

        [JsonStringEnumMemberName("markoriginalcontent")]
        MarkOriginalContent
    }
}
