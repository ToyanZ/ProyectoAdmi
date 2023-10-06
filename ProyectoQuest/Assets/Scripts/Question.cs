using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86;

public class Question : MonoBehaviour
{
    public Bar completed;
    public RectTransform npcPortait;
    [HideInInspector] public RectTransform portaitInstance;
    public bool answered = false;
    public string statement = "";

    [Space(20)]
    public List<Answer> answers;


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


}
