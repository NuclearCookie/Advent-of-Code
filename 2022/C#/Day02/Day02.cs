using Library;


var turns = Library.IO.ReadInputAsStringArray(false);
var totalScore = 0;
foreach (var turn in turns)
{
    totalScore += PlayAndScore(ParseTurn(turn));
}
Console.WriteLine($"Part A: Total Score: {totalScore}");
totalScore = 0;
foreach (var turn in turns)
{
    totalScore += PlayAndScore(PredictOutcome(ParseTurn(turn)));
}
Console.WriteLine($"Part B: Total Score: {totalScore}");

Moves[] ParseTurn(string turn)
{
    var stringMoves = turn.Split(' ');
    Moves[] moves = new Moves[2];

    switch(stringMoves[0])
    {
        case "A":
            moves[0] = Moves.Rock;
            break;
        case "B":
            moves[0] = Moves.Paper;
            break;
        case "C":
            moves[0] = Moves.Scissors;
            break;
    }

    switch(stringMoves[1])
    {
        case "X":
            moves[1] = Moves.Rock;
            break;
        case "Y":
            moves[1] = Moves.Paper;
            break;
        case "Z":
            moves[1] = Moves.Scissors;
            break;
    }

    return moves;
}

int PlayAndScore(Moves[] moves)
{
    var enemyMove = moves[0];
    var ownMove = moves[1];

    var score = (int)ownMove + 1;
    if (ownMove == enemyMove) // draw
    {
        score += 3;
    }
    else if (ownMove == Moves.Rock && enemyMove == Moves.Scissors 
        || ownMove == Moves.Paper && enemyMove == Moves.Rock 
        || ownMove == Moves.Scissors && enemyMove == Moves.Paper)
    {
        score += 6;
    }
    return score;
}

Moves[] PredictOutcome(Moves[] moves)
{
    var enemyMove = moves[0];
    var expectedOutcome = (Outcome)moves[1];
    Moves ownMove = Moves.Rock;
    // first chose our ownMove based on the expectedOutcome
    if (expectedOutcome == Outcome.Lose)
    {
        if (enemyMove == Moves.Rock)
            ownMove = Moves.Scissors;
        else if (enemyMove == Moves.Paper)
            ownMove = Moves.Rock;
        else if (enemyMove == Moves.Scissors)
            ownMove = Moves.Paper;
    }
    else if (expectedOutcome == Outcome.Win)
    {
        if (enemyMove == Moves.Rock)
            ownMove = Moves.Paper;
        else if (enemyMove == Moves.Paper)
            ownMove = Moves.Scissors;
        else if (enemyMove == Moves.Scissors)
            ownMove = Moves.Rock;
    }
    else
    {
        ownMove = enemyMove;
    }
    moves[1] = ownMove;
    return moves;
}

enum Moves
{
    Rock,
    Paper,
    Scissors
};

enum Outcome
{
    Lose,
    Draw,
    Win
};