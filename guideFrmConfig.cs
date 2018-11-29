using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Configuration;

namespace OccultWatcher.Guide
{
    public partial class guideFrmConfig : Form
    {
        private OWGuide m_ParentAddin;

        private String stored_path;
        private String stored_configuration;

        public guideFrmConfig(OWGuide parentAddin, string path, string configuration)
        {
            m_ParentAddin = parentAddin;
            stored_path = path;
            stored_configuration = configuration;
           
            InitializeComponent();

            btnOK.Text = m_ParentAddin.GetResourceString("OK", "OK");
            btnCancel.Text = m_ParentAddin.GetResourceString("Cancel", "Cancel");
            //btnInserTag.Text = m_ParentAddin.GetResourceString("OWLH.btnInserTag", "Insert Tag");
            lblInfoFormat.Text = m_ParentAddin.GetResourceString("OWLH.lblInfoFormat", "Specify the string format of the info to be copied below. Use 'Insert Tag' button below to insert event information tags.");

            try
            {
                //Laden der Eigenschaft aus den Settings
                tbxPathToGuide.Text = stored_path;// getAppSetting("benutzerEingabe");
            }
            catch (Exception ee)
            {
                //Fehler abfangen. z.B. nicht vorhandene AppSettings
                //tbxPathToGuide.Text = ee.Message;
            }

            try
            {
                //Laden der Eigenschaft aus den Settings
                textBox1.Text = stored_configuration;// getAppSetting("benutzerEingabe");
            }
            catch (Exception ee)
            {
                //Fehler abfangen. z.B. nicht vorhandene AppSettings
                //tbxPathToGuide.Text = ee.Message;
            }


            //TODO: Load the format string 
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //TODO: Save the format string
            //m_ParentAddin.
            //FormMain_FormClosing(sender, e);
            m_ParentAddin.setNewPath(tbxPathToGuide.Text, textBox1.Text);
            //setAppSetting("benutzerEingabe", tbxPathToGuide.Text);
            //m_ParentAddin.stored_path = tbxPathToGuide.Text;
            Dispose();
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void tbxPathToGuide_TextChanged(object sender, EventArgs e)
        {
            //TODO: Dateipfad in Config speichern
        }

        private void btnOpenFileDialog_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.  
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "guide|guide?.exe";
            openFileDialog1.Title = "Select path to guide9.exe";

            string path = string.Empty;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = openFileDialog1.FileName;
                tbxPathToGuide.Clear();
                tbxPathToGuide.AppendText(path);
            }



            //TODO: Dateidiolog öffnen
           // CancelEventArgs cancelEventArgs = new CancelEventArgs();
            //openFileDialog1_FileOk(sender, cancelEventArgs);
            
            //m_ParentAddin.
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            // ergebnis abspeichern
            sender.ToString();
        }




 //       public FormMain()
 //       {
 //           InitializeComponent();
 //       }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                //Laden der Eigenschaft aus den Settings
                tbxPathToGuide.Text = stored_path;// getAppSetting("benutzerEingabe");
            }
            catch (Exception ee)
            {
                //Fehler abfangen. z.B. nicht vorhandene AppSettings
                //tbxPathToGuide.Text = ee.Message;
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Speichern der Benutzereingabe
            //setAppSetting("benutzerEingabe", tbxPathToGuide.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}