using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObejctGridData", menuName = "ScriptableObjects/ScriptableObejctGridData")]
public class ScriptableObejctGridData : ScriptableObject
{
    public int width;
    public int height;
    public float cellSize;

}
