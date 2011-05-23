using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace Manina.Windows.Forms.NetWork
{
    public partial class DownloadDialog : Form
    {
        string url = "";
        string version = "";
        public DownloadDialog(string v,string urlToDownload)
        {
            version = v;
            InitializeComponent();
            labelVersion.Text = "Downloading Version " + version;
            url = urlToDownload;
            
        }

        public DialogResult ShowDownLoadDialog()
        {
            DownloadVersion();
            return base.ShowDialog();            
        }


        private void DownloadVersion()
        {                        
            WebClient client = new WebClient ();
            Uri uri = new Uri(url);

        // Specify that the DownloadFileCallback method gets called
        // when the download completes.
           client.DownloadFileCompleted += new AsyncCompletedEventHandler (DownloadFileCallback2);
         // Specify a progress notification handler.
          client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
          FileInfo fi = new FileInfo(Application.ExecutablePath);
          client.DownloadFileAsync(uri, fi.DirectoryName + "\\install\\adminphoto_"+version+".exe");
        }

      
        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {            
            progressBar1.Value = e.ProgressPercentage;
        }

        private void DownloadFileCallback2(object sender, AsyncCompletedEventArgs e)
        {
            this.Close();
        }
    }
}
