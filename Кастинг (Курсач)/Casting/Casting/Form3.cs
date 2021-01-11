using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;



namespace Casting
{
    public partial class Form3 : Form
    {

        private MySqlConnection sqlConnection = null;

        private MySqlCommandBuilder sqlBuilder = null;

        private MySqlDataAdapter sqlDataAdapter = null;

        private DataSet dataSet = null;

        private bool newRowAdding = false;

        public Form3()
        {
            InitializeComponent();
        }

        private void LoadDataEmployeer()
        {
            try
            {

                sqlDataAdapter = new MySqlDataAdapter("SELECT *, 'Delete' FROM pokaz", sqlConnection);


                sqlBuilder = new MySqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetDeleteCommand();
                sqlBuilder.GetUpdateCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "pokaz");

                dataGridView1.DataSource = dataSet.Tables["pokaz"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[7, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadDataEmployeer()
        {
            try
            {

                dataSet.Tables["pokaz"].Clear();

                sqlDataAdapter.Fill(dataSet, "pokaz");

                dataGridView1.DataSource = dataSet.Tables["pokaz"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[7, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

   
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
            Hide();
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            ReloadDataEmployeer();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 7)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataGridView1.Rows.RemoveAt(rowIndex);

                            dataSet.Tables["pokaz"].Rows[rowIndex].Delete();
                            sqlDataAdapter.Update(dataSet, "pokaz");
                        }
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;

                        dataSet.Tables["pokaz"].Rows[r]["idMenedjer"] = dataGridView1.Rows[r].Cells["idMenedjer"].Value;
                        dataSet.Tables["pokaz"].Rows[r]["idMod"] = dataGridView1.Rows[r].Cells["idMod"].Value;
                        dataSet.Tables["pokaz"].Rows[r]["data"] = dataGridView1.Rows[r].Cells["data"].Value;
                        dataSet.Tables["pokaz"].Rows[r]["mesto"] = dataGridView1.Rows[r].Cells["mesto"].Value;
                        dataSet.Tables["pokaz"].Rows[r]["tema"] = dataGridView1.Rows[r].Cells["tema"].Value;
                        dataSet.Tables["pokaz"].Rows[r]["idPhotographa"] = dataGridView1.Rows[r].Cells["idPhotographa"].Value;

                        sqlDataAdapter.Update(dataSet, "pokaz");

                        dataGridView1.Rows[e.RowIndex].Cells[7].Value = "Delete";

                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = dataSet.Tables["pokaz"].NewRow();

                        row["idMenedjer"] = dataGridView1.Rows[rowIndex].Cells["idMenedjer"].Value;
                        row["idMod"] = dataGridView1.Rows[rowIndex].Cells["idMod"].Value;
                        row["data"] = dataGridView1.Rows[rowIndex].Cells["data"].Value;
                        row["mesto"] = dataGridView1.Rows[rowIndex].Cells["mesto"].Value;
                        row["tema"] = dataGridView1.Rows[rowIndex].Cells["tema"].Value;
                        row["idPhotographa"] = dataGridView1.Rows[rowIndex].Cells["idPhotographa"].Value;


                        dataSet.Tables["pokaz"].Rows.Add(row);

                        dataSet.Tables["pokaz"].Rows.RemoveAt(dataSet.Tables["pokaz"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[7].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "pokaz");

                        newRowAdding = false;

                    }

                    ReloadDataEmployeer();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            sqlConnection = new MySqlConnection(@"server=localhost;user=root;database=kasting;password=12345;");

            sqlConnection.Open();

            LoadDataEmployeer();
        }

        private void dataGridView1_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[7, rowIndex] = linkCell;

                    editingRow.Cells["Delete"].Value = "Update";

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(dataGridView1_KeyPress);

            if (dataGridView1.CurrentCell.ColumnIndex == 1 || dataGridView1.CurrentCell.ColumnIndex == 0 || dataGridView1.CurrentCell.ColumnIndex == 5)
            {
                TextBox textBox = e.Control as TextBox;

                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(dataGridView1_KeyPress);
                }
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;

                    int lastRow = dataGridView1.Rows.Count - 2;

                    DataGridViewRow row = dataGridView1.Rows[lastRow];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[7, lastRow] = linkCell;

                    row.Cells["Delete"].Value = "Insert";

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}