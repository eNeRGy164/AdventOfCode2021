using static Manipulation;

var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var scanners = ParseScanners();

var probes = new List<Point>();
probes.AddRange(scanners[0].Points);

var unresolvedScanners = scanners.Count - 1;

while (unresolvedScanners > 0)
{
    for (var i = 0; i < scanners.Count; i++)
    {
        var scannerA = scanners[i];
        if (scannerA.Position == null || scannerA.Done)
        {
            continue;
        }

        for (var j = 0; j < scanners.Count; j++)
        {
            var scannerB = scanners[j];
            if (scannerA == scannerB || scannerB.Position != null)
            {
                continue;
            }

            foreach (var knownProbe in scannerA.Points)
            {
                foreach (var unknownProbeSet in scannerB.Permutations)
                {
                    foreach (var unknownProbe in unknownProbeSet)
                    {
                        var assumption = knownProbe - unknownProbe;
                        var count = 0;
                        var maxPoints = scannerA.Points.Count;

                        foreach (var knownPointToVerify in scannerA.Points)
                        {
                            foreach (var unknownProbeToVerify in unknownProbeSet)
                            {
                                if (assumption + unknownProbeToVerify == knownPointToVerify)
                                {
                                    count++;
                                    if (count >= 12) goto enough;
                                    break;
                                }
                            }

                            if (maxPoints - scannerA.Points.IndexOf(knownPointToVerify) + count < 12)
                            {
                                // Can't reach 12 anymore
                                break;
                            }
                        }

                    enough:
                        if (count >= 12)
                        {
                            scannerB.Position = assumption;
                            unresolvedScanners--;

                            scannerB.Points.Clear();
                            scannerB.Points.AddRange(unknownProbeSet.Select(up => up + scannerB.Position));

                            probes = probes.Union(scannerB.Points).ToList();
                            Console.WriteLine($"{scannerB.Name} found by {scannerA.Name}");

                            goto quickOut;
                        }
                    }
                }
            }
        quickOut:
            Thread.Sleep(0);
        }

        scannerA.Done = true;
    }
}

var part1 = probes.Count;

var part2 = 0;
for (var i = 0; i < scanners.Count; i++)
{
    for (var j = i + 1; j < scanners.Count; j++)
    {
        var distance = Math.Abs(scanners[i].Position!.X - scanners[j].Position!.X) +
                       Math.Abs(scanners[i].Position!.Y - scanners[j].Position!.Y) +
                       Math.Abs(scanners[i].Position!.Z - scanners[j].Position!.Z);

        if (distance > part2) part2 = distance;
    }
}

stopWatch.Stop();
Console.WriteLine($"{stopWatch.Elapsed.TotalSeconds:N2}s");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

List<Scanner> ParseScanners()
{
    var scanners = new List<Scanner>();

    var name = string.Empty;
    var points = new List<Point>();
    for (var i = 0; i < input.Length; i++)
    {
        if (input[i].Length == 0) continue;

        if (input[i].StartsWith("---"))
        {
            if (i > 0)
            {
                scanners.Add(new Scanner(name, points));
                points = new List<Point>();
            }

            name = input[i];
        }
        else
        {
            points.Add(Point.Parse(input[i]));
        }
    }

    scanners.Add(new Scanner(name, points));
    scanners[0].Position = new Point(0, 0, 0);

    return scanners;
}

class Scanner
{
    static readonly Manipulation[] manipulations = new[] { None, SwappedXZY, SwappedYXZ, SwappedYZX, SwappedZXY, SwappedZYX };

    static readonly List<Manipulation> manipulationSets = ManipulationSets();

    public string Name { get; init; }
    public List<Point> Points { get; init; }
    public List<List<Point>> Permutations { get; init; }
    public Point? Position { get; set; }
    public bool Done { get; set; }

    public Scanner(string name, List<Point> points)
    {
        Name = name.Trim(' ', '-');
        Points = points;
        Permutations = CalculatePermutations().ToList();
    }

    IEnumerable<List<Point>> CalculatePermutations()
    {
        foreach (var manipulation in manipulationSets)
        {
            yield return Points.Select(p => p.Manipulate(manipulation)).ToList();
        }
    }

    static List<Manipulation> ManipulationSets()
    {
        var manipulationSets = new List<Manipulation>();
        for (var v = 0; v < 6; v++)
        {
            var manipulation = manipulations[v];

            for (var mX = 0; mX < 2; mX++)
            {
                if (mX == 1) manipulation |= FlipX;

                for (var mY = 0; mY < 2; mY++)
                {
                    if (mY == 1) manipulation |= FlipY;

                    for (var mZ = 0; mZ < 2; mZ++)
                    {
                        if (mZ == 1) manipulation |= FlipZ;

                        manipulationSets.Add(manipulation);
                        manipulation &= ~FlipZ;
                    }

                    manipulation &= ~FlipY;
                }

                manipulation &= ~FlipX;
            }
        }

        return manipulationSets;
    }
}

record class Point(int X, int Y, int Z)
{
    public static Point Parse(in string value)
    {
        var values = value.Split(',');
        var x = int.Parse(values[0]);
        var y = int.Parse(values[1]);
        var z = int.Parse(values[2]);

        return new Point(x, y, z);
    }

    public Point Manipulate(in Manipulation manipulation)
    {
        var x = X;
        var y = Y;
        var z = Z;

        if ((manipulation & SwappedXZY) == SwappedXZY) { x = X; y = Z; z = Y; }
        if ((manipulation & SwappedYXZ) == SwappedYXZ) { x = Y; y = X; z = Z; }
        if ((manipulation & SwappedYZX) == SwappedYZX) { x = Y; y = Z; z = X; }
        if ((manipulation & SwappedZXY) == SwappedZXY) { x = Z; y = X; z = Y; }
        if ((manipulation & SwappedZYX) == SwappedZYX) { x = Z; y = Y; z = X; }

        if ((manipulation & FlipX) == FlipX) { x *= -1; }
        if ((manipulation & FlipY) == FlipY) { y *= -1; }
        if ((manipulation & FlipZ) == FlipZ) { z *= -1; }

        return new Point(x, y, z);
    }

    public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
}

[Flags]
enum Manipulation
{
    None = 0,
    SwappedXZY = 1,
    SwappedYXZ = 2,
    SwappedYZX = 4,
    SwappedZXY = 8,
    SwappedZYX = 16,
    FlipX = 32,
    FlipY = 64,
    FlipZ = 128,
}
