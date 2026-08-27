using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;

namespace HeThongHocNgoaiNguTrucTuyen.Services
{
    public class LanguageService : ILanguageService
    {
        public Task<List<LanguageInfoResponse>> GetLanguagesAsync(int pageSize, int pageNumber, string name,CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public Task<bool> CreateLanguagesAsync(LanguageRequest request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public Task<LanguageInfoResponse> UpdateLanguagesAsync(int id, LanguageRequest request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteLanguagesAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
