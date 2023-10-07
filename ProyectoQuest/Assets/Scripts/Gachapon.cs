using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Gachapon : MonoBehaviour
{

    [System.Serializable]
    public class CharactersToUnlock
    {
        public string name;
        public Sprite characterSprite;
        public int idCharacter;
        public int areUnlock;
    }

    public CharactersToUnlock[] allCharacter;

    public List<CharactersToUnlock> lockCharacters = new List<CharactersToUnlock>();

    public Sprite[] charactersSprites;

    public Image characterAnimation;

    public GameObject[] canvas;

    public Animator anim;

    public GameObject particles;

    public TMP_Text coinsTXT;
    public int currentCoins;
    
    public Animator coinAnim;

    private void Start()
    {

        for (int i = 0; i < allCharacter.Length; i++)
        {
            string character = "character_"+i;
            Debug.Log(character);
            allCharacter[i].areUnlock = PlayerPrefs.GetInt(character, 0);
            if (allCharacter[i].areUnlock != 1)
            {
                lockCharacters.Add(allCharacter[i]);
            }
        }

        if(lockCharacters.Count == 0)
        {
            canvas[0].gameObject.SetActive(false);
        }

        if(GameManager.instance != null)
        {
            coinsTXT.text = ""+GameManager.instance.playerCoins;
        }
        else
        {
            currentCoins = 10;
            coinsTXT.text = "10";
        }
    }

    public void UnlockCharacter()
    {
        if (currentCoins > 3)
        {
            if (lockCharacters.Count == 0)
            {
                Debug.Log("No quedan mas personajes");
            }
            else
            {
                coinAnim.SetTrigger("Money");
                particles.SetActive(false);
                anim.SetTrigger("NewCharacter");

                int characterRandom = Random.Range(0, lockCharacters.Count);
                characterAnimation.sprite = lockCharacters[characterRandom].characterSprite;
                string characterID = "character_" + lockCharacters[characterRandom].idCharacter;
                Debug.Log(characterID);
                PlayerPrefs.SetInt(characterID, 1);

                lockCharacters.RemoveAt(characterRandom);
                canvas[0].SetActive(false);
                canvas[2].SetActive(false);
                canvas[1].SetActive(true);
                StartCoroutine("lessMoney");
                StartCoroutine("WaitForUnlock");
            }
        }
        
        else
        {
            StartCoroutine("NeedMoney");
        }
    }

    IEnumerator WaitForUnlock()
    {
        yield return new WaitForSeconds(1f);
        particles.SetActive(true);
        yield return new WaitForSeconds(2f);
        canvas[0].SetActive(true);
    }

    IEnumerator lessMoney()
    {
        if(GameManager.instance != null)
        {
            yield return new WaitForSeconds(0.1f);
            GameManager.instance.playerCoins--;
            coinsTXT.text = "" + GameManager.instance.playerCoins;
            yield return new WaitForSeconds(0.1f);
            GameManager.instance.playerCoins--;
            coinsTXT.text = "" + GameManager.instance.playerCoins;
            yield return new WaitForSeconds(0.1f);
            GameManager.instance.playerCoins--;
            coinsTXT.text = "" + GameManager.instance.playerCoins;
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            currentCoins = currentCoins - 1;
            coinsTXT.text = "" + currentCoins;
            yield return new WaitForSeconds(0.3f);
            currentCoins = currentCoins - 1;
            coinsTXT.text = "" + currentCoins;
            yield return new WaitForSeconds(0.3f);
            currentCoins = currentCoins - 1;
            coinsTXT.text = "" + currentCoins;
        }
    }

    IEnumerator NeedMoney()
    {
        canvas[3].SetActive(true);
        canvas[0].SetActive(false);
        yield return new WaitForSeconds(3f);
        canvas[3].SetActive(false);
        canvas[0].SetActive(true);
    }

    public void LoadScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }
}
