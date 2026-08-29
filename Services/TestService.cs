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

        // =====================================================
        // GET LIST
        // =====================================================

        public async Task<List<TestInfoResponse>> GetTestsAsync(
            int pageSize,
            int pageNumber,
            TestRequest? request,
            CancellationToken ct)
        {
            request ??= new TestRequest();

            var query = _context.Tests
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(x =>
                    x.Title.Contains(request.Title));
            }

            if (request.PartNumber.HasValue)
            {
                query = query.Where(x =>
                    x.PartNumber == request.PartNumber);
            }

            var tests = await query
                .OrderByDescending(x => x.TestId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TestInfoResponse
                {
                    TestId = x.TestId,
                    Title = x.Title,
                    Description = x.Description,
                    TestMode = (int)x.TestMode,
                    TestModeDisplay =
                        x.TestMode == TestMode.PART
                            ? "Part Test"
                            : "Full Test",
                    PartNumber = x.PartNumber,
                    DurationMinutes = x.DurationMinutes,
                    QuestionCount = x.Questions.Count
                })
                .ToListAsync(ct);

            return tests;
        }

        // =====================================================
        // COUNT
        // =====================================================

        public async Task<int> CountTestsAsync(
            TestRequest? request,
            CancellationToken ct)
        {
            request ??= new TestRequest();

            var query = _context.Tests
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(x =>
                    x.Title.Contains(request.Title));
            }

            if (request.PartNumber.HasValue)
            {
                query = query.Where(x =>
                    x.PartNumber == request.PartNumber);
            }

            return await query.CountAsync(ct);
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<TestInfoResponse?> GetTestByIdAsync(
            int id,
            CancellationToken ct)
        {
            return await _context.Tests
                .AsNoTracking()
                .Where(x => x.TestId == id)
                .Select(x => new TestInfoResponse
                {
                    TestId = x.TestId,

                    Title = x.Title,

                    Description = x.Description,

                    TestMode = (int)x.TestMode,

                    TestModeDisplay =
                        x.TestMode == TestMode.PART
                            ? "Part Test"
                            : "Full Test",

                    PartNumber = x.PartNumber,

                    DurationMinutes = x.DurationMinutes,

                    QuestionCount = x.Questions.Count
                })
                .FirstOrDefaultAsync(ct);
        }

        // =====================================================
        // CREATE
        // =====================================================

        public async Task CreateTestAsync(
            TestRequest request,
            CancellationToken ct)
        {
            ValidateTestRequest(request);

            var test = new Test
            {
                Title = request.Title.Trim(),

                Description =
                    string.IsNullOrWhiteSpace(request.Description)
                        ? null
                        : request.Description.Trim(),

                TestMode = request.TestMode,

                PartNumber =
                    request.TestMode == TestMode.PART
                        ? request.PartNumber
                        : null,

                DurationMinutes =
                    request.DurationMinutes
            };

            _context.Tests.Add(test);

            await _context.SaveChangesAsync(ct);
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<bool> UpdateTestAsync(
            int id,
            TestRequest request,
            CancellationToken ct)
        {
            ValidateTestRequest(request);

            var test = await _context.Tests
                .FirstOrDefaultAsync(
                    x => x.TestId == id,
                    ct);

            if (test == null)
            {
                return false;
            }

            // Kiểm tra xem Test đã có Question chưa
            var hasQuestions = await _context.Questions
                .AnyAsync(
                    x => x.TestId == id,
                    ct);

            /*
             * Nếu Test đã có Question
             * thì không cho thay đổi Mode
             */
            if (hasQuestions &&
                test.TestMode != request.TestMode)
            {
                throw new InvalidOperationException(
                    "Không thể thay đổi loại bài kiểm tra khi đã có câu hỏi.");
            }

            /*
             * Nếu là PART TEST và đã có Question
             * thì không cho đổi Part
             */
            if (hasQuestions &&
                test.TestMode == TestMode.PART &&
                test.PartNumber != request.PartNumber)
            {
                throw new InvalidOperationException(
                    "Không thể thay đổi Part khi bài kiểm tra đã có câu hỏi.");
            }

            test.Title = request.Title.Trim();

            test.Description =
                string.IsNullOrWhiteSpace(request.Description)
                    ? null
                    : request.Description.Trim();

            test.TestMode = request.TestMode;

            test.PartNumber =
                request.TestMode == TestMode.PART
                    ? request.PartNumber
                    : null;

            test.DurationMinutes =
                request.DurationMinutes;

            await _context.SaveChangesAsync(ct);

            return true;
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteTestAsync(
            int id,
            CancellationToken ct)
        {
            var test = await _context.Tests
                .FirstOrDefaultAsync(
                    x => x.TestId == id,
                    ct);

            if (test == null)
            {
                return false;
            }

            var hasQuestions = await _context.Questions
                .AnyAsync(
                    x => x.TestId == id,
                    ct);

            if (hasQuestions)
            {
                throw new InvalidOperationException(
                    "Không thể xóa bài kiểm tra vì bài kiểm tra đã có câu hỏi.");
            }

            _context.Tests.Remove(test);

            await _context.SaveChangesAsync(ct);

            return true;
        }

        // =====================================================
        // VALIDATE
        // =====================================================

        private static void ValidateTestRequest(
            TestRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ArgumentException(
                    "Tên bài kiểm tra không được để trống.");
            }

            if (request.DurationMinutes <= 0)
            {
                throw new ArgumentException(
                    "Thời gian làm bài phải lớn hơn 0.");
            }

            // PART TEST
            if (request.TestMode == TestMode.PART)
            {
                if (!request.PartNumber.HasValue)
                {
                    throw new ArgumentException(
                        "Part Test bắt buộc phải chọn Part.");
                }

                if (request.PartNumber < 1 ||
                    request.PartNumber > 7)
                {
                    throw new ArgumentException(
                        "Part phải nằm trong khoảng từ 1 đến 7.");
                }
            }

            // FULL TEST
            if (request.TestMode == TestMode.FULL)
            {
                request.PartNumber = null;
            }
        }
    }
}