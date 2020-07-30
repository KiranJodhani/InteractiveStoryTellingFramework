using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class MainMenuManager : MonoBehaviour
{

    public ModalWindowManager ModalWindowManagerInstance;
    [Header("##### SETTINGS MENU #########")]
    public GameObject SettingScreen;
    public Button MenuButton;

    public GameObject ScreenTransitionAnimation;
    private string panelFadeIn = "Panel Open";


    [Header("##### HAIR #########")]
    public Image HairStyle;
    public Image HairStyleBack;
    public Image HairStyleShadow;
    public Button HairButton;
    public GameObject HairStylePanel;
    public Button HairColorButton;
    public Button HairStyleButton;
    public GameObject HairColorScroll;
    public GameObject HairStyleScroll;

    [Header("##### LOOK #########")]
    public Image Body;
    public Image Face;
    public Image Eye;
    public Button LookButton;
    public GameObject LookPanel;
    public Button SkinToneButton;
    public Button EyeColorButton;
    public GameObject SkinToneScroll;
    public GameObject EyeColorScroll;



    [Header("##### BUTTON COLOR #########")]
    public Color SelectedColor;
    public Color NormalColor;


    void Start()
    {
        
    }

    public void PlayScreenTransitionAnimation()
    {
        ScreenTransitionAnimation.SetActive(true);
        ScreenTransitionAnimation.GetComponent<Animator>().Play("");
        ScreenTransitionAnimation.GetComponent<Animator>().Play(panelFadeIn);
    }
    public void ToogleSettingMenu()
    {
        PlayScreenTransitionAnimation();
        Invoke("SettingScreenToogle", 1);
        StartCoroutine(ManageButtonInteraction());
    }

    IEnumerator ManageButtonInteraction()
    {
        MenuButton.interactable = false;
        yield return new WaitForSeconds(1.5f);
        MenuButton.interactable = true;
    }

    void SettingScreenToogle()
    {
        SettingScreen.SetActive(!SettingScreen.activeSelf);
    }

    public void OpenFreeDiamondPopup()
    {
        ModalWindowManagerInstance.OpenWindow();
    }
}


