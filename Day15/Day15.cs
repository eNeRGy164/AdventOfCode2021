var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var mapSize = input.Length;

Node.MapSize = mapSize;
Node.AllNodes = new Dictionary<Point, Node>(Node.MapSize * Node.MapSize);

for (var y = 0; y < mapSize; y++)
{
    for (var x = 0; x < mapSize; x++)
    {
        var value = input[y][x] - 48;

        var position = new Point(x, y);
        Node.AllNodes[position] = new Node(position, value < 10 ? value : value % 9);
    }
}

var part1 = Dijkstra();

Node.MapSize = mapSize * 5;
Node.AllNodes = new Dictionary<Point, Node>(Node.MapSize * Node.MapSize);

for (var y = 0; y < mapSize; y++)
{
    for (var x = 0; x < mapSize; x++)
    {
        for (var i = 0; i < 5; i++)
        {
            for (var j = 0; j < 5; j++)
            {
                var value = input[y][x] - 48 + i + j;

                var position = new Point(x + (mapSize * i), y + (mapSize * j));
                Node.AllNodes[position] = new Node(position, value < 10 ? value : value % 9);
            }
        }
    }
}

var part2 = Dijkstra();

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

static int Dijkstra()
{
    var finish = Node.AllNodes[new Point(Node.MapSize - 1, Node.MapSize - 1)];

    var queue = new SortedSet<Node>(CostToStartComparer.Instance)
    {
        Node.AllNodes[new Point(0, 0)] with { CostToStart = 0 }
    };

    do
    {
        var node = queue.ElementAt(0);
        queue.Remove(node);

        foreach (var connection in node.Connections())
        {
            if (connection.Visited)
            {
                continue;
            }

            if (node.CostToStart + connection.Cost < connection.CostToStart)
            {
                connection.CostToStart = node.CostToStart + connection.Cost;
                connection.Previous = node;

                if (!queue.Contains(connection))
                {
                    queue.Add(connection);
                }
            }
        }

        if (node == finish)
        {
            break;
        }

        node.Visited = true;

    } while (queue.Count > 0);

    return finish.CostToStart;
}

class CostToStartComparer : IComparer<Node>
{
    public static CostToStartComparer Instance = new();

    public int Compare(Node? x, Node? y)
    {
        var value = x!.CostToStart.CompareTo(y!.CostToStart);

        return value != 0 ? value : x.Position.CompareTo(y.Position);
    }
}

record struct Point(int X, int Y) : IComparable<Point>
{
    public int CompareTo(Point other)
    {
        if (X == other.X && Y == other.Y)
        {
            return 0;
        }
        else if (X < other.X || (X == other.X && Y < other.Y))
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}

record class Node(Point Position, int Cost)
{
    public static int MapSize { get; set; }
    public static Dictionary<Point, Node> AllNodes { get; set; } = new Dictionary<Point, Node>(0);
    public bool Visited { get; set; } = false;
    public int CostToStart { get; set; } = int.MaxValue;
    public Node? Previous { get; set; }

    public IEnumerable<Node> Connections()
    {
        if (Position.X < MapSize - 1)
        {
            yield return AllNodes[Position with { X = Position.X + 1 }];
        }

        if (Position.Y < MapSize - 1)
        {
            yield return AllNodes[Position with { Y = Position.Y + 1 }];
        }

        if (Position.X > 0)
        {
            yield return AllNodes[Position with { X = Position.X - 1 }];
        }

        if (Position.Y > 0)
        {
            yield return AllNodes[Position with { Y = Position.Y - 1 }];
        }
    }
}
