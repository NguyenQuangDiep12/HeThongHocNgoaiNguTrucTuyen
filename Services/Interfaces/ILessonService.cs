using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface ILessonService
    {
        Task<List<LessonInfoResponse>> GetLessonsAsync(
            string? title,
            int? topicId,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<int> CountLessonsAsync(
            string? title,
            int? topicId,
            CancellationToken ct = default);

        Task<LessonInfoResponse?> GetLessonByIdAsync(
            int id,
            CancellationToken ct = default);

        Task CreateLessonAsync(
            LessonRequest request,
            CancellationToken ct = default);

        Task<bool> UpdateLessonAsync(
            int id,
            LessonRequest request,
            CancellationToken ct = default);

        Task<bool> DeleteLessonAsync(
            int id,
            CancellationToken ct = default);
    }
}