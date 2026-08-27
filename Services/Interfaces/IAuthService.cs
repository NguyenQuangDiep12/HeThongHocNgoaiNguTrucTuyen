using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequest request, CancellationToken ct);
        Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct);
    }
}
