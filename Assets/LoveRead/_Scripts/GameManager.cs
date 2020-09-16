using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using System;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [Header("##### SCREEN TRANSITION #####")]
    public GameObject ScreenTransitionAnimation;
    public GameObject EventSystem;
    private string panelFadeIn = "Panel Open";



    void Start()
    {
        
    }

    public void PlayScreenTransitionAnimation()
    {
        ScreenTransitionAnimation.SetActive(true);
        ScreenTransitionAnimation.GetComponent<Animator>().Play("");
        ScreenTransitionAnimation.GetComponent<Animator>().Play(panelFadeIn);
        StartCoroutine(ManageUI_Interaction());
    }

    IEnumerator ManageUI_Interaction()
    {
        EventSystem.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        EventSystem.SetActive(true);
    }

    

    public void GoToCharacterSelectionScene()
    {
        SceneManager.LoadScene(LoveRead_Backend.MAIN_MENU_SCENE);
    }
}
