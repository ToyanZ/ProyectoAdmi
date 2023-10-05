using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86;

public class Question : MonoBehaviour
{
    public bool answered = false;
    public string statement = "";
    public List<Answer> answers;
    public float answerTimeMod = 0;

    [Space(20)]
    public Enemy enemy;
    public int attackPoints = 0;
    public Bar completed;

    [Space(20)]
    [SerializeField]float time = 0;

    //Se llama desde trigger (editor)
    public void LoadPopUp()
    {
        InterfaceManager.instance.LoadQuestion(this);
    }
    public void SelectThisAnswer(string statement)
    {
        answered = true;
        
        Answer answer = answers.Find(answer => answer.statement == statement);

        foreach(Area area in InterfaceManager.instance.afinityAreas)
        {
            AddPointIfAnswerMatch(answer, area);
        }


        //GameManager.instance.UpdateAffinityPointCount();
        GameManager.instance.affinityPointMax += 1;
        //InterfaceManager.instance.OnAnswerSelected?.Invoke(); //Update HUD

        StartCoroutine(UpdateProgressBar());
    }
    IEnumerator UpdateProgressBar()
    {
        yield return new WaitForSeconds(0.2f);
        float time2 = 0;
        while (time2 < GameManager.instance.answerCompletedBarUpdateTime)
        {
            time2 += Time.deltaTime;
            completed.SimpleRefresh(Mathf.Clamp01(time2), 0.5f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


    //Modificar ya que cada respuesta tiene directamente las áres asociadas.
    void AddPointIfAnswerMatch(Answer answer, Area area)
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

    //Se llama desde trigger, eliminar.
    public void AnswerTimeCounter()
    {
        if (answered) return;
        time += Time.deltaTime;

        float max = GameManager.instance.answerTimeMax + answerTimeMod;
        InterfaceManager.instance.answerTimerBar.SimpleRefresh(time, max);

        float min = GameManager.instance.answerTimeMin + answerTimeMod;
        float avg = GameManager.instance.answerTimeAvg + answerTimeMod;
        attackPoints = time < min ? 1 : (time < avg ? 3 : 2);

        for (int i = 0; i < InterfaceManager.instance.attackPoints.Count; i++)
        {
            InterfaceManager.instance.attackPoints[i].enabled = i < attackPoints;
        }
    }

    

}
