using System.Web.Http;
using System.Web.Http.Cors;
using RealWorldRest.Common.Data;

namespace RealWorldRest.WebApi.Controllers {

    [EnableCors(origins: "http://www.herobook.local", headers: "*", methods: "*")]
    public class ProfilesController : ApiController {
        private readonly IDatabase db;

        public ProfilesController() {
            this.db = new DemoDatabase();
        }

        // GET /profiles
        public dynamic Get() {
            return db.ListProfiles();
        }

        //public dynamic Get(string username) {
        //    return db.LoadProfile(username);
        //}

        //// POST api/values
        //public void Post([FromBody]string value) {
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value) {
        //}

        //// DELETE api/values/5
        //public void Delete(int id) {
        //}
    }
}
