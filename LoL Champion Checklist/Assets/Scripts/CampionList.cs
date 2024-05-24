using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChampionList", menuName = "ScriptableObjects/ChampionList", order = 1)]
public class ChampionList : ScriptableObject
{
    public List<Champion> champions;
}