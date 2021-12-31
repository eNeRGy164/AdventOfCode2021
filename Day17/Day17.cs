var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

// target area: x=nnn..nnn, y=nnn..nnn
var split = input[0].Split('=', '.', ',');
var x1 = Convert.ToInt32(split[1]);
var x2 = Convert.ToInt32(split[3]);
var y1 = Convert.ToInt32(split[5]);
var y2 = Convert.ToInt32(split[7]);

var target = new Box(new Point(x1, y2), new Point(x2, y1));

var start = new Point(0, 0);
var results = new List<int>();

for (var x = 1; x <= x2; x++)
{
    for (var y = Math.Abs(y1); y >= y1; y--)
    {
        var initialVelocity = new Point(x, y);
        var highestPoint = int.MinValue;

        var position = start;
        var velocity = initialVelocity;

        var result = -1;

        do
        {
            position = position with
            {
                X = position.X + velocity.X,
                Y = position.Y + velocity.Y
            };

            if (position.Y > highestPoint)
            {
                highestPoint = position.Y;
            }

            velocity = velocity with
            {
                X = Math.Max(0, velocity.X - 1),
                Y = velocity.Y - 1
            };

            if (velocity.X == 0 && position.X < x1)
            {
                break;
            }

            result = target.CompareTo(position);
        } while (result < 0);

        if (result == 0)
        {
            results.Add(highestPoint);
        }
    }
}

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {results.Max()}");
Console.WriteLine($"Part 2: {results.Count}");

record struct Box(Point LeftTop, Point RightBottom) : IComparable<Point>
{
    public int CompareTo(Point other)
    {
        // Left or above
        if (LeftTop.X > other.X || LeftTop.Y < other.Y) return -1;

        // Right or under
        if (RightBottom.X < other.X || RightBottom.Y > other.Y) return 1;

        // InBox
        return 0;
    }
}

record struct Point(int X, int Y);
