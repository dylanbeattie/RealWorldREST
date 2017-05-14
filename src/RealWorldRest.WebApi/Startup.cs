using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using RealWorldRest.WebApi;

[assembly: OwinStartup(typeof(Startup))]

namespace RealWorldRest.WebApi {
  public partial class Startup {
    public void Configuration(IAppBuilder app) {
      //   ConfigureAuth(app);
    }
  }
}
