using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86;

public class Question : MonoBehaviour
{
    public bool answered = false;
    public string statement = "";
    public Bar completed;

    [Space(20)]
    public List<Answer> answers;

    //Eliminar
    float time = 0;
    [HideInInspector] public float answerTimeMod = 0;
    [HideInInspector] public Enemy enemy;
    [HideInInspector] public int attackPoints = 0;

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

        StartCoroutine(UpdateQuestionProgressBar());
        InterfaceManager.instance.UpdateAfinityBars();
    }
    IEnumerator UpdateQuestionProgressBar()
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

    void AddPointIfAnswerMatch(Answer answer, Area area)
    {
        for (int i = 0; i < answer.relatedAreas.Count; i++)
        {
            if (area.aName.ToString() == answer.relatedAreas[i].ToString())
            {
                area.affinity += 1;
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
