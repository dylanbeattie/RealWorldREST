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
      db = new DemoDatabase();
    }

    public object Get(int index = 0) {
      var RESULTS_PER_PAGE = 10;
      var profiles = db.ListProfiles()
        .Skip(index)
        .Take(RESULTS_PER_PAGE);

      var total = db.CountProfiles();

      var result = new {
        _links = Hal.Paginate("/profiles", index, RESULTS_PER_PAGE, total),
        items = profiles
      };
      return result;
    }
  }
}
