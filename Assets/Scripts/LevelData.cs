using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{
    public Notes[] notes;
    public Difficulties levelDifficulty;

    private List<int> petalIndices => CalculatePetalIndicesFromNotes();

    private List<int> CalculatePetalIndicesFromNotes()
    {
        List<int> petalIndices = new List<int>();

        foreach (var note in notes)
        {
            petalIndices.Add((int)note);
        }

        return petalIndices;
    }
}

public enum Notes
{
    C  =  0,
    DB =  7,
    D  =  2,
    EB =  9,
    E  =  4,
    F  = 11,
    GB =  6,
    G  =  1,
    AB =  8,
    A  =  3,
    BB = 10,
    B  =  5
}

public enum Difficulties
{
    SINGLE, CHORD 
}