namespace RealWorldRest.Common.Data.Entities {
  public class Profile {
    public Profile() { }

    public Profile(string username, string name) {
      Username = username;
      Name = name;
    }

    public string Name { get; set; }
    public string Username { get; set; }
  }
}
