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

        public async Task<List<TopicInfoResponse>> GetTopicsAsync(int pageSize, int pageNumber, TopicRequest request, CancellationToken ct)
        {
            var query = _context.Topics
                .Include(t => t.Language)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(t => t.Name.Contains(request.Name));

            if (request.LanguageId > 0)
                query = query.Where(t => t.LanguageId == request.LanguageId);

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

        public async Task<int> CountTopicsAsync(TopicRequest request, CancellationToken ct)
        {
            var query = _context.Topics
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(t => t.Name.Contains(request.Name));

            if (request.LanguageId > 0)
                query = query.Where(t => t.LanguageId == request.LanguageId);

            if (!string.IsNullOrWhiteSpace(request.Level))
                query = query.Where(t => t.Level == request.Level);

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

        public async Task<List<string>> GetLevelsAsync(CancellationToken ct)
        {
            return await _context.Topics
                .AsNoTracking()
                .Where(t => !string.IsNullOrEmpty(t.Level))
                .Select(t => t.Level!)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync(ct);
        }

        public async Task CreateTopicAsync(TopicRequest request, CancellationToken ct)
        {
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

        public async Task<bool> UpdateTopicAsync(int id, TopicRequest request, CancellationToken ct)
        {
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
            var topic = await _context.Topics
                .FirstOrDefaultAsync(t => t.TopicId == id, ct);

            if (topic == null)
                return false;

            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync(ct);

            return true;
        }
    }
}
