using Nancy;

namespace RealWorldRest.Modules {
    public class IndexModule : NancyModule {
        public IndexModule() {
            Get["/"] = parameters => {
                return View["index"];
            };
        }
    }
}