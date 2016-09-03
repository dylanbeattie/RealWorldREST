using System;
using System.Collections.Generic;
using RealWorldRest.Data.Entities;

namespace RealWorldRest.Data {
    public interface IDatabase {
        IEnumerable<Profile> ListProfiles(int index, out int total);
        void CreateProfile(Profile profile);
        Profile LoadProfile(string username);
        IEnumerable<Profile> LoadFriends(string username);
        void CreateFriendship(string username1, string username2);
    }
}