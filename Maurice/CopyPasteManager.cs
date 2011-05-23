using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ShellLib;

namespace Manina.Windows.Forms
{
   public static class CopyPasteManager
    {
        static public string[] file;
        static public bool _cut = false;
        static public bool _copy = false;
        static public int nbFile = 0;

        public static void Copy(List<String> l)
        {
            _copy = true;
            _cut = false;

            nbFile = l.Count;
            file = new string[l.Count];

            int n=0;
            foreach (string f in l)
            {
                file[n] = f;
                n++;
            }          
        }

        public static void Cut(List<String> l)
        {
            _copy = true;
            _cut = false;

            nbFile = l.Count;
            file = new string[l.Count];

            int n = 0;
            foreach (string f in l)
            {
                file[n] = f;
                n++;
            }
        }

        public static void Paste(string directory)
        {
            //FilenamePhotoProvider fpp = new FilenamePhotoProvider();
            string [] filedst = new String[nbFile];
            for (int n = 0; n < nbFile; n++)
            {
                char [] separator = new char[1];
                separator[0]='_';
                string [] liststr = file[n].Split(separator);
                if (liststr.Length == 2)
                {
                    FilenamePhotoProvider fpp = new FilenamePhotoProvider(liststr[0]);
                    List<String> result = fpp.getPossibleFilename(directory, 1);
                    filedst[n] = result[0];
                }
                else
                {
                    FileInfo fi = new FileInfo(file[n]);
                    filedst[n] =  DirUtil.JoinDirAndFile(directory ,fi.Name);
                }
            }

            ShellFileOperation sfo = new ShellFileOperation();
            sfo.SourceFiles = file;
            sfo.DestFiles = filedst;
            if (_copy)
            {
                sfo.Operation = ShellFileOperation.FileOperations.FO_COPY;
                sfo.ProgressTitle="Copy";
            }
            if (_cut)
            {
                sfo.Operation = ShellFileOperation.FileOperations.FO_MOVE;
                sfo.ProgressTitle = "Move";
            }            
            sfo.DoOperation();
        }
    }
}
