namespace RealWorldRest.Common.Data.Entities {
  public class Friendship {
    public Friendship() { }

    public Friendship(params string[] names) {
      Names = names;
    }

    public string[] Names { get; set; }
  }
}
