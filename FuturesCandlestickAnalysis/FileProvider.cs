using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FuturesCandlestickAnalysis
{
     class FileProvider
    {
         public List<string> ReadFile()
        {
            List<string> data = new List<string>();
            try
            {
                using (StreamReader reader = new StreamReader(Properties.Resources.PathToFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        data.Add(line);
                    }
                }
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
