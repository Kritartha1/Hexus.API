namespace Hexus.Models.DTO
{
    public class RegisterUserRequestDto
    {
        public string Username { get; set; }
        public string Email {  get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string[] Roles { get; set; }
    }
}
