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
    public partial class Form9 : Form
    {

        private MySqlConnection sqlConnection = null;

        private MySqlCommandBuilder sqlBuilder = null;

        private MySqlDataAdapter sqlDataAdapter = null;

        private DataSet dataSet = null;

        private bool newRowAdding = false;

        public Form9()
        {
            InitializeComponent();
        }

        private void LoadDataEmployeer()
        {
            try
            {

                sqlDataAdapter = new MySqlDataAdapter("SELECT *, 'Delete' FROM lichnyedannyemenedjera", sqlConnection);


                sqlBuilder = new MySqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetDeleteCommand();
                sqlBuilder.GetUpdateCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "lichnyedannyemenedjera");

                dataGridView1.DataSource = dataSet.Tables["lichnyedannyemenedjera"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, i] = linkCell;
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

                dataSet.Tables["lichnyedannyemenedjera"].Clear();

                sqlDataAdapter.Fill(dataSet, "lichnyedannyemenedjera");

                dataGridView1.DataSource = dataSet.Tables["lichnyedannyemenedjera"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Form9_Load(object sender, EventArgs e)
        {
            sqlConnection = new MySqlConnection(@"server=localhost;user=root;database=kasting;password=12345;");

            sqlConnection.Open();

            LoadDataEmployeer();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadDataEmployeer();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form8 newForm = new Form8();
            newForm.Show();
            Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 6)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataGridView1.Rows.RemoveAt(rowIndex);

                            dataSet.Tables["lichnyedannyemenedjera"].Rows[rowIndex].Delete();
                            sqlDataAdapter.Update(dataSet, "lichnyedannyemenedjera");
                        }
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;



                        dataSet.Tables["lichnyedannyemenedjera"].Rows[r]["idMenedjera"] = dataGridView1.Rows[r].Cells["idMenedjera"].Value;
                        dataSet.Tables["lichnyedannyemenedjera"].Rows[r]["strana"] = dataGridView1.Rows[r].Cells["strana"].Value;
                        dataSet.Tables["lichnyedannyemenedjera"].Rows[r]["city"] = dataGridView1.Rows[r].Cells["city"].Value;
                        dataSet.Tables["lichnyedannyemenedjera"].Rows[r]["adress"] = dataGridView1.Rows[r].Cells["adress"].Value;
                        dataSet.Tables["lichnyedannyemenedjera"].Rows[r]["dataBirthday"] = dataGridView1.Rows[r].Cells["dataBirthday"].Value;

                        sqlDataAdapter.Update(dataSet, "lichnyedannyemenedjera");

                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";

                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = dataSet.Tables["lichnyedannyemenedjera"].NewRow();



                        row["idMenedjera"] = dataGridView1.Rows[rowIndex].Cells["idMenedjera"].Value;
                        row["strana"] = dataGridView1.Rows[rowIndex].Cells["strana"].Value;
                        row["city"] = dataGridView1.Rows[rowIndex].Cells["city"].Value;
                        row["adress"] = dataGridView1.Rows[rowIndex].Cells["adress"].Value;
                        row["dataBirthday"] = dataGridView1.Rows[rowIndex].Cells["dataBirthday"].Value;


                        dataSet.Tables["lichnyedannyemenedjera"].Rows.Add(row);

                        dataSet.Tables["lichnyedannyemenedjera"].Rows.RemoveAt(dataSet.Tables["lichnyedannyemenedjera"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "lichnyedannyemenedjera");

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

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
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

                    dataGridView1[6, lastRow] = linkCell;

                    row.Cells["Delete"].Value = "Insert";

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

            if (dataGridView1.CurrentCell.ColumnIndex == 1 || dataGridView1.CurrentCell.ColumnIndex == 0)
            {
                TextBox textBox = e.Control as TextBox;

                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(dataGridView1_KeyPress);
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[6, rowIndex] = linkCell;

                    editingRow.Cells["Delete"].Value = "Update";

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}