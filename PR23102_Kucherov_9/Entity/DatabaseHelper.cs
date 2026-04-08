using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using PR23102_Kucherov_9.Entity;

namespace PR23102_Kucherov_9
{
    public class DatabaseHelper
    {
        private string connectionString = "Server=MISHA-FRACTAL;Database=PR23102_09_RK2026;Trusted_Connection=True;";

        public List<Teachers> SearchTeachers(string lastName, string sortType)
        {
            List<Teachers> teachers = new List<Teachers>();

            string query = @"SELECT t.IdTeachers, t.LastName, t.FirstName, t.Patronymic, t.Email, s.Title as SpecialityTitle
                             FROM Teachers t
                             LEFT JOIN Speciality s ON t.IdSpeciality = s.IdSpeciality
                             WHERE t.LastName LIKE @lastName";

            // Добавляем сортировку
            if (sortType == "По убыванию имени")
                query += " ORDER BY t.FirstName DESC";
            else if (sortType == "По возрастанию специальности")
                query += " ORDER BY s.Title ASC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@lastName", "%" + lastName + "%");

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Teachers teacher = new Teachers
                    {
                        IdTeachers = Convert.ToInt32(reader["IdTeachers"]),
                        LastName = reader["LastName"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        Patronymic = reader["Patronymic"]?.ToString(),
                        Email = reader["Email"].ToString(),
                        SpecialityTitle = reader["SpecialityTitle"]?.ToString()
                    };
                    teachers.Add(teacher);
                }
            }
            return teachers;
        }

        public List<Teachers> GetAllTeachers()
        {
            return SearchTeachers("", "По убыванию имени");
        }
    }
}