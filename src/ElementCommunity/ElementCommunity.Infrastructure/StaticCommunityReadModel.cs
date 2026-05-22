using ElementCommunity.Domain;

namespace ElementCommunity.Infrastructure;

public sealed class StaticCommunityReadModel : ICommunityReadModel
{
    private static readonly CommunitySnapshot Snapshot = CreateSnapshot();

    public CommunitySnapshot GetSnapshot() => Snapshot;

    public Topic? GetTopic(int id) => Snapshot.Topics.FirstOrDefault(topic => topic.Id == id);

    public UserProfile? GetUser(string id) => Snapshot.Users.FirstOrDefault(user => user.Id == id);

    public IReadOnlyList<Reply> GetReplies(int topicId) =>
        Snapshot.Replies
            .Where(reply => reply.TopicId == topicId)
            .OrderBy(reply => reply.Floor)
            .ToArray();

    private static CommunitySnapshot CreateSnapshot()
    {
        var users = new[]
        {
            new UserProfile("u-elena", "Elena", "Maintainer", "E", "Element-Blazor 组件维护与可访问性审阅。", 1280, 42, 318),
            new UserProfile("u-chen", "Chen", "版主", "C", "关注 Blazor Server、SSR 与社区运营体验。", 864, 35, 166),
            new UserProfile("u-moss", "Moss", "Contributor", "M", "喜欢把真实业务流整理成可复用组件。", 512, 18, 92),
            new UserProfile("u-river", "River", "Member", "R", "正在从 Element Plus 迁移设计系统。", 226, 8, 41)
        };

        var forums = new[]
        {
            new Forum(1, "general", "综合讨论", "Element-Blazor 使用经验、升级路线与社区公告。", "#409eff", 128, 846, new DateTimeOffset(2026, 5, 22, 9, 25, 0, TimeSpan.FromHours(8))),
            new Forum(2, "components", "组件问答", "Button、Form、Table、Dialog 等组件实践问题。", "#67c23a", 214, 1390, new DateTimeOffset(2026, 5, 22, 10, 10, 0, TimeSpan.FromHours(8))),
            new Forum(3, "showcase", "案例展示", "业务模板、后台页面、主题方案与集成案例。", "#e6a23c", 73, 308, new DateTimeOffset(2026, 5, 21, 18, 40, 0, TimeSpan.FromHours(8))),
            new Forum(4, "releases", "版本发布", "2.14 主线、破坏性变更、迁移说明与路线图。", "#f56c6c", 38, 162, new DateTimeOffset(2026, 5, 21, 14, 10, 0, TimeSpan.FromHours(8)))
        };

        var topics = new[]
        {
            new Topic(1001, "releases", "Element-Blazor 2.14 社区主线如何跟进 Element Plus?", "整理 2.14 组件命名、主题 token、文档示例与迁移检查清单。", "u-elena", TopicKind.Release, TopicStatus.Pinned, 4820, 36, new DateTimeOffset(2026, 5, 21, 10, 0, 0, TimeSpan.FromHours(8)), new DateTimeOffset(2026, 5, 22, 10, 18, 0, TimeSpan.FromHours(8)), new[] { "2.14", "Element Plus", "Roadmap" }),
            new Topic(1002, "components", "ElTable 的筛选区应该放在表格内还是页面工具栏?", "我们正在重构管理后台模板，希望确认更符合 Element Plus 的密度与交互。", "u-chen", TopicKind.Question, TopicStatus.Featured, 1380, 18, new DateTimeOffset(2026, 5, 20, 16, 30, 0, TimeSpan.FromHours(8)), new DateTimeOffset(2026, 5, 22, 9, 55, 0, TimeSpan.FromHours(8)), new[] { "ElTable", "Form", "UX" }),
            new Topic(1003, "showcase", "用 ElCard 和 ElTag 搭一个组件总览页面", "这是一套轻量业务首页草图，第一屏直接展示版块和主题流。", "u-moss", TopicKind.Showcase, TopicStatus.Normal, 920, 11, new DateTimeOffset(2026, 5, 19, 20, 15, 0, TimeSpan.FromHours(8)), new DateTimeOffset(2026, 5, 21, 20, 12, 0, TimeSpan.FromHours(8)), new[] { "ElCard", "ElTag", "Layout" }),
            new Topic(1004, "general", "Blazor Web App 模式下 Element 静态资源加载顺序", "记录 fix.css、index.css、theme.css 和 dom.js 的接入顺序。", "u-river", TopicKind.Guide, TopicStatus.Normal, 740, 9, new DateTimeOffset(2026, 5, 18, 11, 5, 0, TimeSpan.FromHours(8)), new DateTimeOffset(2026, 5, 21, 9, 2, 0, TimeSpan.FromHours(8)), new[] { "Host", "Static Assets", "Blazor" }),
            new Topic(1005, "components", "ElInput Textarea 在发帖页的最小高度建议", "希望发帖页保持工具型密度，同时给 Markdown 内容足够的输入空间。", "u-elena", TopicKind.Discussion, TopicStatus.Closed, 510, 6, new DateTimeOffset(2026, 5, 17, 15, 45, 0, TimeSpan.FromHours(8)), new DateTimeOffset(2026, 5, 20, 22, 26, 0, TimeSpan.FromHours(8)), new[] { "ElInput", "Markdown" })
        };

        var replies = new[]
        {
            new Reply(1, 1001, "u-chen", "建议先冻结旧项目写法，把新主线的路由、命名和主题 token 一次性定下来。", 1, new DateTimeOffset(2026, 5, 21, 11, 20, 0, TimeSpan.FromHours(8))),
            new Reply(2, 1001, "u-moss", "示例代码可以全部使用 El* 命名，后续文档站也能直接复用这些片段。", 2, new DateTimeOffset(2026, 5, 21, 13, 45, 0, TimeSpan.FromHours(8))),
            new Reply(3, 1001, "u-river", "启动脚本最好只跑新 Host，避免大家误进旧社区启动链路。", 3, new DateTimeOffset(2026, 5, 22, 10, 18, 0, TimeSpan.FromHours(8))),
            new Reply(4, 1002, "u-elena", "搜索表单建议放在页面工具栏，表格卡片只承担数据展示和分页。", 1, new DateTimeOffset(2026, 5, 22, 9, 55, 0, TimeSpan.FromHours(8)))
        };

        return new CommunitySnapshot(
            forums,
            topics.OrderByDescending(topic => topic.Status == TopicStatus.Pinned).ThenByDescending(topic => topic.LastActivityAt).ToArray(),
            replies,
            users,
            new[] { "Element Plus", "ElButton", "ElInput", "ElTable", "Blazor Web App", "主题变量" });
    }
}
