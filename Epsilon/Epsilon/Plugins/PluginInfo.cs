
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Host
{
    public class PluginInfo : IEquatable<PluginInfo>
    {
        public FileInfo File { get; set; }
        public Assembly Assembly { get; set; }
        public HashSet<PluginInfo> Dependencies { get; set; } = new HashSet<PluginInfo>();


        public bool Equals(PluginInfo other)
        {
            return Assembly == other.Assembly;
        }
    }
}
