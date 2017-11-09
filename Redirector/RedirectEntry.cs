using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redirector
{
    class RedirectEntry
    {
        /**
         * To and From have been inverted
         */

        public string To;
        public string From;
        public bool Enabled;
        public string Text;

        public RedirectEntry(string From, string To, bool Enabled, string Text)
        {
            this.To = To;
            this.From = From;
            this.Enabled = Enabled;
            this.Text = Text;
        }

        public RedirectEntry(string From, string To)
        {
            this.To = To;
            this.From = From;
            this.Enabled = true;

            CreateEntry();
        }

        public void SetEnabled(bool NewEnabled)
        {
            Enabled = NewEnabled;
            Update(To);
        }

        public void Update(string To)
        {
            this.To = To;
            if (To == "")
            {
                DeleteEntry();
                return;
            }

            string CurrentText = Text;

            string Hosts = File.ReadAllText(TrayIcon.HostsPath);
            File.WriteAllText(TrayIcon.HostsPath, Hosts.Replace(CurrentText, GenerateEntryText()));
        }

        private void DeleteEntry()
        {
            string Hosts = File.ReadAllText(TrayIcon.HostsPath);
            File.WriteAllText(TrayIcon.HostsPath, Hosts.Replace(Text, ""));
        }

        private string GenerateEntryText()
        {
            Text = Environment.NewLine + "#Redirector"
                + Environment.NewLine
                + (Enabled ? "" : "#") + To + " " + From
                + Environment.NewLine;
            return Text;
        }

        private void CreateEntry()
        {
            using (StreamWriter SWriter = File.AppendText(TrayIcon.HostsPath))
            {
                SWriter.Write(GenerateEntryText());
            }
        }
    }
}
