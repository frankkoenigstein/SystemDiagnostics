using SystemDiagnostics.model;

namespace SystemDiagnosticsReader.model
{
    /// <summary>
    /// Interface for system diagnostic values.
    /// </summary>
    public class SystemDiagnosticsValue : ISystemDiagnosticsValue
    {
        /// <summary>
        /// Name(key) of counter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Numeric value of counter
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// String value of counter
        /// </summary>
        public string ValueString { get; set; }
    }
}
