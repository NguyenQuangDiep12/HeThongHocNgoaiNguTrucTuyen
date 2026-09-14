using HeThongHocNgoaiNguTrucTuyen.Data;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests.Lesson;
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

        public async Task<List<LessonInfoResponse>> GetLessonsAsync(int topicId, LessonFilterRequest request, int pageNumber, int pageSize, CancellationToken ct)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 10);

            var query = _context.Lessons
                .AsNoTracking()
                .Where(l => l.TopicId == topicId);

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(l => l.Title.Contains(request.Title));
            }

            return await query.OrderByDescending(l => l.LessonId)
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

        public async Task<List<LessonInfoResponse>> GetAllLessonsAsync(int topicId, CancellationToken ct)
        {
            return await _context.Lessons
                .AsNoTracking()
                .Where(l => l.TopicId == topicId)
                .OrderBy(l => l.Title)
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

        public async Task<int> CountLessonsAsync(int topicId, LessonFilterRequest request, CancellationToken ct)
        {
            var query = _context.Lessons
                .AsNoTracking()
                .Where(l => l.TopicId == topicId);

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(l => l.Title.Contains(request.Title));
            }

            return await query.CountAsync(ct);
        }

        public async Task<LessonInfoResponse?> GetLessonByIdAsync(int id, CancellationToken ct)
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

        public async Task CreateLessonAsync(CreateLessonRequest request, CancellationToken ct)
        {
            var topicExists = await _context.Topics.AnyAsync(x => x.TopicId == request.TopicId, ct);
            if (!topicExists)
            {
                throw new ArgumentException("Chủ đề không tồn tại.");
            }

            var lesson = new Lesson
            {
                TopicId = request.TopicId,
                Title = request.Title,
                Description = request.Description,
                Content = request.Content
            };

            await _context.Lessons.AddAsync(lesson, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> UpdateLessonAsync(int id, UpdateLessonRequest request, CancellationToken ct)
        {
            var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.LessonId == id, ct);

            if (lesson == null)
            {
                return false;
            }

            lesson.Title = request.Title;
            lesson.Description = request.Description;
            lesson.Content = request.Content;

            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteLessonAsync(int id, CancellationToken ct)
        {
            var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.LessonId == id, ct);

            if (lesson == null)
            {
                return false;
            }

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync(ct);

            return true;
        }
    }
}