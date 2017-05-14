using System;
using System.Dynamic;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using RealWorldRest.Common;
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

    private dynamic GetProfile(string username) {
      var profile = db.LoadProfile(username);      
      return profile;
    }

    private dynamic GetProfiles() {
      var index = (int?)Request.Query["index"] ?? 0;
      var count = 10;
      var profiles = db.ListProfiles().Skip(index).Take(count);
      var total = db.CountProfiles();
      var links = Hal.Paginate("/profiles", index, count, total);
      return new {
        _links = links,
        items = profiles
      };
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


//.ToDynamic();
//profile._links = new {
//                friends = new {
//                    href = $"/profiles/{username}/friends"
//                }
//            };
//            dynamic _embedded = new ExpandoObject();
//            if (Request.Query["expand"] == "friends") {
//                _embedded.friends = GetFriends(username);
//            }
//            profile._embedded = _embedded;
