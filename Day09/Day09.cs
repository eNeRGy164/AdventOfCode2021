var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var rows = input.Length;
var cols = input[0].Length;

var map = new int[rows, cols];
MapWalker((x, y) => { map[y, x] = input[y][x] - 48; }); // 0..9

var part1 = 0;
MapWalker((x, y) =>
{
    var value = map[y, x];

    if ((y == 0 || value < map[y - 1, x]) // above
        && (y == rows - 1 || value < map[y + 1, x]) // below
        && (x == 0 || value < map[y, x - 1]) // before
        && (x == cols - 1 || value < map[y, x + 1]) // after
        )
    {
        part1 += value + 1;
    }
});

var basins = new List<int>(50); // A guess to prevent too many list resizes
MapWalker((x, y) =>
{
    var basin = CheckFloor(x, y);
    if (basin > 0)
    {
        basins.Add(basin);
    }
});

var largestBasins = basins.ToArray();
Array.Sort(largestBasins);
largestBasins = largestBasins[^3..];

var part2 = 1;
for (var i = 0; i < 3; i++)
{
    part2 *= largestBasins[i];
}

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

int CheckFloor(in int x, in int y)
{
    if (map[y, x] > 8) // already checked, or wall
    {
        return 0;
    }

    map[y, x] = 9;

    var value = 1;

    if (x < cols - 1)
    {
        value += CheckFloor(x + 1, y);
    }

    if (x > 0)
    {
        value += CheckFloor(x - 1, y);
    }

    if (y < rows - 1)
    {
        value += CheckFloor(x, y + 1);
    }

    if (y > 0)
    {
        value += CheckFloor(x, y - 1);
    }

    return value;
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
