using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface ITestService
    {
        Task<List<TestInfoResponse>> GetTestsAsync(
            int pageSize,
            int pageNumber,
            TestRequest? request,
            CancellationToken ct);

        Task<int> CountTestsAsync(
            TestRequest? request,
            CancellationToken ct);

        Task<TestInfoResponse?> GetTestByIdAsync(
            int id,
            CancellationToken ct);

        Task CreateTestAsync(
            TestRequest request,
            CancellationToken ct);

        Task<bool> UpdateTestAsync(
            int id,
            TestRequest request,
            CancellationToken ct);

        Task<bool> DeleteTestAsync(
            int id,
            CancellationToken ct);
    }
}