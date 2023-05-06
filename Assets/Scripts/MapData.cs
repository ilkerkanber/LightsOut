using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "MapData")]

[Serializable]
public class MapData : ScriptableObject
{
    public int val;
    public List<int> MatrixData;
}
