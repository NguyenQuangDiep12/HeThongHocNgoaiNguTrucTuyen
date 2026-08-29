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
        public async Task<List<LessonInfoResponse>> GetLessonsAsync(
            string? title,
            int? topicId,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Lessons
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(x =>
                    x.Title.Contains(title));
            }

            if (topicId.HasValue)
            {
                query = query.Where(x =>
                    x.TopicId == topicId.Value);
            }

            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            return await query
                .OrderByDescending(x => x.LessonId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new LessonInfoResponse
                {
                    LessonId = x.LessonId,
                    TopicId = x.TopicId,
                    Title = x.Title,
                    Description = x.Description,
                    Content = x.Content,

                    TopicName = x.Topic != null
                        ? x.Topic.Name
                        : "",

                    LanguageName =
                        x.Topic != null &&
                        x.Topic.Language != null
                            ? x.Topic.Language.Name
                            : ""
                })
                .ToListAsync(ct);
        }
        public async Task<List<LessonInfoResponse>> GetAllLessonsAsync(int? TopicId, CancellationToken ct)
        {
            return await _context
                .Lessons
                .Include(l => l.Topic)
                .AsNoTracking()
                .OrderBy(l => l.Title)
                .Select(l => new LessonInfoResponse
                {
                    Title = l.Title,
                    Description = l.Description,
                    Content = l.Content,
                    LanguageName = l.Topic.Description
                }).ToListAsync(ct);
        }
        public async Task<int> CountLessonsAsync(
            string? title,
            int? topicId,
            CancellationToken ct = default)
        {
            var query = _context.Lessons
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(x =>
                    x.Title.Contains(title));
            }

            if (topicId.HasValue)
            {
                query = query.Where(x =>
                    x.TopicId == topicId.Value);
            }

            return await query.CountAsync(ct);
        }
        public async Task<LessonInfoResponse?> GetLessonByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _context.Lessons
                .AsNoTracking()
                .Where(x => x.LessonId == id)
                .Select(x => new LessonInfoResponse
                {
                    LessonId = x.LessonId,
                    TopicId = x.TopicId,
                    Title = x.Title,
                    Description = x.Description,
                    Content = x.Content,

                    TopicName = x.Topic != null
                        ? x.Topic.Name
                        : "",

                    LanguageName =
                        x.Topic != null &&
                        x.Topic.Language != null
                            ? x.Topic.Language.Name
                            : ""
                })
                .FirstOrDefaultAsync(ct);
        }
        public async Task CreateLessonAsync(
            LessonRequest request,
            CancellationToken ct = default)
        {
            var lesson = new Lesson
            {
                TopicId = request.TopicId,
                Title = request.Title,
                Description = request.Description,
                Content = request.Content
            };

            await _context.Lessons.AddAsync(
                lesson,
                ct);

            await _context.SaveChangesAsync(ct);
        }
        public async Task<bool> UpdateLessonAsync(
            int id,
            LessonRequest request,
            CancellationToken ct = default)
        {
            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(
                    x => x.LessonId == id,
                    ct);

            if (lesson == null)
            {
                return false;
            }

            lesson.TopicId = request.TopicId;
            lesson.Title = request.Title;
            lesson.Description = request.Description;
            lesson.Content = request.Content;

            await _context.SaveChangesAsync(ct);

            return true;
        }
        public async Task<bool> DeleteLessonAsync(
            int id,
            CancellationToken ct = default)
        {
            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(
                    x => x.LessonId == id,
                    ct);

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

