using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutoComplete.FileLogger
{
    class Logger
    {

        public static void Log(string line, string filename)
        {
            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(@"C:\Users\User\Documents\Logs\" + filename + ".txt", true))
            {
                file.WriteLine(line);
            }
        }
    }
}
