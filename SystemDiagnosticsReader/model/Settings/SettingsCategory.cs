using System.Collections.Generic;

namespace SystemDiagnosticsReader.model.Settings
{
    public class SettingsCategory
    {
        /// <summary>
        /// Category name
        /// </summary>
        public string Name;

        /// <summary>
        /// List of instances of category
        /// </summary>
        public List<SettingsInstance> Instances;
    }
}
