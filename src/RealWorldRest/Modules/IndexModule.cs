using System;
using System.Diagnostics;
using System.Dynamic;
using Nancy;
using Nancy.ModelBinding;
using RealWorldRest.Data;
using RealWorldRest.Data.Entities;
using System.Linq;

namespace RealWorldRest.Modules {
    public class IndexModule : NancyModule {
        public IndexModule() {
            Get["/"] = parameters => {
                var api = new ApiIndex {
                    _links = new {
                        profiles = new { title = "List Profiles", href = "/profiles" }
                    }
                };
                return api;
            };
        }
    }

    public class ApiIndex {
        // ReSharper disable InconsistentNaming
        public dynamic _links { get; set; }
        public dynamic _embedded { get; set; }
        // ReSharper restore InconsistentNaming
    }

    public class ProfilesModule : NancyModule {
        private readonly IDatabase db = new DemoDatabase();

        private dynamic ListProfiles() {
            var profiles = db.ListProfiles().Select(p => p.ToDynamic()).ToList();
            foreach (var profile in profiles) {
                profile.foo = "bar";
                profile.fnord = "big bag of badgers!";
                profile._links = new {
                    self = new { href = String.Format("/profiles/{0}", profile.UserName) }
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
                result.Headers.Add("Location", String.Format("/profiles/{0}", profile.UserName));
                return (result);
            };
        }


    }
}