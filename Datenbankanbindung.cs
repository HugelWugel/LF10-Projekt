using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using LF10_Lager_Projekt;
using System.Collections.ObjectModel;

namespace Datenbankanbindung
{
    public class Backend
    {
        private static string databasePath = @"C:\Users\simmatm\source\repos\LF10\Lager.mdf";
        private static string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={databasePath};Integrated Security=True";
        public static DataTable Bestand = new DataTable();
        public static DataTable kritBes = new DataTable();
        private static ObservableCollection<Lagerartikel> AllArtikel = new ObservableCollection<Lagerartikel>();
        private static ObservableCollection<kritArtikel> KritArtikel = new ObservableCollection<kritArtikel>();

        public ObservableCollection<Lagerartikel> getAllData()
        {
            AllArtikel.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Lagerbestand where Materialnummer is not null";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AllArtikel.Add(new Lagerartikel
                        {
                            Materialnummer = Convert.ToInt32(reader["Materialnummer"]),
                            Materialname = reader["Materialname"].ToString(),
                            Warengruppe= reader["Warengruppe"].ToString(),
                            Menge = Convert.ToInt32(reader["Menge"]),
                            Grenzwert = Convert.ToInt32(reader["Grenzwert"])
                        });
                    }
                }
            }
            return AllArtikel;
        }

        public ObservableCollection<kritArtikel> getKritData()
        {
            KritArtikel.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select Materialnummer, Menge, Grenzwert From Lagerbestand Where Menge < Grenzwert and Materialnummer is not null";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        KritArtikel.Add(new kritArtikel
                        {
                            Materialnummer = Convert.ToInt32(reader["Materialnummer"]),
                            Menge = Convert.ToInt32(reader["Menge"]),
                            Grenzwert = Convert.ToInt32(reader["Grenzwert"])
                        });
                    }
                }
            }
            return KritArtikel;
        }

        public static void createDataEntry(string materialname, string warengruppe, int menge, int grenzwert)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Lagerbestand (Materialname, Warengruppe, Menge, Grenzwert) Values(@Materialname, @Warengruppe, @Menge, @Grenzwert)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    // Parameter setzen, um SQL-Injection zu vermeiden
                    command.Parameters.AddWithValue("@Materialname", materialname);
                    command.Parameters.AddWithValue("@Warengruppe", warengruppe);
                    command.Parameters.AddWithValue("@Menge", menge);
                    command.Parameters.AddWithValue("@Grenzwert", grenzwert);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void changeDataEntry(int id, string materialname, string warengruppe, int menge, int grenzwert)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select Materialnummer, Menge, Grenzwert From Lagerbestand Where Menge < Grenzwert and Materialnummer is not null";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Parameter setzen, um SQL-Injection zu vermeiden
                    command.Parameters.AddWithValue("@Materialname", materialname);
                    command.Parameters.AddWithValue("@Warengruppe", warengruppe);
                    command.Parameters.AddWithValue("@Menge", menge);
                    command.Parameters.AddWithValue("@Grenzwert", grenzwert);

                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public static void deleteDataEntry(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select Materialnummer, Menge, Grenzwert From Lagerbestand Where Menge < Grenzwert and Materialnummer is not null";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Parameter setzen, um SQL-Injection zu vermeiden
                    command.Parameters.AddWithValue("@Materialnummer", id);

                    int rowsAffected = command.ExecuteNonQuery();
                }
            }
        }
    }
}