using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    public List<TextMeshProUGUI> PhrasesTextList = new List<TextMeshProUGUI>();
    //public TextMeshProUGUI[] PhrasesText;
    //public string[] Phrases;
    public int PhraseIndex;
    public Image LoadingBarSlider;
    public float LoadingSpeed;
    public TextMeshProUGUI LoadingProgressText;
    
    void Start()
    {
        //ScreenTransitionAnimation.SetActive(true);
        //ScreenTransitionAnimation.GetComponent<Animator>().Play("");
        //ScreenTransitionAnimation.GetComponent<Animator>().Play(panelFadeIn);
        Shuffle();
    }


    void HideAllPhrases()
    {
        for(int i = 0; i < PhrasesTextList.Count;i++)
        {
            PhrasesTextList[i].gameObject.SetActive(false);
        }
    }

    public void Shuffle()
    {
        for (int i = 0; i < PhrasesTextList.Count; i++)
        {
            int rnd = Random.Range(0, PhrasesTextList.Count);
            TextMeshProUGUI tempGO = PhrasesTextList[rnd];
            PhrasesTextList[rnd] = PhrasesTextList[i];
            PhrasesTextList[i] = tempGO;
        }

        SwitchPhrase();
    }

    void SwitchPhrase()
    {
        HideAllPhrases();
        PhraseIndex++;
        if(PhraseIndex== PhrasesTextList.Count)
        {
            PhraseIndex = 0;
        }
        PhrasesTextList[PhraseIndex].gameObject.SetActive(true);
        Invoke("SwitchPhrase", 3);
    }

    

    bool CanGoToNextScene = false;

    private void Update()
    {
        LoadingBarSlider.fillAmount = LoadingBarSlider.fillAmount + (LoadingSpeed * Time.deltaTime);
        LoadingProgressText.text = "Loading:" + LoadingBarSlider.fillAmount.ToString("P0");
        if(!CanGoToNextScene && LoadingBarSlider.fillAmount.ToString("P0")=="100%")
        {
            CanGoToNextScene = true;
            PlayScreenTransitionAnimation();
        }
    }

    public GameObject ScreenTransitionAnimation;
    private string panelFadeIn = "Panel Open";

    public void PlayScreenTransitionAnimation()
    {
        ScreenTransitionAnimation.SetActive(true);
        ScreenTransitionAnimation.GetComponent<Animator>().Play("");
        ScreenTransitionAnimation.GetComponent<Animator>().Play(panelFadeIn);
        Invoke("LoadNextScene", 1f);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(LoveRead_Backend.AUTHENTICATION_SCENE);
    }


}
