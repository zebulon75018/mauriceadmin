using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

using Manina.Windows.Forms;
namespace ThumbnailMakers
{
    public  class ThumbMaker
    {
        private String appPath;

        public delegate void UpdateGUI(int file,string message);

        public int tbOutputWidth = 240;
        public int tbOutputHeight = 240;
        public int tbQuality = 80;
        public int tbDPI = 72;

        public enum ROTATION {
            ROTATE_LEFT,
            ROTATE_RIGHT
        } ;

        public ThumbMaker()
        {
            appPath = CommunConfig.getInstance().imageMagicPath;
            //appPath = "C:\\Program Files (x86)\\ImageMagick-6.6.5-Q16"; // Properties.Settings.Default.ImageMagikPath;
            if (File.Exists(appPath + @"\convert.exe") == false)
            {
                MessageBox.Show("ImageMagik Not found at " + appPath);                
            }
        }

        public void Rotate(String[] fileEntries, UpdateGUI update, ROTATION rotate)
        {                                      

                for (int n = 0; n < fileEntries.Length; n++)
                {
                    try
                    {

                        Process myProcess = new Process();
                        myProcess.StartInfo.FileName = "\"" + appPath + "\\convert\"";
                        myProcess.StartInfo.Arguments = (" -rotate 90 \"" + fileEntries[n] + "\" \"" + fileEntries[n]+ "\"  ");
                        myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.Start();
                        myProcess.WaitForExit();
                        update(n, "");
                        Application.DoEvents();       
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Rotation " + ex.Message);
                    }
                }

        }

        public void BlackandWhite(String input,String output)
        {            
                try
                {

                    Process myProcess = new Process();
                    myProcess.StartInfo.FileName = "\"" + appPath + "\\convert\"";
                    myProcess.StartInfo.Arguments = (" -monochrome \"" + input + "\" \"" + output + "\"  ");
                    myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    myProcess.StartInfo.UseShellExecute = false;
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                    myProcess.WaitForExit();                    
                    Application.DoEvents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Rotation " + ex.Message);
                }            

        }
        
        public bool processFiles(String[] fileEntries, String outputDir,UpdateGUI update)
        {

            if (Directory.Exists(outputDir) == false)
            {
                Directory.CreateDirectory(outputDir);
            }
            //For each selected file type in the source directory, do            
           // pbStatus.Minimum = 0;
           // pbStatus.Maximum = fileEntries.Length;
          //  pbStatus.Step = 1;
            int fileCount = 0;
            bool fail = false;
            
          //  Console.WriteLine("PROCESSING FILES....");
            foreach (String fileName in fileEntries)
            {
                if (fail == true)
                    break;
                //If the found file is of the correct file type then process
                FileInfo fi = new FileInfo(fileName);
              
                    fileCount += 1;
                //    pbStatus.Value = fileCount;
                    String currentFile = fileName;

                    String outputFile = DirUtil.JoinDirAndFile(outputDir, fi.Name);
                /*
                    if (outputDir[outputDir.Length - 1] != '\\')
                    {
                        outputFile = outputDir + "\\" + fi.Name;
                    }
                    else
                    {
                        outputFile = outputDir + fi.Name;
                    }*/

                    Console.WriteLine("OUTPUT TO: " + outputFile);
                    String resizeString = "";

                    resizeString = " -resize " + tbOutputWidth.ToString();                            

                 //   if (tbOutputHeight.Text != "" && tbOutputWidth.Text != "")
                 //       resizeString = " -resize " + tbOutputWidth.Text + "x" + tbOutputHeight.Text;
                    //lblStatus.Text = "Processing File " + fileCount.ToString() + " of " + fileEntries.Length.ToString();


                    Process myProcess = new Process();
                    try
                    {
                        myProcess.StartInfo.FileName = "\"" + appPath + "\\convert\"";
                        myProcess.StartInfo.Arguments = (@"-type truecolor -quality " + tbQuality+ " -density " + tbDPI+ " \"" + currentFile + "\"" + resizeString + " \"" + outputFile + "\"");
                        myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.CreateNoWindow = true;
                        System.Console.WriteLine(myProcess.StartInfo.FileName + " " + myProcess.StartInfo.Arguments);
                       // myProcess.StartInfo.RedirectStandardOutput = true;
                       // myProcess.OutputDataReceived += new DataReceivedEventHandler(OnDataReceived);
                        //myProcess.ErrorDataReceived += new DataReceivedEventHandler(SortOutputHandler);
                       
           

                        myProcess.Start();                                          
                        myProcess.WaitForExit();
                        if (myProcess.ExitCode != 0)
                        {
                            //update(-1, "Error " + ex.Message);
                            //  lblStatus.Text = "Error " + ex.ToString();
                            //fail = true;
                        }
                   
               
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        //Console.WriteLine(ex.ToString());
                        update(-1, "Error "+ex.Message);
                      //  lblStatus.Text = "Error " + ex.ToString();
                        fail = true;
                    }
                    update(fileCount,"");
                    Application.DoEvents();                
            }
        //    pbStatus.Value = 0;
            return fail;
        }

        // Called asynchronously with a line of data
        private void OnDataReceived(object Sender, DataReceivedEventArgs e)
        {
            if ((e.Data != null))
            {
                MessageBox.Show(e.Data.ToString());
            }                
        }

    }
}
