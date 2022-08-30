using System;
using System.Linq;

namespace VoiceChattingManagementServer.Core.Database.Manager
{
    public class RepositoryManager
    {


        public RepositoryManager()
        {

        }

        private void ScanAllAssembly()
        {
            var type = typeof(Repository);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
        }
    }
}
