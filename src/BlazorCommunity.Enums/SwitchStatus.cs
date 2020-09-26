using System.ComponentModel;

namespace BlazorCommunity.Enums
{
    public enum SwitchStatus
    {
        [Description("否")]
        Normal = 0,

        [Description("是")]
        Deleted = 1
    }
}