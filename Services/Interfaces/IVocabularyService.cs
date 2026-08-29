using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface IVocabularyService
    {
        Task<List<VocabularyInfoResponse>> GetVocabulariesAsync(string? word, int? lessonId, int pageNumber, int pageSize, CancellationToken ct);
        Task<int> CountVocabulariesAsync(string? word, int? lessonId, CancellationToken ct);
        Task<VocabularyInfoResponse?> GetVocabularyByIdAsync(int id, CancellationToken ct);
        Task CreateVocabularyAsync(VocabularyRequest request, CancellationToken ct);
        Task<bool> UpdateVocabularyAsync(int id, VocabularyRequest request, CancellationToken ct);
        Task<bool> DeleteVocabularyAsync(int id, CancellationToken ct);
    }
}