namespace Propeus.Module.Package.Contracts
{
    public interface IModuleDependeceContract
    {
        void AddDependencePackage(string dependenceName, string fullPathModule);
        void RegisterDependencePackage(string dependenceName, string fullPathDependence);
        void RemoveDependencePackAge(string dependenceName, string fullPathModule);
    }
}