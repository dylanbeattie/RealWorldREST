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