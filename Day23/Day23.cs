var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var lines = new List<string[]>
{
    input[2].Trim('#', ' ').Split('#'),
    input[3].Trim('#', ' ').Split('#'),
};

var inputAmphipods = new Dictionary<char, List<int>>(4);
for (var y = 0; y < lines.Count; y++)
{
    for (var x = 30; x <= 90; x += 20)
    {
        var key = lines[y][(x - 30) / 20][0];
        var value = x + y + 1;
        if (!inputAmphipods.TryAdd(key, new() { value }))
        {
            inputAmphipods[key].Add(value);
        }
    }
}

Game.HouseSize = 2;
Game.LowestScore = 30_000;
Amphipod.HouseSize = 2;

var amphipods = new Amphipod[8];
var count = 0;

for (var c = 'A'; c <= 'D'; c++)
{
    for (var i = 0; i < Game.HouseSize; i++)
    {
        amphipods[count] = new Amphipod(count++, c, inputAmphipods[c][i]);
    }
}

var part1 = new Game(amphipods, 0, 0).Step();

inputAmphipods = new Dictionary<char, List<int>>(4);
for (var y = 0; y < 2; y++)
{
    for (var x = 30; x <= 90; x += 20)
    {
        var key = lines[y][(x - 30) / 20][0];
        var value = x + (y * 3) + 1;

        if (!inputAmphipods.TryAdd(key, new() { value }))
        {
            inputAmphipods[key].Add(value);
        }
    }
}

inputAmphipods['A'].Add(73);
inputAmphipods['A'].Add(92);
inputAmphipods['B'].Add(53);
inputAmphipods['B'].Add(72);
inputAmphipods['C'].Add(52);
inputAmphipods['C'].Add(93);
inputAmphipods['D'].Add(32);
inputAmphipods['D'].Add(33);

Game.HouseSize = 4;
Game.LowestScore = 100_000;
Amphipod.HouseSize = 4;

amphipods = new Amphipod[16];
count = 0;

for (var c = 'A'; c <= 'D'; c++)
{
    for (var i = 0; i < Game.HouseSize; i++)
    {
        amphipods[count] = new Amphipod(count++, c, inputAmphipods[c][i]);
    }
}

var part2 = new Game(amphipods, 0, 0).Step();

stopWatch.Stop();
Console.WriteLine($"{stopWatch.Elapsed:mm\\mss\\s}");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

record struct Amphipod(int ID, char Type, int Position)
{
    public static int HouseSize { get; set; } = 2;
    static readonly Dictionary<char, int> costs = new(4) { { 'A', 1 }, { 'B', 10 }, { 'C', 100 }, { 'D', 1000 } };
    static readonly Dictionary<char, int> home = new(4) { { 'A', 30 }, { 'B', 50 }, { 'C', 70 }, { 'D', 90 } };

    public int Cost { get; init; } = costs[Type];

    public int HomeEntry { get; init; } = home[Type];

    public bool IsHome => Position == home[Type] + HouseSize;

    public bool IsInOwnHouse => Position > HomeEntry & Position < HomeEntry + 10;

    public bool InHallway => Position % 10 == 0;

    public bool CanMove => InHallway || !IsHome;
}

record struct Game(Amphipod[] Amphipods, long Score, int Turns, int LastMove = -1)
{
    public static int HouseSize { get; set; } = 2;
    static readonly int[] HallwayPositions = new[] { 10, 20, 40, 60, 80, 100, 110 };

    Dictionary<int, Amphipod> Positions { get; init; } = Amphipods.ToDictionary(p => p.Position, p => p);

    public static long LowestScore = 100000;

    public long Step()
    {
        if (Amphipods.All(p => p.IsInOwnHouse))
        {
            // Game finished
            if (Score < LowestScore)
            {
                LowestScore = Score;
            }

            return LowestScore;
        }

        var games = new List<Game>();

        var amphipodsThatCanMove = MovableAmphipods();
        for (var i = 0; i < amphipodsThatCanMove.Count; i++)
        {
            var amphipod = amphipodsThatCanMove[i];

            var destinations = AvailableLocations(amphipod.ID);
            for (var j = 0; j < destinations.Count; j++)
            {
                var destination = destinations[j];
                var cost = CalculatePathLength(amphipod.Position, destination) * amphipod.Cost;
                if (Score + cost > LowestScore)
                {
                    continue;
                }
                else
                {
                    var updatedAmphipods = new Amphipod[Amphipods.Length];
                    Array.Copy(Amphipods, updatedAmphipods, Amphipods.Length);

                    updatedAmphipods[amphipod.ID] = amphipod with { Position = destination };

                    new Game(updatedAmphipods, Score + cost, Turns + 1, amphipod.ID).Step();
                }
            }
        }

        return LowestScore;
    }

