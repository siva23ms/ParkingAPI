using System.Data;
using System.Data.SqlClient;

public class DatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DataTable ExecuteQuery(string query, SqlParameter[] ? parameters = null)
    {
        DataTable table = new DataTable();
        using (SqlConnection myCon = new SqlConnection(_connectionString))
        {
            myCon.Open();
            using (SqlCommand myCommand = new SqlCommand(query, myCon))
            {
                if (parameters != null)
                {
                    myCommand.Parameters.AddRange(parameters);
                }

                using (SqlDataReader myReader = myCommand.ExecuteReader())
                {
                    table.Load(myReader);
                }
            }
        }
        return table;
    }

    public void ExecuteNonQuery(string query, SqlParameter[] ? parameters = null)
    {
        using (SqlConnection myCon = new SqlConnection(_connectionString))
        {
            myCon.Open();
            using (SqlCommand myCommand = new SqlCommand(query, myCon))
            {
                if (parameters != null)
                {
                    myCommand.Parameters.AddRange(parameters);
                }

                myCommand.ExecuteNonQuery();
            }
        }
    }
}
