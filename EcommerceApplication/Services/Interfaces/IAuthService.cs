using EcommerceApplication.DTO;

namespace EcommerceApplication.Services.Interfaces
{

	public interface IAuthService
	{
		Task<AuthResult> RegisterAsync(RegisterDTO dto);
		Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
		Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO dto);
		Task<bool> RevokeTokenAsync(string userID);
    }
}