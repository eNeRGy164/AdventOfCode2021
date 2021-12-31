var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var rows = input.Length;
var cols = input[0].Length;

var seaCucumbers = new List<SeaCucumber>(rows * cols);
var positions = new HashSet<Position>(rows * cols);

for (var y = 0; y < rows; y++)
{
    for (var x = 0; x < cols; x++)
    {
        if (input[y][x] != '.')
        {
            var position = new Position(x, y);

            seaCucumbers.Add(new SeaCucumber((Direction)input[y][x], position));
            positions.Add(position);
        }
    }
}

var part1 = 0;
bool anyMoved;

do
{
    var movers = 0;

    movers += Move(Direction.East);
    movers += Move(Direction.South);

    anyMoved = movers > 0;

    part1++;
} while (anyMoved);

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part1: {part1}");

int Move(in Direction direction)
{
    var movers = 0;

    var newPositions = new HashSet<Position>(positions);
    for (var i = 0; i < seaCucumbers.Count; i++)
    {
        var seaCucumber = seaCucumbers[i];
        if (seaCucumber.Species == direction)
        {
            var newPosition = direction switch
            {
                Direction.East => seaCucumber.Position with { X = seaCucumber.Position.X < cols - 1 ? seaCucumber.Position.X + 1 : 0 },
                Direction.South => seaCucumber.Position with { Y = seaCucumber.Position.Y < rows - 1 ? seaCucumber.Position.Y + 1 : 0 },
                _ => throw new NotImplementedException(),
            };

            if (!positions.Contains(newPosition))
            {
                seaCucumbers[i] = seaCucumber with { Position = newPosition };
                newPositions.Remove(seaCucumber.Position);
                newPositions.Add(newPosition);
                movers++;
            }
        }
    }

    if (movers > 0)
    {
        positions = new HashSet<Position>(newPositions);
    }

    return movers;
}

record struct Position(int X, int Y);

record struct SeaCucumber(Direction Species, Position Position);

enum Direction : ushort { East = '>', South = 'v' }
