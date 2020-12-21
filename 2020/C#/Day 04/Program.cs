using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_04
{
    class Program
    {
        [Flags]
        private enum Fields
        {
            None = 0,
            BirthYear = 1 << 0,
            IssueYear = 1 << 1,
            ExpirationYear = 1 << 2,
            Height = 1 << 3,
            HairColor = 1 << 4,
            EyeColor = 1 << 5,
            PassportId = 1 << 6,
            All = BirthYear | IssueYear | ExpirationYear | Height | HairColor | EyeColor | PassportId
        };

        private static string[] required_fields = new string[] { "byr:", "iyr:", "eyr:", "hgt:", "hcl:", "ecl:", "pid:" };
        private static string[] valid_colors = new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

        static void Main(string[] args)
        {
            var data = File.ReadAllText("Input/data.txt");

            var records = data.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            PartA(records);
            PartB(records);
        }

        private static void PartA(string[] records)
        {
            Console.WriteLine("Part A:");
            var valid_records = GetRecordsWithRequiredFields(records).Count();

            Console.WriteLine($"Valid records: {valid_records}");
        }

        private static IEnumerable<string> GetRecordsWithRequiredFields(string[] records)
        {
            return records
                .Where(data =>
                {
                    foreach (var requirement in required_fields)
                    {
                        if (!data.Contains(requirement))
                        {
                            return false;
                        }
                    }
                    return true;
                });
        }

        private static void PartB(string[] records)
        {
            Console.WriteLine("Part B:");
            var valid_records =
                GetRecordsWithRequiredFields(records)
                    .Select(data => data.Replace(Environment.NewLine, " ").Split(' '))
                    .Where(data =>
                    {
                        var valid_fields = Fields.None;
                        foreach (var segment in data)
                        {
                            // Can't use span because switch case doesn't like that.
                            var identifier = segment.Substring(0, 4);
                            var segment_data = segment.AsSpan().Slice(start: 4);
                            switch (identifier)
                            {
                                case "byr:":
                                    {
                                        if (int.TryParse(segment_data, out int year))
                                        {
                                            if (year >= 1920 && year <= 2002)
                                            {
                                                valid_fields |= Fields.BirthYear;
                                            }
                                        }
                                    }
                                    break;
                                case "iyr:":
                                    {
                                        if (int.TryParse(segment_data, out int year))
                                        {
                                            if (year >= 2010 && year <= 2020)
                                            {
                                                valid_fields |= Fields.IssueYear;
                                            }
                                        }
                                    }
                                    break;
                                case "eyr:":
                                    {
                                        if (int.TryParse(segment_data, out int year))
                                        {
                                            if (year >= 2020 && year <= 2030)
                                            {
                                                valid_fields |= Fields.ExpirationYear;
                                            }
                                        }

                                    }
                                    break;
                                case "hgt:":
                                    {
                                        var unit = segment_data.Slice(start: segment_data.Length - 2);
                                        var height_str = segment_data.Slice(0, segment_data.Length - 2);
                                        if (unit.Equals("cm", StringComparison.Ordinal))
                                        {
                                            if (int.TryParse(height_str, out int height))
                                            {
                                                if (height >= 150 && height <= 193)
                                                {
                                                    valid_fields |= Fields.Height;
                                                }
                                            }
                                        }
                                        else if (unit.Equals("in", StringComparison.Ordinal))
                                        {
                                            if (int.TryParse(height_str, out int height))
                                            {
                                                if (height >= 59 && height <= 76)
                                                {
                                                    valid_fields |= Fields.Height;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case "hcl:":
                                    {
                                        if (segment_data[0] == '#' && segment_data.Length == 7)
                                        {
                                            var color = segment_data.Slice(start: 1);
                                            if (int.TryParse(color, System.Globalization.NumberStyles.HexNumber, null, out var color_int))
                                            {
                                                valid_fields |= Fields.HairColor;
                                            }
                                        }
                                    }
                                    break;
                                case "ecl:":
                                    {
                                        foreach (var valid_value in valid_colors)
                                        {
                                            if (segment_data.Equals(valid_value, StringComparison.Ordinal))
                                            {
                                                valid_fields |= Fields.EyeColor;
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                case "pid:":
                                    {
                                        if (segment_data.Length == 9 && int.TryParse(segment_data, out int number))
                                        {
                                            valid_fields |= Fields.PassportId;
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        return valid_fields == Fields.All;
                    }).Count();
            Console.WriteLine($"Valid records: {valid_records}");
        }
    }
}
