using HeThongHocNgoaiNguTrucTuyen.Data;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Requests;
using HeThongHocNgoaiNguTrucTuyen.Dtos.Responses;
using HeThongHocNgoaiNguTrucTuyen.Models;
using HeThongHocNgoaiNguTrucTuyen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HeThongHocNgoaiNguTrucTuyen.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct)
        {
            var email = request.Email.Trim().ToLower();

            // query bang user kem bang role
            var userExist = await _context
                .Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email, ct);

            if(userExist == null)
            {
                throw new Exception("Tai khoan nguoi dung khong ton tai!");
            }

            // kiem tra mat khau nguoi dung gui
            var CorrectPassword = BCrypt.Net.BCrypt.Verify(request.Password, userExist.PasswordHash);

            if (!CorrectPassword)
            {
                throw new Exception("Tai khoan hoac mat khau khong chinh xac!");
            }

            return new LoginResponse
            {
                FullName = userExist.FullName,
                UserId = userExist.UserId,
                RoleName = userExist.Role.RoleName
            };
            
        }

        public async Task<bool> RegisterAsync(RegisterRequest request, CancellationToken ct)
        {
            var email = request.Email.Trim().ToLower(); // lay gia tri email va loai bo khoang trang, chuyen ve chu thuong

            bool emailExist = await _context
                .Users
                .AnyAsync(u => u.Email == email);

            if(emailExist == true)
            {
                throw new InvalidOperationException("Email da ton tai truoc do");
            }

            var user = new User
            {
                FullName = request.FullName,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                RoleId = 2 // tuong ung voi User
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
