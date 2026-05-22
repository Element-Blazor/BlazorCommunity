namespace ElementCommunity.Domain;

public sealed record CommunitySnapshot(
    IReadOnlyList<Forum> Forums,
    IReadOnlyList<Topic> Topics,
    IReadOnlyList<Reply> Replies,
    IReadOnlyList<UserProfile> Users,
    IReadOnlyList<string> SuggestedTags);

public sealed record Forum(
    int Id,
    string Slug,
    string Name,
    string Summary,
    string Accent,
    int TopicCount,
    int ReplyCount,
    DateTimeOffset LastActivityAt);

public sealed record Topic(
    int Id,
    string ForumSlug,
    string Title,
    string Excerpt,
    string AuthorId,
    TopicKind Kind,
    TopicStatus Status,
    int ViewCount,
    int ReplyCount,
    DateTimeOffset PublishedAt,
    DateTimeOffset LastActivityAt,
    IReadOnlyList<string> Tags);

public sealed record Reply(
    int Id,
    int TopicId,
    string AuthorId,
    string Content,
    int Floor,
    DateTimeOffset PublishedAt);

public sealed record UserProfile(
    string Id,
    string DisplayName,
    string Role,
    string AvatarText,
    string Bio,
    int Reputation,
    int TopicCount,
    int ReplyCount);

public enum TopicKind
{
    Discussion,
    Question,
    Showcase,
    Guide,
    Release
}

public enum TopicStatus
{
    Normal,
    Pinned,
    Featured,
    Closed
}
