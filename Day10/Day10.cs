var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var pairs = new Dictionary<char, char>(4) { ['('] = ')', ['<'] = '>', ['['] = ']', ['{'] = '}' };
var syntaxErrorScores = new Dictionary<char, int>(4) { [')'] = 3, [']'] = 57, ['}'] = 1197, ['>'] = 25137 };
var autoCompleteScores = new List<char>(4) { ')', ']', '}', '>' };
var fixScores = new List<long>();

var part1 = 0;
for (var i = 0; i < input.Length; i++)
{
    var line = input[i];
    var corrupt = false;
    var stack = new Stack<char>();

    for (var j = 0; j < line.Length; j++)
    {
        var c = line[j];

        if (pairs.ContainsKey(c))
        {
            stack.Push(c);
        }
        else if (c == pairs[stack.Peek()])
        {
            stack.Pop();
        }
        else
        {
            part1 += syntaxErrorScores[c];
            corrupt = true;
            break;
        }
    }

    if (!corrupt)
    {
        var score = 0L;

        while (stack.TryPop(out var c))
        {
            score = (score * 5) + autoCompleteScores.IndexOf(pairs[c]) + 1;
        }

        fixScores.Add(score);
    }
}

var scores = fixScores.ToArray();
Array.Sort(scores);

var part2 = scores[scores.Length / 2];

stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
