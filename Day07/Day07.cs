var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var numbers = input[0].Split(',');
var positions = new int[numbers.Length];

for (var i = 0; i < numbers.Length; i++)
{
    var value = int.Parse(numbers[i]);
    positions[i] = value;
}

var part1 = FindBottomOfCurve(CalculateConstantConsumption);
var part2 = FindBottomOfCurve(CalculateIncreasingConsumption);

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

int CalculateConstantConsumption(int position)
{
    var sum = 0;

    for (var i = 0; i < positions.Length; i++)
    {
        sum += Math.Abs(positions[i] - position);
    }

    return sum;
}

int CalculateIncreasingConsumption(int position)
{
    var sum = 0;

    for (var i = 0; i < positions.Length; i++)
    {
        for (var j = 1; j <= Math.Abs(positions[i] - position); j++)
        {
            sum += j;
        }
    }

    return sum;
}

int FindBottomOfCurve(Func<int, int> func)
{
    var position = 0;
    var limit = 2000; // Assume we never have lowest values at the end

    var c1 = 0;
    var c2 = 0;

    for (var i = 100; i > 0; i /= 10)
    {
        for (var p = position; p < limit; p += i)
        {
            c1 = func(p);
            c2 = func(p + 1);

            if (c1 > c2)
            {
                position = p;
            }
            else
            {
                limit = p;
                break;
            }
        }
    }

    return c1;
}
