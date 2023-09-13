using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyPressed : MonoBehaviour
{
    public EventSystem eventSystem;
    public Button w;
    public Button a;
    public Button s;
    public Button d;

    private void Update()
    {
        //eventSystem.SetSelectedGameObject(Input.GetKey(KeyCode.W) ? w.gameObject : null);
        //eventSystem.SetSelectedGameObject(Input.GetKey(KeyCode.A) ? a.gameObject : null);
        //eventSystem.SetSelectedGameObject(Input.GetKey(KeyCode.S) ? s.gameObject : null);
        //eventSystem.SetSelectedGameObject(Input.GetKey(KeyCode.D) ? d.gameObject : null);
    }
}
