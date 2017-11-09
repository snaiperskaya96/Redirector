using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redirector
{
    public partial class Input : Form
    {
        public Input()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            OnClose();
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            OnClose();
            Close();
        }

        public string From {
            get { return FromText.Text; }
            set { From = value.Trim(); }
        }
        public string To {
            get { return ToText.Text; }
            set { To = value.Trim(); }
        }

        private void OnClose()
        {
            if (From == "")
                DialogResult = DialogResult.Cancel;
        }
    }
}
