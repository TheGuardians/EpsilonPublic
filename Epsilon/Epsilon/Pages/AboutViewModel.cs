using System.Diagnostics;

namespace Epsilon.Pages
{
    class AboutViewModel
    {
        public string Name { get; }
        public string Version { get; }

        public AboutViewModel()
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(typeof(AboutView).Assembly.Location);

            Name = versionInfo.ProductName;
            Version = versionInfo.ProductVersion;
        }
    }
}
