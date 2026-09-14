using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface ILessonService
    {
        Task<List<LessonInfoResponse>> GetLessonsAsync(LessonFilterRequest request, int pageNumber, int pageSize, CancellationToken ct = default);
        Task<int> CountLessonsAsync(LessonFilterRequest request, CancellationToken ct = default);
        Task<List<LessonInfoResponse>> GetAllLessonsAsync(CancellationToken ct = default);
        Task<List<LessonInfoResponse>> GetLessonsByTopicIdAsync(int topicId, CancellationToken ct = default);
        Task<LessonInfoResponse?> GetLessonByIdAsync(int id, CancellationToken ct = default);
        Task CreateLessonAsync(CreateLessonRequest request, CancellationToken ct = default);
        Task<bool> UpdateLessonAsync(int id, UpdateLessonRequest request, CancellationToken ct = default);
        Task<bool> DeleteLessonAsync(int id, CancellationToken ct = default);
    }
}