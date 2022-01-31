using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live2DCrack
{
    internal class Utils
    {
        public static string GetLive2DInstallPath()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Live2D\Live2D Cubism\");
            return key?.GetValue("Install_Dir") as string ?? "";
        }

        /// <summary>
        /// 获取Live2D软件的版本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool GetLive2DVersion(string path, out string version)
        {
            try
            {
                string txtString = File.ReadAllText(path + "\\ReadMe.txt");

                int versionIndex = txtString.IndexOf("Live2D Cubism Editor Version") + "Live2D Cubism Editor Version".Length;

                version = txtString.Substring(versionIndex + 1, 6);
                return true;
            }
            catch (Exception)
            {
                version = string.Empty;
                return false;
            }
        }

        public static bool WriteResource(string path)
        {
            var resource = Resource.rlm1221;
            try
            {
                File.WriteAllBytes(path, resource);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
