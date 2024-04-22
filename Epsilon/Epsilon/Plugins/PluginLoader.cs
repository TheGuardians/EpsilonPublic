using EpsilonLib.Core;
using EpsilonLib.Logging;
using Host;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Epsilon
{
    public class PluginLoader
    {
        private List<PluginInfo> _plugins = new List<PluginInfo>();
        private List<PluginInfo> _loadQueue = new List<PluginInfo>();

        public IEnumerable<PluginInfo> Plugins => _loadQueue;

        public void LoadPlugins()
        {
            var pluginDir = new DirectoryInfo("plugins");
            pluginDir.Create();

            foreach (var pluginFolder in pluginDir.GetDirectories())
            {
                var pluginDll = new FileInfo(Path.Combine(pluginFolder.FullName, $"{pluginFolder.Name}.dll"));
                if (!pluginDll.Exists)
                {
                    Logger.Warn($"Plugin folder encountered with no matching dll. expected: '{pluginDll}'");
                    continue;
                }

                if (!TryLoadPluginAssembly(pluginDll, out Assembly pluginAssembly))
                    continue;

                var pluginInfo = new PluginInfo()
                {
                    File = pluginDll,
                    Assembly = pluginAssembly,
                };

                _plugins.Add(pluginInfo);
            }

            ResolveDependencies();
            BuildLoadQueue();
        }

        private static bool TryLoadPluginAssembly(FileInfo pluginDll, out Assembly pluginAssembly)
        {
            pluginAssembly = null;
            try
            {
                pluginAssembly = Assembly.LoadFrom(pluginDll.FullName);
                if (pluginAssembly.GetCustomAttribute<DisabledPluginAttribute>() != null)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to load plugin '{pluginDll}'.\nException:\n{ex}'");
            }

            return false;
        }

        private void ResolveDependencies()
        {
            foreach (var plugin in _plugins)
            {
                var referencedAssemblies = plugin.Assembly.GetReferencedAssemblies();
                foreach (var refAssembly in referencedAssemblies)
                {
                    var referencedPlugin = _plugins.FirstOrDefault(p => p.Assembly.GetName().FullName == refAssembly.FullName);
                    if (referencedPlugin != null)
                        plugin.Dependencies.Add(referencedPlugin);
                }
            }
        }

        private void BuildLoadQueue()
        {
            foreach (var plugin in _plugins)
                AddPluginToLoadQueue(plugin);
        }

        private void AddPluginToLoadQueue(PluginInfo plugin)
        {
            if (_loadQueue.Contains(plugin))
                return;

            foreach (var dependency in plugin.Dependencies)
                AddPluginToLoadQueue(dependency);

            _loadQueue.Add(plugin);
        }
    }
}
