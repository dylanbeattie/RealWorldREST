using System;
using System.Diagnostics;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using RealWorldRest.Data;
using RealWorldRest.Data.Entities;

namespace RealWorldRest.Modules {
    public class ProfilesModule : ModuleBase {
        private readonly IDatabase db = new DemoDatabase();

        public ProfilesModule() {

            Get["/profiles"] = _ => {
                var profiles = db.ListProfiles();
                var result = new {
                    items = profiles,
                    count = profiles.Count(),
                    index = 0,
                    _links = new {
                        self = new {
                            href = "http://restdemo/profiles"
                        }
                    }
                };
                return (result);
            };

            Get["/profiles/{username}"] = args => {
                var expand = (string)Request.Query["expand"];
                var profile = db.LoadProfile((string)args.username).ToDynamic();
                profile._links = new {
                    self = new {
                        href = String.Format("http://restdemo/profiles/{0}", args.username)
                    },
                    friends = new {
                        href = String.Format("http://restdemo/profiles/{0}/friends", args.username)
                    }
                };
                if (expand == "friends") {
                    profile._embedded = new {
                        friends = db.LoadFriends(args.username)
                    };
                }

                return (profile);
            };


            Post["/profiles"] = args => {
                var profile = this.Bind<Profile>();
                var existing = db.LoadProfile(profile.Name);
                if (existing != null) return (Conflict("Username not available"));
                db.CreateProfile(profile);
                var result = (Response)HttpStatusCode.Created;
                result.Headers.Add("Location", String.Format("/profiles/{0}", profile.Name));
                return (result);
            };

            Post["/profiles/{name}/friends"] = args => {
                var friend = this.Bind<Profile>();
                var person = db.LoadProfile(args.name);
                db.CreateFriendship(person.Username, friend.Username);
                var result = (Response)HttpStatusCode.Created;
                return (result);
            };
        }
    }
}