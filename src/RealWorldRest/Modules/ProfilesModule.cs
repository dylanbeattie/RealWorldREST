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
                var profiles = db.ListProfiles().Select(p => {
                    var d = p.ToDynamic();
                    d._links = new {
                        self = new {
                            href = String.Format("/profiles/{0}", p.Username)
                        }
                    };
                    return (d);
                });
                return new {
                    items = profiles,
                    count = profiles.Count(),
                    index = 0,
                    _links = new {
                        self = new {
                            href = "/profiles"
                        }
                    }
                };
            };

            Get["/profiles/{username}"] = args => {
                var profile = (object)db.LoadProfile(args.username);
                var d = profile.ToDynamic();
                d._links = new {
                    self = new { href = String.Format("/profiles/{0}", d.Username) },
                    friends = new { href = String.Format("/profiles/{0}/friends", d.Username) }
                };
                if (Request.Query["expand"] == "friends") {
                    d._embedded = new {
                        friends = db.LoadFriends(args.username)
                    };
                }
                return (d);
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

    public class FriendsModule : ModuleBase {

        private readonly IDatabase db = new DemoDatabase();

        public FriendsModule() {
            Get["/profiles/{name}/friends"] = args => db.LoadFriends(args.Name);

            Post["/profiles/{name}/friends"] = args => {
                var profile1 = db.LoadProfile(args.Name);
                var profile2 = this.Bind<Profile>();
                db.CreateFriendship(profile1.Username, profile2.Username);
                var result = (Response)HttpStatusCode.Created;
                return (result);
            };
        }
    }
}