using ConsoleApp1.Models;

namespace ConsoleApp1.Sturctures;

/// <summary>
/// This class represents all possible moves.
/// The tree contains nodes, all of which should have exactly 4 or 0 children.
/// </summary>
public class MoveTree
{
    public Node<Direction> Root { get; set; }

    public Dictionary<(int Row, int Column), char> _mazeData;

    public (int Row, int Column) _PlayerPosition;

    public Dictionary<(int Row, int Column), IEnumerable<Direction>> _cache = new() { [(19, 0)] = new List<Direction> { Direction.____ } };

    // thread properties
    public byte threadsCompleted = 0;

    public MoveTree(Node<Direction> root, Dictionary<(int Row, int Column), char> mazeData)
    {
        Root = root;
        _mazeData = mazeData;
    }

    public void GenerateMoves(int depth, (int Row, int Column) playerPosition, (int Row, int Column) endPosition)
    {
        var currentNode = Root;

        var XDifference = endPosition.Column - playerPosition.Column;
        var YDifference = endPosition.Row - playerPosition.Row;

        _PlayerPosition = playerPosition;

        if (depth == 0)
            return;
        if (XDifference == 0 && YDifference == 0)
            return;

        currentNode.Next.Add(new Node<Direction>(currentNode, Direction.Left));
        currentNode.Next.Add(new Node<Direction>(currentNode, Direction.Right));
        currentNode.Next.Add(new Node<Direction>(currentNode, Direction.Up));
        currentNode.Next.Add(new Node<Direction>(currentNode, Direction.Down));

        Node<Direction> leftbranch = currentNode.Next[0];
        Node<Direction> rightbranch = currentNode.Next[1];
        Node<Direction> upbranch = currentNode.Next[2];
        Node<Direction> downbranch = currentNode.Next[3];

        GenerateMoves(
            depth - 1,
            leftbranch,
            new List<Direction>(),
            XDifference + TranslateXDirection(leftbranch.Value),
            YDifference + TranslateYDirection(leftbranch.Value));

        GenerateMoves(
            depth - 1,
            rightbranch,
            new List<Direction>(),
            XDifference + TranslateXDirection(rightbranch.Value),
            YDifference + TranslateYDirection(rightbranch.Value));

        GenerateMoves(
            depth - 1,
            upbranch,
            new List<Direction>(),
            XDifference + TranslateXDirection(upbranch.Value),
            YDifference + TranslateYDirection(upbranch.Value));

        GenerateMoves(
            depth - 1,
            downbranch,
            new List<Direction>(),
            XDifference + TranslateXDirection(downbranch.Value),
            YDifference + TranslateYDirection(downbranch.Value));
    }


    public void GenerateMoves(int depth, Node<Direction> currentNode, List<Direction> path, int XDifference, int YDifference)
    {
        // End of the line
        if (depth == 0)
        {
            currentNode.Previous!.Next = new();
            return;
        }

        path.Add(currentNode.Value);

        if (!ValidPath(path) || IsInCache(path))
        {
            // If the path that lead here is invalid, or this position this path ends at has already been reached, remove this branch of the path
            currentNode.Previous!.Next.Remove(
                currentNode.Previous!.Next.SingleOrDefault(
                    x => x.Value == currentNode.Value)!);
            return;
        }

        if (XDifference == 0 && YDifference == 0)
            return;

        currentNode.Next.Add(new Node<Direction>(currentNode, Direction.Left));
        currentNode.Next.Add(new Node<Direction>(currentNode, Direction.Right));
        currentNode.Next.Add(new Node<Direction>(currentNode, Direction.Up));
        currentNode.Next.Add(new Node<Direction>(currentNode, Direction.Down));

        var leftbranch = currentNode.Next[0];
        var rightbranch = currentNode.Next[1];
        var upbranch = currentNode.Next[2];
        var downbranch = currentNode.Next[3];

        GenerateMoves(
            depth - 1,
            leftbranch,
            new List<Direction>(path),
            XDifference + TranslateXDirection(leftbranch.Value),
            YDifference + TranslateYDirection(leftbranch.Value));

        GenerateMoves(
            depth - 1,
            rightbranch,
            new List<Direction>(path),
            XDifference + TranslateXDirection(rightbranch.Value),
            YDifference + TranslateYDirection(rightbranch.Value));

        GenerateMoves(
            depth - 1,
            upbranch,
            new List<Direction>(path),
            XDifference + TranslateXDirection(upbranch.Value),
            YDifference + TranslateYDirection(upbranch.Value));

        GenerateMoves(
            depth - 1,
            downbranch,
            new List<Direction>(path),
            XDifference + TranslateXDirection(downbranch.Value),
            YDifference + TranslateYDirection(downbranch.Value));
    }


