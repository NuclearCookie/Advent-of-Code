﻿using System;
using System.IO;
using System.Linq;

namespace Day_4
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
            var valid_records = records
                            .Where(data =>
                            {
                                return
                                    data.Contains("byr:") &&
                                    data.Contains("iyr:") &&
                                    data.Contains("eyr:") &&
                                    data.Contains("hgt:") &&
                                    data.Contains("hcl:") &&
                                    data.Contains("ecl:") &&
                                    data.Contains("pid:");

                            }).Count();

            Console.WriteLine($"Valid records: {valid_records}");
        }
        private static void PartB(string[] records)
        {
            Console.WriteLine("Part B:");
            var valid_records = records
                            .Select(data => data.Replace(Environment.NewLine, " ").Split(' '))
                            .Where(data =>
                            {
                                var valid_fields = Fields.None;
                                foreach (var segment in data)
                                {
                                    var identifier = segment.Substring(0, 4);
                                    var segment_data = segment.Substring(4).Trim();
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
                                            var unit = segment_data.Substring(segment_data.Length - 2);
                                            var height_str = segment_data.Substring(0, segment_data.Length - 2);
                                            if (unit == "cm")
                                            {
                                                if (int.TryParse(height_str, out int height))
                                                {
                                                    if (height >= 150 && height <= 193)
                                                    {
                                                        valid_fields |= Fields.Height;
                                                    }
                                                }
                                            }
                                            else if (unit == "in")
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
                                            if (segment_data.StartsWith('#') && segment_data.Length == 7)
                                            {
                                                var color = segment_data.Substring(1);
                                                if (int.TryParse(color, System.Globalization.NumberStyles.HexNumber, null, out var color_int))
                                                {
                                                    valid_fields |= Fields.HairColor;
                                                }
                                            }
                                        }
                                        break;
                                        case "ecl:":
                                            {
                                                if (segment_data == "amb" || segment_data == "blu" || segment_data == "brn"
                                                    || segment_data == "gry" || segment_data == "grn" || segment_data == "hzl" || segment_data == "oth")
                                                {
                                                    valid_fields |= Fields.EyeColor;
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
