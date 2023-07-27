﻿// useful shapes [light shade ░, medium shade ▒, dark shade ▓]

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

        // Add walls, this is testing so positions are not generic

        // chicanery Layout

        Board[19][10] = '▒';
        Board[18][10] = '▒';
        Board[17][10] = '▒';
        Board[16][10] = '▒';
        Board[15][10] = '▒';
        Board[14][10] = '▒';
        Board[13][10] = '▒';
        Board[12][10] = '▒';
        Board[11][10] = '▒';
        Board[10][10] = '▒';
        Board[9][10] = '▒';
        Board[8][10] = '▒';
        Board[7][10] = '▒';
        Board[6][10] = '▒';
        Board[5][10] = '▒';
        Board[4][10] = '▒';
        Board[3][10] = '▒';
        Board[2][10] = '▒';
        Board[1][10] = '▒';

        Board[18][20] = '▒';
        Board[17][20] = '▒';
        Board[16][20] = '▒';
        Board[15][20] = '▒';
        Board[14][20] = '▒';
        Board[13][20] = '▒';
        Board[12][20] = '▒';
        Board[11][20] = '▒';
        Board[10][20] = '▒';
        Board[9][20] = '▒';
        Board[8][20] = '▒';
        Board[7][20] = '▒';
        Board[6][20] = '▒';
        Board[5][20] = '▒';
        Board[4][20] = '▒';
        Board[3][20] = '▒';
        Board[2][20] = '▒';
        Board[1][20] = '▒';
        Board[0][20] = '▒';

        Board[5][39] = '▒';
        Board[5][38] = '▒';
        Board[5][37] = '▒';
        Board[5][36] = '▒';
        Board[5][35] = '▒';
        Board[5][34] = '▒';
        Board[5][33] = '▒';
        Board[5][32] = '▒';
        Board[5][31] = '▒';
        Board[5][30] = '▒';
        Board[5][29] = '▒';
        Board[5][28] = '▒';
        Board[5][27] = '▒';
        Board[5][26] = '▒';
        Board[5][25] = '▒';
        Board[5][24] = '▒';
        Board[5][23] = '▒';
        Board[5][22] = '▒';

        Board[10][38] = '▒';
        Board[10][37] = '▒';
        Board[10][36] = '▒';
        Board[10][35] = '▒';
        Board[10][34] = '▒';
        Board[10][33] = '▒';
        Board[10][32] = '▒';
        Board[10][31] = '▒';
        Board[10][30] = '▒';
        Board[10][29] = '▒';
        Board[10][28] = '▒';
        Board[10][27] = '▒';
        Board[10][26] = '▒';
        Board[10][25] = '▒';
        Board[10][24] = '▒';
        Board[10][23] = '▒';
        Board[10][22] = '▒';
        Board[10][21] = '▒';

        // set maze data
        int currentRow = 0;
        int currentCol = 0;

        foreach (var row in Board)
        {
            foreach (var c in row)
            {
                MazeData.Add((currentRow, currentCol), c);
                currentCol++;
            }
            currentRow++;
            currentCol = 0;
        }

        // create the player

        _startPossition = (height - 1, 0);
        Player = new(_startPossition, "Harry", null);
        Board[_startPossition.Row][_startPossition.Column] = Player.Appearance;
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
