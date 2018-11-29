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

namespace OccultWatcher.Guide
{
    public class OWGuide : IOWAddin
    {
        private const string OWGuideAddinName = "OWGuide.addinName";
        private readonly int START_GUIDE = 1;
        static private string stored_path;
        static private string stored_configuration;

        enum ActionEnumeration {START_GUIDE, END };
        private IOWHost m_HostInfo = null;
        private IOWResourceProvider m_ResourceProvider = null;

        public OWAddinAction[] ADDIN_ACTIONS  = null;

        internal string GetResourceString(string resourceName, string defaultValue)
        {
            if (m_ResourceProvider == null)
                // Backwards compatibility with older versions of OW
                return defaultValue;
            else
                return m_ResourceProvider.GetResourceString(resourceName, defaultValue);
        }

        void IOWAddin.InitializeAddin(IOWHost hostInfo)
        {
            m_HostInfo = hostInfo;
            m_ResourceProvider = hostInfo as IOWResourceProvider;

            ADDIN_ACTIONS = new OWAddinAction[]
            {
                new OWAddinAction(START_GUIDE, GetResourceString("OWGuide.startGuide", "Show Event in Guide"), OWAddinActionType.SelectedEventAction, Properties.Resources.Details.ToBitmap()),
            };

            try
            {
                //load path to Guide from setting
                stored_path = getAppSetting("GuidePath");
            }
            catch (Exception ee)
            {
                //catch error message (e.g. no AppSettings)
                stored_path = "";
            }

            try
            {
                //load properties from setting
                stored_configuration = getAppSetting("GuideConfiguration");
            }
            catch (Exception ee)
            {
                //catch error message (e.g. no AppSettings)
                stored_configuration = "";
            }
        }

        void IOWAddin.FinalizeAddin()
        {
            // Do nothing special
        }

        OWAddinInfo IOWAddin.GetAddinInfo()
        {
            return new OWAddinInfo(GetResourceString(OWGuideAddinName, "OW Guide Plugin"), true, Properties.Resources.AddinImg.ToBitmap());
        }

        void IOWAddin.Configure(IWin32Window owner)
        {
            guideFrmConfig guideFrm = new guideFrmConfig(this, stored_path, stored_configuration);
            guideFrm.ShowDialog(owner);
        }


        IEnumerable<OWAddinAction> IOWAddin.SupportedActions()
        {
            return ADDIN_ACTIONS;
        }

        bool IOWAddin.ShouldDisplayAction(int actionId, IOWAsteroidEvent astEvent)
        {
            // Our actions will be always displayed for all events
            return true;
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

                    DateTime dateAndTime = siteData.EventTime;

                    int year = dateAndTime.Year;
                    int month = dateAndTime.Month;
                    int day = dateAndTime.Day;

                    int hour = dateAndTime.Hour;
                    int minute = dateAndTime.Minute;
                    int second = dateAndTime.Second;

                    string year_string = dateAndTime.Year.ToString();
                    string month_string = AddLeadingZero(month);
                    string day_string = AddLeadingZero(day);

                    string hour_string = AddLeadingZero(hour);
                    string minute_string = AddLeadingZero(minute);
                    string second_string = AddLeadingZero(second);

                    // time setting for Guide: yyyymmdd hh:mm:ssUT
                    string date = string.Concat(year_string, month_string, day_string, " ", hour_string, ":", minute_string, ":", second_string, "UT");

                    // Read coordintes
                    double coord_RA = evt2.Occelmnt.StarRAHours;
                    double coord_DEC = evt2.Occelmnt.StarDEDeg;

                    // Convert RA-format from hh.hhhhh into hhmmss.ssss
                    if(coord_RA < 0.0)
                    {
                        coord_RA = coord_RA + 24.0;
                    }

                    string coord_RA_hours = AddLeadingZero(Math.Floor(coord_RA));

                    coord_RA = (coord_RA - Math.Floor(coord_RA)) * 60.0;
                    string coord_RA_minutes = AddLeadingZero(Math.Floor(coord_RA));

                    coord_RA = (coord_RA - Math.Floor(coord_RA)) * 60.0;
                    string coord_RA_seconds = AddLeadingZero(coord_RA).Replace(',','.');

                    // coordinate setting for Guide: hhmmss.ssss,dd.dddd
                    string coord = string.Concat(coord_RA_hours, coord_RA_minutes, coord_RA_seconds, ",", coord_DEC.ToString().Replace(',','.'));


                    // Build Guide Options
                    // Format: -tyyyymmdd hh:mm:ss.ssss -ohhmmss.ssss,dd.ddddd
                    string guideOptions;

                    if(stored_configuration == "")
                    {
                        guideOptions = string.Concat("-t", date, " -o", coord);
                    }
                    else
                    {
                        guideOptions = string.Concat("-m", stored_configuration, ".mar ", "-t", date, " -o", coord);
                    }
                    

                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.UseShellExecute = false;
                    startInfo.WorkingDirectory = Path.GetDirectoryName(stored_path);
                    startInfo.FileName = stored_path;
                    startInfo.Verb = "runas";
                    startInfo.Arguments = guideOptions;
                    startInfo.ErrorDialog = true;

                    System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo);
                }
            }
        }


        private static string AddLeadingZero(int number)
        {
            if( number < 10 )
            {
                return string.Concat("0", number.ToString());
            }
            else
            {
                return number.ToString();
            }
        }

        private static string AddLeadingZero(double number)
        {
            if (number < 10.0)
            {
                return string.Concat("0", number.ToString());
            }
            else
            {
                return number.ToString();
            }
        }

        public void setNewPath(string guidePath, string guideConfiguration)
        {
            stored_path = guidePath;
            stored_configuration = guideConfiguration;
            setAppSetting("GuidePath", guidePath);
            setAppSetting("GuideConfiguration", guideConfiguration);
        }

        public string getAppSetting(string key)
        {
            //Laden der AppSettings
            Configuration config = ConfigurationManager.
                                    OpenExeConfiguration(
                                    System.Reflection.Assembly.
                                    GetExecutingAssembly().Location);
            //Zurückgeben der dem Key zugehörigen Value
            return config.AppSettings.Settings[key].Value;
        }

        public void setAppSetting(string key, string value)
        {
            //Laden der AppSettings
            Configuration config = ConfigurationManager.
                                    OpenExeConfiguration(
                                    System.Reflection.Assembly.
                                    GetExecutingAssembly().Location);
            //Überprüfen ob Key existiert
            if (config.AppSettings.Settings[key] != null)
            {
                //Key existiert. Löschen des Keys zum "überschreiben"
                config.AppSettings.Settings.Remove(key);
            }
            //Anlegen eines neuen KeyValue-Paars
            config.AppSettings.Settings.Add(key, value);
            //Speichern der aktualisierten AppSettings
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
