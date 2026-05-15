using EcommerceApplication.DTO;
using EcommerceApplication.Models;

namespace EcommerceApplication.Services.Interfaces
{

	public interface IAuthService
	{
		Task RegisterAsync(RegisterDTO dto);
		Task<string> LoginAsync(LoginDTO dto);
	}
}