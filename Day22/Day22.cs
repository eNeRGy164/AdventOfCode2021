using static System.Math;

var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var initializationSteps = 0;

var space = new Cuboid[input.Length];
var initializationPassed = false;

for (var i = 0; i < input.Length; i++)
{
    // on x=nnn..nnn,y=nnn..nnn,z=nnn..nnn
    var line = input[i].Split(' ', '=', '.', ',');

    var command = line[0][1] == 'n' ? Mode.Positive : Mode.Negative; // on|off
    var x1 = int.Parse(line[2]);
    var x2 = int.Parse(line[4]);
    var y1 = int.Parse(line[6]);
    var y2 = int.Parse(line[8]);
    var z1 = int.Parse(line[10]);
    var z2 = int.Parse(line[12]);

    var cuboid = new Cuboid(x1, x2, y1, y2, z1, z2, command);
    space[i] = cuboid;

    if (!initializationPassed)
    {
        if (cuboid.IsInInitializationRange)
        {
            initializationSteps++;
        }
        else
        {
            initializationPassed = true;
        }
    }
}

var part1 = CalculateVolume(space[..initializationSteps]);
var part2 = CalculateVolume(space);

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

static long CalculateVolume(in Cuboid[] space)
{
    var sum = new List<Cuboid>();

    for (var i = 0; i < space.Length; i++)
    {
        var a = space[i];
        if (a.Mode == Mode.Positive) sum.Add(a);

        var cuboids = sum.Count - 1;
        for (var j = 0; j < cuboids; j++)
        {
            var b = sum[j];

            if (a.Intersects(b))
            {
                sum.Add(b.Overlap(a));
            }
        }
    }

    return sum.Sum(c => c.Volume);
}

record struct Cuboid(int MinX, int MaxX, int MinY, int MaxY, int MinZ, int MaxZ, Mode Mode)
{
    const int InitializationRange = 50;

    public long Volume { get; init; } = (long)(MaxX - MinX + 1) * (MaxY - MinY + 1) * (MaxZ - MinZ + 1) * (long)Mode;

    public bool Intersects(in Cuboid b)
    {
        return MinX <= b.MaxX && MaxX >= b.MinX &&
               MinY <= b.MaxY && MaxY >= b.MinY &&
               MinZ <= b.MaxZ && MaxZ >= b.MinZ;
    }

    public Cuboid Overlap(in Cuboid b)
    {
        return new Cuboid(
            Max(MinX, b.MinX),
            Min(MaxX, b.MaxX),
            Max(MinY, b.MinY),
            Min(MaxY, b.MaxY),
            Max(MinZ, b.MinZ),
            Min(MaxZ, b.MaxZ),
            Mode == Mode.Positive ? Mode.Negative : Mode.Positive
        );
    }

    public bool IsInInitializationRange => !(Abs(MinX) > InitializationRange
                                             || Abs(MaxX) > InitializationRange
                                             || Abs(MinY) > InitializationRange
                                             || Abs(MaxY) > InitializationRange
                                             || Abs(MinZ) > InitializationRange
                                             || Abs(MaxZ) > InitializationRange);
}

enum Mode { Negative = -1, Positive = 1 }
