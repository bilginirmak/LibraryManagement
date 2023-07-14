using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace LibraryManagement
{
    /// <summary>
    /// Interaction logic for Librarian.xaml
    /// </summary>
    public partial class Librarian : Window
    {

        /*
         * Database Connection & Operation
         * 
         * The working steps of the Database Connection are:
         * 
         * 1. Database connection with the Connection String (Need to Form)
         * 2. Establish the DB Connection
         * -- Open the Connection
         * 3. Generate the SQL Command for performing SQL Operations
         * 4. Execute the Command
         * 5. Close the Connection to Save the Results
         * 
         */

        //Step> 1. Database Connection Creation

        private static string getConnectionString()
        {
            // in this method, we are going to create a connection string for the PostGreSQL server..
            string host = "Host=localhost;";
            string port = "Port=5432;";
            string dbName = "Database=libraryManagementDB;";
            string userName = "Username=postgres;";
            string password = "Password=;";
            // Now we need to add all these values/strings to one string, means we are going to merge
            // the strings together and going to create our string
            string connectionString = string.Format("{0}{1}{2}{3}{4}", host, port, dbName, userName, password);
            return connectionString;
        }

        //Step> 2. Establishing DB Connection
        /*
         * to perform any database connection, we need an adapter. For the adapter, we can consider
         * it as a library and that library we need to add in our program. 
         * 
         * for any library addition/installation, we need to search for the library in NuGet package
         * manager.
         */

        // connection adapter
        public static NpgsqlConnection con;

        // sql command adapter
        public static NpgsqlCommand cmd;
        private static void establishConnection()
        {
            // in establishing database connection, you need to use exception handling, because it helps
            // to detect if somehow the connection fails or your database server is not going to be 
            // connected

            try
            {
                // create the connection
                con = new NpgsqlConnection(getConnectionString());

                /*
                 * we actually need to pass the connectionString inside the NpgsqlConnection adapter
                 * class constructor. To do so, we are calling the getConnectionString() method as 
                 * this method returns us the Database connection String we have created for 
                 * our work. 
                 */

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            }
            public Librarian()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1()
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // establish connection
            establishConnection();
            try
            {
                // open the connection
                con.Open();

                // create the SQL query
                string Query = "insert into books values(default,@title,@author,@published_year,@isbn,@quantity )";

                // create sql Command 
                cmd = new NpgsqlCommand(Query, con); // this is the command adapter, we need to pass
                                                     // two values in the adapter, one is the query, another one is the connection itself
                DateTime publishedYear;
                if (!DateTime.TryParse(textboxPublishedYear.Text, out publishedYear))


                {
                    // Invalid date format or empty input
                    // Handle the error condition (display an error message, handle default value, etc.)
                }

                int isbn;
                if (!int.TryParse(textboxISBN.Text, out isbn))
                {
                    // Invalid integer format or empty input
                    // Handle the error condition (display an error message, handle default value, etc.)
                }

                int quantity;
                if (!int.TryParse(textboxQuantity.Text, out quantity))
                {
                    // Invalid integer format or empty input
                    // Handle the error condition (display an error message, handle default value, etc.)
                }

                // we now need to add the values for the parameters in the sql query
                cmd.Parameters.AddWithValue("@title", textboxTitle.Text);
                cmd.Parameters.AddWithValue("@author", textboxAuthor.Text);
                cmd.Parameters.AddWithValue("@published_year", NpgsqlDbType.Date, publishedYear);

                //cmd.Parameters.AddWithValue("@published_year", textboxPublishedYear.Text);
                cmd.Parameters.AddWithValue("@isbn", NpgsqlDbType.Integer, isbn);
                cmd.Parameters.AddWithValue("@quantity", NpgsqlDbType.Integer, quantity);


                // execute the Query
                cmd.ExecuteNonQuery();

                MessageBox.Show("Book Added Successfully");
                // close the Connection
                con.Close();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // establish connection
            establishConnection();
            try
            {
                con.Open();
                string Query = "select * from books";
                cmd = new NpgsqlCommand(Query, con);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataView.ItemsSource = dt.AsDataView();
                DataContext = da;  
                con.Close();
                 
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            establishConnection();
            try
            {
                con.Open();
                string Query = "select * from books where id = @ID";
                cmd = new NpgsqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@ID", int.Parse(textboxSearch.Text));
                
                var reader = cmd.ExecuteReader();

                DateTime publishedYear;
                if (!DateTime.TryParse(textboxPublishedYear.Text, out publishedYear))


                {
                    // Invalid date format or empty input
                    // Handle the error condition (display an error message, handle default value, etc.)
                }

                int isbn;
                if (!int.TryParse(textboxISBN.Text, out isbn))
                {
                    // Invalid integer format or empty input
                    // Handle the error condition (display an error message, handle default value, etc.)
                }

                int quantity;
                if (!int.TryParse(textboxQuantity.Text, out quantity))
                {
                    // Invalid integer format or empty input
                    // Handle the error condition (display an error message, handle default value, etc.)
                }

                while (reader.Read())
                {
                    textboxID.Text = reader["id"].ToString();
                    textboxTitle.Text = reader["title"].ToString();
                    textboxAuthor.Text = reader["author"].ToString();
                    textboxPublishedYear.Text = reader["published_year"].ToString();
                    textboxISBN.Text = reader["isbn"].ToString();
                    textboxQuantity.Text = reader["quantity"].ToString();
                }


                con.Close();

            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textboxSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            establishConnection();
            try
            {
                con.Open();
                string Query = "delete from books where id = @ID";
                cmd = new NpgsqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@ID", int.Parse(textboxID.Text));
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                
                // execute the Query
                cmd.ExecuteNonQuery();

                MessageBox.Show("Book Deleted Successfully");
                con.Close();
            }catch(NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textboxID_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            establishConnection();
            try
            {
                con.Open();
                string Query = "update books set title = @title, author = @author, published_year = @published_year, isbn = @isbn, quantity = @quantity where id = @ID";
                cmd = new NpgsqlCommand(Query, con);
                cmd.Parameters.AddWithValue("@title", textboxTitle.Text);
                cmd.Parameters.AddWithValue("@author", textboxAuthor.Text);

                DateTime publishedYear;
                if (!DateTime.TryParse(textboxPublishedYear.Text, out publishedYear))
                {
                    // Invalid date format or empty input
                    // Handle the error condition (display an error message, handle default value, etc.)
                }
                cmd.Parameters.AddWithValue("@published_year", NpgsqlDbType.Date, publishedYear);
                cmd.Parameters.AddWithValue("@isbn", int.Parse(textboxISBN.Text));
                cmd.Parameters.AddWithValue("@quantity", int.Parse(textboxQuantity.Text));
                cmd.Parameters.AddWithValue("@ID", int.Parse(textboxID.Text));

                // execute the Query
                cmd.ExecuteNonQuery();

                MessageBox.Show("Book Updated Successfully");
                con.Close();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void textboxSearch_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void textboxTitle_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
