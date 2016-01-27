using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SystemDiagnostics.model;
using SystemDiagnosticsReader.model;
using SystemDiagnosticsReader.model.Settings;
using Newtonsoft.Json.Linq;

namespace SystemDiagnosticsReader
{
    public class SystemDiagnosticsReader
    {
        /// <summary>
        /// A list of all performance counters available.
        /// </summary>
        public static List<PerformanceCounter> PerformanceCounters = new List<PerformanceCounter>();

        /// <summary>
        /// Fetches all performance counters available and stores a reference in a static list.
        /// </summary>
        public static void InitCounters()
        {
            var categories = PerformanceCounterCategory.GetCategories();

            foreach (var category in categories)
            {
                var instances = category.GetInstanceNames();

                foreach (var instance in instances)
                {
                    try
                    {
                        var counters = category.GetCounters(instance);
                        PerformanceCounters.AddRange(counters);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Reads diagnostics settings from JSON file.
        /// </summary>
        /// <param name="path">File path to JSON settings file.</param>
        /// <returns>Diagnostics settings read from JSON file.</returns>
        public static SystemDiagnosticSettings ReadDiagnosticSettings(string path)
        {
            var o1 = JObject.Parse(System.IO.File.ReadAllText(path));

            return o1.ToObject<SystemDiagnosticSettings>();
        }

        /// <summary>
        /// Reads all counters specified in JSON settings file.
        /// </summary>
        /// <param name="path">File path to JSON settings file.</param>
        /// <returns>List of all read counters.</returns>
        public static List<ISystemDiagnosticsValue> ReadCounters(string path)
        {
            var settings = ReadDiagnosticSettings(path);
            return ReadCounters(settings);
        }

        /// <summary>
        /// Reads all counters specified in settings.
        /// </summary>
        /// <param name="settings">Settings which specify counters to read.</param>
        /// <returns>List of all read counters.</returns>
        public static List<ISystemDiagnosticsValue> ReadCounters(SystemDiagnosticSettings settings)
        {
            if (PerformanceCounters.Count <= 0)
            {
                InitCounters();
            }

            var diagnosticValues = new List<ISystemDiagnosticsValue>();

            settings.Categories.ForEach(settingsCategory =>
            {
                if (PerformanceCounterCategory.Exists(settingsCategory.Name)) {
                    var categoryCounters = PerformanceCounters
                        .Where(pc => pc.CategoryName == settingsCategory.Name);
                    
                    settingsCategory.Instances.ForEach(settingsInstance =>
                    {
                        if (PerformanceCounterCategory.InstanceExists(settingsInstance.Name, settingsCategory.Name)) {
                            var instanceCounters = categoryCounters.Where(pc => pc.InstanceName == settingsInstance.Name);

                            settingsInstance.Counters.ForEach(settingsCounter =>
                            {
                                var counter = instanceCounters.FirstOrDefault(pc => pc.CounterName == settingsCounter);

                                if (counter != null)
                                {
                                    diagnosticValues.Add(CounterToSystemDiagnosticsValue(counter));
                                }
                                else
                                {
                                    diagnosticValues.Add(new SystemDiagnosticsValue
                                    {
                                        Name = "Counter does not exist: " + settingsCategory.Name + " " + settingsInstance.Name + " " + settingsCounter
                                    });
                                }
                            });
                        }
                        else
                        {
                            diagnosticValues.Add(new SystemDiagnosticsValue
                            {
                                Name = "Instance does not exist: " + settingsCategory.Name + " " + settingsInstance.Name
                            });                                
                        }
                    });           
                }
                else
                {
                    diagnosticValues.Add(new SystemDiagnosticsValue
                    {
                        Name = "Category does not exist: " + settingsCategory.Name
                    });
                }
            });

            return diagnosticValues;
        }

        /// <summary>
        /// Reads all counters available on local platform.
        /// </summary>
        /// <returns>List of all read counters.</returns>
        public static List<ISystemDiagnosticsValue> ReadAllCounters()
        {
            if (PerformanceCounters.Count <= 0)
            {
                InitCounters();
            }

            var diagnosticValues = new List<ISystemDiagnosticsValue>();
            diagnosticValues.AddRange(PerformanceCounters.Select(CounterToSystemDiagnosticsValue));

            return diagnosticValues;
        }

        /// <summary>
        /// Create a SystemDiagnosticsValue from PerfomanceCounter.
        /// </summary>
        /// <param name="counter">Counter to create from.</param>
        /// <returns>ISystemDiagnosticsValue Key-Value pair of counter.</returns>
        public static ISystemDiagnosticsValue CounterToSystemDiagnosticsValue(PerformanceCounter counter)
        {
            try
            {
                return new SystemDiagnosticsValue
                {
                    Name = "\\" + counter.CategoryName + "(" + counter.InstanceName + ")\\" + counter.CounterName,
                    Value = counter.NextValue()
                };
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
        }
    }
}
