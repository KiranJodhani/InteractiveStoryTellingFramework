using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using TMPro;
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
    public GameObject HairPanel;
    public GameObject Back_HairColor;
    public TextMeshProUGUI HairOptionText;
    //public Button HairColorButton;
    //public Button HairStyleButton;
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
    public Color MainButtonSelectedColor;
    public Color MainButtonNormalColor;
    public Color SubButtonSelectedColor;
    public Color SubButtonNormalColor;
    public Color ScrollItemSelectedColor;
    public Color ScrollItemNormalColor;


    void Start()
    {
        Main_Look_Button();
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

    /**********************************************************/
    /************* CHARACTER CUSTOMISATION STARTS ************/

    public void Main_Look_Button()
    {
        LookButton.GetComponent<Image>().color = MainButtonSelectedColor;
        HairButton.GetComponent<Image>().color = MainButtonNormalColor;
        LookPanel.SetActive(true);
        HairPanel.SetActive(false);
        Sub_SkinTone_Button();
    }

    public void Main_Hair_Button()
    {
        LookButton.GetComponent<Image>().color = MainButtonNormalColor;
        HairButton.GetComponent<Image>().color = MainButtonSelectedColor;
        LookPanel.SetActive(false);
        HairPanel.SetActive(true);
        Sub_HairStyle_Button();
    }

    public void Sub_HairStyle_Button()
    {
        HairStyleScroll.SetActive(true);
        HairColorScroll.SetActive(false);
        Back_HairColor.SetActive(false);
        HairOptionText.text = "Hair Style";
    }

    public void Sub_HairColor_Button()
    {
        HairStyleScroll.SetActive(false);
    }

    public void Open_HairColor_option()
    {
        HairStyleScroll.SetActive(false);
        HairColorScroll.SetActive(true);
        Back_HairColor.SetActive(true);
        HairOptionText.text = "Hair Color";
    }
   

    public void Sub_SkinTone_Button()
    {
        SkinToneButton.GetComponent<Image>().color = SubButtonSelectedColor;
        EyeColorButton.GetComponent<Image>().color = SubButtonNormalColor;
        SkinToneScroll.SetActive(true);
        EyeColorScroll.SetActive(false);
    }

    public void Sub_EyeColor_Button()
    {
        SkinToneButton.GetComponent<Image>().color = SubButtonNormalColor;
        EyeColorButton.GetComponent<Image>().color = SubButtonSelectedColor;
        SkinToneScroll.SetActive(false);
        EyeColorScroll.SetActive(true);
    }


    /************* CHARACTER CUSTOMISATION ENDS ************/
    /**********************************************************/
}


