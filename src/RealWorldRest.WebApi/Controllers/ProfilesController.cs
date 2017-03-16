using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        // GET api/values
        public dynamic Get() {
            return db.ListProfiles();
        }

        // GET api/values/5
        public string Get(int id) {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value) {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE api/values/5
        public void Delete(int id) {
        }
    }
}
