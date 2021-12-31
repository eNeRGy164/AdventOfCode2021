var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var parsingDots = true;

var dots = new List<Dot>();
var instructions = new List<Instruction>();

for (var i = 0; i < input.Length; i++)
{
    var line = input[i];

    if (line.Length == 0)
    {
        parsingDots = false;
        continue;
    }

    if (parsingDots)
    {
        // xxx,yyy
        var coordinates = line.Split(',');
        dots.Add(new Dot(int.Parse(coordinates[0]), int.Parse(coordinates[1])));
    }
    else
    {
        // (x|y)=nnn
        var instruction = line.Split('=');
        instructions.Add(new Instruction(instruction[0][^1], int.Parse(instruction[1])));
    }
}

var part1 = 0;

for (var i = 0; i < instructions.Count; i++)
{
    var instruction = instructions[i];

    for (var d = 0; d < dots.Count; d++)
    {
        dots[d] = dots[d].Fold(instruction);
    }

    dots = new(dots.Distinct());

    if (part1 == 0) part1 = dots.Count;
}

dots.Sort();

var stringbuilder = new StringBuilder();

for (var y = 0; dots.Count > 0 && y <= dots[^1].Y; y++) // Until next dot is for another row
{
    for (var x = 0; dots.Count > 0 && dots[0].Y == y; x++) // While next dot is for the same row
    {
        if (dots[0].X == x)
        {
            stringbuilder.Append('█');
            dots.RemoveAt(0);
        }
        else
        {
            stringbuilder.Append(' ');
        }
    }

    stringbuilder.AppendLine();
}

var part2 = stringbuilder.ToString();

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2:\n{part2}");

record struct Instruction(char Axis, int Value);

record struct Dot(int X, int Y) : IComparable<Dot>
{
    static readonly Comparer<int> comparer = Comparer<int>.Default;

    public Dot Fold(Instruction instruction) =>
        instruction.Axis switch
        {
            'y' => FoldY(instruction.Value),
            'x' => FoldX(instruction.Value),
            _ => throw new NotImplementedException(),
        };

    Dot FoldX(int fold) => X > fold ? (this with { X = fold - (X - fold) }) : this;
    Dot FoldY(int fold) => Y > fold ? (this with { Y = fold - (Y - fold) }) : this;

    public int CompareTo(Dot other)
    {
        var value = comparer.Compare(Y, other.Y);

        return value != 0 ? value : comparer.Compare(X, other.X);
    }
}
