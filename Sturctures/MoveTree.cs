using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;

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


    public MoveTree(Node<Direction> root, Dictionary<(int Column, int Row), char> mazeData)
    {
        Root = root;
        _mazeData = mazeData;
    }

    public void AddNoFilterDepth(int depth)
    {
        var currentNode = Root;

        if (depth > 0)
        {
            if (currentNode.Next.Count != 0)
            {
                AddNoFilterDepth(depth - 1, currentNode.Next[0]);
                AddNoFilterDepth(depth - 1, currentNode.Next[1]);
                AddNoFilterDepth(depth - 1, currentNode.Next[2]);
                AddNoFilterDepth(depth - 1, currentNode.Next[3]);
            }
            else
            {
                currentNode.Next.Add(new Node<Direction>(Direction.Left));
                currentNode.Next.Add(new Node<Direction>(Direction.Right));
                currentNode.Next.Add(new Node<Direction>(Direction.Up));
                currentNode.Next.Add(new Node<Direction>(Direction.Down));

                AddNoFilterDepth(depth - 1, currentNode.Next[0]);
                AddNoFilterDepth(depth - 1, currentNode.Next[1]);
                AddNoFilterDepth(depth - 1, currentNode.Next[2]);
                AddNoFilterDepth(depth - 1, currentNode.Next[3]);
            }
        }
    }

    /// <summary>
    /// Adds depths for a non-binary tree where each node has 4 children.
    /// </summary>
    /// <param name="depth"></param>
    /// <param name="currentNode"></param>
    public void AddNoFilterDepth(int depth, Node<Direction> currentNode)
    {
        if (depth > 0)
        {
            if (currentNode.Next.Count != 0)
            {
                AddNoFilterDepth(depth - 1, currentNode.Next[0]);
                AddNoFilterDepth(depth - 1, currentNode.Next[1]);
                AddNoFilterDepth(depth - 1, currentNode.Next[2]);
                AddNoFilterDepth(depth - 1, currentNode.Next[3]);
            }
            else
            {
                currentNode.Next.Add(new Node<Direction>(Direction.Left));
                currentNode.Next.Add(new Node<Direction>(Direction.Right));
                currentNode.Next.Add(new Node<Direction>(Direction.Up));
                currentNode.Next.Add(new Node<Direction>(Direction.Down));

                AddNoFilterDepth(depth - 1, currentNode.Next[0]);
                AddNoFilterDepth(depth - 1, currentNode.Next[1]);
                AddNoFilterDepth(depth - 1, currentNode.Next[2]);
                AddNoFilterDepth(depth - 1, currentNode.Next[3]);
            }
        }
    }

    public void AddFilterDepth(int depth, (int Row, int Column) playerPosition, (int Row, int Column) endPosition)
    {
        var currentNode = Root;

        var XDifference = endPosition.Column - playerPosition.Column;
        var YDifference = endPosition.Row - playerPosition.Row;

        _PlayerPosition = playerPosition;

        List<Direction> path = new List<Direction>();

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

        AddFilterDepth(
            depth - 1,
            leftbranch,
            new List<Direction>(),
            XDifference + TranslateXDirection(leftbranch.Value),
            YDifference + TranslateYDirection(leftbranch.Value));

        AddFilterDepth(
            depth - 1,
            rightbranch,
            new List<Direction>(),
            XDifference + TranslateXDirection(rightbranch.Value),
            YDifference + TranslateYDirection(rightbranch.Value));

        AddFilterDepth(
            depth - 1,
            upbranch,
            new List<Direction>(),
            XDifference + TranslateXDirection(upbranch.Value),
            YDifference + TranslateYDirection(upbranch.Value));

        AddFilterDepth(
            depth - 1,
            downbranch,
            new List<Direction>(),
            XDifference + TranslateXDirection(downbranch.Value),
            YDifference + TranslateYDirection(downbranch.Value));
    }

    public void AddFilterDepthMultiThread(int depth, (int Row, int Column) playerPosition, (int Row, int Column) endPosition)
    {
        var currentNode = Root;

        var XDifference = endPosition.Column - playerPosition.Column;
        var YDifference = endPosition.Row - playerPosition.Row;

        _PlayerPosition = playerPosition;

        List<Direction> path = new List<Direction>();

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
        
        Thread a = new(this.AddfilterDepthThreadStartObj);
        Thread b = new(this.AddfilterDepthThreadStartObj);
        Thread c = new(this.AddfilterDepthThreadStartObj);
        Thread d = new(this.AddfilterDepthThreadStartObj);

        a.Start((
            depth - 1,
            leftbranch,
            new List<Direction>(),
            XDifference + TranslateXDirection(leftbranch.Value),
            YDifference + TranslateYDirection(leftbranch.Value)));

        b.Start((
            depth - 1,
            rightbranch,
            new List<Direction>(),
            XDifference + TranslateXDirection(rightbranch.Value),
            YDifference + TranslateYDirection(rightbranch.Value)));

        c.Start((
            depth - 1,
            upbranch,
            new List<Direction>(),
            XDifference + TranslateXDirection(upbranch.Value),
            YDifference + TranslateYDirection(upbranch.Value)));

        d.Start((
            depth - 1,
            downbranch,
            new List<Direction>(),
            XDifference + TranslateXDirection(downbranch.Value),
            YDifference + TranslateYDirection(downbranch.Value)));
    }
    

    public async void AddFilterDepth(int depth, Node<Direction> currentNode, List<Direction> path, int XDifference, int YDifference)
    {
        path.Add(currentNode.Value);
        
        if (depth == 0)
            return;

        if (!ValidPath(path))
        {
            // If the path that lead here is invalid, remove this branch of the path
            await RemoveCurrentBranchAsync(currentNode);
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
        
        AddFilterDepth(
            depth - 1, 
            leftbranch, 
            new List<Direction>(path), 
            XDifference + TranslateXDirection(leftbranch.Value), 
            YDifference + TranslateYDirection(leftbranch.Value));

        AddFilterDepth(
            depth - 1,
            rightbranch,
            new List<Direction>(path),
            XDifference + TranslateXDirection(rightbranch.Value),
            YDifference + TranslateYDirection(rightbranch.Value));

        AddFilterDepth(
            depth - 1, 
            upbranch, 
            new List<Direction>(path), 
            XDifference + TranslateXDirection(upbranch.Value), 
            YDifference + TranslateYDirection(upbranch.Value));

        AddFilterDepth(
            depth - 1,
            downbranch,
            new List<Direction>(path),
            XDifference + TranslateXDirection(downbranch.Value),
            YDifference + TranslateYDirection(downbranch.Value));
    }

    private static async Task RemoveCurrentBranchAsync(Node<Direction> currentNode)
    {
        currentNode.Previous!.Next.Remove(
            currentNode.Previous!.Next.SingleOrDefault(
                x => x.Value == currentNode.Value)!);
    }

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

    public void AddfilterDepthThreadStart((
        int depth,
        Node<Direction> branch,
        List<Direction> path,
        int XDifference,
        int YDifference) parameters)
    {
        this.AddFilterDepth(
            parameters.depth - 1,
            parameters.branch,
            new List<Direction>(),
            parameters.XDifference + this.TranslateXDirection(parameters.branch.Value),
            parameters.YDifference + this.TranslateYDirection(parameters.branch.Value));
    }

    public void AddfilterDepthThreadStartObj(object? obj)
    {
        (int depth, Node<Direction> branch, List<Direction> path, int XDifference, int YDifference) parameters = ((int, Node<Direction>, List<Direction>, int, int))obj!;
        
        this.AddFilterDepth(
            parameters.depth - 1,
            parameters.branch,
            new List<Direction>(),
            parameters.XDifference + this.TranslateXDirection(parameters.branch.Value),
            parameters.YDifference + this.TranslateYDirection(parameters.branch.Value));
    }

    public virtual IEnumerable<Direction> PrintPathsRecursive(Node<Direction> node, List<Direction> path, bool first = false)
    {
        if (node == null)
        {
			yield break;
        }

		/* append this node to the path array */
		if(!first)
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

				var result = PrintPathsRecursive(n, nl);

				foreach(var p in result) 
					yield return p;
            }
        }
    }

    public virtual void PrintList(List<Direction> path)
    {
		foreach (var p in path)
			if (p != Direction.____)
				Console.Write(p.ToString() + " ");
			else
				Console.Write("\n");
    }

	public void PrintTreeData(IEnumerable<Direction> paths)
	{
        var leafs = paths.Select(x => x).Where(x => x == Direction.____);
        Console.WriteLine($"Number of Paths: {leafs.Count() - 1}");
	}
}
