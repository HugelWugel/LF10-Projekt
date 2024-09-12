using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using LF10_Lager_Projekt;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace Datenbankanbindung
{
    public class dbService
    {
        private static string databasePath = @"C:\Users\simmatm\source\repos\LF10\Lager.mdf";
        private static string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={databasePath};Integrated Security=True";
        public static DataTable Bestand = new DataTable();
        public static DataTable kritBes = new DataTable();
        public static ObservableCollection<Lagerartikel> AllArtikel = new ObservableCollection<Lagerartikel>();
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
                connection.Close();
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
                connection.Close();
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
                    command.Parameters.AddWithValue("@Materialname", materialname);
                    command.Parameters.AddWithValue("@Warengruppe", warengruppe);
                    command.Parameters.AddWithValue("@Menge", menge);
                    command.Parameters.AddWithValue("@Grenzwert", grenzwert);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public static void changeDataEntry(int id, string materialname, string warengruppe, int menge, int grenzwert)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"Update Lagerbestand SET Materialname = @Materialname, Warengruppe = @Warengruppe, Menge = @Menge, Grenzwert = @Grenzwert WHERE Materialnummer = @ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@Materialname", materialname);
                    command.Parameters.AddWithValue("@Warengruppe", warengruppe);
                    command.Parameters.AddWithValue("@Menge", menge);
                    command.Parameters.AddWithValue("@Grenzwert", grenzwert);

                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public static int deleteDataEntry(List<int> ids)
        {
            int rowsAffected;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"DELETE FROM Lagerbestand WHERE Materialnummer IN ({string.Join(",", ids.Select((id, index) => $"@id{index}"))})";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    for (int i = 0; i < ids.Count; i++)
                    {
                        command.Parameters.AddWithValue($"@id{i}", ids[i]);
                    }
                    rowsAffected = command.ExecuteNonQuery();
                }
                connection.Close();
            }
            return rowsAffected;
        }
    }
}