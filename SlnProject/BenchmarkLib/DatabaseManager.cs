using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkLib.Models;


namespace BenchmarkLib
{
    public class DatabaseManager
    {
        private const string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BenchmarkDB;Integrated Security=True;Encrypt=False";

        public List<Company> GetAllCompanies()
        {
            List<Company> companies = new List<Company>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT 
            id, name, contact, address, zip, city, country, phone, email, btw, login, password, 
            regdate, acceptdate, lastmodified, status, language, logo, nacecode_code 
            FROM Companies";
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Company company = new Company
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Contact = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Address = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Zip = reader.IsDBNull(4) ? null : reader.GetString(4),
                            City = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Country = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Phone = reader.IsDBNull(7) ? null : reader.GetString(7),
                            Email = reader.IsDBNull(8) ? null : reader.GetString(8),
                            Btw = reader.IsDBNull(9) ? null : reader.GetString(9),
                            Login = reader.IsDBNull(10) ? null : reader.GetString(10),
                            Password = reader.IsDBNull(11) ? null : reader.GetString(11),
                            RegDate = reader.IsDBNull(12) ? (DateTime?)null : reader.GetDateTime(12),
                            AcceptDate = reader.IsDBNull(13) ? (DateTime?)null : reader.GetDateTime(13),
                            LastModified = reader.IsDBNull(14) ? (DateTime?)null : reader.GetDateTime(14),
                            Status = reader.IsDBNull(15) ? null : reader.GetString(15),
                            Language = reader.IsDBNull(16) ? null : reader.GetString(16),
                            Logo = reader.IsDBNull(17) ? null : (byte[])reader[17],
                            Nacecode_Code = reader.IsDBNull(18) ? null : reader.GetString(18)
                        };
                        companies.Add(company);
                    }
                }
            }
            return companies;
        }


    }
}
