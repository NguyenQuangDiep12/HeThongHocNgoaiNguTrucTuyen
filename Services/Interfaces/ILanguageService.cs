using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface ILanguageService
    {
        Task<List<LanguageInfoResponse>> GetLanguagesAsync(LanguageFilterRequest request ,int pageSize, int pageNumber,CancellationToken ct = default);
        Task<int> CountLanguagesAsync(LanguageFilterRequest request, CancellationToken ct = default);
        Task<List<LanguageInfoResponse>> GetAllLanguagesAsync(CancellationToken ct = default);
        Task<LanguageInfoResponse> GetLanguageByIdAsync(int Id, CancellationToken ct = default);
        Task CreateLanguagesAsync(CreateLanguageRequest request, CancellationToken ct = default);
        Task<bool> UpdateLanguagesAsync(int id, UpdateLanguageRequest request, CancellationToken ct = default);
        Task<bool> DeleteLanguagesAsync(int id, CancellationToken ct = default);
    }
}



