using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorCharacter : MonoBehaviour
{
    public int characterSelect;
    public Image[] spriteBackground;

    public void selectedCharacter(int id)
    {
        spriteBackground[characterSelect].enabled = false;
        spriteBackground[id].enabled = true;
        characterSelect = id;
    }
}
