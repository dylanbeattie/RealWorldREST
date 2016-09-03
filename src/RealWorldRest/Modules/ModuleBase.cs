using Nancy;

namespace RealWorldRest.Modules {
    public class ModuleBase : NancyModule {
        protected Response Conflict(string reason) {
            var result = (Response)HttpStatusCode.Conflict;
            result.ReasonPhrase = "Conflict (" + reason + ")";
            return (result);
        }
    }
}