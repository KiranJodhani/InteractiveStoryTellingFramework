using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using System;

public class MainMenuManager : MonoBehaviour
{
    public MainCharacter MainCharacterInstance;
    public ModalWindowManager ModalWindowManagerInstance;
    [Header("##### SETTINGS MENU #########")]
    public GameObject SettingScreen;
    public Button MenuButton;

    public GameObject ScreenTransitionAnimation;
    private string panelFadeIn = "Panel Open";

    [Header("##### HAIR #########")]
    public Button HairButton;
    public GameObject HairPanel;
    public Button HairColorButton;
    public Button HairStyleButton;
    public GameObject HairColorScroll;
    public Transform HairColorScrollContent;
    public GameObject HairStyleScroll;
    public Transform HairStyleScrollContent;
    public int SelectedHairStyle;
    public int SelectedHairColor;

    [Header("##### LOOK #########")]
    public Button LookButton;
    public GameObject LookPanel;
    public Button SkinToneButton;
    public Button EyeColorButton;
    public GameObject SkinToneScroll;
    public Transform SkinColorScrollContent;
    public GameObject EyeColorScroll;
    public Transform EyeColorScrollContent;
    public int SelectedEyeColor;
    public int SelectedSkinColor;


    [Header("##### BUTTON COLOR #########")]
    public Color MainButtonSelectedColor;
    public Color MainButtonNormalColor;
    public Color SubButtonSelectedColor;
    public Color SubButtonNormalColor;
    public Color ScrollItemSelectedColor;
    public Color ScrollItemNormalColor;

    [Header("##### FX #########")]
    public ParticleSystem BoomParticle;
    void Start()
    {
        Main_Look_Button();
        ApplySelectedEyeColor(0);
        ApplySelectedSkinColor(0);
        ApplySelectedHairStyle(0);
        FixContentPosition();
    }

    void FixContentPosition()
    {
        SkinColorScrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        EyeColorScrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        HairColorScrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        HairStyleScrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
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
        HairStyleButton.GetComponent<Image>().color = SubButtonSelectedColor;
        HairColorButton.GetComponent<Image>().color = SubButtonNormalColor;
        HairStyleScroll.SetActive(true);
        HairColorScroll.SetActive(false);
    }

