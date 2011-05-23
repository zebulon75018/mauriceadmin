using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manina.Windows.Forms
{

    public  static class DirUtil
    {
    
    public static String JoinDirAndFile(String dir,String file)
    {
        String result;
        if (dir.EndsWith("\\"))
        {
            result =  dir + file;
        }
        else
        {
            result=  dir +"\\"+ file;
        }
        return result;   
    }
    }
}
