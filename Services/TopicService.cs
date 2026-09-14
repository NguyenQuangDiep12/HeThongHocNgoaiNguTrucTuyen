using HeThongHocNgoaiNguTrucTuyen.Data;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;
using HeThongHocNgoaiNguTrucTuyen.Models;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HeThongHocNgoaiNguTrucTuyen.Services
{
    public class TopicService : ITopicService
    {
        private readonly ApplicationDbContext _context;
        public TopicService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<TopicInfoResponse>> GetTopicsAsync(int languageId, TopicFilterRequest request, int pageSize, int pageNumber, CancellationToken ct)
        {
            var query = _context.Topics.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(t => t.Name.Contains(request.Name));

            if (languageId > 0)
                query = query.Where(t => t.LanguageId == languageId);

            if (!string.IsNullOrWhiteSpace(request.Level))
                query = query.Where(t => t.Level == request.Level);

            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 10);

            return await query
                .OrderBy(t => t.TopicId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TopicInfoResponse
                {
                    TopicId = t.TopicId,
                    Name = t.Name,
                    Level = t.Level,
                    Description = t.Description,
                    ImageUrl = t.ImageUrl,
                    LanguageId = t.LanguageId,
                    LanguageName = t.Language.Name
                })
                .ToListAsync(ct);
        }

        public async Task<int> CountTopicsAsync(int languageId, TopicFilterRequest request, CancellationToken ct)
        {
            var query = _context.Topics
               .AsNoTracking();
            if(languageId > 0)
            {
                query = query.Where(t => t.LanguageId == languageId);
            }
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(t => t.Name.Contains(request.Name));
            }
            if (!string.IsNullOrWhiteSpace(request.Level))
            {
                query = query.Where(t => t.Level == request.Level);
            }
            return await query.CountAsync(ct);
        }

        public async Task<TopicInfoResponse?> GetTopicByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Topics
                .AsNoTracking()
                .Where(t => t.TopicId == id)
                .Select(t => new TopicInfoResponse
                {
                    TopicId = t.TopicId,
                    Name = t.Name,
                    Level = t.Level,
                    Description = t.Description,
                    ImageUrl = t.ImageUrl,
                    LanguageId = t.LanguageId,
                    LanguageName = t.Language.Name
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<List<string>> GetLevelsByLanguageIdAsync(int languageId, CancellationToken ct)
        {
            return await _context.Topics
                .AsNoTracking()
                .Where(t => t.LanguageId == languageId)
                .Where(t => !string.IsNullOrEmpty(t.Level))
                .Select(t => t.Level!)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync(ct);
        }
        public async Task<List<TopicInfoResponse>> GetTopicsByLanguageIdAsync(int languageId, CancellationToken ct)
        {
            return await _context.Topics
                .AsNoTracking()
                .Where(t => t.LanguageId == languageId)
                .OrderBy(t => t.Name)
                .Select(t => new TopicInfoResponse
                {
                    TopicId = t.TopicId,
                    LanguageId = t.LanguageId,
                    LanguageName = t.Language.Name,
                    Name = t.Name,
                    Level = t.Level,
                    Description = t.Description,
                    ImageUrl = t.ImageUrl
                }).ToListAsync(ct);
        }

        public async Task CreateTopicAsync(CreateTopicRequest request, CancellationToken ct)
        {
            var languageExists = await _context.Languages.AnyAsync(l => l.LanguageId == request.LanguageId);
            if (!languageExists)
            {
                throw new Exception("Ngôn ngữ không tồn tại");
            }
            var topic = new Topic
            {
                Name = request.Name,
                Level = request.Level,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                LanguageId = request.LanguageId
            };

            await _context.Topics.AddAsync(topic, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> UpdateTopicAsync(int id, UpdateTopicRequest request, CancellationToken ct)
        {
            var languageExists = await _context.Languages.AnyAsync(l => l.LanguageId == request.LanguageId);
            if (!languageExists)
            {
                throw new Exception("Ngôn ngữ không tồn tại");
            }
            return await _context.Topics
                .Where(t => t.TopicId == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.Name, request.Name)
                    .SetProperty(t => t.Level, request.Level)
                    .SetProperty(t => t.Description, request.Description)
                    .SetProperty(t => t.ImageUrl, request.ImageUrl)
                    .SetProperty(t => t.LanguageId, request.LanguageId), ct) > 0;
        }

        public async Task<bool> DeleteTopicAsync(int id, CancellationToken ct)
        {
            var topic = await _context.Topics.FirstOrDefaultAsync(t => t.TopicId == id, ct);
            if (topic == null)
            {
                throw new Exception("Chủ đề không tồn tại");
            }
            var hasLessons = await _context.Lessons.AnyAsync(l => l.TopicId == id, ct);
            if (hasLessons)
            {
                throw new Exception($"Không thể xóa chủ đề khi còn bài học tồn tại");
            }

            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
