using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TMP_TextSetter : MonoBehaviour
{
    public TMP_Text text;
    public Transform parent;
    void Start()
    {
        text.text = parent.name;
    }

    
    private void Update()
    {
        text.text = parent.name;
    }

}
