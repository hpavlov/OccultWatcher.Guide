using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using OccultWatcher.SDK;
using System.IO.Compression;
using System.Configuration;
using System.Diagnostics;
using OccultWatcher.Guide.Properties;

namespace OccultWatcher.Guide
{
    public class OWGuide : IOWAddin
    {
        public const string OWGuideAddinName = "OWGuide.addinName";
        public const string OWGuideStartGuide = "OWGuide.startGuide";
        public const string OWGuideConfigTitle = "OWGuide.configTitle";
        public const string OWGuideBrowse = "OWGuide.btnBrowse";
        public const string OWGuideOptional = "OWGuide.lblOptional";
        public const string OWGuideGuidePath = "OWGuide.lblGuidePath";
        public const string OWGuideAlwaysInNewInstance = "OWGuide.cbxAlwaysInNewInstance";
        public const string OWGuideCouldNotFindPath = "OWGuide.couldNotFindPath";
        public const string OWGuideAdditionalCommandLineArguments = "OWGuide.lblAdditionalArguments";
        public const string OWGuideAddinCredits = "OWGuide.aboutMessage";

        private readonly int START_GUIDE = 1;
        private readonly int CHANGE_LANGUAGE = 2;

        bool isEnglish = false;
        bool isGerman = false;

        private IOWHost m_HostInfo = null;
        private IOWResourceProvider m_ResourceProvider = null;
        private Process m_CurrentGuideProcess = null;

        public OWAddinAction[] ADDIN_ACTIONS  = null;

        internal string GetResourceString(string resourceName, string defaultValue)
        {
            string resourceValue = null;
            if (isEnglish)
                resourceValue = defaultValue;
            else if (isGerman)
                resourceValue = GetGermanResource(resourceName);

            if (resourceValue != null)
            {
                return resourceValue;
            }

            if (m_ResourceProvider == null)
                // Backwards compatibility with older versions of OW
                return defaultValue;
            else
                return m_ResourceProvider.GetResourceString(resourceName, defaultValue);
        }

        private string GetGermanResource(string resourceId)
        {
            if (resourceId == OWGuideAddinName)
                return "OW Guide Add-in";
            if (resourceId == OWGuideStartGuide)
                return "Zeige Ereignis in Guide";
            if (resourceId == OWGuideConfigTitle)
                return "OW Guide Add-in Konfiguration";
            if (resourceId == OWGuideBrowse)
                return "Durchsuchen ...";
            if (resourceId == OWGuideOptional)
                return "Optional: Name einer Guide-Konfiguration (genau 8 Buchstaben lang)";
            if (resourceId == OWGuideGuidePath)
                return "Dateipfad angeben um Guide zu starten";
            if (resourceId == OWGuideCouldNotFindPath)
                return "Pfad '{0}' konnte nicht gefunden werden";
            if (resourceId == OWGuideAlwaysInNewInstance)
                return "Ereignis in neuem Guide-Fenster öffnen";
            if (resourceId == OWGuideAdditionalCommandLineArguments)
                return "Optional: Zusätzliche Befehlszeilen-Argumente";

            return null;
        }

        void IOWAddin.InitializeAddin(IOWHost hostInfo)
        {
            m_HostInfo = hostInfo;
            m_ResourceProvider = hostInfo as IOWResourceProvider;

            SetLanguage(m_HostInfo.CurrentLanguage);

            ADDIN_ACTIONS = new OWAddinAction[]
            {
                new OWAddinAction(START_GUIDE, GetResourceString(OWGuideStartGuide, "Show Event in Guide"), OWAddinActionType.SelectedEventAction, Properties.Resources.Details.ToBitmap()),
                new OWAddinAction(CHANGE_LANGUAGE, "Language Change", OWAddinActionType.EventReceiver, null),
            };
        }

        void IOWAddin.FinalizeAddin()
        {
            // Do nothing special
        }

        OWAddinInfo IOWAddin.GetAddinInfo()
        {
            return new OWAddinInfo(GetResourceString(OWGuideAddinName, "OW Guide Add-in"), true, Properties.Resources.AddinImg.ToBitmap());
        }

