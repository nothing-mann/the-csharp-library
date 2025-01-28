// Importing the namespaces used
using System.Diagnostics;

// For displaying the results of the game.
List<string> QuestionList = [];
List<string> AnswerList = [];
List<string> SavedAnswersList = [];
string TimeElapsedString;

// For the history of the previous games.
List<string> QuestionHistory = [];
List<string> AnswerHistory = [];
List<string> UserAnswerHistory = [];
List<string> TimeElapsedList = [];
List<int> CorrectAnswersPerGame = [];

// Stopwatch is used to record time for each game.
Stopwatch stopwatch= new Stopwatch();

// Game Starter.
Console.WriteLine("Press Any Key to continue");
string? startKey = Console.ReadLine();


if (startKey != null)
{
    Console.Clear();
    Console.WriteLine("Do you wish to start a new game? (y/N)");
    string? newGameStarterInput = Console.ReadLine();
    if(newGameStarterInput == "y")
    {
        Console.Clear();
        int difficultyLevel = SelectDifficulty();
        Console.Clear();
        StartGame(difficultyLevel);    
    }
}


/*
,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
 Functions declared below here.
......................................................................................................................
*/


// Function to set difficulty right after starting the game.
int SelectDifficulty()
{
    Console.WriteLine("Select the difficulty of the game:");
        Console.WriteLine("[0] I'm a noob give me something easy");
        Console.WriteLine("[1] I'm fine with moderate difficulty");
        Console.WriteLine("[2] I'm the human calculator. Test my boundaries");

        string? difficultySelection = Console.ReadLine();

    return difficultySelection switch 
        {
            "0" => 0,
            "1" => 1,
            "2" => 2,
            _ => throw new ArgumentException(message: "This difficulty is not implemented yet"),
        };
}

// Main game loop function.
void StartGame(int difficultyLevel)
{
    int counter = 0;

    Console.WriteLine("Select an operator you want to try:");
    Console.WriteLine($"[0] Plus");
    Console.WriteLine($"[1] Minus");
    Console.WriteLine($"[2] Multiply");
    Console.WriteLine($"[3] Divide");
    Console.WriteLine($"[4] Random");
    string? operatorSymbolSelector = Console.ReadLine();
    Console.Clear();
    string operatorSymbol = CheckAndReturnOperator(operatorSymbolSelector);
    stopwatch.Start();
    while(true)
    {
        var questionAnswer = GenerateQuestion(difficultyLevel, operatorSymbolSelector != "4" ? operatorSymbol : RandomOperator());
        QuestionList.Add(questionAnswer.Item1);
        AnswerList.Add(questionAnswer.Item2);
        Console.WriteLine($"What is {questionAnswer.Item1}?");
        string? givenAnswer = Console.ReadLine();
        Console.Clear();
        SavedAnswersList.Add(givenAnswer ?? "Invalid answer");
        counter++;
        if (counter < 10)
            continue;
        break;
    }
    CheckAnswers();

    AskToContinue();
    
}

// This function generates operands (in this case numbers) to perform calculations on.
(int, int) GenerateOperands(int difficultyLevel)
{
    Random randomNumberGenerator = new Random();
    int operand1;
    int operand2;
    switch (difficultyLevel)
    {
        case 0:
            operand1 = randomNumberGenerator.Next(10);
            operand2 = randomNumberGenerator.Next(10);
            break;
        case 1:
            operand1 = randomNumberGenerator.Next(10, 99);
            operand2 = randomNumberGenerator.Next(10, 99);
            break;
        case 2:
            operand1 = randomNumberGenerator.Next(100, 999);
            operand2 = randomNumberGenerator.Next(100, 999);
            break;
        default:
            throw new ArgumentException(message: "The difficulty level is invalid.");
    } 
    return (operand1, operand2);
}

// This function checks which operator the player selects and returns the respective operator.
string CheckAndReturnOperator(string? operatorSymbol)
{
   return operatorSymbol switch
    {
        "0" => "+",
        "1" => "-",
        "2" => "*",
        "3" => "/",
        "4" => RandomOperator(),
        _ => throw new ArgumentException("The operator is null")
    };
    
}

// This function is for when the player selects random operator option.
string RandomOperator()
{
    var random = new Random();
    var number = random.Next(4);
    return number switch
    {
        0 => "+",
        1 => "-",
        2 => "*",
        3 => "/",
        _ => throw new ArgumentOutOfRangeException("This is not even possible")
    };
}

