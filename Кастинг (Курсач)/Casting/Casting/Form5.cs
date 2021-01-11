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
    public partial class Form5 : Form
    {

        private MySqlConnection sqlConnection = null;

        private MySqlCommandBuilder sqlBuilder = null;

        private MySqlDataAdapter sqlDataAdapter = null;

        private DataSet dataSet = null;

        private bool newRowAdding = false;

        public Form5()
        {
            InitializeComponent();
        }

        private void LoadDataEmployeer()
        {
            try
            {

                sqlDataAdapter = new MySqlDataAdapter("SELECT idPhotosessiya, modeli.surname, photograph.surname,data, mesto, tema,  'Delete' FROM photosessia INNER JOIN photograph on photograph.idPhotograph = photosessia.idFotographaa inner join modeli on modeli.idModeli = photosessia.idModele", sqlConnection);


                sqlBuilder = new MySqlCommandBuilder(sqlDataAdapter);

                //sqlBuilder.GetInsertCommand();
                //sqlBuilder.GetDeleteCommand();
                //sqlBuilder.GetUpdateCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "photosessia");

                dataGridView1.DataSource = dataSet.Tables["photosessia"];

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

                dataSet.Tables["photosessia"].Clear();

                sqlDataAdapter.Fill(dataSet, "photosessia");

                dataGridView1.DataSource = dataSet.Tables["photosessia"];

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

 
        private void Form5_Load(object sender, EventArgs e)
        {
            sqlConnection = new MySqlConnection(@"server=localhost;user=root;database=kasting;password=12345;");

            sqlConnection.Open();

            LoadDataEmployeer();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadDataEmployeer();
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
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

                            dataSet.Tables["photosessia"].Rows[rowIndex].Delete();
                            sqlDataAdapter.Update(dataSet, "photosessia");
                        }
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;



                        dataSet.Tables["photosessia"].Rows[r]["idModele"] = dataGridView1.Rows[r].Cells["idModele"].Value;
                        dataSet.Tables["photosessia"].Rows[r]["idFotographaa"] = dataGridView1.Rows[r].Cells["idFotographaa"].Value;
                        dataSet.Tables["photosessia"].Rows[r]["data"] = dataGridView1.Rows[r].Cells["data"].Value;
                        dataSet.Tables["photosessia"].Rows[r]["mesto"] = dataGridView1.Rows[r].Cells["mesto"].Value;
                        dataSet.Tables["photosessia"].Rows[r]["tema"] = dataGridView1.Rows[r].Cells["tema"].Value;

                        sqlDataAdapter.Update(dataSet, "photosessia");

                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";

                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = dataSet.Tables["photosessia"].NewRow();



                        row["idModele"] = dataGridView1.Rows[rowIndex].Cells["idModele"].Value;
                        row["idFotographaa"] = dataGridView1.Rows[rowIndex].Cells["idFotographaa"].Value;
                        row["data"] = dataGridView1.Rows[rowIndex].Cells["data"].Value;
                        row["mesto"] = dataGridView1.Rows[rowIndex].Cells["mesto"].Value;
                        row["tema"] = dataGridView1.Rows[rowIndex].Cells["tema"].Value;


                        dataSet.Tables["photosessia"].Rows.Add(row);

                        dataSet.Tables["photosessia"].Rows.RemoveAt(dataSet.Tables["photosessia"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[4].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "photosessia");

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

            if (dataGridView1.CurrentCell.ColumnIndex == 1 || dataGridView1.CurrentCell.ColumnIndex == 0 || dataGridView1.CurrentCell.ColumnIndex == 2)
            {
                TextBox textBox = e.Control as TextBox;

                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(dataGridView1_KeyPress);
                }
            }
        }
    }
}

