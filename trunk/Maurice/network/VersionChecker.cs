using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Windows.Forms;


namespace  Manina.Windows.Forms.NetWork
{
    public class VersionChecker
    {
        string netVersion = "";
        public VersionChecker()
        {
        }

        public string GetNetworkVersion()
        {
            //Initialization
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}", "http://tcltk.free.fr/maurice/getversion.php", ""));
            //This time, our method is GET.
            WebReq.Method = "GET";
            //From here on, it's all the same as above.
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
            //Let's show some information about the response
           // Console.WriteLine(WebResp.StatusCode);
           // Console.WriteLine(WebResp.Server);

            //Now, we read the response (the string), and output it.
            Stream Answer = WebResp.GetResponseStream();
            StreamReader _Answer = new StreamReader(Answer);
            netVersion = _Answer.ReadToEnd();

            if (netVersion == "FAILED")
            {
            }
            if (netVersion == "REBOOT")
            {
            }
            return netVersion;
        }

         public void DownloadFile()
        {
            DownloadDialog dd = new DownloadDialog(netVersion, "http://tcltk.free.fr/maurice/getFile.php");
            dd.ShowDownLoadDialog();
        }
       

    }
}
