namespace Modules.Users.Application.Dtos.Requests
{
    public class ForgetPasswordRequestPhoneDto
    {
        public int CountryCode { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }
}