using System;
using System.Collections.ObjectModel;
namespace ICH
{
    /// <summary>
    /// Log collection with datetime and cleanup.
    /// </summary>
    public class LogCollection : ObservableCollection<string>
    {
        public new void Add(string s)
        {
            Add(s, false);
        }

        public void Add(string s, bool error = false)
        {
            //Shorten
            s = s.Length > 100 ? s.Remove(100, s.Length - 100) + "..." : s;

            //Clean up.
            if (base.Items.Count > 500)
            {
                for (int i = 0; i < 250; i++)
                {
                    base.RemoveItem(0);
                }
            }
            if (error) base.Add(DateTime.Now.ToString("hh.mm.ss") + "[ERROR] " + s);
            else base.Add(DateTime.Now.ToString("hh.mm.ss") + "[OK] " + s);
        }
    }
}
