using ParkingAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace ParkingAPI.Services
{
    public class UserService
    {
        private readonly DatabaseHelper _dbHelper;

        public UserService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("HyperParking");
            _dbHelper = new DatabaseHelper(connectionString);
        }

        public DataTable GetAllUsers()
        {
            string query = "SELECT * FROM Users";
            return _dbHelper.ExecuteQuery(query);
        }

        public DataTable GetUserById(int id)
        {
            string query = "SELECT * FROM Users WHERE Id = @Id";
            SqlParameter[] parameters = { new SqlParameter("@Id", id) };
            return _dbHelper.ExecuteQuery(query, parameters);
        }

        public void AddUser(User user)
        {
            string query = @"
                INSERT INTO Users (Username, Email, Phone, Age, Address, Qualification, DateOfBirth, Status, FirstName, LastName, MiddleName, Password) 
                VALUES (@Username, @Email, @Phone, @Age, @Address, @Qualification, @DateOfBirth, @Status, @FirstName, @LastName, @MiddleName, @Password)";

            SqlParameter[] parameters = {
                new SqlParameter("@Username", user.Username),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Phone", user.Phone),
                new SqlParameter("@Age", user.Age),
                new SqlParameter("@Address", user.Address),
                new SqlParameter("@Qualification", user.Qualification),
                new SqlParameter("@DateOfBirth", user.DateOfBirth),
                new SqlParameter("@Status", user.Status),
                new SqlParameter("@FirstName", user.FirstName),
                new SqlParameter("@LastName", user.LastName),
                new SqlParameter("@MiddleName", user.MiddleName),
                new SqlParameter("@Password", user.Password)
            };

            _dbHelper.ExecuteNonQuery(query, parameters);
        }

        public void UpdateUser(User user)
        {
            string query = @"
                UPDATE Users 
                SET Username = @Username, 
                    Email = @Email, 
                    Phone = @Phone, 
                    Age = @Age, 
                    Address = @Address,
                    Qualification = @Qualification,
                    DateOfBirth = @DateOfBirth,
                    Status = @Status,
                    FirstName = @FirstName,
                    LastName = @LastName,
                    MiddleName = @MiddleName
                WHERE Id = @Id";

            SqlParameter[] parameters = {
                new SqlParameter("@Id", user.Id),
                new SqlParameter("@Username", user.Username),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Phone", user.Phone),
                new SqlParameter("@Age", user.Age),
                new SqlParameter("@Address", user.Address),
                new SqlParameter("@Qualification", user.Qualification),
                new SqlParameter("@DateOfBirth", user.DateOfBirth),
                new SqlParameter("@Status", user.Status),
                new SqlParameter("@FirstName", user.FirstName),
                new SqlParameter("@LastName", user.LastName),
                new SqlParameter("@MiddleName", user.MiddleName)
            };

            _dbHelper.ExecuteNonQuery(query, parameters);
        }

        public void DeleteUser(int id)
        {
            string query = "DELETE FROM Users WHERE Id = @Id";
            SqlParameter[] parameters = { new SqlParameter("@Id", id) };
            _dbHelper.ExecuteNonQuery(query, parameters);
        }

        public User ValidateUser(string username, string password)
        {
            string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            };
            DataTable table = _dbHelper.ExecuteQuery(query, parameters);

            if (table.Rows.Count == 1)
            {
                var row = table.Rows[0];
                return new User
                {
                    Username = row["Username"].ToString(),
                    Email = row["Email"].ToString()
                };
            }

            return null;
        }
    }
}
