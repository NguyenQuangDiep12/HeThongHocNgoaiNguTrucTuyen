using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests.Language;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface ILanguageService
    {
        Task<List<LanguageInfoResponse>> GetLanguagesAsync(LanguageFilterRequest request, int pageSize, int pageNumber, CancellationToken ct);
        Task<List<LanguageInfoResponse>> GetAllLanguagesAsync(CancellationToken ct);
        Task<int> CountLanguagesAsync(LanguageFilterRequest request, CancellationToken ct);
        Task<LanguageInfoResponse> GetLanguageByIdAsync(int Id, CancellationToken ct);
        Task CreateLanguagesAsync(CreateLanguageRequest request, CancellationToken ct);
        Task<bool> UpdateLanguagesAsync(int id, UpdateLanguageRequest request, CancellationToken ct);
        Task<bool> DeleteLanguagesAsync(int id, CancellationToken ct);
    }
}



