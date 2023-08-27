
using System.Collections.Generic;
using UnityEngine;

public class Area : StatElement
{
    public enum Name { Disign, Biology, Nursing}
    public enum Stat { Logic, TeamWork, SocialSkill}

    public Name aName = Name.Disign;
    public int affinity = 0;

    [Space(10)]
    public List<Stat> stats;

    public override float GetMaxValue() { return GameManager.instance.affinityPointMax; }
    public override float GetCurrentValue() { return affinity; }
    public override bool GetBoolValue() { return false; }
    public override string GetStringValue() { return ""; }
}
