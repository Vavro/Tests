using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneNetCzechSupport.WpfClient.Helpers
{
    public static class FileHelpers
    {
        public static bool IsDirectory(string fileName)
        {
            var attr = File.GetAttributes(fileName);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return true;
            }
            return false;
        }
    }
}
