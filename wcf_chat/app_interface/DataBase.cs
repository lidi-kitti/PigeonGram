using System.Data.SqlClient;

namespace login_registration

{

    internal class DataBase

    {

        SqlConnection con = new SqlConnection(@"Data Source=""10.0.13.26\SQLDEGREE, 1433""; Network Library = DBMSSOCN; Initial Catalog = Database; User ID=sa;Password=12345");




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