var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var school = new List<long>(new long[9]); // 0 .. 8

var values = input[0].Split(',');
for (var i = 0; i < values.Length; i++)
{
    var fish = values[i][0] - 48; // 1 .. 5
    school[fish]++;
}

for (var i = 1; i <= 80; i++)
{
    Day();
}

var part1 = Sum();

for (var i = 81; i <= 256; i++)
{
    Day();
}

var part2 = Sum();

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

void Day()
{
    var spawning = school[0];
    school.RemoveAt(0);
    school[6] += spawning;
    school.Add(spawning);
}

long Sum()
{
    var total = 0L;

    for (var i = 0; i < 9; i++)
    {
        total += school[i];
    }

    return total;
}
