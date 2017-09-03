using System.Reflection;

namespace Maple
{
    public class VersionService : IVersionService
    {
        private readonly AssemblyName _assemblyName;

        public VersionService()
        {
            _assemblyName = Assembly.GetExecutingAssembly().GetName();
        }

        public string Get()
        {
            var version = _assemblyName.Version;
            return $"v{version.Major}.{version.Minor}.{version.Revision}";
        }
    }
}