// After generating the operands and the operator using the functions above, this function is used to generate questions.
(string,string) GenerateQuestion(int difficultyLevel, string operatorSymbol)
{
    var operands = GenerateOperands(difficultyLevel);
    string answer;
    switch (operatorSymbol)
    {
        case "+":
            answer = $"{Operators.Addition(operands.Item1, operands.Item2)}";
            break;
        case "-":
            answer = $"{Operators.Subtraction(operands.Item1, operands.Item2)}";       
            break;
        case "*":
            answer = $"{Operators.Multiplication(operands.Item1, operands.Item2)}";
            break;
        case "/":
            while (operands.Item2 == 0 || operands.Item1 % operands.Item2 != 0)
            {
                operands = GenerateOperands(difficultyLevel);
            }
            answer = $"{Operators.Division(operands.Item1, operands.Item2)}";
            break;
        default:
            throw new ArgumentException($"operator symbol '{operatorSymbol}' is not supported.");
    }
    string expression = $"{operands.Item1} {operatorSymbol} {operands.Item2}";
    return (expression, answer);
}


// This function deals with asking whether to continue or quit.
void AskToContinue()
{
    Console.WriteLine("\n\n[0] Do you want to start the game again?");
    Console.WriteLine("[1] See the history of previous games");
    Console.WriteLine("Press anything else to quit");
    var optionSelection = Console.ReadLine();
    Console.Clear();
    if (optionSelection == "0")
    {
        int difficultyLevel = SelectDifficulty();
        StartGame(difficultyLevel);
    }else if (optionSelection == "1"){
        DisplayHistory();
    }else
    {
        return;
    }
}


// This is used to check and display the scores and corrections to the operations after each game.
void CheckAnswers()
{
    stopwatch.Stop();
    TimeSpan elapsed = stopwatch.Elapsed;
    TimeElapsedString = $"{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
    Console.WriteLine($"You solved the questions in {TimeElapsedString}");
    TimeElapsedList.Add(TimeElapsedString);
    int count = 0;
    for (int i = 0; i < AnswerList.Count; i++)
    {
        if (AnswerList[i] == SavedAnswersList[i])
            count++;
        else
        {
            Console.WriteLine($"{QuestionList[i]} is not {SavedAnswersList[i]}");
        }
    }
    Console.WriteLine($"You have answered {count} questions correctly.");
    CorrectAnswersPerGame.Add(count);
    StoreToHistory();
}

// This function stores the history all the games you have played in a single session.
void StoreToHistory()
{
    for (int i=0; i<QuestionList.Count; i++)
    {
        QuestionHistory.Add(QuestionList[i]);
        AnswerHistory.Add(AnswerList[i]);
        UserAnswerHistory.Add(SavedAnswersList[i]);
    }
    QuestionList = [];
    AnswerList = [];
    SavedAnswersList = [];
}

// This function displays the history of all the games played in the current (single) session.
void DisplayHistory()
{
    int correctAnswerCount = 0;
    int gameCount = 0;
    Console.WriteLine("---------------------------------------------------------");
    Console.WriteLine("GAME HISTORY");
    Console.WriteLine("---------------------------------------------------------");
        
    for (int i=0; i<QuestionHistory.Count; i++)
    {
        if (i%10 == 0)
        {
            gameCount++;
            Console.WriteLine($"\n\nGame {gameCount}");
            Console.WriteLine($"You took {TimeElapsedList[gameCount - 1]} to complete this game.");
            Console.WriteLine($"You answered {CorrectAnswersPerGame[gameCount -1]} questions correctly in this game.");
        }
        if (AnswerHistory[i] == UserAnswerHistory[i])
            correctAnswerCount++;
        else
        {
            Console.WriteLine($"{QuestionHistory[i]} is not {AnswerHistory[i]}");
        }
    }
    Console.WriteLine("\n\n\n---------------------------------------------------------------");
    Console.WriteLine($"You have answered {correctAnswerCount} questions correctly in total {gameCount} games.");

    AskToContinue();
}


// Operators class to generate the correct answers for different operations.
public static class Operators
{
    public static int Addition(int operand1, int operand2)=>operand1 + operand2;
    public static int Subtraction(int operand1, int operand2)=> operand1 - operand2;
    public static int Multiplication(int operand1, int operand2)=>operand1 * operand2;
    public static int Division(int operand1, int operand2)=>operand1 / operand2;
    
}