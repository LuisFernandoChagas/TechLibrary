using System.Net;

namespace TechLibrary.Exception;

public class InvalidLoginException : TechLibraryException {
    public override List<string> GetErrorMessages() {
        return ["Email e/ou senha invalidos."];
    }

    public override HttpStatusCode GetStatusCode() {
        return HttpStatusCode.Unauthorized;
    }
}
