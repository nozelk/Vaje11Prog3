using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string povNiz = @"Data Source=C:\Users\kosmr\Desktop\CSharp\Vaje11\filmi.sqlite; Version=3;";
            SQLiteConnection povezava = new SQLiteConnection(povNiz);
            povezava.Open();
            SQLiteCommand ukaz = new SQLiteCommand();
            ukaz.CommandType = CommandType.Text;
            ukaz.Connection = povezava;

            Console.WriteLine("Vnesi stavek: ");
            string preberi = Console.ReadLine();

            ukaz.CommandText = preberi;


            SQLiteDataReader rez = ukaz.ExecuteReader();
            while (rez.Read())
            {
                for (int i = 0; i < rez.VisibleFieldCount; i++)
                {
                    Console.WriteLine(rez[i].ToString());
                }
                Console.WriteLine();
            }
            povezava.Close();
        }
    }
}
