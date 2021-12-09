using Library;
using Library.Datastructures;

var input = IO.ReadInputAsStringArray().ToArray();
var numbers = input[0].Split(',').Select(x => int.Parse(x)).ToArray();
var boards = new List<Array2D<int>> ();
var boardResults = new List<Array2D<int>> ();

SetupData();
PartA();
PartB();

void SetupData()
{
    Array2D<int> currentBoard = new Array2D<int>(5, 5);
    Array2D<int> emptyBoard = new Array2D<int>(5, 5);
    var currentRow = 0;

    foreach (var line in input[2..])
    {
        if (string.IsNullOrWhiteSpace(line) || line == "\n")
        {
            boards.Add(currentBoard);
            currentBoard = new Array2D<int>(5, 5);
            currentRow = 0;
            emptyBoard = new Array2D<int>(5, 5);
            boardResults.Add(emptyBoard);
        }
        else
        {
            var row = line.Split(' ').Select(x => x.Trim()).Where(x => int.TryParse(x, out _)).Select(x => int.Parse(x)).ToArray();
            currentBoard.SetRow(currentRow, row);
            currentRow++;
        }
    }
    boards.Add(currentBoard);
    emptyBoard = new Array2D<int>(5, 5);
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
            Array2D<int> board = boards[boardIndex];
            for (int numberIndex = 0; numberIndex < board.Length; ++numberIndex)
            {
                if (board[numberIndex] == number)
                {
                    boardResults[boardIndex][numberIndex] = 1;
                }
            }
        }

        int[] rowCache = new int[5];
        int[] colCache = new int[5];
        // check if we have a result
        for (int boardIndex = 0; boardIndex < boardResults.Count; boardIndex++)
        {
            if (wonBoards.Contains(boardIndex) == true)
            {
                continue;
            }
            Array2D<int> resultBoard = boardResults[boardIndex];
            for (int rowIndex = 0; rowIndex < 5; ++rowIndex)
            {
                resultBoard.GetRow(rowIndex, rowCache);
                resultBoard.GetColumn(rowIndex, colCache);
                if (rowCache.Sum() == 5 || colCache.Sum() == 5)
                {
                    Console.WriteLine($"BINGO! On board {boardIndex + 1}");
                    //DebugDraw(board);
                    wonBoards.Add(boardIndex);
                    if (wonBoards.Count == 1 && !playUntilLastBoard || wonBoards.Count == boardResults.Count && playUntilLastBoard)
                    {
                        var sum = 0;
                        for (int numberIndex = 0; numberIndex < resultBoard.Length; ++numberIndex)
                        {
                            if (resultBoard[numberIndex] == 0)
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
