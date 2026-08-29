using HeThongHocNgoaiNguTrucTuyen.Data;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;
using HeThongHocNgoaiNguTrucTuyen.Models;
using HeThongHocNgoaiNguTrucTuyen.Models.Enums;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HeThongHocNgoaiNguTrucTuyen.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext _context;

        public QuestionService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET QUESTIONS
        // =====================================================

        public async Task<List<QuestionInfoResponse>> GetQuestionsAsync(
            int pageSize,
            int pageNumber,
            QuestionRequest request,
            CancellationToken ct)
        {
            var query = _context.Questions
                .Include(q => q.Test)
                .Include(q => q.Answers)
                .AsQueryable();

            if (request.TestId > 0)
            {
                query = query.Where(q =>
                    q.TestId == request.TestId);
            }

            if (request.PartNumber.HasValue)
            {
                query = query.Where(q =>
                    q.PartNumber == request.PartNumber);
            }

            var questions = await query
                .OrderBy(q => q.TestId)
                .ThenBy(q => q.PartNumber)
                .ThenBy(q => q.QuestionOrder)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(q => new QuestionInfoResponse
                {
                    QuestionId = q.QuestionId,

                    TestId = q.TestId,

                    TestTitle = q.Test.Title,

                    Content = q.Content,

                    ImageUrl = q.ImageUrl,

                    PartNumber = q.PartNumber,

                    QuestionType =
                        q.QuestionType.ToString(),

                    QuestionTypeDisplay =
                        q.QuestionType ==
                        QuestionType.ListenAndChoose
                            ? "Nghe và chọn đáp án"
                            : "Đọc và chọn đáp án",

                    QuestionOrder = q.QuestionOrder,

                    AudioUrl = q.AudioUrl,

                    GroupCode = q.GroupCode,

                    AnswerCount = q.Answers.Count
                })
                .ToListAsync(ct);

            return questions;
        }

        // =====================================================
        // COUNT QUESTIONS
        // =====================================================

        public async Task<int> CountQuestionsAsync(
            QuestionRequest request,
            CancellationToken ct)
        {
            var query = _context.Questions
                .AsQueryable();

            if (request.TestId > 0)
            {
                query = query.Where(q =>
                    q.TestId == request.TestId);
            }

            if (request.PartNumber.HasValue)
            {
                query = query.Where(q =>
                    q.PartNumber == request.PartNumber);
            }

            return await query.CountAsync(ct);
        }

        // =====================================================
        // GET QUESTION BY ID
        // =====================================================

        public async Task<QuestionInfoResponse?>
            GetQuestionByIdAsync(
                int id,
                CancellationToken ct)
        {
            var question = await _context.Questions
                .Include(q => q.Test)
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q =>
                    q.QuestionId == id,
                    ct);

            if (question == null)
            {
                return null;
            }

            return new QuestionInfoResponse
            {
                QuestionId = question.QuestionId,

                TestId = question.TestId,

                TestTitle = question.Test.Title,

                Content = question.Content,

                ImageUrl = question.ImageUrl,

                PartNumber = question.PartNumber,

                QuestionType =
                    question.QuestionType.ToString(),

                QuestionTypeDisplay =
                    question.QuestionType ==
                    QuestionType.ListenAndChoose
                        ? "Nghe và chọn đáp án"
                        : "Đọc và chọn đáp án",

                QuestionOrder = question.QuestionOrder,

                AudioUrl = question.AudioUrl,

                GroupCode = question.GroupCode,

                AnswerCount = question.Answers.Count
            };
        }

        // =====================================================
        // CREATE QUESTION
        // =====================================================

        public async Task CreateQuestionAsync(
            QuestionRequest request,
            CancellationToken ct)
        {
            var test = await _context.Tests
                .FirstOrDefaultAsync(t =>
                    t.TestId == request.TestId,
                    ct);

            if (test == null)
            {
                throw new ArgumentException(
                    "Bài kiểm tra không tồn tại.");
            }

            int partNumber;

            // ==========================================
            // TEST MODE = PART
            // ==========================================

            if (test.TestMode == TestMode.PART)
            {
                if (!test.PartNumber.HasValue)
                {
                    throw new ArgumentException(
                        "Bài kiểm tra PART chưa có PartNumber.");
                }

                partNumber = test.PartNumber.Value;
            }

            // ==========================================
            // TEST MODE = FULL
            // ==========================================

            else
            {
                if (!request.PartNumber.HasValue)
                {
                    throw new ArgumentException(
                        "Vui lòng chọn Part cho câu hỏi.");
                }

                partNumber = request.PartNumber.Value;
            }

            // Xác định QuestionType tự động
            var questionType =
                GetQuestionTypeByPart(partNumber);

            var question = new Question
            {
                TestId = request.TestId,

                Content = request.Content,

                ImageUrl = request.ImageUrl,

                PartNumber = partNumber,

                QuestionType = questionType,

                QuestionOrder = request.QuestionOrder,

                AudioUrl = request.AudioUrl,

                GroupCode = request.GroupCode
            };

            _context.Questions.Add(question);

            await _context.SaveChangesAsync(ct);
        }

        // =====================================================
        // UPDATE QUESTION
        // =====================================================

        public async Task<bool> UpdateQuestionAsync(
            int id,
            QuestionRequest request,
            CancellationToken ct)
        {
            var question = await _context.Questions
                .FirstOrDefaultAsync(q =>
                    q.QuestionId == id,
                    ct);

            if (question == null)
            {
                return false;
            }

            var test = await _context.Tests
                .FirstOrDefaultAsync(t =>
                    t.TestId == request.TestId,
                    ct);

            if (test == null)
            {
                throw new ArgumentException(
                    "Bài kiểm tra không tồn tại.");
            }

            int partNumber;

            // ==========================================
            // PART TEST
            // ==========================================

            if (test.TestMode == TestMode.PART)
            {
                if (!test.PartNumber.HasValue)
                {
                    throw new ArgumentException(
                        "Bài kiểm tra PART chưa có PartNumber.");
                }

                partNumber = test.PartNumber.Value;
            }

            // ==========================================
            // FULL TEST
            // ==========================================

            else
            {
                if (!request.PartNumber.HasValue)
                {
                    throw new ArgumentException(
                        "Vui lòng chọn Part.");
                }

                partNumber = request.PartNumber.Value;
            }

            var questionType =
                GetQuestionTypeByPart(partNumber);

            question.TestId = request.TestId;

            question.Content = request.Content;

            question.ImageUrl = request.ImageUrl;

            question.PartNumber = partNumber;

            question.QuestionType = questionType;

            question.QuestionOrder = request.QuestionOrder;

            question.AudioUrl = request.AudioUrl;

            question.GroupCode = request.GroupCode;

            await _context.SaveChangesAsync(ct);

            return true;
        }

        // =====================================================
        // DELETE QUESTION
        // =====================================================

        public async Task<bool> DeleteQuestionAsync(
            int id,
            CancellationToken ct)
        {
            var question = await _context.Questions
                .FirstOrDefaultAsync(q =>
                    q.QuestionId == id,
                    ct);

            if (question == null)
            {
                return false;
            }

            _context.Questions.Remove(question);

            await _context.SaveChangesAsync(ct);

            return true;
        }

        // =====================================================
        // PRIVATE METHOD
        // PART -> QUESTION TYPE
        // =====================================================

        private static QuestionType
            GetQuestionTypeByPart(
                int partNumber)
        {
            return partNumber switch
            {
                1 or 2 or 3 or 4
                    => QuestionType.ListenAndChoose,

                5 or 6 or 7
                    => QuestionType.MultipleChoice,

                _ => throw new ArgumentException(
                    "Part không hợp lệ. Part phải từ 1 đến 7.")
            };
        }
    }
}