using System.Dynamic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using RealWorldRest.Common;
using RealWorldRest.Common.Data;

namespace RealWorldRest.WebApi.Controllers {
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProfilesController : ApiController {
        private readonly IDatabase db;

        public ProfilesController() {
            this.db = new DemoDatabase();
        }

        public object Get() {
            var profiles = db.ListProfiles();
            return profiles;
        }
    }
}
