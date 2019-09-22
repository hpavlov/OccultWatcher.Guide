using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using OccultWatcher.Guide.Properties;

namespace OccultWatcher.Guide
{
    public static class GuideConfig
    {
        public static string GuidePath;
        public static string GuideConfiguration;
        public static string CommandLineArguments;
        public static bool AlwaysNewInstance;

        public static void LoadRegistryConfig()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\OccultWatcher"))
            {
                if (key != null)
                {
                    GuidePath = (string)key.GetValue("GuidePath", Settings.Default.GuidePath);
                    GuideConfiguration = (string)key.GetValue("GuideConfiguration", Settings.Default.GuideConfiguration);
                    CommandLineArguments = (string)key.GetValue("CommandLineArguments", Settings.Default.CommandLineArguments);
                    AlwaysNewInstance = (string)key.GetValue("AlwaysNewInstance", Settings.Default.AlwaysNewInstance ? "true" : "false") == "true";
                }
            }
        }

        public static void SaveRegistryConfig()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\OccultWatcher"))
            {
                if (key != null)
                {
                    key.SetValue("GuidePath", GuidePath);
                    key.SetValue("GuideConfiguration", GuideConfiguration);
                    key.SetValue("CommandLineArguments", CommandLineArguments);
                    key.SetValue("AlwaysNewInstance", AlwaysNewInstance ? "true" : "false");
                }
                else
                {
                    Settings.Default.GuidePath = GuidePath;
                    Settings.Default.GuideConfiguration = GuideConfiguration;
                    Settings.Default.CommandLineArguments = CommandLineArguments;
                    Settings.Default.AlwaysNewInstance = AlwaysNewInstance;
                    Settings.Default.Save();
                }
            }
        }
    }
}
