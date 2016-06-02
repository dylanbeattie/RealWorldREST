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

            Get["/profiles"] = _ => db.ListProfiles();

            Get["/profiles/{username}"] = args => {
                var profile = db.LoadProfile((string)args.username);
                var qs = (string)Request.Query["expand"];
                var result = profile.ToDynamic();
                result._links = new {
                    self = new {
                        href = "http://restdemo/profiles/" + result.Username
                    },
                    friends = new {
                        href = String.Format("http://restdemo/profiles/{0}/friends", result.Username)
                    }
                };

                if (qs == "friends") {
                    result._embedded = new {
                        friends = db.LoadFriends(result.Username)

                    };
                }


                return (result);
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