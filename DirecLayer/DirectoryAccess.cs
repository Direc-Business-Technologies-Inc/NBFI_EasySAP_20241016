using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirecLayer
{
    public class DirectoryAccess
    {
        public void CreateDirectory(string path)
        {
            if (!ifExists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public bool ifExists(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
