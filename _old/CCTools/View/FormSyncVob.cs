using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCTools.Model;

namespace CCTools.View
{
    public partial class FormSyncVob : Form
    {
        private readonly MainModel m_model;

        public FormSyncVob(MainModel model)
        {
            m_model = model;
            InitializeComponent();
        }

        public void Show(SyncVobData data)
        {
            Render(data);
            Show();
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            //draw green
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Cells[i + 1].Style = new DataGridViewCellStyle { BackColor = Color.LightGreen };
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SyncVobData data = m_model.GetSyncVobDataForVob(comboBox1.SelectedItem.ToString());
            Render(data);
        }           

        private void Render(SyncVobData data)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = data.AsTable();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].Width = 80;
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            comboBox1.DataSource = data.Vobs.ToList();
            comboBox1.SelectedItem = comboBox1.Items.Cast<string>().Where(s => s == data.VobName).First();
        }
    }
}
