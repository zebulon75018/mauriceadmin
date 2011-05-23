using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Manina.Windows.Forms.NodeView;

namespace Manina.Windows.Forms
{
    public class FilenamePhotoProvider
    {
        string photographe;
        public FilenamePhotoProvider(string p)
        {
            photographe = p;
        }

        private bool isInList(String[] list, string valueToFind)
        {
            foreach (String s in list)
            {
                if (s == valueToFind) return true;
            }
            return false;
        }

        public List<String> getPossibleFilename(string path, int nbFile)
        {
            List<String> result = new List<string>();
            String[] listFile = Directory.GetFiles(path, "*.jpg");

            int maxpossible = 0;
            for(int n = 0; n < nbFile; n++)
            {                
                string filename = "";
                do
                {
                    filename = DirUtil.JoinDirAndFile(path, photographe + "_" + maxpossible.ToString().PadLeft(5, '0') + ".jpg");
                    maxpossible++;
                } while (isInList(listFile, filename) ==true);
                result.Add(filename);
            }

            return result;
        }
    }
}
