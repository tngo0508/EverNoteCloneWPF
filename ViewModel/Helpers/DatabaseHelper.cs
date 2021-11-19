using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace EverNoteCloneWPF.ViewModel.Helpers
{
    public class DatabaseHelper
    {
        private static readonly string _dbFile = Path.Combine(Environment.CurrentDirectory, "notesDb.db3");

        public static bool Insert<T>(T item)
        {
            var result = false;
            using var connection = new SQLiteConnection(_dbFile);
            connection.CreateTable<T>();
            var rows = connection.Insert(item);
            if (rows > 0) result = true;

            return result;
        }

        public static bool Update<T>(T item)
        {
            var result = false;
            using var connection = new SQLiteConnection(_dbFile);
            connection.CreateTable<T>();
            var rows = connection.Update(item);
            if (rows > 0) result = true;

            return result;
        }

        public static bool Delete<T>(T item)
        {
            var result = false;
            using var connection = new SQLiteConnection(_dbFile);
            connection.CreateTable<T>();
            var rows = connection.Delete(item);
            if (rows > 0) result = true;

            return result;
        }

        public static List<T> Read<T>() where T : new()
        {
            List<T> items;
            using SQLiteConnection connection = new SQLiteConnection(_dbFile);
            connection.CreateTable<T>();
            items = connection.Table<T>().ToList();

            return items;
        }
    }
}
