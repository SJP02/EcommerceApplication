namespace EcommerceApplication.DTO
{
    public class AuthResponseDTO
        {
            public bool Succeeded { get; set; }
            public string Message { get; set; }
            public string Token {  get; set; }
            public string RefreshToken {  get; set; }
            public DateTime ExpiresIn {  get; set; }
        }       
}