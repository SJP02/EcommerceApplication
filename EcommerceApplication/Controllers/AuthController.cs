using EcommerceApplication.DTO;
using EcommerceApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger= logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            try
            { 
            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            {
                    _logger.LogWarning("Registration attempt with missing email or password");
                    return BadRequest(new { error = "Email and password are required" });
            }

            var result = await _authService.RegisterAsync(dto);

            if (!result.Succeeded)
            {
                    _logger.LogWarning("Registration failed for email {Email}: {Message}", dto.Email, result.Message);
                    return BadRequest(new { error = result.Message });
            }
            _logger.LogInformation("User registered successfully with email {Email}", dto.Email);
            return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration");
                return BadRequest(new { error = "An error occurred during registration" });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            {
                _logger.LogWarning("Login failed due to invalid input. Email: {Email}",dto?.Email);
                return BadRequest(new { error = "Email and password are required" });
            }

            try
            {
                var result = await _authService.LoginAsync(dto);

                if (!result.Succeeded)
                {
                    _logger.LogWarning("Login failed for email {Email}: {Message}", dto.Email, result.Message);
                    return Unauthorized(new { error = result.Message });
                }
                _logger.LogInformation("User logged in successfully with email {Email}", dto.Email);
                return Ok(new
                {
                    token = result.Token,
                    refreshToken = result.RefreshToken,
                    expiresIn = result.ExpiresIn,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for email {Email}", dto.Email);
                return Unauthorized(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.RefreshToken))
            {
                _logger.LogWarning("Token refresh attempt with missing token or refresh token");
                return BadRequest(new { error = "Token and refresh token are required" });
            }

            try
            {
                var result = await _authService.RefreshTokenAsync(request);

                if (!result.Succeeded)
                {
                    _logger.LogWarning("Token refresh failed for token {Token}: {Message}", request.Token, result.Message);
                    return Unauthorized(new { error = result.Message });
                }
                _logger.LogInformation("Token refreshed successfully for user associated with token {Token}", request.Token);
                return Ok(new
                {
                    token = result.Token,
                    refreshToken = result.RefreshToken,
                    expiresIn = result.ExpiresIn,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during token refresh");
                return Unauthorized(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Logout attempt with missing user ID in claims");
                    return BadRequest(new { error = "User not found" });
                }

                var result = await _authService.RevokeTokenAsync(userId);

                if (!result)
                {
                    _logger.LogWarning("Logout failed for user ID {UserId}", userId);
                    return BadRequest(new { error = "Logout failed" });
                }
                _logger.LogInformation("User with ID {UserId} logged out successfully", userId);
                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during logout");
                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}