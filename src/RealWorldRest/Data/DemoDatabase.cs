using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using RealWorldRest.Data.Entities;

namespace RealWorldRest.Data {
    public class DemoDatabase : IDatabase {
        private static readonly IList<Profile> profiles = new List<Profile> {
            new Profile {
                Id = "alice",
                Name = "Alice "
            }
        };

        public IEnumerable<Profile> ListProfiles() {
            return (profiles);
        }

        public void SaveProfile(Profile profile) {
            profiles.Add(profile);
        }
    }
}