﻿using System;
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
    public partial class Form6 : Form
    {

        private MySqlConnection sqlConnection = null;

        private MySqlCommandBuilder sqlBuilder = null;

        private MySqlDataAdapter sqlDataAdapter = null;

        private DataSet dataSet = null;

        private bool newRowAdding = false;

        public Form6()
        {
            InitializeComponent();
        }

        private void LoadDataEmployeer()
        {
            try
            {

                sqlDataAdapter = new MySqlDataAdapter("SELECT *, 'Delete' FROM modeli", sqlConnection);


                sqlBuilder = new MySqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetDeleteCommand();
                sqlBuilder.GetUpdateCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "modeli");

                dataGridView1.DataSource = dataSet.Tables["modeli"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[4, i] = linkCell;
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

                dataSet.Tables["modeli"].Clear();

                sqlDataAdapter.Fill(dataSet, "modeli");

                dataGridView1.DataSource = dataSet.Tables["modeli"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[4, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
   

     
        private void Form6_Load(object sender, EventArgs e)
        {
            sqlConnection = new MySqlConnection(@"server=localhost;user=root;database=kasting;password=12345;");

            sqlConnection.Open();

            LoadDataEmployeer();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadDataEmployeer();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
            newForm.Show();
            Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 4)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataGridView1.Rows.RemoveAt(rowIndex);

                            dataSet.Tables["modeli"].Rows[rowIndex].Delete();
                            sqlDataAdapter.Update(dataSet, "modeli");
                        }
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;



                        dataSet.Tables["modeli"].Rows[r]["name"] = dataGridView1.Rows[r].Cells["name"].Value;
                        dataSet.Tables["modeli"].Rows[r]["surname"] = dataGridView1.Rows[r].Cells["surname"].Value;
                        dataSet.Tables["modeli"].Rows[r]["secondName"] = dataGridView1.Rows[r].Cells["secondName"].Value;

                        sqlDataAdapter.Update(dataSet, "modeli");

                        dataGridView1.Rows[e.RowIndex].Cells[4].Value = "Delete";

                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = dataSet.Tables["modeli"].NewRow();



                        row["name"] = dataGridView1.Rows[rowIndex].Cells["name"].Value;
                        row["surname"] = dataGridView1.Rows[rowIndex].Cells["surname"].Value;
                        row["secondName"] = dataGridView1.Rows[rowIndex].Cells["secondName"].Value;


                        dataSet.Tables["modeli"].Rows.Add(row);

                        dataSet.Tables["modeli"].Rows.RemoveAt(dataSet.Tables["modeli"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[4].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "modeli");

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

                    dataGridView1[4, rowIndex] = linkCell;

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

        private void dataGridView1_EditingControlShowing_1(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(dataGridView1_KeyPress);

            if (dataGridView1.CurrentCell.ColumnIndex == 0)
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

                    dataGridView1[4, lastRow] = linkCell;

                    row.Cells["Delete"].Value = "Insert";

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form7 form = new Form7();
            form.Show();
            Hide();
        }
    }
}
