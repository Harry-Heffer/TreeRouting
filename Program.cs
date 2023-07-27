using ConsoleApp1.Models;
using ConsoleApp1.NewFolder;
using ConsoleApp1.Sturctures;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Transactions;

internal class Program
{
    private static void Main(string[] args)
    {
        ////////////////////////////////////////
        ///  Set Console Options             ///
        ////////////////////////////////////////
        
        Console.ForegroundColor = ConsoleColor.Green;

        ////////////////////////////////////////
        ///  Instantiate Play Area           ///
        ////////////////////////////////////////

        Maze PlayArea = new Maze(20, 40);

        ////////////////////////////////////////
        ///  Testing Area                    ///
        ////////////////////////////////////////

        foreach (var c in PlayArea._corners)
            Console.WriteLine(c);

        var timer = new Stopwatch();

        MoveTree AllPossibleMoves = new MoveTree(new Node<Direction>(Direction.____), PlayArea.MazeData);

        timer.Start();

        AllPossibleMoves.AddFilterDepthMultiThread(14, PlayArea.Player.CurrentPosition, PlayArea._goalPossition);

        Console.WriteLine(timer.Elapsed);
        
        List<Direction> path = new List<Direction>();

        timer.Restart();

        var moves = AllPossibleMoves.PrintPathsRecursive(AllPossibleMoves.Root, path, true);

        Console.WriteLine(timer.Elapsed);

        //timer.Restart();

        //AllPossibleMoves.PrintTreeData(moves);

        //Console.WriteLine(timer.Elapsed);

        //AllPossibleMoves.PrintList(moves.ToList());

        ////////////////////////////////////////
        ///  Start Main Loop                 ///
        ////////////////////////////////////////

        bool active = true;

        while (active)
        {
            PlayArea.PrintCurrentState();

            // Wait for the player to move.
            Move:
            PlayArea.Player.Move();

            try
            {
                if (PlayArea.Board[PlayArea.Player.CurrentPosition.Column][PlayArea.Player.CurrentPosition.Row] == '░')
                {
                    PlayArea.Board
                        [PlayArea.Player.PreviousPosition.Column]
                        [PlayArea.Player.PreviousPosition.Row]
                        = '░';

                    PlayArea.Board
                        [PlayArea.Player.CurrentPosition.Column]
                        [PlayArea.Player.CurrentPosition.Row]
                        = PlayArea.Player.Appearance;
                }
                else
                {
                    Console.WriteLine($"Can't Move there! {PlayArea.Player.CurrentPosition}");
                    PlayArea.Player.CurrentPosition = PlayArea.Player.PreviousPosition;
                    goto Move;
                }
            }
            catch
            {
                Console.WriteLine($"Can't Move there! {PlayArea.Player.CurrentPosition}");
                PlayArea.Player.CurrentPosition = PlayArea.Player.PreviousPosition;
                goto Move;
            }
            Console.Clear();
        }

        

        Console.ReadLine();
    }
}