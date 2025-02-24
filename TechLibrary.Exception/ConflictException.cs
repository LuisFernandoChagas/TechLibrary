using System.Net;

namespace TechLibrary.Exception;

public class ConflictException : TechLibraryException {
    
    public ConflictException(string message) : base(message) { }

    public override List<string> GetErrorMessages() {
        return [Message] ;
    }

    public override HttpStatusCode GetStatusCode() {
        return HttpStatusCode.Conflict;
    }
}
