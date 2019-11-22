using System.Collections.Generic;

namespace Interfaces
{
    public interface IClearCaseHelper
    {
        /// <summary>
        /// Get the config spec for the given viewPath
        /// </summary>
        /// <param name="viewPath">View path</param>
        /// <returns>List constisting of the Config spec</returns>
        List<string> GetConfigSpec(string viewPath);

        /// <summary>
        /// Set the Config Spec for the given viewPath.
        /// </summary>
        /// <param name="configSpec">List consisting of the Config Spec</param>
        /// <param name="viewPath">View name</param>
        /// <returns>true if config spec was set successfully, else false</returns>        
        void SetConfigSpec(List<string> configSpec, string viewPath);
    }
}
