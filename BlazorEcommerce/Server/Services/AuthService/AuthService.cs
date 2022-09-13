using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlazorEcommerce.Server.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly DataContext _dataContext;
    private readonly IConfiguration _configuration;

    public AuthService(DataContext dataContext, IConfiguration configuration)
    {
        _dataContext = dataContext;
        _configuration = configuration;
    }

    public async Task<MessageResponse<int>> Register(User user, string password)
    {
        if (await UserExists(user.Email))
        {
            return new MessageResponse<int>(1, true, "User alredy exists.");
        }
        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        _dataContext.Users.Add(user);
        await _dataContext.SaveChangesAsync();

        return new MessageResponse<int>(user.Id, true, "Register done");
    }

    public Task<bool> UserExists(string email)
    {
        return _dataContext.Users.AnyAsync(p => p.Email.ToUpper().Equals(email.ToUpper()));
    }

    public async Task<MessageResponse<string>> Login(string email, string password)
    {
        var user = await _dataContext.Users.FirstOrDefaultAsync(p => p.Email.ToUpper().Equals(email.ToUpper()));
        if (user == null)
        {
            return new MessageResponse<string>(email, false, "User not found.");
        }
        else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            return new MessageResponse<string>(email, false, "Wrong password.");
        }
        else
        {
            return new MessageResponse<string>(CreateToken(user), true, "login done");
        }
    }

    public async Task<MessageResponse<bool>> ChangePassword(int userId, string newPassword)
    {
        var user = await _dataContext.Users.FindAsync(userId);
        if (user == null)
        {
            return new MessageResponse<bool>(false, false, "User not found.");
        }
        CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await _dataContext.SaveChangesAsync();

        return new MessageResponse<bool>(true, true, "Password has been changed.");
    }

    private string? CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id+""),
            new Claim(ClaimTypes.Name,user.Email),
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }

    private bool VerifyPasswordHash(string password, byte[] userPasswordHash, byte[] userPasswordSalt)
    {
        using (var hmac = new HMACSHA512(userPasswordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(userPasswordHash);
        }
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}