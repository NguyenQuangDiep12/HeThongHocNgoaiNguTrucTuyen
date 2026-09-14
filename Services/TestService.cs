using HeThongHocNgoaiNguTrucTuyen.Data;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;
using HeThongHocNgoaiNguTrucTuyen.Models;
using HeThongHocNgoaiNguTrucTuyen.Models.Enums;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HeThongHocNgoaiNguTrucTuyen.Services
{
    public class TestService : ITestService
    {
        private readonly ApplicationDbContext _context;

        public TestService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<TestInfoResponse>> GetTestsAsync(int pageSize, int pageNumber, TestFilterRequest request, CancellationToken ct)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var query = _context
                .Tests
                .Include(t => t.Questions)
                .AsNoTracking();

            if(!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(t => t.Title.Contains(request.Title));
            }

            return await query
                .OrderBy(t => t.TestId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TestInfoResponse
                {
                    TestId = t.TestId,
                    Title = t.Title,
                    Description = t.Description,
                    DurationMinutes = t.DurationMinutes,
                    TestMode = t.TestMode.ToString(),
                    QuestionCount = t.Questions.Count,
                }).ToListAsync(ct);
        }
        public async Task<int> CountTestsAsync(TestFilterRequest request, CancellationToken ct)
        {
            var query = _context
                .Tests
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(t => t.Title.Contains(request.Title));
            }

            return await query.CountAsync(ct);
        }
        public async Task<TestInfoResponse?> GetTestByIdAsync(int id, CancellationToken ct)
        {
            return await _context
                .Tests
                .Include(t => t.Questions)
                .AsNoTracking()
                .Where(t => t.TestId == id)
                .Select(t => new TestInfoResponse
                {
                    TestId = t.TestId,
                    Title = t.Title,
                    Description = t.Description,
                    DurationMinutes = t.DurationMinutes,
                    TestMode = t.TestMode.ToString(),
                    QuestionCount = t.Questions.Count,
                })
                .FirstOrDefaultAsync(ct);
        }
        public async Task CreateTestAsync(CreateTestRequest request, CancellationToken ct)
        {

            var newTest = new Test
            {
                Title = request.Title,
                Description = request.Description,
                DurationMinutes = request.DurationMinutes,
                TestMode = Enum.Parse<TestMode>(request.TestMode)
            };

            await _context.Tests.AddAsync(newTest);
            await _context.SaveChangesAsync(ct);

        }
        public async Task<bool> UpdateTestAsync(int id, UpdateTestRequest request, CancellationToken ct)
        {
            var query = _context.Tests.Where(t => t.TestId == id);
            var testExist = await _context
                .Tests
                .Where(t => t.TestId == id)
                .FirstOrDefaultAsync(ct);

            if(testExist == null)
            {
                throw new Exception("Không tìm thấy bài kiểm tra");
            }

            return await query
                .ExecuteUpdateAsync(settle =>
                    settle.SetProperty(t => t.Title, request.Title)
                          .SetProperty(t => t.Description, request.Description)
                          .SetProperty(t => t.DurationMinutes, request.DurationMinutes)
                          .SetProperty(t => t.TestMode, Enum.Parse<TestMode>(request.TestMode))) > 0 ? true : false;
        }
        public async Task<bool> DeleteTestAsync(int id, CancellationToken ct)
        {
            var test = await _context.Tests.FirstOrDefaultAsync(x => x.TestId == id, ct);
            if (test == null)
            {
                return false;
            }
            var hasQuestions = await _context.Questions.AnyAsync(x => x.TestId == id, ct);
            if (hasQuestions)
            {
                throw new InvalidOperationException("Không thể xóa bài kiểm tra vì bài kiểm tra đã có câu hỏi.");
            }
            _context.Tests.Remove(test);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}