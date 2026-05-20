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


namespace EcommerceApplication.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;//UserManager is a package for managing users in application used for creating users, deleting updating, checking passwords, etc.
        public readonly IConfiguration _configuration;//to read application settings like JWT secret key, issuer, audience, etc.

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        private bool IsValidPasswordFormat(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            if (!Regex.IsMatch(password, @"[A-Z]"))
                return false;

            if (!Regex.IsMatch(password, @"[a-z]"))
                return false;

            if (!Regex.IsMatch(password, @"[0-9]"))
                return false;

            if (!Regex.IsMatch(password, @"[*$%&!@(){}#]"))
                return false;

            return true;
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
            if (!IsValidPasswordFormat(dto.Password))
            {
                return new AuthResult
                {
                    Succeeded = false,
                    Message = "Password must contain at least one uppercase letter, one lowercase letter, and one number. Only alphanumeric characters are allowed. Minimum 8 characters required."
                };
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
                return new AuthResult
                {
                    Succeeded = false,
                    Message = errorMessages
                };
            }
            await _userManager.AddToRoleAsync(user, "CUSTOMER");
            await _userManager.AddToRoleAsync(user, "ADMIN");
            return new AuthResult
            {
                Succeeded = true,
                Message = "User registered successfully",
                User = user
            };
        }
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);

                if (user == null)
                    throw new Exception("Invalid email");

                var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

                if (!isPasswordValid)
                    throw new Exception("Invalid password");

                var roles = await _userManager.GetRolesAsync(user);

                // Generate access token
                var accessToken = GenerateAccessToken(user, roles.ToList());

                // Generate refresh token
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                // Save refresh token and expiry to database
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
                await _userManager.UpdateAsync(user);

                return new AuthResponseDTO
                {
                    Succeeded = true,
                    Token = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = DateTime.UtcNow.AddHours(1),
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDTO
                {
                    Succeeded = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO request)
        {//used to refresh tokens when they expire especially access token having 1 hour validity, so instead of the user logging in again, this function is called
            try
            {
                var principal = GetPrincipalFromExpiredToken(request.Token);//principle is a class to represent the user and their claims,
                                                                            //GetPrincipalFromExpiredToken is a method to extract claims from an expired token without validating lifetime
                if (principal == null)//checks if the JWT is fake, malformed or tampered
                {
                    return new AuthResponseDTO
                    {
                        Succeeded = false,
                        Message = "Invalid token"
                    };
                }

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;//get the userId from the claims
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null || user.RefreshToken != request.RefreshToken)//checking if the existing refresh token matches the one in the database for that user, if not then it is invalid
                {
                    return new AuthResponseDTO
                    {
                        Succeeded = false,
                        Message = "Invalid refresh token"
                    };
                }

                if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)//if refresh token expired, have to login again
                {
                    return new AuthResponseDTO
                    {
                        Succeeded = false,
                        Message = "Refresh token has expired"
                    };
                }
                //access token is how long the user can access the page while refresh token is to keep the logging for a few days, if not opened the page within 7 days, then new refresh token is to be generated by logging in again.
                var roles = await _userManager.GetRolesAsync(user);
                var newAccessToken = GenerateAccessToken(user, roles.ToList());
                var newRefreshToken = GenerateRefreshToken();//refresh token is generated again to prevent reuse of the same refresh token, which is a security risk
                var newRefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = newRefreshTokenExpiryTime;
                await _userManager.UpdateAsync(user);

                return new AuthResponseDTO
                {
                    Succeeded = true,
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresIn = DateTime.UtcNow.AddHours(1),
                    Message = "Token refreshed successfully"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDTO
                {
                    Succeeded = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<bool> RevokeTokenAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return false;

                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = DateTime.MinValue;
                await _userManager.UpdateAsync(user);

                return true;
            }
            catch
            {
                return false;
            }
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