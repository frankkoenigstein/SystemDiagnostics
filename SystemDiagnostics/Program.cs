using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemDiagnostics.model;
using Newtonsoft.Json.Linq;

namespace SystemDiagnostics
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                List<ISystemDiagnosticsValue> diagnosticValues;

                //const string path = @"s:\fkoenigstein\Diagnostic.json";
                if (args.Length > 0)
                {
                    var path = args[0];
                    diagnosticValues = SystemDiagnosticsReader.SystemDiagnosticsReader.ReadCounters(path);
                }
                else
                {
                    diagnosticValues = SystemDiagnosticsReader.SystemDiagnosticsReader.ReadAllCounters();
                }

                diagnosticValues.ForEach(dv =>
                {
                    if (dv != null)
                    {
                        Console.WriteLine(dv.Name + ": " + dv.Value);
                    }
                });

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
