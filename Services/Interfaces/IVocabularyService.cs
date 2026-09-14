using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests.Vocabulary;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface IVocabularyService
    {
        Task<List<VocabularyInfoResponse>> GetVocabulariesAsync(int lessonId, VocabularyFilterRequest request, int pageNumber, int pageSize, CancellationToken ct);
        Task<int> CountVocabulariesAsync(int lessonId, VocabularyFilterRequest request, CancellationToken ct);
        Task<VocabularyInfoResponse?> GetVocabularyByIdAsync(int id, CancellationToken ct);
        Task CreateVocabularyAsync(CreateVocabularyRequest request, CancellationToken ct);
        Task<bool> UpdateVocabularyAsync(int id, UpdateVocabularyRequest request, CancellationToken ct);
        Task<bool> DeleteVocabularyAsync(int id, CancellationToken ct);
    }
}