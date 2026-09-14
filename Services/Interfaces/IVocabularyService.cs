using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface IVocabularyService
    {
        Task<List<VocabularyInfoResponse>> GetVocabulariesAsync(VocabularyFilterRequest request, int pageNumber, int pageSize, CancellationToken ct = default);
        Task<int> CountVocabulariesAsync(VocabularyFilterRequest request, CancellationToken ct = default);
        Task<VocabularyInfoResponse?> GetVocabularyByIdAsync(int vocabularyId, CancellationToken ct = default);
        Task<List<VocabularyInfoResponse>> GetVocabulariesByLessonIdAsync(int lessonId, CancellationToken ct = default);
        Task CreateVocabularyAsync(int lessonId, CreateVocabularyRequest request, CancellationToken ct = default);
        Task<bool> UpdateVocabularyAsync(int vocabularyId, UpdateVocabularyRequest request, CancellationToken ct = default);
        Task<bool> DeleteVocabularyAsync(int vocabularyId, CancellationToken ct = default);
    }
}