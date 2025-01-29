// Importing the namespaces used
using System.Diagnostics;

// For displaying the results of the game.
List<string> QuestionList = [];
List<int> AnswerList = [];
List<int> SavedAnswersList = [];
string TimeElapsedString;

// For the history of the previous games.
List<string> QuestionHistory = [];
List<int> AnswerHistory = [];
List<int> UserAnswerHistory = [];
List<string> TimeElapsedList = [];
List<int> CorrectAnswersPerGame = [];

// Stopwatch is used to record time for each game.
Stopwatch stopwatch= new Stopwatch();



Console.Title = "Math Game";
Console.BackgroundColor = ConsoleColor.White;
Console.ForegroundColor = ConsoleColor.Black;
Console.Clear();


// Game Starter.
WriteCenterContent("Press Any Key to continue", 'a');
string? startKey = ReadLineCenter();


if (startKey != null)
{
    Console.Clear();
    WriteCenterContent("Do you wish to start a new game? (y/N)", 'a');
    string? newGameStarterInput = ReadLineCenter();
    if(newGameStarterInput != null && newGameStarterInput.Trim().ToLower().StartsWith("y"))
    {
        int difficultyLevel = SelectDifficulty();
        Console.Clear();
        StartGame(difficultyLevel);    
    }
    Environment.Exit(0);
}


/*
,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
 Functions declared below here.
......................................................................................................................
*/

// Center content to the center of the screen by placing the cursor to the center and relative to the screen.
void WriteCenterContent(string? stringToBeCentered = null, char typeOfCenter = 'h')
{
    switch (typeOfCenter)
    {
        case 'a':
            if (stringToBeCentered == null)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
                Console.WriteLine();
            }
            else
            {
                Console.SetCursorPosition((Console.WindowWidth - stringToBeCentered.Length) / 2, Console.WindowHeight / 2);
                Console.WriteLine(stringToBeCentered);
            }
            break;
        case 'h':
            if (stringToBeCentered == null)
            {
                int padding = Console.WindowWidth / 2;
                if (padding < 0)
                {
                    padding = 0;
                }
                string paddedText = new string(' ', padding);
                // Write the text to the console
                Console.WriteLine();
                Console.Write(paddedText);
            }
            else
            {
                int padding = (Console.WindowWidth - stringToBeCentered.Length) / 2;
                if (padding < 0)
                {
                    padding = 0; 
                }
                string paddedText = new string(' ', padding) + stringToBeCentered;
                Console.WriteLine(paddedText);
            }
            break;
        default:
            throw new ArgumentException("No such type of centering available");
    }
}


// This function reads the values from the center of screen horizontally.
string? ReadLineCenter()
{
    WriteCenterContent();
    string? readLine = Console.ReadLine();
    return readLine;
}



// Function to set difficulty right after starting the game.
int SelectDifficulty()
{
    Console.Clear();
    WriteCenterContent(@"Select the difficulty of the game:", 'a');
    Console.WriteLine();
    WriteCenterContent("[0] I'm a noob give me something easy");
    WriteCenterContent("[1] I'm fine with moderate difficulty");
    WriteCenterContent("[2] I'm the human calculator. Test my boundaries");
    string? difficultySelection = ReadLineCenter();

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
    Console.Clear();
    int counter = 0;
    WriteCenterContent("Select an operator you want to try:\n", 'a');
    WriteCenterContent($"[0] Plus");
    WriteCenterContent($"[1] Minus");
    WriteCenterContent($"[2] Multiply");
    WriteCenterContent($"[3] Divide");
    WriteCenterContent($"[4] Random");
    string? operatorSymbolSelector = ReadLineCenter();
    Console.Clear();
    string operatorSymbol = CheckAndReturnOperator(operatorSymbolSelector);
    stopwatch.Start();
    while(true)
    {
        var questionAnswer = GenerateQuestion(difficultyLevel, operatorSymbolSelector != "4" ? operatorSymbol : RandomOperator());
        QuestionList.Add(questionAnswer.Item1);
        AnswerList.Add(questionAnswer.Item2);
        WriteCenterContent($"What is {questionAnswer.Item1}?", 'a');
        int userAnswer = GetIntegerAnswer();
        SavedAnswersList.Add(userAnswer);
        counter++;
        if (counter < 10)
            continue;
        break;
    }
    CheckAnswers();

    AskToContinue();
    
}


