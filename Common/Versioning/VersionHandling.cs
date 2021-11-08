using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace ABB.InSecTT.Common
{
    public static class VersionDisplay
    {
        public static void VersionWrite()
        {
            Assembly assemblyExecution = Assembly.GetExecutingAssembly();
            var githubVersion = FileVersionInfo.GetVersionInfo(assemblyExecution.Location);
            var name = Assembly.GetEntryAssembly().GetName().Name;
            var version = Assembly.GetEntryAssembly().GetName().Version;
            var buildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(
            TimeSpan.TicksPerDay * version.Build +
            TimeSpan.TicksPerSecond * 2 * version.Revision));
            Console.WriteLine($"Running InSecTT { name }, version: { version }. Build date: { buildDateTime }"  );
            Console.WriteLine($"Github Actions Run ID: { githubVersion.ProductVersion }");
        } 

    }
}
