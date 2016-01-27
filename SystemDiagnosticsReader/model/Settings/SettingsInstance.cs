using System.Collections.Generic;

namespace SystemDiagnosticsReader.model.Settings
{
    public class SettingsInstance
    {
        /// <summary>
        /// Instance name
        /// </summary>
        public string Name;

        /// <summary>
        /// List of counters of instance
        /// </summary>
        public List<string> Counters;
    }
}
