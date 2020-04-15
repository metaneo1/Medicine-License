using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public static class FileSystemHelper
    {

        public static string GetLocalPathForFile(string fileName)
        {
            return Path.Combine(Const.Const.FilePath.PATH + fileName);
        }

        public static byte[] GetFileByName(string fileName)
        {
            var filePath = Path.Combine(Const.Const.FilePath.PATH + fileName);
            var filedata = System.IO.File.ReadAllBytes(filePath);
            return filedata;
        }

        public static string MapFileName(string fileName)
        {
            var filePath = Path.Combine(Const.Const.FilePath.PATH + fileName);
            return filePath;
        }
    }
}
