using Library;

var input = IO.ReadInputAsStringArray().ToArray();
var numbers = input[0].Split(',').Select(x => int.Parse(x)).ToArray();
var boards = new List<int[][]> ();
var boardResults = new List<int[][]> ();

SetupData();
PartA();

void SetupData()
{
    int[][] currentBoard = new int[5][];
    int[][] emptyBoard;
    var currentRow = 0;

    foreach (var line in input[2..])
    {
        if (string.IsNullOrWhiteSpace(line) || line == "\n")
        {
            boards.Add(currentBoard);
            currentBoard = new int[5][];
            currentRow = 0;
            emptyBoard = new int[5][];
            for (int i = 0; i < emptyBoard.Length; i++)
            {
                emptyBoard[i] = Enumerable.Repeat(0, 5).ToArray();
            }
            boardResults.Add(emptyBoard);
        }
        else
        {
            var row = line.Split(' ').Select(x => x.Trim()).Where(x => int.TryParse(x, out _)).Select(x => int.Parse(x)).ToArray();
            currentBoard[currentRow] = row;
            currentRow++;
        }
    }
    boards.Add(currentBoard);
    emptyBoard = new int[5][];
    for (int i = 0; i < emptyBoard.Length; i++)
    {
        emptyBoard[i] = Enumerable.Repeat(0, 5).ToArray();
    }
    boardResults.Add(emptyBoard);
}

void PartA()
{
    foreach(var number in numbers)
    {
        // process number on each board
        for (int boardIndex = 0; boardIndex < boards.Count; boardIndex++)
        {
            int[][] board = boards[boardIndex];
            for (int row = 0; row < board.Length; ++row)
            {
                for (int col = 0; col < board[row].Length; ++col)
                {
                    if (board[row][col] == number)
                    {
                        boardResults[boardIndex][row][col] = 1;
                    }
                }
            }
        }

        // check if we have a result
        for (int boardIndex = 0; boardIndex < boardResults.Count; boardIndex++)
        {
            int[][] board = boardResults[boardIndex];
            for (int row = 0; row < board.Length; ++row)
            {
                if (board[row].Sum() == board[row].Length)
                {
                    // row bingo!
                }
                for (int col = 0; col < board[row].Length; ++col)
                {
                    if (board[row][col] == number)
                    {
                        boardResults[boardIndex][row][col] = 1;
                    }
                }
            }
        }
    }
}
