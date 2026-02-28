using Reddit.Api.Models.Enums;

namespace Reddit.Api.Models.Json.Flair
{
    /// <summary>
    /// Request parameters for POST /api/flairtemplate_v2.
    /// </summary>
    public class FlairTemplateRequest
    {
        /// <summary>
        /// Allowable content: all, emoji, text.
        /// </summary>
        public string? AllowableContent { get; set; }

        /// <summary>
        /// Background color (hex).
        /// </summary>
        public string? BackgroundColor { get; set; }

        /// <summary>
        /// CSS class.
        /// </summary>
        public string? CssClass { get; set; }

        /// <summary>
        /// Flair template ID to edit (omit for new).
        /// </summary>
        public string? FlairTemplateId { get; set; }

        /// <summary>
        /// Flair type: USER_FLAIR or LINK_FLAIR.
        /// </summary>
        public FlairType FlairType { get; set; } = FlairType.Link;

        /// <summary>
        /// Maximum number of emojis.
        /// </summary>
        public int? MaxEmojis { get; set; }

        /// <summary>
        /// Whether only mods can use this flair.
        /// </summary>
        public bool? ModOnly { get; set; }

        /// <summary>
        /// Override CSS with new colors.
        /// </summary>
        public bool? OverrideCss { get; set; }

        /// <summary>
        /// Flair text.
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Text color: light or dark.
        /// </summary>
        public string? TextColor { get; set; }

        /// <summary>
        /// Whether text is editable by users.
        /// </summary>
        public bool? TextEditable { get; set; }
    }

    /// <summary>
    /// Request parameters for POST /api/selectflair.
    /// </summary>
    public class SelectFlairRequest
    {
        /// <summary>
        /// Background color (hex).
        /// </summary>
        public string? BackgroundColor { get; set; }

        /// <summary>
        /// CSS class for flair.
        /// </summary>
        public string? CssClass { get; set; }

        /// <summary>
        /// Flair template ID.
        /// </summary>
        public string? FlairTemplateId { get; set; }

        /// <summary>
        /// Fullname of the link if setting link flair.
        /// </summary>
        public string? Link { get; set; }

        /// <summary>
        /// Username if setting user flair.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Custom flair text.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Text color: light or dark.
        /// </summary>
        public string? TextColor { get; set; }
    }
}
