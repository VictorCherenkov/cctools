using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace CCTools.Platform
{
    public class Platform
    {
        private readonly IUnityContainer m_container;

        public Platform()
        {
            m_container = new UnityContainer();
        }

        public void AddModule<T>(T module)
        {
            m_container.RegisterInstance(module);
        }

        public void AddModule<T>()
        {
            m_container.RegisterType<T>();
        }

        public T Resolve<T>()
        {
            return m_container.Resolve<T>();
        }

        public void LoadConfiguration()
        {
            LoadConfiguration("RealContainer");
        }

        public void LoadFakeConfiguration()
        {
            LoadConfiguration("FakeContainer");
        }

        private void LoadConfiguration(string containerName)
        {
            ((UnityConfigurationSection)ConfigurationManager
                .GetSection("unity"))
                .Containers[containerName]
                .Configure(m_container);
        }
    }
}