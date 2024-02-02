using Propeus.Module.Abstract;

namespace Propeus.Module.Package.Models
{
    internal class PackageManifest : BaseModel
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public Dictionary<string, string> Dependences { get; set; }

        public PackageManifest() { }
    }
}
