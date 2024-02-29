using System.Data.SqlClient;

namespace login_registration

{

    internal class DataBase

    {

        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-S3L918JB; Initial Catalog=test; Integrated Security=True");




        public void openConnection()

        {

            if (con.State == System.Data.ConnectionState.Closed)

            {

                con.Open();

            }

        }


        public void closeConnection()

        {

            if (con.State == System.Data.ConnectionState.Open)

            {

                con.Close();

            }

        }

        public SqlConnection getConnection()

        {

            return con;

        }

    }

}