using System;
using System.Collections.Generic;
using System.IO;
using MySql.Data.MySqlClient;

namespace ProjectNet
{
    public class PatientManager
    {
        public void AddPatient(Patient p)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Patient (TEL, Nom, Prenom, Genre, DateNaissance, Adresse) VALUES (@tel, @nom, @prenom, @genre, @dob, @adresse)";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tel", p.Tel);
                    cmd.Parameters.AddWithValue("@nom", p.Nom);
                    cmd.Parameters.AddWithValue("@prenom", p.Prenom);
                    cmd.Parameters.AddWithValue("@genre", p.Genre);
                    cmd.Parameters.AddWithValue("@dob", p.DateNaissance);
                    cmd.Parameters.AddWithValue("@adresse", p.Adresse);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePatient(Patient p)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "UPDATE Patient SET TEL=@tel, Nom=@nom, Prenom=@prenom, Genre=@genre, DateNaissance=@dob, Adresse=@adresse WHERE ID=@id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", p.Id);
                    cmd.Parameters.AddWithValue("@tel", p.Tel);
                    cmd.Parameters.AddWithValue("@nom", p.Nom);
                    cmd.Parameters.AddWithValue("@prenom", p.Prenom);
                    cmd.Parameters.AddWithValue("@genre", p.Genre);
                    cmd.Parameters.AddWithValue("@dob", p.DateNaissance);
                    cmd.Parameters.AddWithValue("@adresse", p.Adresse);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeletePatient(int id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                
                string query = "DELETE FROM Patient WHERE ID=@id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Patient> GetAllPatients()
        {
            List<Patient> list = new List<Patient>();
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Patient";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Patient
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                Tel = reader["TEL"].ToString(),
                                Nom = reader["Nom"].ToString(),
                                Prenom = reader["Prenom"].ToString(),
                                Genre = reader["Genre"].ToString(),
                                DateNaissance = Convert.ToDateTime(reader["DateNaissance"]),
                                Adresse = reader["Adresse"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public void ExportPatientsToTextFile(string filePath)
        {
            var patients = GetAllPatients();
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine("ID | Nom | Prenom | Tel | Genre | Date Naissance | Adresse");
                sw.WriteLine(new string('-', 50));
                foreach (var p in patients)
                {
                    sw.WriteLine($"{p.Id} | {p.Nom} | {p.Prenom} | {p.Tel} | {p.Genre} | {p.DateNaissance.ToShortDateString()} | {p.Adresse}");
                }
            }
        }

        public void SaveConsultation(int patientId, string consultationDetails)
        {
            
            string fileName = $"{patientId}.txt";
            
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string directory = Path.Combine(desktopPath, "Cabinet_Consultations");
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            string path = Path.Combine(directory, fileName);

            string entry = $"Date: {DateTime.Now}\nDetails: {consultationDetails}\n-----------------------------------\n";
            File.AppendAllText(path, entry);
        }
        
        public string GetConsultationHistory(int patientId)
        {
            string fileName = $"{patientId}.txt";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string directory = Path.Combine(desktopPath, "Cabinet_Consultations");
            string path = Path.Combine(directory, fileName);
            
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return "Pas d'historique (Dossier: " + directory + ")";
        }
    }
}
