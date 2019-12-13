using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YPLast
{
    public partial class MainForm : Form
    {
        DataSet ds;
        DataTable dt;
        SqlCommand command;
        SqlDataAdapter adapter;
        SqlTransaction transaction;
        SqlCommandBuilder builder;
        
        public MainForm()
        {
            InitializeComponent();
        }


        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=YPProject;Integrated Security=True";

        private void MainForm_Load(object sender, EventArgs e)
        {
            comboBoxCategory.SelectedIndex= 0;
            comboBox1.SelectedIndex = 0;
            string sql = "SELECT * FROM CookBookSecond";
            adapter = new SqlDataAdapter(sql, connectionString);
            builder = new SqlCommandBuilder(adapter);
            ds = new DataSet();
            dt = new DataTable();
            adapter.Fill(ds);

            bindingSource1.DataSource = ds.Tables[0];
            dataGridView1.DataSource = bindingSource1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;

                    command.CommandText = @"INSERT INTO CookBookSecond(Id, Name, Products, Recipe, Category) VALUES(@Id, @Name, @Products, @Recipe, @Category)";
                    command.Parameters.AddWithValue("Id", Convert.ToInt32(textBoxId.Text));
                    command.Parameters.AddWithValue("Name", textBoxName.Text);
                    command.Parameters.AddWithValue("Products", textBoxProducts.Text);
                    command.Parameters.AddWithValue("Recipe", textBoxRecipe.Text);
                    command.Parameters.AddWithValue("Category", comboBoxCategory.SelectedItem.ToString());
                    command.ExecuteNonQuery();
                    transaction.Commit();

                    dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    row["Id"] = Convert.ToInt32(textBoxId.Text);
                    row["Name"] = textBoxName.Text;
                    row["Products"] = textBoxProducts.Text;
                    row["Recipe"] = textBoxRecipe.Text;
                    row["Category"] = comboBoxCategory.SelectedItem.ToString();
                    dt.Rows.Add(row);

                    ds.Clear();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;

                    command.CommandText = @"DELETE From CookBookSecond WHERE Name = @Name";
                    command.Parameters.AddWithValue("Name", textBoxDelete.Text);
                    command.ExecuteNonQuery();
                    transaction.Commit();

                    ds.Clear();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;

                    command.CommandText = @"UPDATE CookBookSecond SET Recipe = @Recipe WHERE Name = @Name";
                    command.Parameters.AddWithValue("Name", textBox1.Text);
                    command.Parameters.AddWithValue("Recipe", textBoxUpdateRecipe.Text);
                    command.ExecuteNonQuery();
                    transaction.Commit();

                    ds.Clear();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                }
                finally
                {
                    connection.Close();
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {


                    string sql = @"SELECT * FROM CookBookSecond WHERE Category=@Category";

                    command = new SqlCommand(sql, connection);

                    connection.Open();
                    command.Parameters.AddWithValue("Category", comboBox1.SelectedItem.ToString());

                    command.ExecuteNonQuery();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        richTextBox1.AppendText("\nИндекс: " + reader[0].ToString() + "\nНазвание: " + reader[1].ToString() + "\nПродукты: " + reader[2].ToString() + "\nРецепт: " + reader[3].ToString() + "\nКатегория: " + reader[4].ToString() + "\n-------------------------------------------");
                    }
                    reader.Close();

                    connection.Close();

                }
                catch (SqlException ex)
                {

                }
                finally
                {
                    connection.Close();
                }

            }
        }

        private void textBoxId_TextChanged(object sender, EventArgs e)
        {
            if (textBoxId.Text != "" && textBoxName.Text != "" && textBoxProducts.Text != "" && textBoxRecipe.Text != "")
            {
                button1.Enabled = true;
            }
            else button1.Enabled=false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBoxUpdateRecipe.Text != "")
            {
                button3.Enabled = true;
            }
            else button3.Enabled = false;
        }

        private void textBoxDelete_TextChanged(object sender, EventArgs e)
        {
            if (textBoxDelete.Text != "")
            {
                button2.Enabled = true;
            }
            else button2.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
    }
}
