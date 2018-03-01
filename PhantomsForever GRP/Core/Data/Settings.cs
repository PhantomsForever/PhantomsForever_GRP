using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace PhantomsForever_GRP.Core.Data
{
    public static class Settings
    {
        private static readonly string SettingsPath = Path.Combine(Application.StartupPath, "settings.xml");
        public static string RepositoryURL = @"https://github.com/PhantomsForever/PhantomsForever_GRP";
        public static string DiscordBotToken
        {
            get
            {
                return ReadValueSafe("DiscordBotToken");
            }
            set
            {
                WriteValue("DiscordBotToken", value.ToString());
            }
        }
        public static ulong Guild
        {
            get
            {
                return Convert.ToUInt64(ReadValueSafe("Guild"));
            }
            set
            {
                WriteValue("Guild", value.ToString());
            }
        }
        public static ulong LogChannel
        {
            get
            {
                return Convert.ToUInt64(ReadValueSafe("LogChannel"));
            }
            set
            {
                WriteValue("LogChannel", value.ToString());
            }
        }
        public static ulong AdministratorRole
        {
            get
            {
                return Convert.ToUInt64(ReadValueSafe("AdministratorRole"));
            }
            set
            {
                WriteValue("AdministratorRole", value.ToString());
            }
        }
        public static ulong ModeratorRole
        {
            get
            {
                return Convert.ToUInt64(ReadValueSafe("ModeratorRole"));
            }
            set
            {
                WriteValue("ModeratorRole", value.ToString());
            }
        }
        private static string ReadValue(string pstrValueToRead)
        {
            try
            {
                var doc = new XPathDocument(SettingsPath);
                var nav = doc.CreateNavigator();
                var expr = nav.Compile(@"/settings/" + pstrValueToRead);
                var iterator = nav.Select(expr);
                while (iterator.MoveNext())
                {
                    return iterator.Current.Value;
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string ReadValueSafe(string pstrValueToRead, string defaultValue = "")
        {
            string value = ReadValue(pstrValueToRead);
            return (!string.IsNullOrEmpty(value)) ? value : defaultValue;
        }

        private static void WriteValue(string pstrValueToRead, string pstrValueToWrite)
        {
            try
            {
                var doc = new XmlDocument();

                if (File.Exists(SettingsPath))
                {
                    using (var reader = new XmlTextReader(SettingsPath))
                    {
                        doc.Load(reader);
                    }
                }
                else
                {
                    var dir = Path.GetDirectoryName(SettingsPath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    doc.AppendChild(doc.CreateElement("settings"));
                }

                var root = doc.DocumentElement;
                var oldNode = root.SelectSingleNode(@"/settings/" + pstrValueToRead);
                if (oldNode == null)
                {
                    oldNode = doc.SelectSingleNode("settings");
                    oldNode.AppendChild(doc.CreateElement(pstrValueToRead)).InnerText = pstrValueToWrite;
                    doc.Save(SettingsPath);
                    return;
                }
                oldNode.InnerText = pstrValueToWrite;
                doc.Save(SettingsPath);
            }
            catch
            {
            }
        }
    }
}