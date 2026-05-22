using Element;
using ElementCommunity.Domain;

namespace ElementCommunity.Components;

public static class ComponentTagMapper
{
    public static TagType ToTagType(this TopicKind kind) => kind switch
    {
        TopicKind.Question => TagType.Warning,
        TopicKind.Showcase => TagType.Success,
        TopicKind.Guide => TagType.Info,
        TopicKind.Release => TagType.Danger,
        _ => TagType.Info
    };

    public static TagType ToTagType(this TopicStatus status) => status switch
    {
        TopicStatus.Pinned => TagType.Danger,
        TopicStatus.Featured => TagType.Success,
        TopicStatus.Closed => TagType.Info,
        _ => TagType.Info
    };

    public static string ToLabel(this TopicKind kind) => kind switch
    {
        TopicKind.Question => "问答",
        TopicKind.Showcase => "案例",
        TopicKind.Guide => "指南",
        TopicKind.Release => "发布",
        _ => "讨论"
    };

    public static string ToLabel(this TopicStatus status) => status switch
    {
        TopicStatus.Pinned => "置顶",
        TopicStatus.Featured => "精华",
        TopicStatus.Closed => "已关闭",
        _ => string.Empty
    };
}
