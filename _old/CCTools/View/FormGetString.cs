using System.Windows.Forms;

namespace CCTools.View
{
    public partial class FormGetString : Form
    {
        private string m_result; 

        public FormGetString(string caption, string text)
        {
            InitializeComponent();
            Text = caption;
            label1.Text = text;
            m_result = string.Empty;
        }

        public string Result
        {
            get { return m_result; }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            m_result = textBox1.Text;
            Close();
        }


    }
}