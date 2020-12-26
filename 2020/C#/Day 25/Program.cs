using System;
using System.IO;

namespace Day_25
{
    class Program
    {
        private static ulong initial_subject_number = 7;
        static void Main(string[] args)
        {
            var keys = File.ReadAllLines("Input/data.txt");
            var card_public_key = ulong.Parse(keys[0]);
            var door_public_key = ulong.Parse(keys[1]);
            var card_loop_size = FindLoopSize(card_public_key);
            var door_loop_size = FindLoopSize(door_public_key);
            Console.WriteLine($"Card loop size: {card_loop_size}, Door loop size: {door_loop_size}");

            var encryption_key = 1uL;
            for(int i = 0; i < card_loop_size; ++i)
            {
                encryption_key = Transform(door_public_key, encryption_key);
            }
            Console.WriteLine($"Encryption key: {encryption_key}");
        }

        private static int FindLoopSize(ulong card_key)
        {
            var loops = 1;
            var value = 1uL;
            while(true)
            {
                value = Transform(initial_subject_number, value);
                if (card_key == value)
                    return loops;
                loops++;
            }
        }

        static ulong Transform(ulong subject_number, ulong value)
        {
            value *= subject_number;
            value %= 20201227uL;
            return value;
        }
    }
}
