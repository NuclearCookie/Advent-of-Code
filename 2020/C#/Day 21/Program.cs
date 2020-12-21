using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day_21
{
    class Program
    {
        private static Regex food_parser = new Regex(@"(.*) \(contains (.*)\)");
        static void Main(string[] args)
        {
            var data = File.ReadAllLines("Input/data.txt");
            var allergen_mapping = new Dictionary<string, HashSet<string>>();
            List<string> ingredient_list = new List<string>();
            foreach (var food in data)
            {
                var matches = food_parser.Match(food);
                Debug.Assert(matches.Success);
                var ingredients = matches.Groups[1].Value.Split(' ');
                var allergens = matches.Groups[2].Value.Split(", ");
                ingredient_list.AddRange(ingredients);
                foreach (var allergen in allergens)
                {
                    HashSet<string> matching_ingredient_set = null;
                    if (!allergen_mapping.TryGetValue(allergen, out matching_ingredient_set))
                    {
                        matching_ingredient_set = new HashSet<string>(ingredients);
                        allergen_mapping[allergen] = matching_ingredient_set;
                    }
                    else
                    {
                        matching_ingredient_set.IntersectWith(ingredients);
                    }
                }
            }
            var ingredients_with_allergens = allergen_mapping.SelectMany(kvp => kvp.Value);
            var ingredients_without_allergens = ingredient_list.Where(ingredient => !ingredients_with_allergens.Contains(ingredient));
            var amount_of_ingredients_without_allergens = ingredients_without_allergens.Count();

            Console.WriteLine($"Amount of ingredients without allergens: {amount_of_ingredients_without_allergens}");

            var resolved_collection = from item in allergen_mapping
                                      select item;
            while(resolved_collection.Where(kvp => kvp.Value.Count > 1).Any())
                resolved_collection = ResolveAllergens(allergen_mapping, resolved_collection);
            var canonical_dangerous_ingredient_list = string.Join(',', resolved_collection
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => { Debug.Assert(kvp.Value.Count == 1); return kvp.Value.First(); })
                .ToArray());
            Console.WriteLine("Canonical Dangerous Ingredient list:");
            Console.WriteLine(canonical_dangerous_ingredient_list);
        }

        private static IEnumerable<KeyValuePair<string, HashSet<string>>> ResolveAllergens(Dictionary<string, HashSet<string>> allergen_mapping, IEnumerable<KeyValuePair<string, HashSet<string>>> resolved_collection)
        {
            for (int i = 0; i < allergen_mapping.Count - 1; ++i)
            {

                var sorted_collection = resolved_collection
                    .OrderBy(kvp => kvp.Value.Count)
                    .ThenBy(kvp => kvp.Key);
                var eliminator = sorted_collection.Skip(i)
                    .Select(kvp => { Debug.Assert(kvp.Value.Count == 1); return kvp.Value.First(); })
                    .First();
                resolved_collection = resolved_collection
                    .Select(kvp => { if (kvp.Value.Count > 1) kvp.Value.Remove(eliminator); return kvp; });
            }

            return resolved_collection;
        }
    }
}
