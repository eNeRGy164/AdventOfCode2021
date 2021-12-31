var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var entries = new (string[] Signals, string[] Outputs)[input.Length];
for (var i = 0; i < input.Length; i++)
{
    var line = input[i].Split(" | ");
    var signals = line[0].Split(' ');
    var outputs = line[1].Split(' ');

    for (var s = 0; s < signals.Length; s++)
    {
        var signal = signals[s].ToCharArray();
        Array.Sort(signal);
        signals[s] = new string(signal);
    }

    for (var o = 0; o < outputs.Length; o++)
    {
        var output = outputs[o].ToCharArray();
        Array.Sort(output);
        outputs[o] = new string(output);
    }

    entries[i] = (signals, outputs);
}

var part1 = 0;
for (var i = 0; i < entries.Length; i++)
{
    for (var j = 0; j < 4; j++)
    {
        var digitLength = entries[i].Outputs[j].Length;

        if (digitLength <= 4 || digitLength >= 7) part1++;
    }
}

var part2 = 0;
const string eight = "abcdefg";

for (var i = 0; i < entries.Length; i++)
{
    var signals = entries[i].Signals;

    var one = signals.First(v => v.Length == 2);
    var four = signals.First(v => v.Length == 4);
    var seven = signals.First(v => v.Length == 3);
    var five = signals.First(v => v.Length == 5 && four.Except(one).All(v.Contains));
    var two = signals.First(v => v.Length == 5 && v.Except(five).Count() == 2);
    var three = signals.First(v => v.Length == 5 && v.Except(five).Count() == 1);
    var six = signals.First(v => v.Length == 6 && v.Except(one).Count() == 5);
    var nine = signals.First(v => v.Length == 6 && v.Except(one).Count() == 4 && v.Except(four).Count() == 2);
    var zero = signals.First(v => v.Length == 6 && v.Except(one).Count() == 4 && v.Except(four).Count() == 3);

    var lookup = new List<string>(10)
    {
        zero,
        one,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine,
    };

    var reading = string.Empty;
    for (var j = 0; j < 4; j++)
    {
        var item = entries[i].Outputs[j];
        reading += lookup.IndexOf(item);
    }

    part2 += int.Parse(reading);
}

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
