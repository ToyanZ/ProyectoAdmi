using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorCharacter : MonoBehaviour
{
    public int characterSelect;
    public Image[] spriteBackground;

    public int[] characterStatus;

    public Button[] buttonStatus;

    public GameObject[] principalCanvas;

    private void OnEnable()
    {
        Debug.Log("Se ejecuta");
        for (int i = 0; i < characterStatus.Length; i++)
        {
            string character = "character_" + i;
            characterStatus[i] = PlayerPrefs.GetInt(character, 0);
            if(characterStatus[i] == 1)
            {
                Debug.Log("Desbloqueado "+character);
                buttonStatus[i].interactable = true;
            }
        }
    }

    public void selectedCharacter(int id)
    {
        spriteBackground[characterSelect].enabled = false;
        spriteBackground[id].enabled = true;
        characterSelect = id;
    }

    public void GoToShop()
    {
        principalCanvas[0].SetActive(true);
        principalCanvas[1].SetActive(false);
    }
}
