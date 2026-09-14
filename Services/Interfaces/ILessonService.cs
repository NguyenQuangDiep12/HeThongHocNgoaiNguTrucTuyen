using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests.Lesson;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface ILessonService
    {
        Task<List<LessonInfoResponse>> GetLessonsAsync(int topicId, LessonFilterRequest request, int pageNumber, int pageSize, CancellationToken ct);
        Task<List<LessonInfoResponse>> GetAllLessonsAsync(int topicId, CancellationToken ct);
        Task<int> CountLessonsAsync(int topicId, LessonFilterRequest request, CancellationToken ct);
        Task<LessonInfoResponse?> GetLessonByIdAsync(int id, CancellationToken ct);
        Task CreateLessonAsync(CreateLessonRequest request, CancellationToken ct);
        Task<bool> UpdateLessonAsync(int id, UpdateLessonRequest request, CancellationToken ct);
        Task<bool> DeleteLessonAsync(int id, CancellationToken ct);
    }
}