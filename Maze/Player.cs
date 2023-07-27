using ConsoleApp1.Models;
using ConsoleApp1.Sturctures;

namespace ConsoleApp1.NewFolder;

public class Player
{
    public string Name { get; set; } = "Player";
    public char Appearance { get; set; } = 'P';
    public (int Column, int Row) CurrentPosition { get; set; }
    public (int Column, int Row) PreviousPosition { get; set; }

    public Dictionary<(int Column, int Row), char> MazeData { get; set;}

    public MoveTree PossibleMoves;
    
    public Player((int, int) currentPosition, string? name, char? appearance, Dictionary<(int Column, int Row), char> mazeData)
    {
        CurrentPosition = currentPosition;

        if (!string.IsNullOrEmpty(name))
            Name = name;

        if (appearance != null)
            Appearance = (char)appearance;

        MazeData = mazeData;
        PossibleMoves = new MoveTree(new Node<Direction>(Direction.____), MazeData);
    }

    public void Move()
    {
        var a = Console.ReadKey(true);

        ConsoleKey[] acceptedInput = new ConsoleKey[]
        {
            ConsoleKey.UpArrow,
            ConsoleKey.DownArrow,
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
        };

        while (!acceptedInput.Contains(a.Key))
            a = Console.ReadKey(true);

        PreviousPosition = CurrentPosition;

        switch (a.Key)
        {
            case ConsoleKey.UpArrow:
                CurrentPosition = (CurrentPosition.Column - 1, CurrentPosition.Row);
                break;
            case ConsoleKey.DownArrow:
                CurrentPosition = (CurrentPosition.Column + 1, CurrentPosition.Row);
                break;
            case ConsoleKey.LeftArrow:
                CurrentPosition = (CurrentPosition.Column, CurrentPosition.Row - 1);
                break;
            case ConsoleKey.RightArrow:
                CurrentPosition = (CurrentPosition.Column, CurrentPosition.Row + 1);
                break;
        }
    }

    //public void UpdateCurrentPossibleMoves(int depth = 5)
    //{
    //    PossibleMoves = new MoveTree(new Node<Direction>(Direction.Left));

    //    PossibleMoves.AddNoFilterDepth(depth);
    //}
}