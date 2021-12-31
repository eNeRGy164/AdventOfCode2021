var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

const int buffer = 2;
const char off = '.';
const char on = '#';

var infinity = off;
var algorithm = input[0];
var size = input[2].Length;

var map = new List<char[]>(size);

for (var y = 0; y < size; y++)
{
    var row = new char[size];

    for (var x = 0; x < size; x++)
    {
        row[x] = input[y + 2][x];
    }

    map.Add(row);
}

for (var p = 0; p < 2; p++)
{
    Enhance();
}

var part1 = 0;
for (var y = 0; y < size; y++)
{
    for (var x = 0; x < size; x++)
    {
        if (map[y][x] == on) part1++;
    }
}

for (var p = 2; p < 50; p++)
{
    Enhance();
}

var part2 = 0;
for (var y = 0; y < size; y++)
{
    for (var x = 0; x < size; x++)
    {
        if (map[y][x] == on) part2++;
    }
}

void Enhance()
{
    size += (2 * buffer);

    var output = new List<char[]>(size);

    var blank = new char[size];
    Array.Fill(blank, infinity);

    for (var i = 0; i < size; i++)
    {
        var line = new char[size];
        Array.Copy(blank, line, size);

        if (i < buffer || i >= size - buffer)
        {
            output.Add(line);
            continue;
        }

        Array.Copy(map[i - buffer], 0, line, buffer, map[i - buffer].Length);
        output.Add(line);
    }


    map = new List<char[]>(size);
    for (var i = 0; i < output.Count; i++)
    {
        var line = new char[size];
        Array.Copy(output[i], line, output[i].Length);
        map.Add(line);
    }

    for (var y = buffer - 1; y < size - buffer + 1; y++)
    {
        var row = output[y];

        for (var x = buffer - 1; x < size - buffer + 1; x++)
        {
            var value = 0;
            var position = 0;

            for (var a = 1; a >= -1; a--)
            {
                for (var b = 1; b >= -1; b--)
                {
                    if (map[y + a][x + b] == on)
                    {
                        value |= (1 << position);
                    }

                    position++;
                }
            }

            row[x] = algorithm[value];
        }

        output[y] = row[(buffer - 1)..^(buffer - 1)];
    }

    for (var i = 0; i < buffer / 2; i++)
    {
        output.RemoveAt(output.Count - 1);
        output.RemoveAt(0);
    }

    map = output;

    infinity = infinity == off ? algorithm[0] : algorithm[^1];

    size -= buffer;
}

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
