using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;

namespace DatabasePerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=PerformanceTest.db;Version=3;";

            // Veritabanýný oluþtur ve tabloyu ekle
            InitializeDatabase(connectionString);

            // Performans testi yap
            MeasurePerformance(connectionString);

            Console.ReadLine();
        }

        static void InitializeDatabase(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = "CREATE TABLE IF NOT EXISTS SampleTable (ID INTEGER PRIMARY KEY, Name TEXT);";
                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Örnek veri ekleyelim
                string insertDataQuery = "INSERT INTO SampleTable (Name) VALUES ('Example Name');";
                using (SqliteCommand command = new SQLiteCommand(insertDataQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        static void MeasurePerformance(string connectionString)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Performans testi için örnek bir sorgu
                string query = "SELECT * FROM SampleTable;";
                Stopwatch stopwatch = new Stopwatch();

                // Sorguyu belirli sayýda tekrar et
                int numberOfExecutions = 1000;

                for (int i = 0; i < numberOfExecutions; i++)
                {
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        stopwatch.Start();
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            // Sorgu sonuçlarýný oku (burada iþlem yapmýyoruz, sadece sorgu süresini ölçüyoruz)
                        }
                        stopwatch.Stop();
                    }
                }

                // Ortalama sorgu süresini hesapla
                double averageQueryTime = (double)stopwatch.ElapsedMilliseconds / numberOfExecutions;

                Console.WriteLine($"Ortalama Sorgu Süresi: {averageQueryTime} ms");
            }
        }
    }
}
