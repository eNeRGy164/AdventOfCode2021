var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var paths = new HashSet<string>();
var nodes = new HashSet<Node>();
Node? start = null;

for (var i = 0; i < input.Length; i++)
{
    var names = input[i].Split('-');

    var left = new Node(names[0]);
    if (!nodes.Contains(left))
    {
        if (start == null && left.IsStart) start = left;
        nodes.Add(left);
    }
    else
    {
        nodes.TryGetValue(left, out left);
    }

    var right = new Node(names[1]);
    if (!nodes.Contains(right))
    {
        if (start == null && right.IsStart) start = right;
        nodes.Add(right);
    }
    else
    {
        nodes.TryGetValue(right, out right);
    }

    if (!left!.IsStart) right!.Relations.Add(left);
    if (!right!.IsStart) left!.Relations.Add(right);
}

Explore(start!, string.Empty, start!.Name); // Prevents using any small cave twice
var part1 = paths.Count;

paths.Clear();

Explore(start!, string.Empty);
var part2 = paths.Count;

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

void Explore(in Node node, in string path, in string? smallCaveTwice = null)
{
    if (node.IsSmall && path.Contains(node.Name) && (smallCaveTwice != node.Name || path.IndexOf(smallCaveTwice) != path.LastIndexOf(smallCaveTwice)))
    {
        return;
    }

    if (node.IsEnd)
    {
        paths.Add(path);
        return;
    }

    var newPath = $"{path}{node.Name},";

    for (var i = 0; i < node.Relations.Count; i++)
    {
        var r = node.Relations[i];
        if (r.IsSmall && smallCaveTwice == null)
        {
            // Branch for this small cave, it is allowed to show up twice now
            Explore(r, newPath, r.Name);
        }

        Explore(r, newPath, smallCaveTwice);
    }
}

record Node(string Name) : IEquatable<Node>
{
    public bool IsStart { get; init; } = Name == "start";
    public bool IsEnd { get; init; } = Name == "end";
    public bool IsSmall { get; init; } = char.IsLower(Name[0]);

    public List<Node> Relations { get; } = new List<Node>(5);

    public virtual bool Equals(Node? other) => Name.Equals(other?.Name);

    public override int GetHashCode() => Name.GetHashCode();
}
