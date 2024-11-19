using System;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace POEPART3CMCS.Models
{
    public class DBContext
    {
        private static readonly string constr = "server=localhost;uid=POEPART3CMCS;pwd=;database=poepart3";

        public static (int, string) Login(string username, string password)
        {
            try
            {
                using MySqlConnection con = new(constr);
                con.Open();

                string qry = @"SELECT id, roles FROM user WHERE username = @un AND password = @pwd";
                using MySqlCommand cmd = new(qry, con);
                cmd.Parameters.AddWithValue("@un", username);
                cmd.Parameters.AddWithValue("@pwd", password);

                using MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                return (reader.GetInt32("id"), reader.GetString("role"));

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return (0, " ");
            }
        }
        public static UserDetails GetUserDetails(int id)
        {
            try
            {
                using MySqlConnection con = new(constr);
                con.Open();

                string qry = @"SELECT username, firstname, lastname, email, cell FROM user WHERE id = @id";
                using MySqlCommand cmd = new(qry, con);
                cmd.Parameters.AddWithValue("@id", id);

                using MySqlDataReader reader = cmd.ExecuteReader();

                reader.Read();

                UserDetails newUser = new()
                {
                    username = reader.GetString("username"),
                    firstName = reader.GetString("firstname"),
                    lastName = reader.GetString("lastname"),
                    email = reader.GetString("email"),
                    cell = reader.GetString("cell")
                };

                return newUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return new();
            }
        }

        public static bool CreateUser(RegisterModel model, bool isAdmin)
        {
            try
            {
                using MySqlConnection con = new(constr);
                con.Open();

                string qry = @"INSERT INTO user (username, password, firstname, lastname, email, cell, role) 
                       VALUES (@un, @pwd, @fn, @ln, @mail, @cell, @role)";
                using MySqlCommand cmd = new(qry, con);
                cmd.Parameters.AddWithValue("@un", model.Username);
                cmd.Parameters.AddWithValue("@pwd", model.Password);
                cmd.Parameters.AddWithValue("@fn", model.FirstName);
                cmd.Parameters.AddWithValue("@ln", model.LastName);
                cmd.Parameters.AddWithValue("@mail", model.Email);
                cmd.Parameters.AddWithValue("@cell", model.Cell);
                cmd.Parameters.AddWithValue("@role", model.role);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool AddClaims(Claim model)
        {
            try
            {
                using var con = new MySqlConnection(constr);
                con.Open();

                using var memoryStream = new MemoryStream();
                model.Document.CopyTo(memoryStream);
                byte[] documentData = memoryStream.ToArray();

                string qry = @"INSERT INTO Claim (claimdate,hours,rate)
                             VALUES (@claimdate, @hours, @rate)";

                using var cmd = new MySqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@claimdate", model.DateClaimed);
                cmd.Parameters.AddWithValue("@hours", model.HoursWorked);
                cmd.Parameters.AddWithValue("@rate", model.HourlyRate);

                cmd.ExecuteNonQuery();
                return true;
            }catch (Exception)
            {
                return false;
            }
        }

        public static List<ClaimItemModel> GetClaims()
        {
            List<ClaimItemModel> claimItemList = [];

            using MySqlConnection con = new(constr);
            con.Open();

            string qry = "SELECT claimdate, hours, rate, d.document, d.filetype FROM Claim c ON c.id = d.claim_id) ";
            using MySqlCommand cmd = new(qry, con);
            using MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Document documents = new()
                {
                    document_data = reader["d.document"] as byte[] ?? [],
                    filetype = reader.GetString("d.filetype")
                };

                ClaimItemModel claim = new()
                {
                    Id = reader.GetInt32("id"),
                    DateClaimed = reader.GetDateTime("claimdate"),
                    Hours = reader.GetInt32("hours"),
                    rate = reader.GetDouble("rate"),
                    Document = (IFormFile)documents
                };
               claimItemList.Add(claim);
            }
            return claimItemList;   
        }

        public static ClaimItemModel GetClaimbyID(int id)
        {
            MySqlConnection con = new(constr);
            con.Open();

            string qry = "SELECT id, claimdate, hours, rate, d.document, d.filetype FROM Claim c ON c.id = d.claim_id where id = @id ";
            MySqlCommand cmd = new(qry,con);
            cmd.Parameters.AddWithValue("@id", id);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();

            Document documents = new()
            {
                document_data = reader["d.document"] as byte[] ?? [],
                filetype = reader.GetString("d.filetype")
            };

            ClaimItemModel claim = new()
            {
                Id = reader.GetInt32("id"),
                DateClaimed = reader.GetDateTime("claimdate"),
                Hours = reader.GetInt32("hours"),
                rate = reader.GetDouble("rate"),
                Document = (IFormFile)documents
            };
            return claim;
        }

        public static bool editClaimByID(int id, int date , int hours, double rate)
        {
            MySqlConnection con = new(constr);
            con.Open();

            string qry = "UPDATE Claim SET claimdate = @dateclaimed" +
                "hours = @hours" +
                "rate = @rate" +
                "where id = @id ";
            MySqlCommand cmd = new(qry, con);
            cmd.Parameters.AddWithValue("@dateclaimed", date);
            cmd.Parameters.AddWithValue("@hours", hours);
            cmd.Parameters.AddWithValue("@rate", rate);
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }
        public static bool DeleteItemById(int id)
        {
            MySqlConnection con = new(constr);
            con.Open();

            string qry = "DELETE FROM Claim where id = @id";
            MySqlCommand cmd = new(qry, con);
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
