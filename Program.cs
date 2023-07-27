using ConsoleApp1.Models;
using ConsoleApp1.NewFolder;
using ConsoleApp1.Sturctures;
using System.Diagnostics;

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

        var timer = new Stopwatch();

        MoveTree moves = new MoveTree(new Node<Direction>(Direction.____), PlayArea.MazeData);

        timer.Start();

        moves.GenerateMoves(140, PlayArea.Player.CurrentPosition, PlayArea._goalPossition);

        Console.WriteLine(timer.Elapsed);

        timer.Restart();

        Console.WriteLine(timer.Elapsed);

        moves.PrintPath(moves._cache[PlayArea._goalPossition]);
        //moves.PrintPaths(allPossiblePaths.ToList());

        ////////////////////////////////////////
        ///  Start Main Loop                 ///
        ////////////////////////////////////////

        var pathToGoal = moves._cache[PlayArea._goalPossition];

        foreach (var direction in pathToGoal)
        {
            PlayArea.PrintCurrentState();

        Move:
            PlayArea.Player.Move(direction);

            try
            {
                if (PlayArea.Board[PlayArea.Player.CurrentPosition.Row][PlayArea.Player.CurrentPosition.Column] == '░')
                {
                    PlayArea.Board
                        [PlayArea.Player.PreviousPosition.Row]
                        [PlayArea.Player.PreviousPosition.Column]
                        = '0';

                    PlayArea.Board
                        [PlayArea.Player.CurrentPosition.Row]
                        [PlayArea.Player.CurrentPosition.Column]
                        = PlayArea.Player.Appearance;
                }
                else if (PlayArea.Board[PlayArea.Player.CurrentPosition.Row][PlayArea.Player.CurrentPosition.Column] == PlayArea._goalChar)
                {
                    PlayArea.Board
                        [PlayArea.Player.PreviousPosition.Row]
                        [PlayArea.Player.PreviousPosition.Column]
                        = '0';

                    PlayArea.Board
                        [PlayArea.Player.CurrentPosition.Row]
                        [PlayArea.Player.CurrentPosition.Column]
                        = PlayArea.Player.Appearance;

                    Console.Clear();
                    PlayArea.PrintCurrentState();

                    Console.WriteLine($"Success!\nGoal Reached in {pathToGoal.Count()} moves");
                    goto End;
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
            Thread.Sleep(300);
            Console.Clear();
        }
    End:
        Console.ReadLine();
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