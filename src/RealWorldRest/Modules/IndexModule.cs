using System.Collections.Generic;
using System.Web.ModelBinding;
using Nancy;

namespace RealWorldRest.Modules {
    public class IndexModule : ModuleBase {
        public IndexModule() {
            Get["/"] = _ => new {
                _links = new {
                    self = new {
                        href = "/"
                    }
                },
                greeting = "Hello SmartDevsUG"
            };

        }
    }


}