using System.ComponentModel;

namespace BlazorCommunity.Enums
{
    public enum ProjectType
    {
        [Description("Element")]
        Element,

        [Description("Element.Admin")]
        ElementAdmin,

        [Description("Element.Markdown")]
        ElementMarkdown
    }
}
