var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var insertions = new Dictionary<string, char>(input.Length - 2);
for (var i = 2; i < input.Length; i++)
{
    var instruction = input[i];

    insertions.Add(instruction[..2], instruction[^1]);
}

var template = input[0];
var pairs = new Dictionary<string, long>(template.Length * 2);
for (var i = 0; i < template.Length - 1; i++)
{
    var pair = template[i..(i + 2)];

    if (!pairs.TryAdd(pair, 1)) { pairs[pair]++; };
}

for (var i = 0; i < 10; i++)
{
    Insert();
}

var part1 = CalculateTotals();

for (var i = 10; i < 40; i++)
{
    Insert();
}

var part2 = CalculateTotals();

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

void Insert()
{
    var newPairs = new Dictionary<string, long>(pairs.Count * 2);
    foreach (var (key, value) in pairs)
    {
        var element = insertions[key];

        var left = string.Concat(key[0], element);
        var right = string.Concat(element, key[1]);

        if (!newPairs.TryAdd(left, value)) { newPairs[left] += value; };
        if (!newPairs.TryAdd(right, value)) { newPairs[right] += value; };
    }

    pairs = newPairs;
}

long CalculateTotals()
{
    var elements = new Dictionary<char, long>();
    foreach (var (key, value) in pairs)
    {
        if (!elements.TryAdd(key[0], value)) { elements[key[0]] += value; };
        if (!elements.TryAdd(key[1], value)) { elements[key[1]] += value; };
    }

    elements[template[0]]++;
    elements[template[^1]]++;

    var totals = elements.Values.ToArray();
    Array.Sort(totals);

    return (totals[^1] /= 2) - (totals[0] /= 2);
}
