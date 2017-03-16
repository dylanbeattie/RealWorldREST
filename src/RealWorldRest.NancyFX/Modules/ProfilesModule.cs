using System.Dynamic;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using RealWorldRest.Common.Data;
using RealWorldRest.Common.Data.Entities;


namespace RealWorldRest.NancyFX.Modules {
    public class ProfilesModule : NancyModule {
        private readonly IDatabase db;

        public ProfilesModule(IDatabase db) {
            this.db = db;
            Get["/profiles"] = _ => GetProfiles();
            Get["/profiles/{username}"] = args => GetProfile(args.username);
            Get["/profiles/{name}/friends"] = args => GetFriends(args.name);

            Post["/profiles"] = args => PostProfile(args);
            Post["/profiles/{name}/friends"] = args => PostFriendship(args);

        }


        private dynamic GetProfiles() {
            var index = (int?)Request.Query["index"] ?? 0;
            var count = 10;
            var profiles = db.ListProfiles().Skip(index).Take(count);
            var total = db.CountProfiles();
            var links = Paginate("/profiles", index, count, total);
            return new {
                _links = links,
                items = profiles
            };
        }

        private dynamic Href(string url) {
            return new { Href = url };
        }


        private dynamic Paginate(string path, int index, int count, int total) {
            dynamic links = new ExpandoObject();
            var maxIndex = total - 1;
            links.first = Href($"{path}?index=0");
            links.final = Href($"{path}?index={maxIndex - maxIndex % count}");
            if (index > 0) {
                links.last = Href($"{path}?index={index - count}");
            }
            if (index + count < maxIndex) {
                links.next = Href($"{path}?index={index + count}");
            }
            return (links);
        }

        private dynamic GetProfile(string username) {
            var profile = db.LoadProfile(username).ToDynamic();
            profile._links = new {
                friends = new {
                    href = $"/profiles/{username}/friends"
                }
            };
            dynamic _embedded = new ExpandoObject();
            if (Request.Query["expand"] == "friends") {
                _embedded.friends = GetFriends(username);
            }
            profile._embedded = _embedded;
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

        public dynamic GetFriends(string username) {
            return db.LoadFriends(username);
        }

    }
}
