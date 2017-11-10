using Redirector.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redirector
{
    class TrayIcon : ApplicationContext
    {
        public const string RedirectorHostsEntryRegex = @"(#Redirector\r?\n(#?)([0-9a-zA-z\.-_])+ ([0-9a-zA-z\.-_]+))";
        public const string HostsPath = @"C:\Windows\System32\drivers\etc\hosts";
        private NotifyIcon TrayIconHandler;
        private List<RedirectEntry> Entries = new List<RedirectEntry>();
        private RedirectorContextMenu ContextMenu;

        public TrayIcon()
        {
            IntPtr Hicon = Resources.Icon.GetHicon();
            Icon Ico = Icon.FromHandle(Hicon);
            RefreshList();

            ContextMenu = new RedirectorContextMenu(ref Entries);
            ContextMenu.OnAddEntry.Add(AddEntry);
            ContextMenu.OnExit.Add(Exit);
            ContextMenu.OnItemClick.Add(ItemClick);
            
            // Initialize Tray Icon
            TrayIconHandler = new NotifyIcon()
            {
                Icon = Ico,
                ContextMenu = ContextMenu,
                Visible = true
            };
        }

        private void RefreshList()
        {
            Entries.Clear();
            string Hosts = System.IO.File.ReadAllText(HostsPath);
            foreach (Match RMatch in Regex.Matches(Hosts, RedirectorHostsEntryRegex)) {
                //auto SplitString = RMatch.
                string Result = RMatch.Groups[0].ToString();
                string[] Redirection = Result.Split(new[] { Environment.NewLine }, StringSplitOptions.None)[1].Split(' ');
                Entries.Add(new RedirectEntry(
                    Redirection[1],
                    Redirection[0].Replace("#", ""),
                    !Redirection[0].StartsWith("#"),
                    Result
                    )
                );
            }
        }

        void AddEntry()
        {
            using (Input inputForm = new Input())
            {
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    if (Entries.Exists(x => x.From == inputForm.From))
                    {
                        Entries.Find(x => x.From == inputForm.From).Update(inputForm.To);
                    } else
                    {
                        Entries.Add(new RedirectEntry(inputForm.From, inputForm.To));
                    }
                    ContextMenu.Refresh();
                }
            }
        }

        void Exit()
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            TrayIconHandler.Visible = false;

            Application.Exit();
        }

        void ItemClick(string ItemName, bool Enabled)
        {
            string[] Name = ItemName.Split(new[] {"=>"}, StringSplitOptions.None);
            Predicate<RedirectEntry> FindPredicate = x => x.From == Name[0].Trim() && x.To == Name[1].Trim();
            if (Entries.Exists(FindPredicate))
            {
                Entries.Find(FindPredicate).SetEnabled(Enabled);
            }
        }
    }
}