    public void Sub_HairColor_Button()
    {
        HairColorButton.GetComponent<Image>().color = SubButtonSelectedColor;
        HairStyleButton.GetComponent<Image>().color = SubButtonNormalColor;
        HairStyleScroll.SetActive(false);
        HairColorScroll.SetActive(true);
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

    public void ApplySelectedEyeColor(int EyeColorIndex)
    {
        MainCharacterInstance.Eye.sprite = MainCharacterInstance.EyeSprites[EyeColorIndex];
        SelectedEyeColor = EyeColorIndex;
        int children = EyeColorScrollContent.childCount;
        for (int i = 0; i < children; i++)
        {
            if (i == EyeColorIndex)
            {
                EyeColorScrollContent.GetChild(i).GetChild(0).GetComponent<Image>().color = ScrollItemSelectedColor;
            }
            else
            {
                EyeColorScrollContent.GetChild(i).GetChild(0).GetComponent<Image>().color = ScrollItemNormalColor;
            }
        }
    }

    public void ApplySelectedSkinColor(int SkinColorIndex)
    {
        SelectedSkinColor = SkinColorIndex;
        MainCharacterInstance.Face.sprite = MainCharacterInstance.FaceSprites[SkinColorIndex];
        MainCharacterInstance.Body.sprite = MainCharacterInstance.BodySprites[SkinColorIndex];
        MainCharacterInstance.HairStyleShadow.sprite =
            MainCharacterInstance.MainCharacterHairInstance[SelectedHairStyle].HairStyleShadowSprites[SkinColorIndex];
        int children = SkinColorScrollContent.childCount;
        for (int i = 0; i < children; i++)
        {
            if (i == SkinColorIndex)
            {
                SkinColorScrollContent.GetChild(i).GetChild(0).GetComponent<Image>().color = ScrollItemSelectedColor;
            }
            else
            {
                SkinColorScrollContent.GetChild(i).GetChild(0).GetComponent<Image>().color = ScrollItemNormalColor;
            }
        }
    }

    public void ApplySelectedHairStyle(int HairStyleIndex)
    {
        SelectedHairStyle = HairStyleIndex;

        if(MainCharacterInstance.MainCharacterHairInstance[HairStyleIndex].HairStyleBackSprites[SelectedHairColor])
        {
            MainCharacterInstance.HairStyleBack.gameObject.SetActive(true);
            MainCharacterInstance.HairStyleBack.sprite =
    MainCharacterInstance.MainCharacterHairInstance[HairStyleIndex].HairStyleBackSprites[SelectedHairColor];
        }
        else
        {
            MainCharacterInstance.HairStyleBack.gameObject.SetActive(false);
        }

        MainCharacterInstance.HairStyle.sprite =
            MainCharacterInstance.MainCharacterHairInstance[HairStyleIndex].HairStyleSprites[SelectedHairColor];

        if (MainCharacterInstance.MainCharacterHairInstance[SelectedHairStyle].HairStyleShadowSprites[SelectedSkinColor])
        {
            MainCharacterInstance.HairStyleShadow.gameObject.SetActive(true);
            MainCharacterInstance.HairStyleShadow.sprite =
                MainCharacterInstance.MainCharacterHairInstance[SelectedHairStyle].HairStyleShadowSprites[SelectedSkinColor];
        }
        else
        {
            MainCharacterInstance.HairStyleShadow.gameObject.SetActive(false);
        }

        int children = HairStyleScrollContent.childCount;
        for (int i = 0; i < children; i++)
        {
            if (i == HairStyleIndex)
            {
                HairStyleScrollContent.GetChild(i).GetChild(0).GetComponent<Image>().color = ScrollItemSelectedColor;
            }
            else
            {
                HairStyleScrollContent.GetChild(i).GetChild(0).GetComponent<Image>().color = ScrollItemNormalColor;
            }
        }

        foreach(Transform child in HairColorScrollContent)
        {
            if(child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }

        for(int i = 0;i < MainCharacterInstance.MainCharacterHairInstance[HairStyleIndex].HairStyleSprites.Length;i++)
        {
            GameObject tmp = Instantiate(HairColorScrollContent.GetChild(0).gameObject, HairColorScrollContent);
            tmp.transform.GetChild(1).GetComponent<Image>().sprite =
                MainCharacterInstance.MainCharacterHairInstance[HairStyleIndex].HairStyleColorIcon[i];
            string tmpHairColor = i.ToString();
            tmp.name = "HairColor (" + i + ")";
            tmp.GetComponent<Button>().onClick.AddListener(() => ApplySelectedHairColor(tmpHairColor));
            tmp.SetActive(true);
        }
        Invoke("ApplySelectedHairColorDelayed", 0.1f);
        
    }

    void ApplySelectedHairColorDelayed()
    {
        ApplySelectedHairColor("0");
    }

    public void ApplySelectedHairColor(string HairColorIndex)
    {
        SelectedHairColor = int.Parse(HairColorIndex);
        print("Selected Hair Color : " + HairColorIndex);
        if (MainCharacterInstance.MainCharacterHairInstance[SelectedHairStyle].HairStyleBackSprites[SelectedHairColor])
        {
            MainCharacterInstance.HairStyleBack.gameObject.SetActive(true);
            MainCharacterInstance.HairStyleBack.sprite =
                   MainCharacterInstance.MainCharacterHairInstance[SelectedHairStyle].HairStyleBackSprites[SelectedHairColor];
        }
        else
        {
            MainCharacterInstance.HairStyleBack.gameObject.SetActive(false);
        }

        MainCharacterInstance.HairStyle.sprite =
            MainCharacterInstance.MainCharacterHairInstance[SelectedHairStyle].HairStyleSprites[SelectedHairColor];


        int children = HairColorScrollContent.childCount;
        for (int i = 0; i < children-1; i++)
        {
            if (i == SelectedHairColor)
            {
                HairColorScrollContent.GetChild(i+1).GetChild(0).GetComponent<Image>().color = ScrollItemSelectedColor;
            }
            else
            {
                HairColorScrollContent.GetChild(i+1).GetChild(0).GetComponent<Image>().color = ScrollItemNormalColor;
            }
        }
    }

    void PlayParticle()
    {
        BoomParticle.gameObject.SetActive(true);
        BoomParticle.Play();
    }
    /************* CHARACTER CUSTOMISATION ENDS ************/
    /**********************************************************/
}

[Serializable]
public class MainCharacter
{
    public Image HairStyleBack;
    public Image Body;
    public Image Eye;
    public Image Face;
    public Image HairStyleShadow;
    public Image Cloth;
    public Image HairStyle;
    public Sprite[] EyeSprites;
    public Sprite[] FaceSprites;
    public Sprite[] BodySprites;
    public MainCharacterHair[] MainCharacterHairInstance;
}

[Serializable]
public class MainCharacterHair
{
    public string HairName;
    public Sprite[] HairStyleBackSprites;
    public Sprite[] HairStyleShadowSprites;
    public Sprite[] HairStyleSprites;
    public Sprite[] HairStyleColorIcon;
}