        void IOWAddin.Configure(IWin32Window owner)
        {
            guideFrmConfig guideFrm = new guideFrmConfig(this);
            guideFrm.ShowDialog(owner);
        }

        IEnumerable<OWAddinAction> IOWAddin.SupportedActions()
        {
            return ADDIN_ACTIONS;
        }

        bool IOWAddin.ShouldDisplayAction(int actionId, IOWAsteroidEvent astEvent)
        {
            // Our actions will be always displayed if the path to Guide has been configured 
            return File.Exists(Settings.Default.GuidePath);
        }

        void IOWAddin.ExecuteAction(
            int actionId, 
            IOWAsteroidEvent astEvent, 
            IWin32Window owner, 
            OWEventArguments eventArgs)
        {
            if (actionId == START_GUIDE)
            {
                if (astEvent != null)
                {
                    IOWLocalEventData siteData = astEvent as IOWLocalEventData;
                    IOWAsteroidEvent2 evt2 = astEvent as IOWAsteroidEvent2;

                    if (siteData != null && evt2 != null && File.Exists(Settings.Default.GuidePath))
                    {
                        DateTime dateAndTime = siteData.EventTime;

                        string date = dateAndTime.ToString("yyyyMMdd HH:mm:ssUT");

                        // Read coordintes
                        double coord_RA = evt2.Occelmnt.StarRAHours;
                        double coord_DEC = evt2.Occelmnt.StarDEDeg;

                        // Convert RA-format from hh.hhhhh into hhmmss.ssss
                        if (coord_RA < 0.0)
                        {
                            coord_RA = coord_RA + 24.0;
                        }

                        // coordinate setting for Guide: hhmmss.ssss,dd.dddd
                        string coord = string.Concat(AstroConvert.ToStringValue(coord_RA, "HHMMSS.T"), ",", AstroConvert.ToStringValue(coord_DEC, "+DDMMSS"));


                        // Build Guide Options
                        // https://www.projectpluto.com/random.htm#command_line
                        // Format: -tyyyymmdd hh:mm:ss.ssss -ohhmmss.ssss,dd.ddddd
                        string guideOptions;

                        if (Settings.Default.GuideConfiguration == "")
                        {
                            guideOptions = string.Concat("-t", date, " -o", coord, " " + Settings.Default.CommandLineArguments);
                        }
                        else
                        {
                            guideOptions = string.Concat("-m", Settings.Default.GuideConfiguration, ".mar ", "-t", date, " -o", coord, " " + Settings.Default.CommandLineArguments);
                        }

                        if (!Settings.Default.AlwaysNewInstance)
                        {
                            if (m_CurrentGuideProcess != null &&
                                !m_CurrentGuideProcess.HasExited)
                            {
                                try
                                {
                                    m_CurrentGuideProcess.Kill();
                                }
                                catch (Exception ex)
                                {
                                    Trace.WriteLine(ex.ToString());
                                }
                            }
                        }

                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.UseShellExecute = false;
                        startInfo.WorkingDirectory = Path.GetDirectoryName(Settings.Default.GuidePath);
                        startInfo.FileName = Settings.Default.GuidePath;
                        startInfo.Verb = "runas";
                        startInfo.Arguments = guideOptions;
                        startInfo.ErrorDialog = true;

                        m_CurrentGuideProcess = Process.Start(startInfo);                        
                    }
                }
            }
            if (actionId == CHANGE_LANGUAGE)
            {
                if (eventArgs.OWEventId == OWEventArguments.EVT_CURR_LANGUAGE_CHANGED)
                {
                    SetLanguage((int) eventArgs.Args);
                }
            }
        }

        private void SetLanguage(int langId)
        {
            isEnglish = false;
            isGerman = false;

            switch (langId)
            {
                case 1:
                    isEnglish = true;
                    break;
                case 3:
                    isGerman = true;
                    break;
            }
        }
    }
}
