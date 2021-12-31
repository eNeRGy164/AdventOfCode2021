using System.Runtime.InteropServices;
using Multiverse = System.Collections.Generic.Dictionary<Universe, ulong>;

var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

// Player n starting position: n
var startPositionP1 = input[0][^1] - 48;
var startPositionP2 = input[1][^1] - 48;

int Part1()
{
    var positionP1 = startPositionP1;
    var positionP2 = startPositionP2;

    var score1 = 0;
    var score2 = 0;

    var die = 1;
    var rolls = 0;

    do
    {
        var roll = (die + 1) * 3;
        die += 3;

        if (roll >= 297)
        {
            switch (roll)
            {
                case 303:
                    roll -= 200;
                    die = 3;
                    break;
                case 300:
                    roll -= 100;
                    die = 2;
                    break;
                default:
                    die = 1;
                    break;
            }
        }

        rolls += 3;

        if (rolls % 2 == 1) // Player 1
        {
            ThrowDice1(roll, ref positionP1, ref score1);
        }
        else
        {
            ThrowDice1(roll, ref positionP2, ref score2);
        }
    } while (Math.Max(score1, score2) < 1000);

    return Math.Min(score1, score2) * rolls;
}

ulong Part2()
{
    var multiverse = new Multiverse(1)
    {
        [new Universe((byte)startPositionP1, (byte)startPositionP2, 0, 0)] = 1UL
    };

    var dieDistribution = new ulong[] { 0, 0, 0, 1, 3, 6, 7, 6, 3, 1 };

    const int maxScore = 21;
    var turnPlayer1 = true;
    var winsPlayer1 = 0UL;
    var winsPlayer2 = 0UL;

    do
    {
        var newMultiverse = new Multiverse(multiverse.Count * 7);

        foreach (var (universe, count) in multiverse)
        {
            for (var roll = 3; roll <= 9; roll++)
            {
                var universes = count * dieDistribution[roll];

                if (turnPlayer1)
                {
                    ThrowDice2(roll, universe.Position1, universe.Score1, out var newPosition, out var newScore);

                    if (newScore >= maxScore)
                    {
                        winsPlayer1 += universes;
                    }
                    else
                    {
                        var newUniverse = universe with { Position1 = newPosition, Score1 = newScore };

                        if (!newMultiverse.TryAdd(newUniverse, universes))
                        {
                            newMultiverse[newUniverse] += universes;
                        }
                    }
                }
                else
                {
                    ThrowDice2(roll, universe.Position2, universe.Score2, out var newPosition, out var newScore);

                    if (newScore >= maxScore)
                    {
                        winsPlayer2 += universes;
                    }
                    else
                    {
                        var newUniverse = universe with { Position2 = newPosition, Score2 = newScore };
                        if (!newMultiverse.TryAdd(newUniverse, universes))
                        {
                            newMultiverse[newUniverse] += universes;
                        }
                    }
                }
            }
        }

        turnPlayer1 = !turnPlayer1;

        multiverse = new Multiverse(newMultiverse);
    } while (multiverse.Count > 0);

    return Math.Max(winsPlayer1, winsPlayer2);
}

var part1 = Part1();
var part2 = Part2();

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

static void ThrowDice1(in int roll, ref int position, ref int score)
{
    position = (position + roll) % 10;
    if (position == 0) position = 10;

    score += position;
}

static void ThrowDice2(in int roll, in byte position, in byte score, out byte newPosition, out byte newScore)
{
    newPosition = (byte)((position + roll) % 10);
    if (newPosition == 0) newPosition = 10;

    newScore = (byte)(score + newPosition);
}

[StructLayout(LayoutKind.Explicit)]
record struct Universe
{
    [FieldOffset(0)]
    public byte Position1;

    [FieldOffset(1)]
    public byte Position2;

    [FieldOffset(2)]
    public byte Score1;

    [FieldOffset(3)]
    public byte Score2;

    public Universe(byte position1, byte position2, byte score1, byte score2)
    {
        Position1 = position1;
        Position2 = position2;
        Score1 = score1;
        Score2 = score2;
    }
}
