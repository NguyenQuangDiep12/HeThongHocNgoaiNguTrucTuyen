using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface ILanguageService
    {
        Task<List<LanguageInfoResponse>> GetLanguagesAsync(int pageSize, int pageNumber, string? name,CancellationToken ct);
        Task<LanguageInfoResponse> GetLanguageByIdAsync(int Id, CancellationToken ct);
        Task CreateLanguagesAsync(LanguageRequest request, CancellationToken ct);
        Task<bool> UpdateLanguagesAsync(int id, LanguageRequest request, CancellationToken ct);
        Task<bool> DeleteLanguagesAsync(int id, CancellationToken ct);
    }
}
