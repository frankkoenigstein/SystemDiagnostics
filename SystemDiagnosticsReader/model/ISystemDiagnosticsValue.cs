using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemDiagnostics.model
{
    public interface ISystemDiagnosticsValue
    {
        string Name { get; set; }
        double Value { get; set; }
        string ValueString { get; set; }
    }
}
