using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day_22
{
    class Program
    {
        static void Main(string[] args)
        {
            var decks = File.ReadAllText("Input/data.txt").Split(Environment.NewLine + Environment.NewLine);
            PartA(decks);
            PartB(decks);
        }

        private static void PartA(string[] decks)
        {
            var player_1 = new Queue<int>(decks[0].Split(Environment.NewLine).Skip(1).Select(x => int.Parse(x)));
            var player_2 = new Queue<int>(decks[1].Split(Environment.NewLine).Skip(1).Select(x => int.Parse(x)));
            PlayCombat(player_1, player_2);
            Console.WriteLine($"Winner: Player {(player_1.Count > 0 ? 1 : 2)}");
            var winner = player_1.Count > 0 ? player_1 : player_2;
            var score = 0uL;
            var initial_queue_size = (ulong)winner.Count;
            for (ulong i = initial_queue_size; i > 0; --i)
            {
                score += (ulong)winner.Dequeue() * (i);
            }
            Console.WriteLine($"Deck score: {score}");
        }

        private static void PartB(string[] decks)
        {
            var player_1 = new Queue<int>(decks[0].Split(Environment.NewLine).Skip(1).Select(x => int.Parse(x)));
            var player_2 = new Queue<int>(decks[1].Split(Environment.NewLine).Skip(1).Select(x => int.Parse(x)));
            var player_1_is_winner = PlayRecursiveCombat(player_1, player_2);
            Console.WriteLine($"Winner: Player {(player_1_is_winner ? 1 : 2)}");
            var winner = player_1_is_winner ? player_1 : player_2;
            var score = 0uL;
            var initial_queue_size = (ulong)winner.Count;
            for (ulong i = initial_queue_size; i > 0; --i)
            {
                score += (ulong)winner.Dequeue() * (i);
            }
            Console.WriteLine($"Deck score: {score}");
        }

        private static void PlayCombat(Queue<int> player_1, Queue<int> player_2)
        {
            while (player_1.Count > 0 && player_2.Count > 0)
            {
                var player_1_card = player_1.Dequeue();
                var player_2_card = player_2.Dequeue();
                Debug.Assert(player_1_card != player_2_card, "It's a draw!!");
                if (player_1_card > player_2_card)
                {
                    player_1.Enqueue(player_1_card);
                    player_1.Enqueue(player_2_card);
                }
                else if (player_2_card > player_1_card)
                {
                    player_2.Enqueue(player_2_card);
                    player_2.Enqueue(player_1_card);
                }
            }
        }

        // returns true for win player 1, false for win player 2.
        private static bool PlayRecursiveCombat(Queue<int> player_1, Queue<int> player_2)
        {
            HashSet<int> round_hash = new HashSet<int>();
            while (player_1.Count > 0 && player_2.Count > 0)
            {
                var p1_hash = GetSequenceHashCode(player_1);
                var p2_hash = GetSequenceHashCode(player_2);
                // player 1 wins if the round has already been played.
                if (!round_hash.Add(GetSequenceHashCode(new int[] { p1_hash, p2_hash })))
                    return true;
                var player_1_card = player_1.Dequeue();
                var player_2_card = player_2.Dequeue();
                if (player_1.Count >= player_1_card && player_2.Count >= player_2_card)
                {
                    var winner = PlayRecursiveCombat(new Queue<int>(player_1.Take(player_1_card)), new Queue<int>(player_2.Take(player_2_card)));
                    AdvanceCards(winner, player_1_card, player_2_card);
                }
                else
                {
                    AdvanceCards(player_1_card > player_2_card, player_1_card, player_2_card);
                }
            }
            return player_1.Count > 0;

            void AdvanceCards(bool is_player_1_winner, int player_1_card, int player_2_card)
            {
                if (is_player_1_winner)
                {
                    player_1.Enqueue(player_1_card);
                    player_1.Enqueue(player_2_card);
                }
                else
                {
                    player_2.Enqueue(player_2_card);
                    player_2.Enqueue(player_1_card);
                }
            }
        }

        private static int GetSequenceHashCode<T>(IEnumerable<T> sequence)
        {
            const int seed = 487;
            const int modifier = 31;

            unchecked
            {
                return sequence.Aggregate(seed, (current, item) =>
                    (current * modifier) + item.GetHashCode());
            }
        }
    }
}
