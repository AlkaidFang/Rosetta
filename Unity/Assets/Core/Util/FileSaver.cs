using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Alkaid
{
    public class FileSaver
    {
        private StreamWriter mStreamWriter;

        public FileSaver()
        {
            mStreamWriter = null;
        }

        public void Init(string filePathName)
        {
            if (string.IsNullOrEmpty(filePathName)) return;

            mStreamWriter = new StreamWriter(filePathName, true);
        }

        public void WriteLine(string line)
        {
            mStreamWriter.WriteLine(line);
        }

        public void Flush()
        {
            mStreamWriter.Flush();
        }

        public void Close()
        {
            mStreamWriter.Close();
            mStreamWriter.Dispose();
        }
    }
}
