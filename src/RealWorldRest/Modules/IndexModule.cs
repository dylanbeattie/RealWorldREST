using System;
using System.Diagnostics;
using Nancy;
using Nancy.ModelBinding;
using RealWorldRest.Data;
using RealWorldRest.Data.Entities;
using System.Linq;

namespace RealWorldRest.Modules {
    public class IndexModule : NancyModule {
        public IndexModule() {
            Get["/"] = parameters => {
                return View["index"];
            };
        }
    }

    public class ProfilesModule : NancyModule {
        private IDatabase db = new DemoDatabase();

        private dynamic ListProfiles() {
            var profiles = db.ListProfiles().Select(p => p.ToDynamic()).ToList();
            foreach (var profile in profiles) {
                profile.foo = "bar";
                profile.fnord = "big bag of badgers!";
                profile._links = new {
                    self = new { href = $"/profiles/{profile.Id}" }
                };
            };
            return new {
                _links = new {
                    _self = new { href = "/profiles" }
                },
                _embedded = profiles
            };

        }

        public ProfilesModule() {
            Get["/profiles"] = _ => ListProfiles();

            Post["/profiles"] = args => {
                var profile = this.Bind<Profile>();
                db.SaveProfile(profile);
                var result = (Response)HttpStatusCode.Created;
                result.Headers.Add("Location", $"/profiles/{profile.Id}");
                return (result);
            };
        }


    }
}