using Library;

var input = IO.ReadInputAsStringArray().ToArray();
var numbers = input[0].Split(',').Select(x => int.Parse(x)).ToArray();
var boards = new List<int[]> ();
var boardResults = new List<int[]> ();

SetupData();
PartA();
PartB();

void SetupData()
{
    int[] currentBoard = new int[25];
    int[] emptyBoard;
    var currentRow = 0;

    foreach (var line in input[2..])
    {
        if (string.IsNullOrWhiteSpace(line) || line == "\n")
        {
            boards.Add(currentBoard);
            currentBoard = new int[25];
            currentRow = 0;
            emptyBoard = new int[25];
            boardResults.Add(emptyBoard);
        }
        else
        {
            var row = line.Split(' ').Select(x => x.Trim()).Where(x => int.TryParse(x, out _)).Select(x => int.Parse(x)).ToArray();
            Array.Copy(row, 0, currentBoard, currentRow * 5, row.Length);
            currentRow++;
        }
    }
    boards.Add(currentBoard);
    emptyBoard = new int[25];
    boardResults.Add(emptyBoard);
}

void PartA()
{
    PlayBingo(false);
}

void PartB()
{
    PlayBingo(true);
}

void PlayBingo(bool playUntilLastBoard)
{
    List<int> wonBoards = new List<int>();
    foreach(var number in numbers)
    {
        // process number on each board
        for (int boardIndex = 0; boardIndex < boards.Count; boardIndex++)
        {
            if (wonBoards.Contains(boardIndex) == true)
            {
                continue;
            }
            int[] board = boards[boardIndex];
            for (int numberIndex = 0; numberIndex < board.Length; ++numberIndex)
            {
                if (board[numberIndex] == number)
                {
                    boardResults[boardIndex][numberIndex] = 1;
                }
            }
        }

        // check if we have a result
        for (int boardIndex = 0; boardIndex < boardResults.Count; boardIndex++)
        {
            if (wonBoards.Contains(boardIndex) == true)
            {
                continue;
            }
            int[] board = boardResults[boardIndex];
            for (int rowIndex = 0; rowIndex < 5; ++rowIndex)
            {
                if (board[(rowIndex * 5)..(rowIndex * 5 + 5)].Sum() == 5
                    || board[rowIndex] + board[rowIndex + 5] + board[rowIndex + 10] + board[rowIndex + 15] + board[rowIndex + 20] == 5)
                {
                    Console.WriteLine($"BINGO! On board {boardIndex + 1}");
                    //DebugDraw(board);
                    wonBoards.Add(boardIndex);
                    if (wonBoards.Count == 1 && !playUntilLastBoard || wonBoards.Count == boardResults.Count && playUntilLastBoard)
                    {
                        var sum = 0;
                        for (int numberIndex = 0; numberIndex < board.Length; ++numberIndex)
                        {
                            if (board[numberIndex] == 0)
                            {
                                sum += boards[boardIndex][numberIndex];
                            }
                        }
                        Console.WriteLine($"Result: {sum} * {number} = {sum * number}");
                        return;
                    }
                    break;
                }
            }
        }
    }
}

void DebugDraw(int[] board)
{
    for(int i = 0; i < 5; ++i)
    {
        Console.WriteLine(string.Join(" ", board[(i * 5)..(i * 5 + 5)]).ToString());
    }
}
