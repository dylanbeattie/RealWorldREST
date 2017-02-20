using Nancy;
using Nancy.ModelBinding;
using RealWorldRest.Data;
using RealWorldRest.Data.Entities;

namespace RealWorldRest.Modules {
    public class ProfilesModule : NancyModule {
        private readonly IDatabase db;

        public ProfilesModule(IDatabase db) {
            this.db = db;
            Get["/profiles"] = _ => GetProfiles();
            Get["/profiles/{username}"] = args => GetProfile(args.username);
            Post["/profiles"] = args => PostProfile(args);
            ;
            Post["/profiles/{name}/friends"] = args => PostFriendship(args);
        }

        private dynamic GetProfiles() {
            return db.ListProfiles();
        }

        private dynamic GetProfile(string username) {
            var profile = db.LoadProfile(username);
            return profile;
        }

        private dynamic PostProfile(dynamic args) {
            var profile = this.Bind<Profile>();
            var existing = db.LoadProfile(profile.Name);
            if (existing != null) {
                return new Response {
                    StatusCode = HttpStatusCode.Conflict,
                    ReasonPhrase = "That username is not available"
                };
            }

            db.CreateProfile(profile);
            var result = (Response)HttpStatusCode.Created;
            result.Headers.Add("Location", $"/profiles/{profile.Name}");
            return result;
        }

        public dynamic PostFriendship(dynamic args) {
            var friend = this.Bind<Profile>();
            var person = db.LoadProfile(args.name);
            db.CreateFriendship(person.Username, friend.Username);
            var result = (Response)HttpStatusCode.Created;
            return result;
        }
    }
}