// This function makes sure that the answer that is being entered is actually a number
int GetIntegerAnswer()
{
    int userAnswer;
    while(true)
    {
        string? givenAnswer = ReadLineCenter();
        if (givenAnswer == null || givenAnswer.Trim() == "" || !int.TryParse(givenAnswer, out _))
        {
            WriteCenterContent("Are you high? At least enter a number");
            continue;

        }
        userAnswer = int.Parse(givenAnswer);
        Console.Clear();
        break;

    }
    return userAnswer;

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
(string,int) GenerateQuestion(int difficultyLevel, string operatorSymbol)
{
    var operands = GenerateOperands(difficultyLevel);
    int answer;
    switch (operatorSymbol)
    {
        case "+":
            answer = Operators.Addition(operands.Item1, operands.Item2);
            break;
        case "-":
            answer = Operators.Subtraction(operands.Item1, operands.Item2);       
            break;
        case "*":
            answer = Operators.Multiplication(operands.Item1, operands.Item2);
            break;
        case "/":
            while (operands.Item2 == 0 || operands.Item1 % operands.Item2 != 0)
            {
                operands = GenerateOperands(difficultyLevel);
            }
            answer = Operators.Division(operands.Item1, operands.Item2);
            break;
        default:
            throw new ArgumentOutOfRangeException($"operator symbol '{operatorSymbol}' is not supported.");
    }
    string expression = $"{operands.Item1} {operatorSymbol} {operands.Item2}";
    return (expression, answer);
}


// This function deals with asking whether to continue or quit.
void AskToContinue()
{
    Console.WriteLine("\n\n\n");
    WriteCenterContent("[0] Do you want to start the game again?");
    WriteCenterContent("[1] See the history of previous games\n");
    WriteCenterContent("Press anything else to quit");
    var optionSelection = ReadLineCenter();
    Console.Clear();
    if (optionSelection == "0")
    {
        int difficultyLevel = SelectDifficulty();
        StartGame(difficultyLevel);
    }else if (optionSelection == "1"){
        DisplayHistory();
    }else
    {
        Environment.Exit(0);
    }
}


// This is used to check and display the scores and corrections to the operations after each game.
void CheckAnswers()
{
    stopwatch.Stop();
    TimeSpan elapsed = stopwatch.Elapsed;
    stopwatch.Reset();
    TimeElapsedString = $"{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
    WriteCenterContent("--------------------------------------------------------------------------------------------------------");
    WriteCenterContent("GAME SCORE");
    WriteCenterContent("--------------------------------------------------------------------------------------------------------\n\n");
    WriteCenterContent($"You solved the questions in {TimeElapsedString}");
    TimeElapsedList.Add(TimeElapsedString);
    int count = 0;
    for (int i = 0; i < AnswerList.Count; i++)
    {
        if (AnswerList[i] == SavedAnswersList[i])
            count++;
        else
        {
            WriteCenterContent($"{QuestionList[i]} is not {SavedAnswersList[i]} it is {AnswerList[i]}");
        }
    }
    WriteCenterContent($"You have answered {count} questions correctly.");
    WriteCenterContent("--------------------------------------------------------------------------------------------------------\n\n");
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
    WriteCenterContent("--------------------------------------------------------------------------------------------------------");
    WriteCenterContent("GAME HISTORY");
    WriteCenterContent("--------------------------------------------------------------------------------------------------------");
        
    for (int i=0; i<QuestionHistory.Count; i++)
    {
        if (i%10 == 0)
        {
            gameCount++;
            Console.WriteLine();
            WriteCenterContent($"Game {gameCount}");
            WriteCenterContent("...........\n");
            WriteCenterContent($"You took {TimeElapsedList[gameCount - 1]} to complete this game.");
            WriteCenterContent($"You answered {CorrectAnswersPerGame[gameCount -1]} questions correctly in this game.");
        }
        if (AnswerHistory[i] == UserAnswerHistory[i])
            correctAnswerCount++;
        else
        {
            WriteCenterContent($"{QuestionHistory[i]} is not {UserAnswerHistory[i]} it's {AnswerHistory[i]}");
        }
    }
    WriteCenterContent("----------------------------------------------------------------------------------------------------------------------");
    WriteCenterContent($"You have answered {correctAnswerCount} questions correctly in total {gameCount} games.");

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