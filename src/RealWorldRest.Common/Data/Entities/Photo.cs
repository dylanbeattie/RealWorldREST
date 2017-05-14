using System;

namespace RealWorldRest.Common.Data.Entities {
  public class Photo {
    public string Filename { get; set; }
    public string Username { get; set; }
    public string Caption { get; set; }
    public DateTime PostedAt { get; set; }
  }
}
