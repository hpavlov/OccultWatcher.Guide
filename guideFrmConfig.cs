using System;
using System.Windows.Forms;
using System.IO;

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
        }


        private void guideFrmConfig_Load(object sender, EventArgs e)
        {
            btnOK.Text = m_ParentAddin.GetResourceString("OK", "OK");
            btnCancel.Text = m_ParentAddin.GetResourceString("Cancel", "Cancel");
            Text = m_ParentAddin.GetResourceString(OWGuide.OWGuideConfigTitle, "OW Guide Add-in Configuration");
            btnOpenFileDialog.Text = m_ParentAddin.GetResourceString(OWGuide.OWGuideBrowse, "Browse ...");
            lblOptional.Text = m_ParentAddin.GetResourceString(OWGuide.OWGuideOptional, "Optional: Add name of Guide- configuration (exactly 8 characters)");
            lblGuidePath.Text = m_ParentAddin.GetResourceString(OWGuide.OWGuideGuidePath, "Set the path to 'Guide' on your computer .");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
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
    }
}