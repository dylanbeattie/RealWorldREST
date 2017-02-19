using System;
using System.Collections.Generic;
using System.Web.ModelBinding;
using Nancy;

namespace RealWorldRest.Modules {
    public class IndexModule : NancyModule {
        public IndexModule() {
            Get["/"] = _ => new {
                Greeting = new {
                    en = "Hello, World",
                    ru = "Здравствуй, мир!"
                }
            };
        }
    }
}