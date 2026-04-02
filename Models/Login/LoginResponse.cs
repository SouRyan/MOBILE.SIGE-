namespace MOBILE.SIGE.Models.Login
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public int? IdUsuario { get; set; }
        public string? NomeUsuario { get; set; }
        public string? Email { get; set; }
        public int? TipoUsuario { get; set; }
        public string? Message { get; set; }
        public string? Cargo { get; set; }
        public string? Token { get; set; }
    }
}
