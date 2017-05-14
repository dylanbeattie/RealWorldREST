using Nancy;
using Nancy.Conventions;

namespace RealWorldRest.NancyFX {
  public class Bootstrapper : DefaultNancyBootstrapper {
    // The bootstrapper enables you to reconfigure the composition of the framework,
    // by overriding the various methods and properties.
    // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

    protected override void ConfigureConventions(NancyConventions nancyConventions) {
      base.ConfigureConventions(nancyConventions);
      Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("explorer", "explorer"));
    }
  }
}
