using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using OccultWatcher.Guide.Properties;

namespace OccultWatcher.Guide
{
    public partial class guideFrmConfig : Form
    {
        private OWGuide m_ParentAddin;

        public guideFrmConfig(OWGuide parentAddin)
        {
            m_ParentAddin = parentAddin;
           
            InitializeComponent();

            tbxPathToGuide.Text = Settings.Default.GuidePath;
            tbxGuideConfig.Text = Settings.Default.GuideConfiguration;
            tbxAdditionalArguments.Text = Settings.Default.CommandLineArguments;
            cbxAlwaysInNewInstance.Checked = Settings.Default.AlwaysNewInstance;
        }


        private void guideFrmConfig_Load(object sender, EventArgs e)
        {
            btnOK.Text = m_ParentAddin.GetResourceString("OK", "OK");
            btnCancel.Text = m_ParentAddin.GetResourceString("Cancel", "Cancel");
            btnAbout.Text = m_ParentAddin.GetResourceString("About", "About");
            Text = m_ParentAddin.GetResourceString(OWGuide.OWGuideConfigTitle, "OW Guide Add-in Configuration");
            btnOpenFileDialog.Text = m_ParentAddin.GetResourceString(OWGuide.OWGuideBrowse, "Browse ...");
            lblOptional.Text = m_ParentAddin.GetResourceString(OWGuide.OWGuideOptional, "Optional: Add name of Guide configuration (exactly 8 characters)");
            lblGuidePath.Text = m_ParentAddin.GetResourceString(OWGuide.OWGuideGuidePath, "Set the path to Guide's executable on your computer");
            lblAdditionalArguments.Text = m_ParentAddin.GetResourceString(OWGuide.OWGuideAdditionalCommandLineArguments, "Optional: Additional command line arguments");
            cbxAlwaysInNewInstance.Text = m_ParentAddin.GetResourceString(OWGuide.OWGuideAlwaysInNewInstance, "Open events in a new instance of Guide");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(tbxPathToGuide.Text))
            {
                var guideExe = Directory.GetFiles(tbxPathToGuide.Text, "guide?.exe").SingleOrDefault();
                if (guideExe != null)
                {
                    tbxPathToGuide.Text += guideExe;
                }
            }

            if (!File.Exists(tbxPathToGuide.Text))
            {
                MessageBox.Show(this, 
                    string.Format(m_ParentAddin.GetResourceString(OWGuide.OWGuideCouldNotFindPath, "Could not find path '{0}'"), tbxPathToGuide.Text),
                    m_ParentAddin.GetResourceString(OWGuide.OWGuideAddinName, "OW Guide Add-in"), 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                tbxPathToGuide.Focus();
                return;
            }

            Settings.Default.GuidePath = tbxPathToGuide.Text;
            Settings.Default.GuideConfiguration = tbxGuideConfig.Text;
            Settings.Default.CommandLineArguments = tbxAdditionalArguments.Text;
            Settings.Default.AlwaysNewInstance = cbxAlwaysInNewInstance.Checked;
            Settings.Default.Save();

            Close();
        }

        private void btnOpenFileDialog_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.  
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Filter = "guide|guide?.exe";
                openFileDialog1.Title = "Select path to guide9.exe";

                string path = string.Empty;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialog1.FileName;
                    tbxPathToGuide.Clear();
                    tbxPathToGuide.AppendText(path);
                }                
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, 
                m_ParentAddin.GetResourceString(OWGuide.OWGuideAddinCredits, "This add-in was created by Andreas Eberle"), 
                m_ParentAddin.GetResourceString(OWGuide.OWGuideAddinName, "OW Guide Add-in"));
        }
    }
}