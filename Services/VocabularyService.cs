using HeThongHocNgoaiNguTrucTuyen.Data;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests.Vocabulary;
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

        public async Task<List<VocabularyInfoResponse>> GetVocabulariesAsync(int lessonId, VocabularyFilterRequest request, int pageNumber, int pageSize, CancellationToken ct)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : Math.Min(pageSize, 10);

            var query = _context.Vocabularies
                .AsNoTracking()
                .Where(v => v.LessonId == lessonId);

            if (!string.IsNullOrWhiteSpace(request.Word))
            {
                query = query.Where(v => v.Word.Contains(request.Word));
            }

            return await query.OrderByDescending(v => v.VocabularyId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(v => new VocabularyInfoResponse
                {
                    VocabularyId = v.VocabularyId,
                    LessonId = v.LessonId,
                    Word = v.Word,
                    Meaning = v.Meaning,
                    Phoenic = v.Phoenic,
                    Example = v.Example,
                    LessonTitle = v.Lesson.Title
                }).ToListAsync(ct);
        }

        public async Task<int> CountVocabulariesAsync(int lessonId, VocabularyFilterRequest request, CancellationToken ct)
        {
            var query = _context.Vocabularies
                .AsNoTracking()
                .Where(v => v.LessonId == lessonId);

            if (!string.IsNullOrWhiteSpace(request.Word))
            {
                query = query.Where(v => v.Word.Contains(request.Word));
            }

            return await query.CountAsync(ct);
        }

        public async Task<VocabularyInfoResponse?> GetVocabularyByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Vocabularies
                .AsNoTracking()
                .Where(v => v.VocabularyId == id)
                .Select(v => new VocabularyInfoResponse
                {
                    VocabularyId = v.VocabularyId,
                    LessonId = v.LessonId,
                    Word = v.Word,
                    Meaning = v.Meaning,
                    Phoenic = v.Phoenic,
                    Example = v.Example,
                    LessonTitle = v.Lesson.Title
                }).FirstOrDefaultAsync(ct);
        }

        public async Task CreateVocabularyAsync(CreateVocabularyRequest request, CancellationToken ct)
        {
            var lessonExists = await _context.Lessons.AnyAsync(x => x.LessonId == request.LessonId, ct);
            if (!lessonExists)
            {
                throw new ArgumentException("Bài học không tồn tại.");
            }

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

        public async Task<bool> UpdateVocabularyAsync(int id, UpdateVocabularyRequest request, CancellationToken ct)
        {
            var vocabulary = await _context.Vocabularies.FirstOrDefaultAsync(v => v.VocabularyId == id, ct);

            if (vocabulary == null)
            {
                return false;
            }

            vocabulary.Word = request.Word;
            vocabulary.Meaning = request.Meaning;
            vocabulary.Phoenic = request.Phoenic;
            vocabulary.Example = request.Example;

            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteVocabularyAsync(int id, CancellationToken ct)
        {
            var vocabulary = await _context.Vocabularies.FirstOrDefaultAsync(v => v.VocabularyId == id, ct);

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