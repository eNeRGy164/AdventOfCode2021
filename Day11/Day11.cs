var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var rows = input.Length;
var cols = input[0].Length;

var map = new (int Value, bool IsVisited)[rows, cols];

MapWalker((x, y) => { map[y, x] = (input[y][x] - 48, false); });

var part1 = 0;
var part2 = 0;

for (var round = 0; round < 100 || part2 == 0; round++)
{
    var flashes = 0;

    MapWalker((x, y) => Flash(x, y, false));

    MapWalker((x, y) => {
        if (map[y, x].Value == 0) flashes++;
        map[y, x].IsVisited = false;
    });

    if (part2 == 0 && flashes == rows * cols) // All flashed
    {
        part2 = round + 1; // Off by one
    }

    if (round < 100)
    {
        part1 += flashes;
    }
}

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

void Flash(in int x, in int y, in bool increase)
{
    if (!map[y, x].IsVisited)
    {
        map[y, x].IsVisited = true;
        map[y, x].Value++;
    }

    if (map[y, x].Value == 0)
    {
        return;
    }

    if (increase)
    {
        map[y, x].Value++;
    }

    if (map[y, x].Value > 9)
    {
        map[y, x].Value = 0;

        (int X, int Y)[] neighbours = new[]
        {
            (x - 1, y - 1),
            (x - 1, y),
            (x - 1, y + 1),
            (x, y - 1),
            (x, y + 1),
            (x + 1, y - 1),
            (x + 1, y),
            (x + 1, y + 1),
        };

        foreach (var (X, Y) in neighbours.Where(c => c.X > -1 && c.X < cols && c.Y > -1 && c.Y < rows))
        {
            Flash(X, Y, true);
        }
    }
}

void MapWalker(Action<int, int> xyFunc)
{
    for (var y = 0; y < rows; y++)
    {
        for (var x = 0; x < cols; x++)
        {
            xyFunc(x, y);
        }
    }
}
