using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Capnp.Net.Runtime.Tests;

public class DecisionTree
{
    private readonly List<bool> _decisions = new();
    private readonly Stack<int> _freezeStack = new();
    private readonly ILogger Logger = Logging.CreateLogger<DecisionTree>();
    private int _pos,
        _freezeCounter;

    public DecisionTree()
    {
        _freezeStack.Push(0);
    }

    public DecisionTree(params bool[] initialDecisions)
        : this()
    {
        _decisions.AddRange(initialDecisions);
    }

    public bool MakeDecision()
    {
        if (_pos >= _decisions.Count)
            _decisions.Add(false);

        try
        {
            return _decisions[_pos++];
        }
        catch (ArgumentOutOfRangeException)
        {
            Logger.LogError($"WTF?! {_pos - 1}, {_decisions.Count}");
            throw;
        }
    }

    public void Freeze()
    {
        _freezeStack.Push(_pos);
    }

    public bool NextRound()
    {
        _pos = 0;

        for (var i = 0; i < _freezeCounter; i++)
            _freezeStack.Pop();

        while (_freezeStack.Count > 1)
        {
            var end = _freezeStack.Pop();
            var begin = _freezeStack.Peek();

            for (var i = end - 1; i >= begin; i--)
                if (_decisions[i] == false)
                {
                    _decisions[i] = true;
                    _freezeStack.Clear();
                    _freezeStack.Push(0);
                    return true;
                }

            //else
            //{
            //    _decisions.RemoveAt(i);
            //}
            ++_freezeCounter;
        }

        return false;
    }

    public override string ToString()
    {
        return "[" + string.Join("|", _decisions) + "]";
    }

    public void Iterate(Action testMethod)
    {
        Logger.LogInformation("Starting decision-tree based combinatorial test");
        var iter = 0;
        do
        {
            Logger.LogInformation($"Iteration {iter}: pattern {ToString()}");
            testMethod();
            ++iter;
        } while (NextRound());
    }
}
