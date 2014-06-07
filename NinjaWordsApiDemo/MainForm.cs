using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NinjaWordsApiDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private static string GetStringFromTerms(NinjaTerm[] terms)
        {
            var SB = new StringBuilder();

            foreach (var NT in terms.Where(NT => !NT.Defined))
                SB.AppendLine(NT.Text + " was not defined");

            // Put a space between undefined list and entries
            if (terms.Any(t => !t.Defined))
                SB.AppendLine();

            foreach (var ninjaTerm in terms)
                SB.AppendLine(ninjaTerm.ToString());

            return SB.ToString();
        }

        private async void btnLookup_Click(object sender, EventArgs e)
        {
            btnLookup.Enabled = false;
            var ninjaTerms = await Ninja.GetTermsAsync(txtTerm.Text);
            richTextBox.Text = GetStringFromTerms(ninjaTerms);
            webBrowser.Navigate(Ninja.Host + "/" + txtTerm.Text);
            btnLookup.Enabled = true;
        }

        private async void btnRandom_Click(object sender, EventArgs e)
        {
            btnRandom.Enabled = false;
            var term = await Ninja.GetRandomTermAsync();
            richTextBox.Text = term.ToString();
            webBrowser.Navigate(Ninja.Host + "/" + term.Text);
            btnRandom.Enabled = true;
        }
    }
}
