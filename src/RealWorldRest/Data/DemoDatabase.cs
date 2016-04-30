using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using RealWorldRest.Data.Entities;

namespace RealWorldRest.Data {
    public class DemoDatabase : IDatabase {
        private const string DATA_PATH = @"C:\projects\RealWorldRest\data\";

        private static string Qualify(string filePath) {
            return (Path.Combine(DATA_PATH, filePath));
        }
        private static readonly IList<Profile> profiles;

        static DemoDatabase() {
            profiles = JsonConvert.DeserializeObject<IList<Profile>>(File.ReadAllText(Qualify("profiles.json")));
        }

        public IEnumerable<Profile> ListProfiles() {
            return (profiles);
        }

        public void SaveProfile(Profile profile) {
            if (profile.ProfileGuid == default(Guid)) profile.ProfileGuid = Guid.NewGuid();
            profiles.Add(profile);
            File.WriteAllText(Qualify("profiles.json"), JsonConvert.SerializeObject(profiles, Formatting.Indented));
        }
    }
}