using static PacketType;

var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var stringBuilder = new StringBuilder();
for (var i = 0; i < input[0].Length; i++)
{
    stringBuilder.Append(Convert.ToString(Convert.ToInt32(input[0][i].ToString(), 16), 2).PadLeft(4, '0'));
}

var binary = stringBuilder.ToString();

var position = 0;
var part1 = 0;
var part2 = ParsePacket(binary, ref position, ref part1);

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

static long ParsePacket(in ReadOnlySpan<char> binary, ref int position, ref int part1)
{
    if (binary.Length - position < 8)
    {
        // Skip remainder
        position = binary.Length;
        return 0;
    }

    var version = Convert.ToInt32(binary[position..(position += 3)].ToString(), 2);
    part1 += version;

    var type = (PacketType)Convert.ToInt32(binary[position..(position += 3)].ToString(), 2);

    return type switch
    {
        Literal => ParseLiteral(binary, ref position),
        _ => ParseOperator(binary, type, ref position, ref part1),
    };
}

static long ParseLiteral(in ReadOnlySpan<char> binary, ref int position)
{
    var value = new StringBuilder();
    bool stopAfterLiteral;

    do
    {
        stopAfterLiteral = binary[position++] == '0';

        value.Append(binary[position..(position += 4)]);
    } while (!stopAfterLiteral);

    return Convert.ToInt64(value.ToString(), 2);
}

static long ParseOperator(in ReadOnlySpan<char> binary, in PacketType type, ref int position, ref int part1)
{
    var bits = new StringBuilder();
    var values = new List<long>();

    switch (binary[position++])
    {
        case '0':
            {
                var length = Convert.ToInt32(binary[position..(position += 15)].ToString(), 2);
                var start = position;

                while (position < start + length)
                {
                    values.Add(ParsePacket(binary, ref position, ref part1));
                }

                break;
            }

        case '1':
            {
                var length = Convert.ToInt32(binary[position..(position += 11)].ToString(), 2);

                for (var i = 0; i < length; i++)
                {
                    values.Add(ParsePacket(binary, ref position, ref part1));
                }

                break;
            }
    }

    return type switch
    {
        Sum => values.Sum(),
        Product => values.Aggregate(1L, (s, a) => s * a),
        Minimum => values.Min(),
        Maximum => values.Max(),
        GreaterThan => values[0] > values[1] ? 1 : 0,
        LessThan => values[0] < values[1] ? 1 : 0,
        EqualTo => values[0] == values[1] ? 1 : 0,
        _ => 0L,
    };
}

enum PacketType
{
    Sum,
    Product,
    Minimum,
    Maximum,
    Literal,
    GreaterThan,
    LessThan,
    EqualTo
}
