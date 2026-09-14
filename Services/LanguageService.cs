using HeThongHocNgoaiNguTrucTuyen.Data;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;
using HeThongHocNgoaiNguTrucTuyen.Models;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HeThongHocNgoaiNguTrucTuyen.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly ApplicationDbContext _context;
        public LanguageService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<LanguageInfoResponse>> GetLanguagesAsync(LanguageFilterRequest request ,int pageSize, int pageNumber,CancellationToken ct = default)
        {
            var listLanguage = _context.Languages.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                listLanguage = listLanguage.Where(l => l.Name.Contains(request.Name));
            }

            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 10); 
            return await listLanguage
                .OrderBy(l => l.LanguageId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new LanguageInfoResponse
                {
                    Code = l.Code,
                    Name = l.Name,
                    Description = l.Description,
                    LanguageId = l.LanguageId,
                }).ToListAsync(ct);
        }

        public async Task<int> CountLanguagesAsync(LanguageFilterRequest request, CancellationToken ct = default)
        {
            var query = _context.Languages.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(l => l.Name.Contains(request.Name));
            }
            return await query.CountAsync(ct);
        }
        public async Task<List<LanguageInfoResponse>> GetAllLanguagesAsync(CancellationToken ct = default)
        {
            return await _context
                .Languages
                .AsNoTracking()
                .OrderBy(l => l.LanguageId)
                .Select(l => new LanguageInfoResponse
                {
                    LanguageId = l.LanguageId,
                    Name = l.Name,
                    Description = l.Description,
                    Code = l.Code
                }).ToListAsync(ct);
        }

        public async Task<LanguageInfoResponse> GetLanguageByIdAsync(int id, CancellationToken ct)
        {
            return await _context
                .Languages
                .AsNoTracking()
                .Where(l => l.LanguageId == id)
                .Select(l => new LanguageInfoResponse
                {
                    LanguageId = l.LanguageId,
                    Name = l.Name,
                    Description = l.Description,
                    Code = l.Code,
                }).FirstOrDefaultAsync(ct);
        }

        public async Task CreateLanguagesAsync(CreateLanguageRequest request, CancellationToken ct)
        {
            var language = new Language();
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                language.Name = request.Name;
            }
            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                language.Description = request.Description;
            }
            if (!string.IsNullOrWhiteSpace(request.Code))
            {
                language.Code = request.Code;
            }
            await _context.Languages.AddAsync(language);
            await _context.SaveChangesAsync(ct);
        }
        public async Task<bool> UpdateLanguagesAsync(int id, UpdateLanguageRequest request, CancellationToken ct)
        {
            return await _context
                .Languages
                .Where(l => l.LanguageId == id)
                .ExecuteUpdateAsync(setters =>
                    setters
                    .SetProperty(l => l.Name, request.Name)
                    .SetProperty(l => l.Code, request.Code)
                    .SetProperty(l => l.Description, request.Description), ct) > 0 ? true : false;
        }
        public async Task<bool> DeleteLanguagesAsync(int id, CancellationToken ct)
        {
            var languageItem = await _context.Languages.FirstOrDefaultAsync(l => l.LanguageId == id, ct);
            if (languageItem == null)
            {
                return false;
            }
            _context.Languages.Remove(languageItem);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}



