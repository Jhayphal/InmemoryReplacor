using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace InmemoryReplacor
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      ReplaceMap map;

      try
      {
        var config = File.ReadAllText("replace.conf");
        map = ReplaceMap.FromJson(config);
      }
      catch (Exception e)
      {
        MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        return;
      }

      while (true)
      {
        if (Clipboard.ContainsText())
        {
          var copy = Clipboard.GetText();

          foreach (var @case in map.Cases)
          {
            var regex = new Regex(@case.Expression);

            Match match = null;

            try
            {
              match = regex.Match(copy);
            }
            catch (Exception e)
            {
              MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (match == null || !match.Success || match.Groups.Count < @case.ReplaceGroupId)
              continue;

            Group group = match.Groups[@case.ReplaceGroupId];

            var newText = copy.Substring(0, group.Index) + @case.ReplaceTo + copy.Substring(group.Index + group.Length);
            Clipboard.SetText(newText);
          }
        }

        for (int i = 0; i < 50; ++i)
        {
          Thread.Sleep(10);
          Application.DoEvents();
        }
      }
    }
  }
}
