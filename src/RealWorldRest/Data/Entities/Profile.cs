using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealWorldRest.Data.Entities {

    public class Profile {
        public Profile() { }

        public Profile(string username, string name) {
            Username = username;
            Name = name;
        }

        public string Name { get; set; }
        public string Username { get; set; }
    }

    public class Friendship {
        public Friendship() { }

        public Friendship(params string[] names) {
            Names = names;
        }
        public string[] Names { get; set; }
    }


    public class StatusUpdate {
        public string Username { get; set; }
        public string Comment { get; set; }
        public DateTime PostedAt { get; set; }
    }

    public class Photo {
        public string Filename { get; set; }
        public string Username { get; set; }
        public string Caption { get; set; }
        public DateTime PostedAt { get; set; }
    }
}
