﻿using ConsoleApp1.Models;

namespace ConsoleApp1.NewFolder;

public class Player
{
    public string Name { get; set; } = "Player";
    public char Appearance { get; set; } = 'P';
    public (int Row, int Column) CurrentPosition { get; set; }
    public (int Row, int Column) PreviousPosition { get; set; }

    public Player((int, int) currentPosition, string? name, char? appearance)
    {
        CurrentPosition = currentPosition;

        if (!string.IsNullOrEmpty(name))
            Name = name;

        if (appearance != null)
            Appearance = (char)appearance;
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

    public void Move(Direction direction)
    {
        PreviousPosition = CurrentPosition;

        switch (direction)
        {
            case Direction.Left:
                CurrentPosition = (CurrentPosition.Row, CurrentPosition.Column - 1);
                break;
            case Direction.Right:
                CurrentPosition = (CurrentPosition.Row, CurrentPosition.Column + 1);
                break;
            case Direction.Up:
                CurrentPosition = (CurrentPosition.Row - 1, CurrentPosition.Column);
                break;
            case Direction.Down:
                CurrentPosition = (CurrentPosition.Row + 1, CurrentPosition.Column);
                break;
        }
    }

    //public void UpdateCurrentPossibleMoves(int depth = 5)
    //{
    //    PossibleMoves = new MoveTree(new Node<Direction>(Direction.Left));

    //    PossibleMoves.AddNoFilterDepth(depth);
    //}
}