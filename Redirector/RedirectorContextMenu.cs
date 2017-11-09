using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redirector
{
    class RedirectorContextMenu : ContextMenu
    {
        private List<RedirectEntry> Entries;
        private MenuItem EntriesMenu = new MenuItem("Redirections");
        private List<Action> OnAddEntryCallbacks = new List<Action>();
        private List<Action> OnExitCallbacks = new List<Action>();
        private List<Action<string, bool>> OnItemClickCallbacks = new List<Action<string, bool>>();

        public List<Action> OnAddEntry { get { return OnAddEntryCallbacks; } }
        public List<Action> OnExit { get { return OnExitCallbacks; } }
        public List<Action<string, bool>> OnItemClick { get { return OnItemClickCallbacks; } }

        public RedirectorContextMenu(MenuItem[] menuItems) : base(menuItems)
        {
        }

        public RedirectorContextMenu(ref List<RedirectEntry> Entries) : base()
        {
            MenuItems.Add(EntriesMenu);
            MenuItems.Add(new MenuItem("Add Entry", AddEntry));
            MenuItems.Add(new MenuItem("Exit", Exit));

            this.Entries = Entries;
            UpdateEntriesMenu();
        }

        private void Exit(object sender, EventArgs e)
        {
            foreach (Action Exit in OnExitCallbacks) {
                Exit();
            }
        }

        private void AddEntry(object sender, EventArgs e)
        {
            foreach (Action AddEntry in OnAddEntryCallbacks)
            {
                AddEntry();
            }

            UpdateEntriesMenu();
        }

        public void Refresh()
        {
            UpdateEntriesMenu();
        }

        private void UpdateEntriesMenu()
        {
            EntriesMenu.MenuItems.Clear();
            foreach (RedirectEntry Entry in Entries)
            {
                MenuItem NewItem = new MenuItem(Entry.From + " => " + Entry.To);
                NewItem.Checked = Entry.Enabled;
                NewItem.Click += OnItemClicked;
                EntriesMenu.MenuItems.Add(NewItem);
            }
        }

        private void OnItemClicked(object sender, EventArgs e)
        {
            MenuItem Item = ((MenuItem)sender);
            bool Enabled = !Item.Checked;
            Item.Checked = Enabled;

            foreach (Action<string, bool> OnClick in OnItemClickCallbacks)
            {
                OnClick(Item.Text, Enabled);
            }
        }
    }
}
