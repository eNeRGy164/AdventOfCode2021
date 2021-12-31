var input = File.ReadAllLines("input.txt");

var stopwatch = Stopwatch.StartNew();

var lines = new List<string>();
for (var i = 0; i < input.Length; i++)
{
    if (input[i] == "inp w") continue; // Handled by initializing register with digit
    if (input[i] == "mul x 0") continue; // Handled by initializing register with 0

    lines.Add(input[i]);
}

var mainProgram = lines.ToArray();

var part1 = HighestModelNumber(mainProgram);
var part2 = LowestModelNumber(mainProgram);

stopwatch.Stop();
Console.WriteLine($"{stopwatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

static string HighestModelNumber(string[] mainProgram)
{
    var cache = new Dictionary<int, string>(9);

    for (var d = '9'; d >= '1'; d--)
    {
        cache[d - 48] = new string(d, 1);
    }

    var result = string.Empty;

    while (cache.Values.First().Length < 14)
    {
        var lastStep = new Dictionary<int, string>(cache);
        cache.Clear();

        foreach (var output in lastStep)
        {
            var step = output.Value.Length;

            for (var d = '9'; d >= '1'; d--)
            {
                var value = output.Value + d;

                var z = Decode(mainProgram, step, d, output.Key);
                if (z < 0) continue; // Value does not decrease enough to be correct

                if (!cache.ContainsKey(z))
                {
                    // If it exists, a higher/lower value already has added it
                    cache[z] = value;

                    if (z == 0 && step == 14)
                    {
                        // Winner
                        goto end;
                    }
                }
            }
        }

    end:
        if (cache.Values.First().Length == 14) result = cache[0];
    }

    return result;
}

static string LowestModelNumber(string[] mainProgram)
{
    var cache = new Dictionary<int, string>(9);

    for (var d = '1'; d <= '9'; d++)
    {
        cache[d - 48] = new string(d, 1);
    }

    var result = string.Empty;

    while (cache.Values.First().Length < 14)
    {
        var lastStep = new Dictionary<int, string>(cache);
        cache.Clear();

        foreach (var output in lastStep)
        {
            var step = output.Value.Length;

            for (var d = '1'; d <= '9'; d++)
            {
                var value = output.Value + d;

                var z = Decode(mainProgram, step, d, output.Key);
                if (z < 0) continue; // Value does not decrease enough to be correct

                if (!cache.ContainsKey(z))
                {
                    // If it exists, a higher/lower value already has added it
                    cache[z] = value;

                    if (z == 0 && step == 14)
                    {
                        // Winner
                        goto end;
                    }
                }
            }
        }

    end:
        if (cache.Values.First().Length == 14) result = cache[0];
    }

    return result;
}

static int Decode(string[] mainProgram, int step, char inputDigit, int startValue)
{
    var register = new Dictionary<char, int>(4) {
        ['w'] = inputDigit - 48,
        ['x'] = 0,
        ['y'] = 0,
        ['z'] = startValue
    };

    // Only execute the part that covers the current digit
    var length = mainProgram.Length / 14;
    var offset = step * length;
    var program = mainProgram[offset..(offset + length)];

    for (var i = 0; i < program.Length; i++)
    {
        Execute(register, program[i]);
    }

    var result = register['z'];

    if (program[2] == "div z 26" && result > startValue / 25)
    {
        result = -1;
    }

    return result;
}

static void Execute(in Dictionary<char, int> register, in ReadOnlySpan<char> line)
{
    var instruction = line[0..3].ToString();
    var a = line[4];
    var b = 0;

    if (!int.TryParse(line[6..], out b))
    {
        b = register[line[6]];
    }

    register[a] = instruction switch
    {
        "add" => register[a] + b,
        "mul" => register[a] * b,
        "div" => register[a] / b,
        "mod" => register[a] % b,
        "eql" => register[a] == b ? 1 : 0,
        _ => throw new NotImplementedException(),
    };
}
