using System;

namespace RealWorldRest.Common.Data.Entities {
  public class StatusUpdate {
    public string Username { get; set; }
    public string Comment { get; set; }
    public DateTime PostedAt { get; set; }
  }
}
