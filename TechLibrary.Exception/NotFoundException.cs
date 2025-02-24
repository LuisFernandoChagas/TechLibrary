using System.Net;

namespace TechLibrary.Exception;

public class NotFoundException : TechLibraryException {
    public NotFoundException(string message) : base(message) { }

    public override List<string> GetErrorMessages() {
        return [Message];
    }

    public override HttpStatusCode GetStatusCode() {
        return HttpStatusCode.NotFound;
    }
}
