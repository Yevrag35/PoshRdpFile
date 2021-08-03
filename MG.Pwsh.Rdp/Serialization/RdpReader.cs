using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace MG.Pwsh.Rdp.Serialization
{
    public static class RdpReader
    {
        private const BindingFlags FLAGS = BindingFlags.NonPublic | BindingFlags.Static;
        internal static readonly UnicodeEncoding _encoding =
            new UnicodeEncoding(false, true, true);

        private static TEnum CastEnum<TEnum>(object value) where TEnum : Enum
        {
            if (value is int intVal)
            {
                return (TEnum)Enum.ToObject(typeof(TEnum), intVal);
            }
            else if (value is string strVal)
            {
                return (TEnum)Enum.Parse(typeof(TEnum), strVal);
            }
            else
                throw new InvalidCastException("Unable to cast " + nameof(value) + " to either type 'string' or 'int'.");
        }
        private static MethodInfo GetCastEnumMethod(Type genericType) => typeof(RdpReader)
            .GetMethod(nameof(CastEnum), FLAGS).MakeGenericMethod(genericType);

        public static RdpFile Read(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath, _encoding);
            if (null == lines || lines.Length <= 0)
                return null;

            var dict = new Dictionary<string, object>(lines.Length);
            var rdpFile = new RdpFile(filePath);
            rdpFile.Extras = new StringBuilder(lines.Sum(x => x.Length));

            var reverse = rdpFile.GetReverse();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                Match match = Regex.Match(line, "^(.+)\\:(.)\\:(.*)$");

                string name = match.Groups[1].Value;
                if (dict.ContainsKey(name))
                    continue;

                if (reverse.ContainsKey(name))
                {
                    name = reverse[name];
                }
                else
                {
                    rdpFile.Extras.AppendLine(line);
                    continue;
                }

                object value = match.Groups[3].Value;
                if (match.Groups[2].Value.Equals(RdpWriter.LITTLE_I.ToString()))
                    value = Convert.ToInt32(match.Groups[3].Value);

                dict.Add(name, value);
            }

            return RdpFile.FromRead(rdpFile, dict, reverse);
        }
    }
}
