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

            Get["/profiles"] = args => {
                int total = 0;
                int index = Request.Query["index"];
                var profiles = db.ListProfiles(index, out total);
                return
                    new {
                        _links = new {
                            self = new {
                                href = "/profiles?index=" + index
                            },
                            next = new {
                                href = "/profiles?index=" + (index + 1)
                            },
                            previous = new {
                                href = "/profiles?index=" + (index - 1)

                            }
                        },
                        count = DemoDatabase.PAGE_SIZE,
                        total,
                        index = index,
                        items = profiles
                    };
            };

            Get["/profiles/{username}"] = args => {
                var profile = db.LoadProfile((string)args.username);
                var jsonProfile = profile.ToDynamic();
                jsonProfile._links = new {
                    friends = new {
                        href = String.Format("/profiles/{0}/friends", args.username)


                    }
                };
                jsonProfile._embedded = new {
                    friends = db.LoadFriends(args.username)
                };
                return (jsonProfile);
            };

            Post["/profiles"] = args => {
                var profile = this.Bind<Profile>();
                var existing = db.LoadProfile(profile.Username);
                if (existing != null) return (Conflict("Username not available"));
                db.CreateProfile(profile);
                var result = (Response)HttpStatusCode.Created;
                result.Headers.Add("Location", String.Format("/profiles/{0}", profile.Username));
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