using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;

namespace HeThongHocNgoaiNguTrucTuyen.Services.Interfaces
{
    public interface ITestService
    {
        Task<List<TestInfoResponse>> GetTestsAsync(int pageSize, int pageNumber, TestFilterRequest request, CancellationToken ct);
        Task<int> CountTestsAsync(TestFilterRequest request, CancellationToken ct);
        Task<TestInfoResponse?> GetTestByIdAsync(int id, CancellationToken ct);
        Task CreateTestAsync(CreateTestRequest request, CancellationToken ct);
        Task<bool> UpdateTestAsync(int id, UpdateTestRequest request, CancellationToken ct);
        Task<bool> DeleteTestAsync(int id, CancellationToken ct);
    }
}