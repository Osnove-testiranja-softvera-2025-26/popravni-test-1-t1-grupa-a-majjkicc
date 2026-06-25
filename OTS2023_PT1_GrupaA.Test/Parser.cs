using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS2026_PT1_GrupaA.Test
{
    internal class Parser
    {
            public static IEnumerable GetCalculateIncomeData()
            {
                string path = Path.Combine(
                    TestContext.CurrentContext.TestDirectory,
                    "calculate_income_data.txt"
                );

                string[] lines = File.ReadAllLines(path);

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] values = lines[i].Split(
                        new char[] { '\t', ' ' },
                        StringSplitOptions.RemoveEmptyEntries
                    );

                    int fish = int.Parse(values[0]);
                    int bait = int.Parse(values[1]);
                    bool hasBoat = bool.Parse(values[2]);
                    Game.Score expected = ParseScore(values[3]);

                    yield return new TestCaseData(fish, bait, hasBoat, expected);
                }
            }

            private static Game.Score ParseScore(string value)
            {
                switch (value.Trim())
                {
                    case "Bad":
                        return Game.Score.Bad;

                    case "Average":
                        return Game.Score.Average;

                    case "Good":
                        return Game.Score.Good;

                    default:
                        throw new ArgumentException("Unknown score: " + value);
                }
            }
        
    }
}
