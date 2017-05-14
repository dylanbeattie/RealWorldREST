using System.Collections.Generic;
using RealWorldRest.Common.Data.Entities;

namespace RealWorldRest.Common.Data {
  public interface IDatabase {
    IEnumerable<Profile> ListProfiles();
    int CountProfiles();
    void CreateProfile(Profile profile);
    Profile LoadProfile(string username);
    IEnumerable<Profile> LoadFriends(string username);
    void CreateFriendship(string username1, string username2);
  }
}
