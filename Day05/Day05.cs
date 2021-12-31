using static System.Math;

var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var values = new List<Vent>(input.Length);

for (var i = 0; i < input.Length; i++)
{
    var coordinates = input[i].Split(" -> ");

    var first = coordinates[0].Split(',');
    var second = coordinates[1].Split(',');

    var from = new Point(int.Parse(first[0]), int.Parse(first[1]));
    var to = new Point(int.Parse(second[0]), int.Parse(second[1]));

    values.Add(new Vent(from, to));
}

var firstHitsPart1 = new HashSet<Point>(25_000);
var firstHitsPart2 = new HashSet<Point>(25_000);
var secondHitsPart1 = new HashSet<Point>(25_000);
var secondHitsPart2 = new HashSet<Point>(25_000);

foreach ((var from, var to) in values)
{
    if (from.X == to.X)
    {
        // Vertical
        var top = Min(from.Y, to.Y);
        var bottom = Max(from.Y, to.Y);

        for (var y = top; y <= bottom; y++)
        {
            var point = to with { Y = y };
            if (!firstHitsPart1.Add(point))
            {
                secondHitsPart1.Add(point);
            }

            if (!firstHitsPart2.Add(point))
            {
                secondHitsPart2.Add(point);
            }
        }
    }
    else if (from.Y == to.Y)
    {
        // Horizontal
        var left = Min(from.X, to.X);
        var right = Max(from.X, to.X);

        for (var x = left; x <= right; x++)
        {
            var point = to with { X = x };

            if (!firstHitsPart1.Add(point)) { secondHitsPart1.Add(point); }
            if (!firstHitsPart2.Add(point)) { secondHitsPart2.Add(point); }
        }
    }
    else
    {
        // Diagonal
        var length = Abs(from.X - to.X) + 1;

        for (var l = 0; l < length; l++)
        {
            var newX = ((from.X - to.X) < 0) ? from.X + l : from.X - l;
            var newY = ((from.Y - to.Y) < 0) ? from.Y + l : from.Y - l;

            var point = new Point(newX, newY);
            if (!firstHitsPart2.Add(point)) { secondHitsPart2.Add(point); }
        }
    }
}

var part1 = secondHitsPart1.Count;
var part2 = secondHitsPart2.Count;

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

record struct Point(int X, int Y);

record struct Vent(Point From, Point To);
