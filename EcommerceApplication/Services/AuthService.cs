using EcommerceApplication.DTO;
using EcommerceApplication.Models;
using EcommerceApplication.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using EcommerceApplication.Exceptions;


namespace EcommerceApplication.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;//UserManager is a package for managing users in application used for creating users, deleting updating, checking passwords, etc.
        public readonly IConfiguration _configuration;//to read application settings like JWT secret key, issuer, audience, etc.
        private readonly ILogger<AuthService> _logger;
        bool value = false, value1 = false;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration,ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }
        private string IsValidPasswordFormat(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                value = false;
                return "Password length is less than 8 characters";
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                value = false;
                return "Password must conatain atleast one capital letter";
            }

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                value = false;
                return "Password must contain atleast one small letter";
            }


            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                value = false;
                return "Password must contain atleast one number";
            }


            if (!Regex.IsMatch(password, @"[*$%&!@(){}#]"))
            {
                value = false;
                return "Password must contain atleast one non-alphanumerical";
            }
            value = true;
            return "Password is correct";
        }
        public string IsValidateEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                value1 = false;
                return "Email is required.";
            }

            email = email.Trim();

            if (email.Length < 8)
            {
                value1 = false;
                return "Email must be at least 8 characters long.";

            }

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (!Regex.IsMatch(email, pattern))
            {
                value1 = false;
                return "Invalid email format.";
            }
            value1 = true;
            return "Email is correct"; // valid email
        }
        private string GenerateAccessToken(ApplicationUser user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Generates a refresh token (cryptographically secure random string)
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<AuthResult> RegisterAsync(RegisterDTO dto)
        {
            string message = IsValidateEmailFormat(dto.Email);
            if (!value1)
            {
                _logger.LogWarning("Email format is invalid");
                throw new ArgumentException(message);
            }
            string message1 = IsValidPasswordFormat(dto.Password);
            if (!value)
            {
                _logger.LogWarning("Password format is invalid for email: {Email}", dto.Email);
                throw new ArgumentException(message1);
            }
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email
                //RefreshToken=GenerateRefreshToken()
            };

            var result = await _userManager.CreateAsync(user, dto.Password);//CreateAsync is a method to create a new user with the specified password, it will hash the password and store it securely in the database
            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("User creation failed for email: {Email}. Errors: {Errors}", dto.Email, errorMessages);
                throw new ConflictException(errorMessages);
            }
            _logger.LogInformation("User registered successfully with email: {Email}", dto.Email);
            await _userManager.AddToRoleAsync(user, "CUSTOMER");
            //await _userManager.AddToRoleAsync(user, "ADMIN");
            return new AuthResult
            {
                Succeeded = true,
                Message = "User registered successfully",
                User = user
            };
        }
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                _logger.LogWarning("Login attempt with invalid email: {Email}", dto.Email);

                throw new ArgumentException("Invalid email or password");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Login attempt with invalid password for email: {Email}",dto.Email);

                throw new ArgumentException("Invalid email or password");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = GenerateAccessToken(user, roles.ToList());

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

            await _userManager.UpdateAsync(user);

            _logger.LogInformation("User logged in successfully with email {Email}",dto.Email);

            return new AuthResponseDTO
            {
                Succeeded = true,
                Token = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = DateTime.UtcNow.AddHours(1),
                Message = "Login successful"
            };
        }
        public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO request)
        {
            var principal = GetPrincipalFromExpiredToken(request.Token);

            if (principal == null)
            {
                _logger.LogWarning("Token refresh attempt with invalid token: {Token}",request.Token);

                throw new UnauthorizedException("Invalid token");
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedException("Invalid token");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UnauthorizedException("Invalid refresh token");
            }

            if (user.RefreshToken != request.RefreshToken)
            {
                _logger.LogWarning(
                    "Token refresh attempt with invalid refresh token for user ID: {UserId}",
                    userId);

                throw new UnauthorizedException("Invalid refresh token");
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                _logger.LogWarning("Token refresh attempt with expired refresh token for user ID: {UserId}",userId);

                throw new UnauthorizedException("Refresh token has expired");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var newAccessToken = GenerateAccessToken(user, roles.ToList());

            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Token refreshed successfully for user ID: {UserId}",userId);

            return new AuthResponseDTO
            {
                Succeeded = true,
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = DateTime.UtcNow.AddHours(1),
                Message = "Token refreshed successfully"
            };
        }

        public async Task RevokeTokenAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedException("User not found");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UnauthorizedException("User not found");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.MinValue;

            await _userManager.UpdateAsync(user);
        }

        /// Extracts claims from an expired token without validating lifetime
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var tokenHandler = new JwtSecurityTokenHandler();//JwtSecurityTokenHandler is a class to create read and validate JWT tokens

                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,//if signature is valid
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false // Allow expired tokens
                }, out SecurityToken securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) || //checks if the algorithm used is HMAC SHA256
                   !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}