namespace TechLibrary.Api.Infraestructure.Security.Cryptography {
    public class BCryptAlgorithm {
        public string HashPassword(string password) {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash) {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
