using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using RealWorldRest.Common;
using RealWorldRest.Common.Data;

namespace RealWorldRest.WebApi.Controllers {

    [EnableCors(origins: "http://www.herobook.local", headers: "*", methods: "*")]
    public class ProfilesController : ApiController {
        private readonly IDatabase db;

        public ProfilesController() {
            this.db = new DemoDatabase();
        }

        // GET /profiles
        public object Get(int index = 0, int count = 10) {
            var items = db.ListProfiles().Skip(index).Take(count);
            var total = db.CountProfiles();
            var _links = Hal.Paginate("/profiles", index, count, total);
            return new {
                _links,
                items
            };
        }

    }
}
