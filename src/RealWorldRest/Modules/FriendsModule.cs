using Nancy;
using Nancy.ModelBinding;
using RealWorldRest.Data;
using RealWorldRest.Data.Entities;

namespace RealWorldRest.Modules
{
    public class FriendsModule : NancyModule {

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