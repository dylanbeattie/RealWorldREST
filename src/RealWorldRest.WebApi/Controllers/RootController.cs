using System.Web.Http;
using System.Web.Http.Cors;

namespace RealWorldRest.WebApi.Controllers {
  [EnableCors(origins: "http://www.herobook.local", headers: "*", methods: "*")]
  public class RootController : ApiController {
    public object Get() {
      return new {
        _links = new {
          profiles = new {
            href = "/profiles"
          }
        }
      };
    }
  }
}
