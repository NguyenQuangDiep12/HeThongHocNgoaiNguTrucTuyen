using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<List<QuestionInfoResponse>> GetQuestionsAsync(
            int pageSize,
            int pageNumber,
            QuestionRequest request,
            CancellationToken ct);

        Task<int> CountQuestionsAsync(
            QuestionRequest request,
            CancellationToken ct);

        Task<QuestionInfoResponse?> GetQuestionByIdAsync(
            int id,
            CancellationToken ct);

        Task CreateQuestionAsync(
            QuestionRequest request,
            CancellationToken ct);

        Task<bool> UpdateQuestionAsync(
            int id,
            QuestionRequest request,
            CancellationToken ct);

        Task<bool> DeleteQuestionAsync(
            int id,
            CancellationToken ct);
    }
}