using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface ITopicService
    {
        Task<List<TopicInfoResponse>> GetTopicsAsync(int pageSize, int pageNumber, TopicRequest request, CancellationToken ct);
        Task<int> CountTopicsAsync(TopicRequest request, CancellationToken ct);
        Task<TopicInfoResponse?> GetTopicByIdAsync(int id, CancellationToken ct);
        Task<List<string>> GetLevelsAsync(CancellationToken ct);
        Task CreateTopicAsync(TopicRequest request, CancellationToken ct);
        Task<bool> UpdateTopicAsync(int id, TopicRequest request, CancellationToken ct);
        Task<bool> DeleteTopicAsync(int id, CancellationToken ct);
    }
}