    /// <summary>
    /// Checks that the path that lead to this node does not collide with a wall or fall off the board.
    /// </summary>
    /// <param name="path">Path leading to this node.</param>
    /// <param name="newDirection">This nodes value.</param>
    /// <returns></returns>
    public bool ValidPath(List<Direction> path)
    {
        var newPlayerPosition = _PlayerPosition;

        foreach (var d in path)
        {
            switch (d)
            {
                case Direction.Left:
                    newPlayerPosition.Column -= 1;
                    break;
                case Direction.Right:
                    newPlayerPosition.Column += 1;
                    break;
                case Direction.Up:
                    newPlayerPosition.Row -= 1;
                    break;
                case Direction.Down:
                    newPlayerPosition.Row += 1;
                    break;
            }

            if (_mazeData.ContainsKey(newPlayerPosition) == false)
                return false;
            else if (_mazeData[newPlayerPosition] == '▒')
                return false;
        }
        return true;
    }

    /// <summary>
    /// Checks that the path that lead to this node does not collide with a wall or fall off the board.
    /// </summary>
    /// <param name="path">Path leading to this node.</param>
    /// <param name="newDirection">This nodes value.</param>
    /// <returns></returns>
    public bool IsInCache(List<Direction> path)
    {
        var newPlayerPosition = _PlayerPosition;

        foreach (var d in path)
        {
            switch (d)
            {
                case Direction.Left:
                    newPlayerPosition.Column -= 1;
                    break;
                case Direction.Right:
                    newPlayerPosition.Column += 1;
                    break;
                case Direction.Up:
                    newPlayerPosition.Row -= 1;
                    break;
                case Direction.Down:
                    newPlayerPosition.Row += 1;
                    break;
            }
        }
        if (_cache.ContainsKey(newPlayerPosition))
        {
            if (_cache[newPlayerPosition].Count() <= path.Count())
                return true;
            else
            {
                _cache[newPlayerPosition] = path;
                return false;
            }
        }
        else
        {
            _cache.Add(newPlayerPosition, path);
            return false;
        }
    }

    public int TranslateXDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Left:
                return -1;
            case Direction.Right:
                return 1;
            case Direction.Up:
                return 0;
            case Direction.Down:
                return 0;
        }
        return 0;
    }

    public int TranslateYDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Left:
                return 0;
            case Direction.Right:
                return 0;
            case Direction.Up:
                return -1;
            case Direction.Down:
                return 1;
        }
        return 0;
    }

    public virtual IEnumerable<Direction> GetAllPossiblePaths(Node<Direction> node, List<Direction> path, bool first = false)
    {
        if (node == null)
        {
            yield break;
        }

        /* append this node to the path array */
        if (!first)
        {
            path.Add(node.Value);
        }

        /* it's a leaf, so print the path that lead to here  */
        if (node.Next.Count == 0)
        {
            path.Add(Direction.____);

            foreach (var p in path)
                yield return p;
        }
        else
        {
            /* otherwise try all subtrees */
            foreach (var n in node.Next)
            {
                var nl = new List<Direction>(path);

                var result = GetAllPossiblePaths(n, nl);

                foreach (var p in result)
                    yield return p;
            }
        }
    }

    public virtual void PrintPaths(List<Direction> path)
    {
        foreach (var p in path)
            if (p != Direction.____)
                Console.Write(p.ToString() + " ");
            else
                Console.Write("\n");
    }

    public virtual void PrintPaths(IEnumerable<IEnumerable<Direction>> path)
    {
        foreach (var p in path)
            foreach (var d in p)
                if (d != Direction.____)
                    Console.Write(d.ToString() + " ");
                else
                    Console.Write("\n");
    }

    public virtual void PrintPath(IEnumerable<Direction> path)
    {
        foreach (var p in path)
            Console.Write(p.ToString() + " ");

        Console.Write("\n");
    }

    public void PrintTreeData(IEnumerable<Direction> paths)
    {
        Console.WriteLine($"Number of Paths: {_cache.Count()}");
    }
}
