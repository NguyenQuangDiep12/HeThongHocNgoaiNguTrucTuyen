using HeThongHocNgoaiNguTrucTuyen.Data;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;
using HeThongHocNgoaiNguTrucTuyen.Models;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HeThongHocNgoaiNguTrucTuyen.Services
{
    public class LessonService : ILessonService
    {
        private readonly ApplicationDbContext _context;
        public LessonService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<LessonInfoResponse>> GetLessonsAsync(LessonFilterRequest request, int pageNumber, int pageSize, CancellationToken ct = default)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 10);

            var query = _context.Lessons
                .AsNoTracking();
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(x => x.Title.Contains(request.Title));
            }

            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 10);

            return await query
                .OrderByDescending(x => x.LessonId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new LessonInfoResponse
                {
                    LessonId = l.LessonId,
                    TopicId = l.TopicId,
                    Title = l.Title,
                    Description = l.Description,
                    Content = l.Content,
                    TopicName = l.Topic.Name,
                    LanguageName = l.Topic.Language.Name
                }).ToListAsync(ct);
        }
        public async Task<int> CountLessonsAsync(LessonFilterRequest request, CancellationToken ct = default)
        {
            var query = _context.Lessons
                .AsNoTracking()
                .Where(x => x.TopicId == request.TopicId && x.Topic.LanguageId == request.LanguageId);
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(x => x.Title.Contains(request.Title));
            }
            return await query.CountAsync(ct);
        }
        public async Task<List<LessonInfoResponse>> GetAllLessonsAsync(CancellationToken ct = default)
        {
            return await _context
                .Lessons
                .AsNoTracking()
                .Select(l => new LessonInfoResponse
                {
                    TopicId = l.TopicId,
                    Title = l.Title,
                    Description = l.Description,
                    Content = l.Content,
                    TopicName = l.Topic.Name,
                    LanguageName = l.Topic.Language.Name,
                    LessonId = l.LessonId,
                }).ToListAsync(ct);
        }
        public async Task<List<LessonInfoResponse>> GetLessonsByTopicIdAsync(int topicId, CancellationToken ct)
        {
            return await _context.Lessons
                .AsNoTracking()
                .Where(l => l.TopicId == topicId)
                .Select(l => new LessonInfoResponse
                {
                    TopicId = l.TopicId,
                    Title = l.Title,
                    Description = l.Description,
                    Content = l.Content,
                    TopicName = l.Topic.Name,
                    LanguageName = l.Topic.Language.Name,
                    LessonId = l.LessonId
                }).ToListAsync(ct);
        }
        public async Task<LessonInfoResponse?> GetLessonByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Lessons
                .AsNoTracking()
                .Where(l => l.LessonId == id)
                .Select(l => new LessonInfoResponse
                {
                    LessonId = l.LessonId,
                    TopicId = l.TopicId,
                    Title = l.Title,
                    Description = l.Description,
                    Content = l.Content,
                    TopicName = l.Topic.Name,
                    LanguageName = l.Topic.Language.Name
                }).FirstOrDefaultAsync(ct);
        }
        public async Task CreateLessonAsync(CreateLessonRequest request, CancellationToken ct = default)
        {
            // kiem tra topic co ton tai
            var topicExists = _context.Topics.Any(t => t.TopicId == request.TopicId);
            if (!topicExists)
            {
                throw new Exception("Chu de không tồn tại");
            }
            var lesson = new Lesson
            {
                TopicId = request.TopicId,
                Title = request.Title,
                Description = request.Description,
                Content = request.Content
            };

            await _context.Lessons.AddAsync(lesson);
            await _context.SaveChangesAsync(ct);
        }
        public async Task<bool> UpdateLessonAsync(int id, UpdateLessonRequest request, CancellationToken ct = default)
        {
            var lesson = await _context.Lessons.FirstOrDefaultAsync(x => x.LessonId == id, ct);
            if (lesson == null)
            {
                throw new Exception("Không tìm thấy Bài học!");
            }
            lesson.Title = request.Title;
            lesson.Description = request.Description;
            lesson.Content = request.Content;

            await _context.SaveChangesAsync(ct);
            return true;
        }
        public async Task<bool> DeleteLessonAsync(int id, CancellationToken ct = default)
        {
            var lesson = await _context.Lessons.FirstOrDefaultAsync(x => x.LessonId == id, ct);
            if (lesson == null)
            {
                throw new Exception("Không tìm thấy bài học");
            }
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}