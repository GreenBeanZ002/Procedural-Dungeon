//This script was initially made using content shown in this video:https://www.youtube.com/watch?v=LRYfcTKpUhI&list=PLcRSafycjWFenI87z7uZHFv6cUG2Tzu9v&index=8
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WalkParameters_" , menuName ="RandomWalkDataMenu")]
public class RandomWalkData : ScriptableObject
{
    public int iterations = 10, pathLength = 10;
    public bool randomStart = true;
}
