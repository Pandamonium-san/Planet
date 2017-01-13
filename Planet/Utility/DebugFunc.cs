using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Planet
{
  public static class DebugFunc
  {
    public static void WriteToLog(string input)
    {
      using (StreamWriter sw = new StreamWriter("log.txt", true))
      {
        sw.Write(input);
        sw.Close();
      }
    }
  }
}
