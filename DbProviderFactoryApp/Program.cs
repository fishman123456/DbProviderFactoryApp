using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbProviderFactoryApp
{
    internal class Program
    {
        //https://www.cyberforum.ru/ado-net/thread1394014.html здесь ответ
        static void Main(string[] args)
        {
            // реализация инвариантного кода работы с БД
            try
            {
                // 1. чтение конфигов
                string useProvider = ConfigurationManager.AppSettings["useProvider"];
                string useProviderName = ConfigurationManager.AppSettings[useProvider];
                string useConnection = ConfigurationManager.AppSettings["useConnection"];
                string connectionString = ConfigurationManager.ConnectionStrings[useConnection].ConnectionString;
                Console.WriteLine($"User {useProvider}: {useProviderName}");

                // 2. создание фабрики на основе конфигов 
                DbProviderFactory dbFactory = DbProviderFactories.GetFactory(useProviderName);

                // 3. реализуем некоторую логику работы с БД
                DbConnection dbConnection = dbFactory.CreateConnection();
                dbConnection.ConnectionString = connectionString;
                dbConnection.Open();
                DbCommand dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "SELECT * FROM light_table_t;";
                DbDataReader dbDataReader = dbCommand.ExecuteReader();
                while (dbDataReader.Read())
                {
                    Console.WriteLine($"{dbDataReader[0]} - {dbDataReader[1]}");
                }
                dbDataReader.Close();   
                dbConnection.Close();
            } catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            }
            Console.ReadLine();
        }
    }
}
