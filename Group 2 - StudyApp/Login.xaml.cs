﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace Group_2___StudyApp
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        void GetUsername()
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.DataConString);
            con.Open();
            string sqlquery = "select CONCAT (Firstname, ' ',Lastname) as Fullname from Student Where Student_Id = ('" + tbxId.Text + "')";
            SqlCommand cmd = new SqlCommand(sqlquery, con);

            string value = cmd.ExecuteScalar().ToString();
            if (value != null)
            {
                MainWindow.Username = value;
            }

        }
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(Properties.Settings.Default.DataConString);
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string query = "Select count(1) from Student where Student_Id=@Student_Id and password=@password";
                SqlCommand sqlCmd = new SqlCommand(query, con);
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@Student_Id", tbxId.Text);
                sqlCmd.Parameters.AddWithValue("@password", pbxPass.Password);
                int count = Convert.ToInt32(sqlCmd.ExecuteScalar());
                if (count == 1)
                {
                    if (tbxId.Text == "admin")
                    {
                        new Window2().Show();
                        this.Close();
                    }
                    else
                    {
                        GetUsername();
                        MainWindow.UserID = tbxId.Text;
                        new MainWindow().Show();
                        this.Close();
                    }
                    
                }
                else
                {
                    MessageBox.Show("Username or password is incorrect");
                    pbxPass.Password = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            new SignUp().Show();
            this.Close();
        }

        private void BtnResetPass_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
