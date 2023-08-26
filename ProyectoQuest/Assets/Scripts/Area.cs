
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Area
{
    public enum Name { Disign, Biology, Nursing}
    public enum Stat { Logic, TeamWork, SocialSkill}

    public Name name = Name.Disign;
    public int affinity = 0;

    [Space(10)]
    public List<Stat> stats;
}
