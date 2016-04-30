using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWorldRest.Data.Entities {

    public class Profile {
        public Profile() {}

        public Profile(Guid profileGuid, string userName, string fullName) {
            ProfileGuid = profileGuid;
            UserName = userName;
            FullName = fullName;
        }

        public Guid ProfileGuid { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
