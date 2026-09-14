using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests.Topic;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface ITopicService
    {
        Task<List<TopicInfoResponse>> GetTopicsAsync(int languageId, TopicFilterRequest request, int pageNumber, int pageSize, CancellationToken ct);
        Task<List<TopicInfoResponse>> GetAllTopicsAsync(int languageId, CancellationToken ct);
        Task<int> CountTopicsAsync(int languageId, TopicFilterRequest request, CancellationToken ct);
        Task<TopicInfoResponse?> GetTopicByIdAsync(int id, CancellationToken ct);
        Task<List<string>> GetLevelsByLanguageIdAsync(int languageId, CancellationToken ct);
        Task CreateTopicAsync(CreateTopicRequest request, CancellationToken ct);
        Task<bool> UpdateTopicAsync(int id, UpdateTopicRequest request, CancellationToken ct);
        Task<bool> DeleteTopicAsync(int id, CancellationToken ct);
    }
}