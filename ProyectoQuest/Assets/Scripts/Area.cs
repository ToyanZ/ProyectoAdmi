
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Area : StatElement
{
    public enum Name 
    { 
        Administracion,
        CienciasSociales, 
        Comunicacion,
        Educacion,
        Derecho,
        TurismoYGastronomia,
        Diseno,
        MedicinaVeterinaria,
        Odontologia,
        Salud,
        Informatica,
        Ingenieria,
        Deporte
    }
    public enum Stat { Logic, TeamWork, SocialSkill}

    public Name aName = Name.Administracion;
    public int affinity = 0;
    public RectTransform relatedCareers;



    [Space(10)]
    public List<Stat> stats;

    public override float GetMaxValue() { return GameManager.instance.affinityPointMax; }
    public override float GetCurrentValue() { return affinity; }
    public override bool GetBoolValue() { return false; }
    public override string GetStringValue() { return aName.ToString(); }
}
