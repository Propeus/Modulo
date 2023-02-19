﻿using System.Text;
using System.Text.RegularExpressions;

namespace Propeus.Modulo.Abstrato.Util.Tabelas
{

    public class ConsoleTable
    {
        public IList<object> Columns { get; set; }
        public IList<object[]> Rows { get; protected set; }

        public ConsoleTableOptions Options { get; protected set; }
        public Type[] ColumnTypes { get; private set; }

        public static HashSet<Type> NumericTypes = new()
        {
            typeof(int),  typeof(double),  typeof(decimal),
            typeof(long), typeof(short),   typeof(sbyte),
            typeof(byte), typeof(ulong),   typeof(ushort),
            typeof(uint), typeof(float)
        };

        public ConsoleTable(params string[] columns)
            : this(new ConsoleTableOptions { Columns = new List<string>(columns) })
        {
        }

        public ConsoleTable(ConsoleTableOptions options)
        {
            Options = options ?? throw new ArgumentNullException("options");
            Rows = new List<object[]>();
            Columns = new List<object>(options.Columns);
        }

        public ConsoleTable AddColumn(IEnumerable<string> names)
        {
            foreach (string name in names)
            {
                Columns.Add(name);
            }

            return this;
        }

        public ConsoleTable AddRow(params object[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (!Columns.Any())
            {
                throw new Exception("Please set the columns first");
            }

            if (Columns.Count != values.Length)
            {
                throw new Exception(
                    $"The number columns in the row ({Columns.Count}) does not match the values ({values.Length})");
            }

            Rows.Add(values);
            return this;
        }

        public ConsoleTable Configure(Action<ConsoleTableOptions> action)
        {
            action(Options);
            return this;
        }

        public static ConsoleTable From<T>(IEnumerable<T> values)
        {
            ConsoleTable table = new()
            {
                ColumnTypes = GetColumnsType<T>().ToArray()
            };

            IEnumerable<string> columns = GetColumns<T>();

            _ = table.AddColumn(columns);

            foreach (
                IEnumerable<object>? propertyValues
                in values.Select(value => columns.Select(column => GetColumnValue<T>(value, column)))
            )
            {
                _ = table.AddRow(propertyValues.ToArray());
            }

            return table;
        }

        public override string ToString()
        {
            StringBuilder builder = new();

            // find the longest column by searching each row
            List<int> columnLengths = ColumnLengths();

            // set right alinment if is a number
            List<string> columnAlignment = Enumerable.Range(0, Columns.Count)
                .Select(GetNumberAlignment)
                .ToList();

            // create the string format with padding
            string format = Enumerable.Range(0, Columns.Count)
                .Select(i => " | {" + i + "," + columnAlignment[i] + columnLengths[i] + "}")
                .Aggregate((s, a) => s + a) + " |";

            // find the longest formatted line
            int maxRowLength = Math.Max(0, Rows.Any() ? Rows.Max(row => string.Format(format, row).Length) : 0);
            string columnHeaders = string.Format(format, Columns.ToArray());

            // longest line is greater of formatted columnHeader and longest row
            int longestLine = Math.Max(maxRowLength, columnHeaders.Length);

            // add each row
            List<string> results = Rows.Select(row => string.Format(format, row)).ToList();

            // create the divider
            string divider = " " + string.Join("", Enumerable.Repeat("-", longestLine - 1)) + " ";

            _ = builder.AppendLine(divider);
            _ = builder.AppendLine(columnHeaders);

            foreach (string? row in results)
            {
                _ = builder.AppendLine(divider);
                _ = builder.AppendLine(row);
            }

            _ = builder.AppendLine(divider);

            if (Options.EnableCount)
            {
                _ = builder.AppendLine("");
                _ = builder.AppendFormat(" Count: {0}", Rows.Count);
            }

            return builder.ToString();
        }

        public string ToMarkDownString()
        {
            return ToMarkDownString('|');
        }

        private string ToMarkDownString(char delimiter)
        {
            StringBuilder builder = new();

            // find the longest column by searching each row
            List<int> columnLengths = ColumnLengths();

            // create the string format with padding
            string format = Format(columnLengths, delimiter);

            // find the longest formatted line
            string columnHeaders = string.Format(format, Columns.ToArray());

            // add each row
            List<string> results = Rows.Select(row => string.Format(format, row)).ToList();

            // create the divider
            string divider = Regex.Replace(columnHeaders, @"[^|]", "-");

            _ = builder.AppendLine(columnHeaders);
            _ = builder.AppendLine(divider);
            results.ForEach(row => builder.AppendLine(row));

            return builder.ToString();
        }

        public string ToMinimalString()
        {
            return ToMarkDownString(char.MinValue);
        }

        public string ToStringAlternative()
        {
            StringBuilder builder = new();

            // find the longest column by searching each row
            List<int> columnLengths = ColumnLengths();

            // create the string format with padding
            string format = Format(columnLengths);

            // find the longest formatted line
            string columnHeaders = string.Format(format, Columns.ToArray());

            // add each row
            List<string> results = Rows.Select(row => string.Format(format, row)).ToList();

            // create the divider
            string divider = Regex.Replace(columnHeaders, @"[^|]", "-");
            string dividerPlus = divider.Replace("|", "+");

            _ = builder.AppendLine(dividerPlus);
            _ = builder.AppendLine(columnHeaders);

            foreach (string? row in results)
            {
                _ = builder.AppendLine(dividerPlus);
                _ = builder.AppendLine(row);
            }
            _ = builder.AppendLine(dividerPlus);

            return builder.ToString();
        }

        private string Format(List<int> columnLengths, char delimiter = '|')
        {
            // set right alinment if is a number
            List<string> columnAlignment = Enumerable.Range(0, Columns.Count)
                .Select(GetNumberAlignment)
                .ToList();

            string delimiterStr = delimiter == char.MinValue ? string.Empty : delimiter.ToString();
            string format = (Enumerable.Range(0, Columns.Count)
                .Select(i => " " + delimiterStr + " {" + i + "," + columnAlignment[i] + columnLengths[i] + "}")
                .Aggregate((s, a) => s + a) + " " + delimiterStr).Trim();
            return format;
        }

        private string GetNumberAlignment(int i)
        {
            return Options.NumberAlignment == Alignment.Right
                    && ColumnTypes != null
                    && NumericTypes.Contains(ColumnTypes[i])
                ? ""
                : "-";
        }

        private List<int> ColumnLengths()
        {
            List<int> columnLengths = Columns
                .Select((t, i) => Rows.Select(x => x[i])
                    .Union(new[] { Columns[i] })
                    .Where(x => x != null)
                    .Select(x => x.ToString().Length).Max())
                .ToList();
            return columnLengths;
        }

        public void Write(Format format = Propeus.Modulo.Abstrato.Util.Tabelas.Format.Default)
        {
            switch (format)
            {
                case Tabelas.Format.Default:
                    Options.OutputTo.WriteLine(ToString());
                    break;
                case Tabelas.Format.MarkDown:
                    Options.OutputTo.WriteLine(ToMarkDownString());
                    break;
                case Tabelas.Format.Alternative:
                    Options.OutputTo.WriteLine(ToStringAlternative());
                    break;
                case Tabelas.Format.Minimal:
                    Options.OutputTo.WriteLine(ToMinimalString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        private static IEnumerable<string> GetColumns<T>()
        {
            return typeof(T).GetProperties().Select(x => x.Name).ToArray();
        }

        private static object GetColumnValue<T>(object target, string column)
        {
            return typeof(T).GetProperty(column).GetValue(target, null);
        }

        private static IEnumerable<Type> GetColumnsType<T>()
        {
            return typeof(T).GetProperties().Select(x => x.PropertyType).ToArray();
        }
    }

    public class ConsoleTableOptions
    {
        public IEnumerable<string> Columns { get; set; } = new List<string>();
        public bool EnableCount { get; set; } = true;

        /// <summary>
        /// Enable only from a list of objects
        /// </summary>
        public Alignment NumberAlignment { get; set; } = Alignment.Left;

        /// <summary>
        /// The <see cref="TextWriter"/> to write to. Defaults to <see cref="Console.Out"/>.
        /// </summary>
        public TextWriter OutputTo { get; set; } = Console.Out;
    }

    public enum Format
    {
        Default = 0,
        MarkDown = 1,
        Alternative = 2,
        Minimal = 3
    }

    public enum Alignment
    {
        Left,
        Right
    }
}
