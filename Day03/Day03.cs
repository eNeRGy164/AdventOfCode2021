var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var bitLength = input[0].Length;

var sumOfBits = SumBits(input);

var gamma = 0;
var epsilon = 0;

for (var i = 0; i < bitLength; i++)
{
    if (Gamma(sumOfBits[i], input.Length) == 1)
    {
        gamma |= 1 << bitLength - i - 1;
    }

    if (Epsilon(sumOfBits[i], input.Length) == 1)
    {
        epsilon |= 1 << bitLength - i - 1;
    }
}

var oxygen = 0;
var subset = input;

for (var j = 0; j < bitLength; j++)
{
    var localSum = SumBits(subset);
    var bit = (char)(Gamma(localSum[j], subset.Length) + 48);

    subset = subset.Where(l => l[j] == bit).ToArray();

    if (subset.Length == 1)
    {
        oxygen = Convert.ToInt32(subset[0], 2);
        break;
    }
}

var co2 = 0;
subset = input;

for (var j = 0; j < bitLength; j++)
{
    var localSum = SumBits(subset);
    var bit = (char)Epsilon(localSum[j], subset.Length) + 48;

    subset = subset.Where(l => l[j] == bit).ToArray();

    if (subset.Length == 1)
    {
        co2 = Convert.ToInt32(subset[0], 2);
        break;
    }
}

var part1 = gamma * epsilon;
var part2 = oxygen * co2;

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedTicks} ticks");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

static int Gamma(in int value, in int length)
{
    return value >= length / 2.0 ? 1 : 0;
}

static int Epsilon(in int value, in int length)
{
    return value >= length / 2.0 ? 0 : 1;
}

static int[] SumBits(string[] lines)
{
    var bitLength = lines[0].Length;
    var sum = new int[bitLength];

    for (var i = 0; i < lines.Length; i++)
    {
        for (var j = 0; j < bitLength; j++)
        {
            sum[j] += lines[i][j] - 48;
        }
    }

    return sum;
}
