using System;
using System.Collections.Generic;
using System.Web.ModelBinding;
using Nancy;

namespace RealWorldRest.Modules {
    public class IndexModule : ModuleBase {
        public IndexModule() {
            Get["/"] = _ => new {
                Date = DateTime.Now,
                Greeting = "Hello DDD11"
            };
        }
    }
}