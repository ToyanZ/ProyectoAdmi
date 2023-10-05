
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

    public Name aName = Name.Administracion;
    public int affinity = 0;
    public RectTransform relatedCareers;
    public Button creerButton;
    public Bar affinityBar;

    public override float GetMaxValue() { return GameManager.instance.affinityPointMax; }
    public override float GetCurrentValue() { return affinity; }
    public override bool GetBoolValue() { return false; }
    public override string GetStringValue() { return aName.ToString(); }
}
