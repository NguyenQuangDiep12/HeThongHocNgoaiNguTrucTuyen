using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface ITopicService
    {
        Task<List<TopicInfoResponse>> GetTopicsAsync(int languageId, TopicFilterRequest request, int pageSize, int pageNumber, CancellationToken ct);
        Task<int> CountTopicsAsync(int languageId, TopicFilterRequest request, CancellationToken ct);
        Task<List<TopicInfoResponse>> GetTopicsByLanguageIdAsync(int languageId, CancellationToken ct);
        Task<List<string>> GetLevelsByLanguageIdAsync(int languageId, CancellationToken ct);
        Task<TopicInfoResponse?> GetTopicByIdAsync(int id, CancellationToken ct);
        Task CreateTopicAsync(CreateTopicRequest request, CancellationToken ct);
        Task<bool> UpdateTopicAsync(int id, UpdateTopicRequest request, CancellationToken ct);
        Task<bool> DeleteTopicAsync(int id, CancellationToken ct);
    }
}