using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Newtonsoft.Json;
using RealWorldRest.Data.Entities;

namespace RealWorldRest.Data {
    public class DemoDatabase : IDatabase {
        private const string DATA_PATH = @"C:\projects\RealWorldRest\data\";

        private static string Qualify(string filePath) {
            return (Path.Combine(DATA_PATH, filePath));
        }
        private static readonly IList<Profile> profiles;
        private static readonly IList<Friendship> friendships;

        private static T ReadData<T>(string filename) {
            try {
                return (JsonConvert.DeserializeObject<T>(File.ReadAllText(Qualify(filename + ".json"))));
            } catch (FileNotFoundException) {
                return (default(T));
            }
        }

        private void Save() {
            WriteData("profiles", profiles);
            WriteData("friendships", friendships);
        }
        private void WriteData(string filename, object data) {
            File.WriteAllText(Qualify(filename + ".json"), JsonConvert.SerializeObject(data, Formatting.Indented));
        }

        static DemoDatabase() {
            profiles = ReadData<IList<Profile>>("profiles") ?? new List<Profile>();
            friendships = ReadData<IList<Friendship>>("friendships") ?? new List<Friendship>();
        }

        public IEnumerable<Profile> ListProfiles() {
            return (profiles);
        }

        public void CreateProfile(Profile profile) {
            if (LoadProfile(profile.Username) != null) throw new ArgumentException("That username is not available");
            profiles.Add(profile);

        }

        public Profile LoadProfile(String username) {
            return (profiles.FirstOrDefault(p => p.Username == username));
        }

        public void CreateFriendship(string username1, string username2) {
            if (friendships.Any(f => f.Names.Contains(username1) && f.Names.Contains(username1))) return;
            var friendship = new Friendship(username1, username2);
            friendships.Add(friendship);
            Save();

        }
        public IEnumerable<Profile> LoadFriends(String username) {
            var guids = friendships.Where(f => f.Names.Contains(username))
                .SelectMany(f => f.Names)
                .Distinct()
                .Where(g => g != username);
            return (guids.Select(LoadProfile));
        }
    }
}