var input = File.ReadAllLines("input.txt");

var stopWatch = Stopwatch.StartNew();

var numbers = input[0].Split(',');
var drawNumbers = new int[numbers.Length];
for (var i = 0; i < numbers.Length; i++)
{
    drawNumbers[i] = int.Parse(numbers[i]);
}

var boardCount = (input.Length - 1) / 6; // Each board is five lines with an empty line following
var boards = new List<Board>(boardCount);
for (var i = 0; i < boardCount; i++)
{
    var boardOffset = (i * 6) + 2;
    boards.Add(new Board(input[boardOffset..(boardOffset + 5)]));
}

Board? lastBoard = null;
var part1 = 0;
var part2 = 0;

for (var i = 0; i < drawNumbers.Length; i++)
{
    var number = drawNumbers[i];

    for (var b = 0; b < boards.Count; b++)
    {
        var board = boards[b];
        board.Dab(number);

        if (board == lastBoard && lastBoard.Won)
        {
            part2 = lastBoard.Sum * number;
            goto end;
        }
    }

    if (part1 == 0 && boards.Count(b => b.Won) == 1)
    {
        var first = boards.First(b => b.Won);

        part1 = first.Sum * number;
    }

    if (part1 != 0 && boards.Count(b => b.Won) == boardCount - 1)
    {
        lastBoard = boards.First(b => !b.Won);
    }
}

end:
stopWatch.Stop();
Console.WriteLine($"{stopWatch.ElapsedMilliseconds}ms");
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

record class Board
{
    readonly Number[,] numbers = new Number[5, 5];

    bool winner = false;

    public bool Won
    {
        get
        {
            if (!winner)
            {
                winner = CheckWinner();
            }

            return winner;
        }
    }

    public int Sum
    {
        get
        {
            var sum = 0;

            foreach (var number in numbers)
            {
                if (!number.Checked)
                {
                    sum += number.Value;
                }
            }

            return sum;
        }
    }

    public Board(string[] lines)
    {
        for (var y = 0; y < lines.Length; y++)
        {
            var line = lines[y].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (var x = 0; x < line.Length; x++)
            {
                numbers[y, x] = new Number(int.Parse(line[x]));
            }
        }
    }

    public void Dab(in int value)
    {
        foreach (var number in numbers)
        {
            if (number.Value == value)
            {
                number.Check();
            }
        }
    }

    bool CheckWinner()
    {
        // Rows
        for (var y = 0; y < 5; y++)
        {
            if (numbers[y, 0].Checked &&
                numbers[y, 1].Checked &&
                numbers[y, 2].Checked &&
                numbers[y, 3].Checked &&
                numbers[y, 4].Checked)
            {
                return true;
            }
        }

        // Columns
        for (var x = 0; x < 5; x++)
        {
            if (numbers[0, x].Checked &&
                numbers[1, x].Checked &&
                numbers[2, x].Checked &&
                numbers[3, x].Checked &&
                numbers[4, x].Checked)
            {
                return true;
            }
        }

        return false;
    }
}

record class Number(int Value)
{
    public bool Checked { get; private set; } = false;

    public void Check() => Checked = true;
}
