using System.Linq;
using System.Net.NetworkInformation;
using CCTools.Model;

namespace CCTools.Platform
{
    public class PlatformLoader
    {
        public MainModel Load()
        {
            Platform p = new Platform();
            if (IsClearCaseAvailable())
            {
                p.LoadConfiguration();
            }
            else
            {
                p.LoadFakeConfiguration();
            }
            p.AddModule<MainModel>();
            return p.Resolve<MainModel>();
        }

        private static bool IsClearCaseAvailable()
        {
            return !IsTheVaioNotebook();
        }
        private static bool IsTheVaioNotebook()
        {
            return NetworkInterface
                .GetAllNetworkInterfaces()
                .Any(ni => ni.Id == "{6DD0B22D-C026-4940-9700-1362E8BA5673}");
        }
    }
}
