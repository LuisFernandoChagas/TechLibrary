using System.Net;

namespace TechLibrary.Exception;

public class InvalidLoginException : TechLibraryException {
    public InvalidLoginException() : base("Email e/ou senha inválidos.") { }

    public override List<string> GetErrorMessages() {
        return [Message];
    }

    public override HttpStatusCode GetStatusCode() {
        return HttpStatusCode.Unauthorized;
    }
}
