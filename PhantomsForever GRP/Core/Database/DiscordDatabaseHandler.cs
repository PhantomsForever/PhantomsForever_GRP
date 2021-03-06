﻿using PhantomsForever_GRP.Core.Discord;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhantomsForever_GRP.Core.Database
{
    public class DiscordDatabaseHandler
    {
        private static readonly string DbPath = Path.Combine(Application.StartupPath, "discord.sqlite");
        private static readonly string ConnectionString = "Data Source=" + DbPath + ";Version=3;";
        public static void Install()
        {
            string[] queries = new string[]
            {
                "create table mutes (id varchar(128), roles varchar(512), untill varchar(150))"
            };
            if (!File.Exists(DbPath))
                SQLiteConnection.CreateFile(DbPath);
            using (var CONN = new SQLiteConnection(ConnectionString))
            {
                CONN.Open();
                using (var tr = CONN.BeginTransaction())
                {
                    foreach(var query in queries)
                    {
                        using (var COMMAND = new SQLiteCommand(query, CONN, tr))
                        {
                            COMMAND.ExecuteNonQuery();
                        }
                    }
                    tr.Commit();
                }
            }
        }
        public static void MuteUser(string id, string roles, string untill)
        {
            var query = "insert into mutes (id, roles, untill) values (@id, @roles, @untill)";
            try
            {
                using (var CONN = new SQLiteConnection(ConnectionString))
                {
                    CONN.Open();
                    using (var COMMAND = new SQLiteCommand(query, CONN))
                    {
                        COMMAND.Parameters.Add(new SQLiteParameter("@id", id));
                        COMMAND.Parameters.Add(new SQLiteParameter("@roles", roles));
                        COMMAND.Parameters.Add(new SQLiteParameter("@untill", untill));
                        COMMAND.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception)
            {

            }
        }
        public static void CheckTimers()
        {
            var query = "select * from mutes";
            try
            {
                var users = new Dictionary<string, string>();
                using (var CONN = new SQLiteConnection(ConnectionString))
                {
                    CONN.Open();
                    using (var COMMAND = new SQLiteCommand(query, CONN))
                    {
                        using (var READER = COMMAND.ExecuteReader())
                        {
                            while(READER.Read())
                            {
                                if (READER.FieldCount == 0 || !READER.HasRows)
                                    continue;
                                if (DateTime.Parse((string)READER["untill"]) <= DateTime.Now)
                                {
                                    users.Add((string)READER["id"], (string)READER["roles"]);
                                }
                            }
                        }
                    }
                }
                foreach(var user in users)
                    Unmute(user.Key, user.Value);
            }
            catch (Exception)
            {

            }
        }
        public static void Unmute(string id)
        {
            var query = "select roles from mutes where id=@id";
            try
            {
                string roles = "";
                using (var CONN = new SQLiteConnection(ConnectionString))
                {
                    CONN.Open();
                    using (var COMMAND = new SQLiteCommand(query, CONN))
                    {
                        COMMAND.Parameters.Add(new SQLiteParameter("@id", id));
                        using (var READER = COMMAND.ExecuteReader())
                        {
                            while(READER.Read())
                            {
                                roles = (string)READER["roles"];
                            }
                        }
                    }
                }
                PhantomsForeverBot.Instance.UnmuteUser(id, roles);
            }
            catch (Exception)
            {

            }
        }
        public static void Unmute(string id, string roles)
        {
            var query = "delete from mutes where id=@id";
            try
            {
                using (var CONN = new SQLiteConnection(ConnectionString))
                {
                    CONN.Open();
                    using (var COMMAND = new SQLiteCommand(query, CONN))
                    {
                        COMMAND.Parameters.Add(new SQLiteParameter("@id", id));
                        COMMAND.ExecuteNonQuery();
                    }
                }
                PhantomsForeverBot.Instance.UnmuteUser(id, roles);
            }
            catch (Exception)
            {

            }
        }
    }
}