var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var values = new int[input.Length];
for (var i = 0; i < input.Length; i++)
{
    values[i] = Convert.ToInt32(input[i]);
}

var previousValue = int.MaxValue;
var previousSum = int.MaxValue;
var part1 = 0;
var part2 = 0;

for (var i = 0; i < values.Length; i++)
{
    if (values[i] > previousValue)
    {
        part1++;
    }

    previousValue = values[i];

    if (i < values.Length - 2)
    {
        var sum = values[i] + values[i + 1] + values[i + 2];
        if (sum - previousSum > 0)
        {
            part2++;
        }

        previousSum = sum;
    }
}

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
