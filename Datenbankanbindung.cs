using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;

namespace Datenbankanbindung
{
    public class Backend
    {
        private static string databasePath = @"C:\Users\simmatm\source\repos\LF10\Lager.mdf";
        private static string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={databasePath};Integrated Security=True";
        public static DataTable Bestand = new DataTable();
        public static DataTable kritBes = new DataTable();

        public static DataTable getAllData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Lagerbestand where Materialnummer is not null";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                {
                    connection.Open();
                    adapter.Fill(Bestand);
                    connection.Close();
                }
            }
            return Bestand;
        }

        public static DataTable getKritData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select Materialnummer, Menge, Grenzwert From Lagerbestand Where Menge < Grenzwert and Materialnummer is not null";
                SqlDataAdapter adapterKrit = new SqlDataAdapter(query, connection);
                {
                    connection.Open();
                    adapterKrit.Fill(kritBes);
                    connection.Close();
                }
            }
            return kritBes;
        }

        public static int createDataEntry(string materialname, string warengruppe, int menge, int grenzwert)
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

                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected;
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