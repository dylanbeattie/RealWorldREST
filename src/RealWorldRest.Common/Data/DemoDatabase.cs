using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using RealWorldRest.Common.Data.Entities;

namespace RealWorldRest.Common.Data {
  public class DemoDatabase : IDatabase {
    private const string DATA_PATH = @"D:\projects\RealWorldRest\data\";
    private static readonly IList<Profile> profiles;
    private static readonly IList<Friendship> friendships;

    static DemoDatabase() {
      profiles = ReadData<IList<Profile>>("profiles") ?? new List<Profile>();
      friendships = ReadData<IList<Friendship>>("friendships") ?? new List<Friendship>();
    }

    public IEnumerable<Profile> ListProfiles() {
      return profiles;
    }

    public void CreateProfile(Profile profile) {
      if (LoadProfile(profile.Username) != null) throw new ArgumentException("That username is not available");
      profiles.Add(profile);
      Save();
    }

    public Profile LoadProfile(string username) {
      return profiles.FirstOrDefault(p => p.Username == username);
    }

    public void CreateFriendship(string username1, string username2) {
      if (friendships.Any(f => f.Names.Contains(username1) && f.Names.Contains(username1))) return;
      var friendship = new Friendship(username1, username2);
      friendships.Add(friendship);
      Save();
    }

    public IEnumerable<Profile> LoadFriends(string username) {
      var friends = friendships.Where(f => f.Names.Contains(username))
        .SelectMany(f => f.Names)
        .Distinct()
        .Where(g => g != username);
      return friends.Select(LoadProfile);
    }

    public int CountProfiles() {
      return profiles.Count;
    }

    private static string Qualify(string filePath) {
      return Path.Combine(DATA_PATH, filePath);
    }

    private static T ReadData<T>(string filename) {
      try {
        return JsonConvert.DeserializeObject<T>(File.ReadAllText(Qualify(filename + ".json")));
      } catch (FileNotFoundException) {
        return default(T);
      }
    }

    private void Save() {
      WriteData("profiles", profiles);
      WriteData("friendships", friendships);
    }

    private void WriteData(string filename, object data) {
      File.WriteAllText(Qualify(filename + ".json"), JsonConvert.SerializeObject(data, Formatting.Indented));
    }
  }
}
