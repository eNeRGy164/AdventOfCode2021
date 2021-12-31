var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var horizontal = 0;
var depth1 = 0;
var depth2 = 0;
var aim = 0;

foreach (ReadOnlySpan<char> line in input)
{
    var instruction = line[0]; // Only f(orward), d(own), u(p)
    var value = line[^1] - 48; // Only 1 .. 9

    switch (instruction)
    {
        case 'f':
            horizontal += value;
            depth2 += aim * value;
            break;
        case 'd':
            depth1 += value;
            aim += value;
            break;
        case 'u':
            depth1 -= value;
            aim -= value;
            break;
    }
}

var part1 = horizontal * depth1;
var part2 = horizontal * depth2;

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedTicks} ticks");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
