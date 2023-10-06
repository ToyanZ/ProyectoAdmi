using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Gachapon : MonoBehaviour
{
    public bool[] areCharacterUnlock;
    public List<Sprite> gachaponList = new List<Sprite>();

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
            coinAnim.SetTrigger("Money");
            particles.SetActive(false);
            anim.SetTrigger("NewCharacter");
            int character = Random.Range(0, charactersSprites.Length);
            characterAnimation.sprite = charactersSprites[character];
            canvas[0].SetActive(false);
            canvas[2].SetActive(false);
            canvas[1].SetActive(true);
            StartCoroutine("lessMoney");
            StartCoroutine("WaitForUnlock");
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
