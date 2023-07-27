using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

// useful shapes [light shade ░, medium shade ▒, dark shade ▓]

namespace ConsoleApp1.NewFolder;

internal class Maze
{
    public Player Player;

    public char[][] Board;

    public Dictionary<(int Row, int Column), char> MazeData = new Dictionary<(int Column, int Row), char>();

    public int Height { get; set; }
    public int Width { get; set; }

    public char _goalChar = 'E';
    public (int Row, int Column) _goalPossition;

    public char _startChar = 'S';
    public (int Row, int Column) _startPossition;

    public (int Row, int Column)[] _corners => new (int Column, int Row)[4];

    public Maze(int height, int width)
    {
        // set height and width

        Height = height;
        Width = width;
        
        // Create the board

        Board = new char[height][];

        for (int i = 0; i < height; i++)
        {
            char[] row = new char[width];

            for (int j = 0; j < row.Length; j++)
                row[j] = '░';
            Board[i] = row;
        }        

        _goalPossition = (0, width - 1);
        Board[_goalPossition.Row][_goalPossition.Column] = _goalChar;

        // set maze data
        int currentRow = 0;
        int currentCol = 0;

        foreach (var row in Board)
        {
            foreach(var c in row)
            {
                MazeData.Add((currentRow, currentCol), c);
                currentCol++;
            }
            currentRow++;
            currentCol = 0;
        }

        // create the player

        _startPossition = (height - 1, 0);
        Player = new(_startPossition, "Harry", null, MazeData);
        Board[_startPossition.Row][_startPossition.Column] = Player.Appearance;
        
        // set corners
        _corners[0] = (0, 0);
        _corners[1] = (height, 0);
        _corners[2] = (height, width);
        _corners[3] = (0, width);
    }

    public void PrintCurrentState()
    {
        Console.WriteLine(new string('▓', Board.First().Length + 2));     // Top border

        foreach (char[] s in Board)
        {
            Console.Write('▓');                                           // Left border
            Console.Write(new string(s));                                 // Row contents
            Console.Write("▓\n");                                         // Right border
        }
        
        Console.WriteLine(new string('▓', Board.First().Length + 2));     // Bottom border
    }
}
