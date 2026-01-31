using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Iron Henry/LevelData")]
public class LevelData : ScriptableObject
{
    public Notes[] notes;
    public Difficulties levelDifficulty;
    public int frogCount = 1;

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

    public int[] GetPetalIndices()
    {
        return petalIndices.ToArray();
    }
}

public enum Notes
{
    C_MAJ  =  0,
    DB_MAJ =  7,
    D_MAJ  =  2,
    EB_MAJ =  9,
    E_MAJ  =  4,
    F_MAJ  = 11,
    GB_MAJ =  6,
    G_MAJ  =  1,
    AB_MAJ =  8,
    A_MAJ  =  3,
    BB_MAJ = 10,
    B_MAJ  =  5,

    C_MIN = 21,
    DB_MIN = 16,
    D_MIN = 23,
    EB_MIN = 18,
    E_MIN = 13,
    F_MIN = 20,
    GB_MIN = 15,
    G_MIN = 22,
    AB_MIN = 17,
    A_MIN = 12,
    BB_MIN = 19,
    B_MIN = 14

}

public enum Difficulties
{
    SINGLE, MAJOR, MINOR, MIXED 
}