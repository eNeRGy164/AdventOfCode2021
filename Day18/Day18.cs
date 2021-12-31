var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var inputSnails = new Pair[input.Length];
for (var i = 0; i < input.Length; i++)
{
    inputSnails[i] = Pair.Parse(input[i]);
}

var part1Snail = inputSnails[0];

for (var i = 1; i < inputSnails.Length; i++)
{
    part1Snail += inputSnails[i];
}
var part1 = part1Snail.Magnitude;

var part2 = 0;
foreach (var a in inputSnails)
{
    foreach (var b in inputSnails)
    {
        if (a == b) continue;

        var sum = (a + b).Magnitude;
        if (sum > part2) part2 = sum;
    }
}

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

class Pair
{
    public Pair()
    {
    }

    public Pair(int x, int y)
        : this(new Single(x), new Single(y))
    {
    }

    public Pair(Pair x, Pair y)
    {
        Values.Add(x);
        Values.Add(y);

        x.Parent = this;
        y.Parent = this;
    }

    public List<Pair> Values { get; init; } = new(2);

    public Pair? Parent { get; set; }

    public Pair X => Values.ElementAt(0);
    public Pair Y => Values.ElementAt(1);

    public virtual int Magnitude => X.Magnitude * 3 + Y.Magnitude * 2;

    public override string ToString() => $"[{X},{Y}]";

    public static Pair Parse(string value)
    {
        var cursor = 0;
        return Parse(value, ref cursor);
    }

    static Pair Parse(string input, ref int cursor)
    {
        Pair current = new();
        cursor++;

        for (; cursor < input.Length; cursor++)
        {
            switch (input[cursor])
            {
                case '[':
                    current.Values.Add(Parse(input, ref cursor));
                    break;
                case ']':
                    current.Values[0].Parent = current;
                    current.Values[1].Parent = current;
                    return current;
                case ',':
                    break;
                default:
                    current.Values.Add(new Single(input[cursor] - 48)); // 0 .. 9
                    break;
            }
        }

        return current;
    }

    public static Pair operator +(Pair a, Pair b) => new Pair(Parse(a.ToString()), Parse(b.ToString())).Reduce();

    void Explode()
    {
        if (Parent is null)
        {
            return;
        }

        var xParent = Parent;
        var prevXParent = this;

        while (xParent != null)
        {
            if (xParent.X != prevXParent)
            {
                if (xParent.X is Single)
                {
                    xParent.X.Add(X);
                    break;
                }
                else
                {
                    var xSibling = xParent.X;

                    while (xSibling.Y is not Single)
                    {
                        xSibling = xSibling.Y;
                    }

                    xSibling.Y.Add(X);
                    break;
                }
            }

            prevXParent = xParent;
            xParent = xParent.Parent;
        }

        var yParent = Parent;
        var prevYParent = this;

        while (yParent != null)
        {
            if (yParent.Y != prevYParent)
            {
                if (yParent.Y is Single)
                {
                    yParent.Y.Add(Y);
                    break;
                }
                else
                {
                    var ySibling = yParent.Y;

                    while (ySibling.X is not Single)
                    {
                        ySibling = ySibling.X;
                    }

                    ySibling.X.Add(Y);
                    break;
                }
            }

            prevYParent = yParent;
            yParent = yParent.Parent;
        }

        var single = new Single(0) { Parent = Parent };
        Parent.Values[Parent.Values.IndexOf(this)] = single;
    }

    virtual protected void Add(Pair single) => ((Single)this).Add(single);

    Pair Reduce()
    {
        var list = DescendentPairs(this);

        bool exploded, splitted = false;
        do
        {
            exploded = FindExplosions();

            if (!exploded)
            {
                splitted = FindSplits();
            }
        } while (exploded || splitted);

        return this;

        bool FindExplosions()
        {
            var exploded = false;
            foreach (var item in list)
            {
                if (item.Depth >= 4)
                {
                    item.Pair.Explode();
                    exploded = true;
                    list.Remove(item);
                    break;
                }
            }

            return exploded;
        }

        bool FindSplits()
        {
            var splitted = false;
            var queue = new List<Pair> { Values[0], Values[1] };

            do
            {
                var item = queue.ElementAt(0);
                queue.Remove(item);

                var insertAt = 0;

                if (item is Single single)
                {
                    if (single.Value > 9)
                    {
                        var listItem = list.First(li => li.Pair == item.Parent);
                        var pair = single.Split();

                        list.Insert(list.IndexOf(listItem) + 1, (listItem.Depth + 1, pair));

                        splitted = true;
                        goto exit;
                    }
                }
                else
                {
                    for (var i = 0; i < 2; i++)
                    {
                        queue.Insert(insertAt++, item.Values[i]);
                    }
                }
            } while (queue.Count > 0);

        exit:
            return splitted;
        }
    }

    static List<(int Depth, Pair Pair)> DescendentPairs(Pair root)
    {
        var pairs = new List<(int, Pair)> { (0, root) };
        var queue = new List<(int Depth, Pair Pair)> { (0, root) };

        do
        {
            var item = queue.ElementAt(0);
            queue.Remove(item);

            if (item.Pair is Single)
            {
                continue;
            }

            var insertAtQueue = 0;
            var insertAtList = 0;

            for (var i = 0; i < 2; i++)
            {
                if (item.Pair.Values[i] is not Single && item.Pair.Values[i] is Pair pair)
                {
                    var depth = item.Depth + 1;

                    insertAtList++;

                    if (pairs.IndexOf(item) > -1)
                    {
                        pairs.Insert(pairs.IndexOf(item) + insertAtList, (depth, pair));
                    }
                    else
                    {
                        pairs.Add((depth, pair));
                    }

                    queue.Insert(insertAtQueue++, (depth, item.Pair.Values[i]));
                }
            }

        } while (queue.Count > 0);

        return pairs;
    }
}

sealed class Single : Pair
{
    public int Value { get; set; }

    public override int Magnitude => Value;

    public Single(int value) => Value = value;

    override protected void Add(Pair single) => Add((Single)single);

    void Add(Single single) => Value += single.Value;

    public Pair Split()
    {
        if (Parent is null) throw new ArgumentNullException(nameof(Parent));

        var x = (int)Math.Floor(Value / 2.0);
        var y = (int)Math.Ceiling(Value / 2.0);

        var pair = new Pair(x, y) { Parent = Parent };
        Parent.Values[Parent.Values.IndexOf(this)] = pair;

        return pair;
    }

    public override string ToString() => Value.ToString();
}
