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
    public partial class Registration : Form
    {
        DataSet ds;
        DataTable dt;
        SqlCommand command;
        SqlDataAdapter adapter;
        SqlTransaction transaction;
        SqlCommandBuilder builder;

        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=YPProject;Integrated Security=True";


        public Registration()
        {
            InitializeComponent();
            string sql = "SELECT * FROM Registration";
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
            using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=YPProject;Integrated Security=True"))
            {
                try
                {
                    connection.Open();

                    transaction = connection.BeginTransaction();
                    command = connection.CreateCommand();
                    command.Transaction = transaction;

                    command.CommandText = @"INSERT INTO Registration(Login, Password) VALUES(@Login, @Password)";
                    command.Parameters.AddWithValue("Login", textBoxLogin.Text);
                    command.Parameters.AddWithValue("Password", textBoxPassword.Text);
                    command.ExecuteNonQuery();
                    transaction.Commit();

                    dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    row["Login"] = textBoxLogin.Text;
                    row["Password"] = textBoxPassword.Text;
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

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == textBoxPassword2.Text)
            {
                button1.Enabled = true;
            }
            else button1.Enabled = false;
            if (textBoxLogin.Text != "" && textBoxPassword.Text != "")
            {
                button2.Enabled = true;
            }
            else button2.Enabled = false;

        }

        private void textBoxLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((char)e.KeyChar == (Char)Keys.Back) return;
            if (char.IsDigit(e.KeyChar) || char.IsLetter(e.KeyChar)) return;
            e.Handled = true;
        }
        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            textBoxPassword.PasswordChar = '*';

        }
        private void button3_MouseUp_1(object sender, MouseEventArgs e)
        {
            textBoxPassword2.PasswordChar = '*';

        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            textBoxPassword2.PasswordChar = '\0';
        }

        private void button2_Click(object sender, EventArgs e)
        {

            int i = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {



                connection.Open();
                command = new SqlCommand(@"SELECT * FROM Registration WHERE Login=@Login and Password = @Password", connection);


                command.Parameters.AddWithValue("Login", textBoxLogin.Text);
                command.Parameters.AddWithValue("Password", textBoxPassword.Text);
                adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);



                i = Convert.ToInt32(dt.Rows.Count.ToString());
                if (i == 0)
                {
                    MessageBox.Show("Неправильные данные");
                }
                else
                {
                    MessageBox.Show("Вы вошли");
                    this.Close();
                }

            }

            }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            textBoxPassword.PasswordChar = '\0';

        }
    }
}

