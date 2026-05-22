using ElementCommunity.Domain;

namespace ElementCommunity.Infrastructure;

public interface ICommunityReadModel
{
    CommunitySnapshot GetSnapshot();

    Topic? GetTopic(int id);

    UserProfile? GetUser(string id);

    IReadOnlyList<Reply> GetReplies(int topicId);
}
