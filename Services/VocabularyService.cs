using HeThongHocNgoaiNguTrucTuyen.Data;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;
using HeThongHocNgoaiNguTrucTuyen.Models;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HeThongHocNgoaiNguTrucTuyen.Services
{
    public class VocabularyService : IVocabularyService
    {
        private readonly ApplicationDbContext _context;

        public VocabularyService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<VocabularyInfoResponse>> GetVocabulariesAsync(string? word, int? lessonId, int pageNumber, int pageSize, CancellationToken ct)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 10);

            var query = _context.Vocabularies.Include(x => x.Lesson).AsNoTracking();
            if (!string.IsNullOrWhiteSpace(word))
            {
                query = query.Where(x => x.Word.Contains(word));
            }
            if (lessonId.HasValue && lessonId.Value > 0)
            {
                query = query.Where(x => x.LessonId == lessonId.Value);
            }
            return await query
                .OrderByDescending(x => x.VocabularyId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new VocabularyInfoResponse
                {
                    VocabularyId = x.VocabularyId,
                    LessonId = x.LessonId,
                    Word = x.Word,
                    Meaning = x.Meaning,
                    Phoenic = x.Phoenic,
                    Example = x.Example,
                    LessonTitle = x.Lesson.Title
                }).ToListAsync(ct);
        }

        public async Task<int> CountVocabulariesAsync(string? word, int? lessonId, CancellationToken ct)
        {
            var query = _context.Vocabularies.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(word))
            {
                query = query.Where(x => x.Word.Contains(word));
            }
            if (lessonId.HasValue && lessonId.Value > 0)
            {
                query = query.Where(x => x.LessonId == lessonId.Value);
            }
            return await query.CountAsync(ct);
        }

        public async Task<VocabularyInfoResponse?> GetVocabularyByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Vocabularies
                .AsNoTracking()
                .Where(x => x.VocabularyId == id)
                .Select(x => new VocabularyInfoResponse
                {
                    VocabularyId = x.VocabularyId,
                    LessonId = x.LessonId,
                    Word = x.Word,
                    Meaning = x.Meaning,
                    Phoenic = x.Phoenic,
                    Example = x.Example,
                    LessonTitle = x.Lesson.Title
                }).FirstOrDefaultAsync(ct);
        }

        public async Task CreateVocabularyAsync(VocabularyRequest request, CancellationToken ct)
        {
            var vocabulary = new Vocabulary
            {
                LessonId = request.LessonId,
                Word = request.Word,
                Meaning = request.Meaning,
                Phoenic = request.Phoenic,
                Example = request.Example
            };
            await _context.Vocabularies.AddAsync(vocabulary, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> UpdateVocabularyAsync(int id, VocabularyRequest request, CancellationToken ct)
        {
            var vocabulary = await _context.Vocabularies.FirstOrDefaultAsync(x => x.VocabularyId == id, ct);
            if (vocabulary == null)
            {
                return false;
            }
            vocabulary.LessonId = request.LessonId;
            vocabulary.Word = request.Word;
            vocabulary.Meaning = request.Meaning;
            vocabulary.Phoenic = request.Phoenic;
            vocabulary.Example = request.Example;
            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteVocabularyAsync(int id, CancellationToken ct)
        {
            var vocabulary = await _context.Vocabularies.FirstOrDefaultAsync(x => x.VocabularyId == id, ct);
            if (vocabulary == null)
            {
                return false;
            }
            _context.Vocabularies.Remove(vocabulary);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}