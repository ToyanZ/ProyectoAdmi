using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    public bool answered = false;
    public string statement = "";
    public List<Answer> answers;
    

    public void LoadPopUp()
    {
        InterfaceManager.instance.LoadQuestion(this);
    }
    public void SelectAnswer(string statement)
    {
        Answer answer = answers.Find(answer => answer.statement == statement);

        foreach(Area area in GameManager.instance.character.player.target.areas)
        {
            for (int i = 0; i < answer.stats.Count; i++)
            {
                if (area.stats.Contains(answer.stats[i]))
                {
                    if (answer.affinityPoints.Count == 1)
                    {
                        area.affinity += answer.affinityPoints[0];
                    }
                    else
                    {
                        area.affinity += answer.affinityPoints[i];
                    }
                }
            }
        }
        answered = true;
        //GameManager.instance.UpdateAffinityPointCount();
        GameManager.instance.affinityPointMax += 1;
        InterfaceManager.instance.OnAnswerSelected?.Invoke(); //Update HUD
    }

}
