using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Net.NetworkInformation;

using Microsoft.Win32;

namespace Manina.Windows.Forms.NetWork
{
   public class LicenceControler
    {
       private string nameLicenceKey = "server";
       public bool licenceOk = false;
       public LicenceControler()
       {
         
           String crypt = GetCryptMacAdress(MacAdress());
           if (ReadRegisterLicence().ToUpper() != crypt.ToUpper())
           {
               LicenceQuery lq = new LicenceQuery(MacAdress());
               do
               {
                   if (lq.ShowDialog() == DialogResult.OK)
                   {
                       if (lq.LicenceText.ToUpper() == crypt.ToUpper())
                       {
                           WriteRegistryLicence(lq.LicenceText);
                           licenceOk = true;
                       }
                       else
                       {
                           MessageBox.Show("Error Licence Not available ");
                           break;
                       }
                   }
               } while (licenceOk == false);
           }
           else
           {
               licenceOk = true;
           }
       }

       public string MacAdress()
       {
           var macAddress = NetworkInterface.GetAllNetworkInterfaces()
    .Where(i => i.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
    .Select(i => i.GetPhysicalAddress())
    .FirstOrDefault();

           return macAddress.ToString();
       }

       public String GetCryptMacAdress(string macAdress)
       {
           Int64 mac = Int64.Parse(macAdress, System.Globalization.NumberStyles.AllowHexSpecifier);
           Int64 rsqrt = (Int64)Math.Round(Math.Sqrt(mac));
           Int64 rcos = (Int64)Math.Round(Math.Sqrt(mac/2));
           Int64 rsin = (Int64)Math.Round(Math.Sqrt(mac*2));
           Int64 result = rcos;
           Int64 result2 = rsin;
           Int64 result3 = (Int64)(rsqrt);
           //return String.Format("{0:X}", result) + "_" + String.Format("{0:X}", result2) + "_" + String.Format("{0:X}", result3);                     
           return String.Format("{0:X}", result) + "_" + String.Format("{0:X}", result3) + "_" + String.Format("{0:X}", result2);                     
       }

       public string ReadRegisterLicence()
       {
           Microsoft.Win32.RegistryKey OurKey = Registry.CurrentUser;
           RegistryKey k = OurKey.OpenSubKey("SOFTWARE\\cmbsoft");
           if (k == null) return "";           
           return (string )k.GetValue(nameLicenceKey,"");                   
      }

       public void WriteRegistryLicence(string licence)
       {
           try
           {
               Microsoft.Win32.RegistryKey OurKey = Registry.CurrentUser;
               RegistryKey k = OurKey.OpenSubKey("SOFTWARE\\cmbsoft",true);
               if (k == null)
               {
                   k = OurKey.CreateSubKey("SOFTWARE\\cmbsoft", RegistryKeyPermissionCheck.Default);
               }
               k.SetValue(nameLicenceKey, licence, RegistryValueKind.String);
               k.Close();
           }
           catch (Exception e) { MessageBox.Show(e.Message); }
               
       }
    }
}
