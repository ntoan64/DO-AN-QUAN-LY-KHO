namespace QuanLyKho.API.Models
{
    public class UserAccount
    {
        public int UserID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }      // nhận từ client (WebUI) khi đăng ký/đăng nhập
        public string? Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class LoginResponse
    {
        public string? Username { get; set; }
        public string? Role { get; set; }
        public string Message { get; set; } = "";
    }
}