    List<Amphipod> MovableAmphipods()
    {
        var amphipods = new List<Amphipod>();

        for (var i = Amphipods.Length - 1; i >= 0; i--)
        {
            if (i == LastMove) continue;

            var amphipod = Amphipods[i];

            if (!amphipod.CanMove) continue;

            if (amphipod.InHallway)
            {
                amphipods.Add(amphipod);
                continue;
            }

            // Other amphipod in front
            var exitBlocked = false;
            var houseEntry = amphipod.Position / 10 * 10;
            for (var p = houseEntry + 1; p < amphipod.Position; p++)
            {
                if (Positions.ContainsKey(p))
                {
                    // Exit blocked
                    exitBlocked = true;
                    break;
                }
            }

            if (exitBlocked) continue;

            if (amphipod.IsInOwnHouse)
            {
                if (FullHouse(amphipod)) continue;

                // Other types behind this amphipod?
                var blockingOthers = false;
                for (var p = amphipod.Position + 1; p < amphipod.HomeEntry + HouseSize + 1; p++)
                {
                    if (Positions[p].Type != amphipod.Type)
                    {
                        blockingOthers = true;
                        break;
                    }
                }

                if (blockingOthers)
                {
                    amphipods.Add(amphipod);
                }
            }
            else
            {
                amphipods.Add(amphipod);
            }
        }

        return amphipods;
    }

    bool FullHouse(in Amphipod amphipod)
    {
        var bucket = (amphipod.ID / HouseSize) * HouseSize;
        for (var j = bucket; j < bucket + HouseSize; j++)
        {
            if (Amphipods[j] != amphipod && !Amphipods[j].IsInOwnHouse)
            {
                return false;
            }
        }

        return true;
    }

    List<int> AvailableLocations(int id)
    {
        var amphipods = Amphipods[id];
        var possibleDestinations = new List<int>(1);

        if (amphipods.InHallway)
        {
            // If in hallway

            // Check path home
            var homeEntry = amphipods.HomeEntry;
            if (amphipods.Position > homeEntry)
            {
                for (var p = homeEntry + 10; p < amphipods.Position; p += 10)
                {
                    if (Positions.ContainsKey(p))
                    {
                        return possibleDestinations;
                    }
                }
            }
            else
            {
                for (var p = homeEntry - 10; p > amphipods.Position; p -= 10)
                {
                    if (Positions.ContainsKey(p))
                    {
                        return possibleDestinations;
                    }
                }
            }

            var largestHousePositionAvailable = LargestHousePositionAvailable(amphipods);
            if (largestHousePositionAvailable > 0)
            {
                possibleDestinations.Add(largestHousePositionAvailable);
            }

            return possibleDestinations;
        }
        else
        {
            // If in a house

            var occupiedHallwayPositions = Amphipods.Where(a => a.InHallway).Select(a => a.Position).OrderBy(p => p).ToList();

            var left = HallwayPositions.Where(hp => hp < amphipods.Position && (hp > occupiedHallwayPositions.LastOrDefault(op => op < amphipods.Position)));
            var right = HallwayPositions.Where(hp => hp > amphipods.Position && (hp < occupiedHallwayPositions.FirstOrDefault(op => op > amphipods.Position, 120)));

            possibleDestinations.AddRange(left);
            possibleDestinations.AddRange(right);

            if (amphipods.Position < amphipods.HomeEntry)
            {
                if (possibleDestinations.Contains(amphipods.HomeEntry - 10))
                {
                    var largestHousePositionAvailable = LargestHousePositionAvailable(amphipods);
                    if (largestHousePositionAvailable > 0)
                    {
                        possibleDestinations.Add(largestHousePositionAvailable);
                    }
                }
            }
            else
            {
                if (possibleDestinations.Contains(amphipods.HomeEntry + 10))
                {
                    var largestHousePositionAvailable = LargestHousePositionAvailable(amphipods);
                    if (largestHousePositionAvailable > 0)
                    {
                        possibleDestinations.Add(largestHousePositionAvailable);
                    }
                }
            }

            return possibleDestinations;
        }
    }

    int LargestHousePositionAvailable(in Amphipod amphipod)
    {
        var largestHousePositionAvailable = 0;
        for (var p = amphipod.HomeEntry + 1; p < amphipod.HomeEntry + HouseSize + 1; p++)
        {
            if (!Positions.ContainsKey(p))
            {
                largestHousePositionAvailable = p;
            }
            else
            {
                if (Positions[p].Type != amphipod.Type)
                {
                    largestHousePositionAvailable = 0;
                    break;
                }
            }
        }

        return largestHousePositionAvailable;
    }

    static int CalculatePathLength(in int position, in int destination)
    {
        var verticalToHallway = position % 10;
        var verticalToHouse = destination % 10;
        var horizontal = (Math.Max(position, destination) / 10 - Math.Min(position, destination) / 10) % 10;

        return verticalToHallway + horizontal + verticalToHouse;
    }
}
