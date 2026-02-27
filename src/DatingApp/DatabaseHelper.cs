using System;
using System.Data.SQLite;

namespace DatingApp
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString = "Data Source=users.db;Version=3;Cache=Shared;";

        public static void ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            using var pragma = new SQLiteCommand("PRAGMA busy_timeout = 5000;", conn);
            pragma.ExecuteNonQuery();

            using var cmd = new SQLiteCommand(sql, conn);
            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);
            cmd.ExecuteNonQuery();
        }

        public static object ExecuteScalar(string sql, params SQLiteParameter[] parameters)
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            using var pragma = new SQLiteCommand("PRAGMA busy_timeout = 5000;", conn);
            pragma.ExecuteNonQuery();

            using var cmd = new SQLiteCommand(sql, conn);
            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteScalar();
        }

        public static SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] parameters)
        {
            var conn = new SQLiteConnection(connectionString);
            conn.Open();

            using var pragma = new SQLiteCommand("PRAGMA busy_timeout = 5000;", conn);
            pragma.ExecuteNonQuery();

            var cmd = new SQLiteCommand(sql, conn);
            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }
    }
}
