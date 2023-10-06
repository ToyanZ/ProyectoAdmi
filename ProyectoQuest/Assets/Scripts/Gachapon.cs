using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gachapon : MonoBehaviour
{
    public bool[] areCharacterUnlock;
    public List<Sprite> gachaponList = new List<Sprite>();

    public Sprite[] charactersSprites;

    public Image characterAnimation;

    public GameObject[] canvas;

    public Animator anim;

    public GameObject particles;

    private void Start()
    {
        for(int i = 0; i < charactersSprites.Length; i++) 
        {
            
        }
    }

    public void UnlockCharacter()
    {
        particles.SetActive(false);
        anim.SetTrigger("NewCharacter");
        int character = Random.Range(0, charactersSprites.Length);
        characterAnimation.sprite = charactersSprites[character];
        canvas[0].SetActive(false);
        canvas[2].SetActive(false);
        canvas[1].SetActive(true);
        StartCoroutine("WaitForUnlock");
    }

    IEnumerator WaitForUnlock()
    {
        yield return new WaitForSeconds(1f);
        particles.SetActive(true);
        yield return new WaitForSeconds(2f);
        canvas[2].SetActive(true);
    }
    
    public void ShowParticles()
    {
        
    }
}
