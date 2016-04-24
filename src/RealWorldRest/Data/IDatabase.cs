using System.Collections.Generic;
using RealWorldRest.Data.Entities;

namespace RealWorldRest.Data {
    public interface IDatabase {
        IEnumerable<Profile> ListProfiles();
        void SaveProfile(Profile profile);
    }
}