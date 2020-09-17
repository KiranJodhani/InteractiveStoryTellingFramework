using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{
    public main_character main_Character_api;
    public MainCharacter MainCharacterInstance;
    public ModalWindowManager FreeDiamondModalWindowManagerInstance;

    [Header("##### SCREENS #####")]
    public GameObject StorySelectionScreen;
    public GameObject BookSelectionScreen;
    public GameObject MainCharacterScreen;
    public GameObject PlayerNameScreen;
    public GameObject ShopScreen;
    public GameObject SettingScreen;
    public Button MenuButton;
    public GameObject LastOpenedScreen;

    [Header("##### SCREEN TRANSITION #####")]
    public GameObject ScreenTransitionAnimation;
    public GameObject EventSystem;
    private string panelFadeIn = "Panel Open";
    public GameObject ContinueToStory;

    

    [Header("##### LOOK #####")]
    public Button LookButton;
    public GameObject LookPanel;
    public Button SkinToneButton;
    public Button EyeColorButton;
    public GameObject SkinToneScroll;
    public Transform SkinColorScrollContent;
    public GameObject EyeColorScroll;
    public Transform EyeColorScrollContent;
    //public GameObject ConfirmLook_SkinColor;
    //public GameObject ConfirmLook_EyeColor;
    //public bool IsSkinColorConfirmed;
    //public bool IsEyeColorConfirmed;
    //public GameObject PriceTextParent;
    //public TMP_Text PriceText;

    [Header("##### HAIR #####")]
    public Button HairButton;
    public GameObject HairPanel;
    public Button HairColorButton;
    public Button HairStyleButton;
    public GameObject HairColorScroll;
    public Transform HairColorScrollContent;
    public GameObject HairStyleScroll;
    public Transform HairStyleScrollContent;
    //public GameObject ConfirmHair_HairStyle;
    //public GameObject ConfirmHair_HairColor;
    //public bool IsHairStyleConfirmed;
    //public bool IsHairColorConfirmed;

    [Header("##### CLOTHES #####")]
    public Button ClothesButton;
    public GameObject ClothesPanel;
    public GameObject ClothesScroll;
    public Transform ClothesScrollContent;
    public int SelectedCloth;

    [Header("##### BUTTON COLOR #####")]
    public Color MainButtonSelectedColor;
    public Color MainButtonNormalColor;
    public Color SubButtonSelectedColor;
    public Color SubButtonNormalColor;
    public Color ScrollItemSelectedColor;
    public Color ScrollItemNormalColor;

    [Header("##### BOOK SELECTION #####")]
    public Transform BookSelectionContent;
    public GameObject BookSelectionScroll;

    [Header("##### CHAPTER SELECTION #####")]
    public Transform ChapterSelectionContent;
    public GameObject ChapterSelectionScroll;

    [Header("##### CHAPTER #####")]
    public ModalWindowManager ChapterModalWindowManager;
    public ModalWindowManager LetsChooseLookModalWindowManager;

    [Header("##### PLAYER NAME #####")]
    public TMP_InputField PlayerNameInput;

    [Header("##### SHOP #####")]
    public TextMeshProUGUI DiamondText;
    public TextMeshProUGUI KeyText;
    public ModalWindowManager NotEnoughDiamondsPopUp;
    public purchased_data Purchased_Data_Instance;
    //public string LastSelectedCategory;

    public ConfirmedItems confirmedItemsInstance;

    void Start()
    {

        //OpenMainCharacterScreen();
        HideAllScreens();
        if (PlayerPrefs.HasKey(LoveRead_Backend.PlayerStoryProgress))
        {
            StorySelectionScreen.SetActive(true);
        }
        else
        {
            BookSelectionScreen.SetActive(true);
            OpenChapterModalWindow();
        }

        if (PlayerPrefs.HasKey(LoveRead_Backend.PURCHASED_DATA_KEY))
        {
            Purchased_Data_Instance = JsonUtility.FromJson<purchased_data>(PlayerPrefs.GetString(LoveRead_Backend.PURCHASED_DATA_KEY));
            ManagePurchasedData();

        }
        else
        {
            string _purchasedData = JsonUtility.ToJson(Purchased_Data_Instance);
            PlayerPrefs.SetString(LoveRead_Backend.PURCHASED_DATA_KEY, _purchasedData);
            ManagePurchasedData();
        }

    }

    public void PlayScreenTransitionAnimation()
    {
        ScreenTransitionAnimation.SetActive(true);
        ScreenTransitionAnimation.GetComponent<Animator>().Play("");
        ScreenTransitionAnimation.GetComponent<Animator>().Play(panelFadeIn);
        StartCoroutine(ManageUI_Interaction());
    }
    public void ToogleSettingMenu()
    {
        PlayScreenTransitionAnimation();
        Invoke("SettingScreenToogle", 1);
    }

    IEnumerator ManageUI_Interaction()
    {
        EventSystem.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        EventSystem.SetActive(true);
    }

    void SettingScreenToogle()
    {
        SettingScreen.SetActive(!SettingScreen.activeSelf);
        //if (!SettingScreen.activeSelf)
        //{
        //    HideAllScreens();
        //    MainCharacterScreen.SetActive(true);
        //}
    }

    public void OpenFreeDiamondPopup()
    {
        FreeDiamondModalWindowManagerInstance.OpenWindow();
    }

    void HideAllScreens()
    {
        StorySelectionScreen.SetActive(false);
        BookSelectionScreen.SetActive(false);
        PlayerNameScreen.SetActive(false);
        MainCharacterScreen.SetActive(false);
        ShopScreen.SetActive(false);
        SettingScreen.SetActive(false);
    }

    /**********************************************************/
    /************* CHARACTER CUSTOMISATION STARTS ************/

    public void OpenMainCharacterScreen()
    {
        HideAllScreens();
        MainCharacterScreen.SetActive(true);
        LastOpenedScreen = MainCharacterScreen;
        Main_Look_Button();
        ApplySelectedEyeColor(LoveRead_Backend.SelectedEyeColor);
        ApplySelectedSkinColor(LoveRead_Backend.SelectedSkinColor);
        ApplySelectedHairStyle(LoveRead_Backend.SelectedHairStyle);
        ApplySelectedCloth(0);
        confirmedItemsInstance.LastSelectedCategory = 0;
        FixContentPosition();
        ManageConfirmButtons();
    }

    void FixContentPosition()
    {
        SkinColorScrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        EyeColorScrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        HairColorScrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        HairStyleScrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        ClothesScrollContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    // Main Look
    public void Main_Look_Button()
    {
        LookButton.GetComponent<Image>().color = MainButtonSelectedColor;
        HairButton.GetComponent<Image>().color = MainButtonNormalColor;
        ClothesButton.GetComponent<Image>().color = MainButtonNormalColor;
        LookPanel.SetActive(true);
        HairPanel.SetActive(false);
        ClothesPanel.SetActive(false);
        Sub_SkinTone_Button();
    }

    public void Sub_SkinTone_Button()
    {
        SkinToneButton.GetComponent<Image>().color = SubButtonSelectedColor;
        EyeColorButton.GetComponent<Image>().color = SubButtonNormalColor;
        SkinToneScroll.SetActive(true);
        EyeColorScroll.SetActive(false);
        confirmedItemsInstance.LastSelectedCategory = 0;
        ManageConfirmLook(LoveRead_Backend.Look_SkinColor);
    }

    public void Sub_EyeColor_Button()
    {
        SkinToneButton.GetComponent<Image>().color = SubButtonNormalColor;
        EyeColorButton.GetComponent<Image>().color = SubButtonSelectedColor;
        SkinToneScroll.SetActive(false);
        EyeColorScroll.SetActive(true);
        confirmedItemsInstance.LastSelectedCategory = 1;
        ManageConfirmLook(LoveRead_Backend.Look_EyeColor);
    }

    public void ApplySelectedSkinColor(int SkinColorIndex)
    {
        LoveRead_Backend.SelectedSkinColor = SkinColorIndex;
        MainCharacterInstance.Face.sprite = MainCharacterInstance.MainCharacterFaceInstance[SkinColorIndex].FaceSprite;
        MainCharacterInstance.Body.sprite = MainCharacterInstance.MainCharacterBodyInstance[SkinColorIndex].BodySprite;
        MainCharacterInstance.HairStyleShadow.sprite =
            MainCharacterInstance.MainCharacterHairInstance[LoveRead_Backend.SelectedHairStyle].HairStyleShadowSprites[SkinColorIndex];
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
        //IsSkinColorConfirmed = false;
        confirmedItemsInstance.LastSelectedCategory = 0;
        confirmedItemsInstance.CategoriesInstance[0].IsConfirmed = false;
        ManageConfirmLook(LoveRead_Backend.Look_SkinColor);
        ContinueToStory.SetActive(false);        
    }

    public void ApplySelectedEyeColor(int EyeColorIndex)
    {
        MainCharacterInstance.Eye.sprite = MainCharacterInstance.MainCharacterEyeInstance[EyeColorIndex].EyeSprite;
        LoveRead_Backend.SelectedEyeColor = EyeColorIndex;
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
        //IsEyeColorConfirmed = false;
        confirmedItemsInstance.LastSelectedCategory = 1;
        confirmedItemsInstance.CategoriesInstance[1].IsConfirmed = false;
        ManageConfirmLook(LoveRead_Backend.Look_EyeColor);
        ContinueToStory.SetActive(false);
    }


    void ManageConfirmLook(string Type)
    {
        if (Type == LoveRead_Backend.Look_SkinColor)
        {
            confirmedItemsInstance.CategoriesInstance[0].ConfirmButton.SetActive(!confirmedItemsInstance.CategoriesInstance[0].IsConfirmed);
            //ConfirmLook_SkinColor.SetActive(!IsSkinColorConfirmed);
            if (MainCharacterInstance.MainCharacterBodyInstance[LoveRead_Backend.SelectedSkinColor].Price > 0)
            {
                // check if already purchased
                confirmedItemsInstance.CategoriesInstance[0].ConfirmButton.transform.GetChild(1).GetChild(1).transform.GetComponent<TextMeshProUGUI>().text =
                    MainCharacterInstance.MainCharacterBodyInstance[LoveRead_Backend.SelectedSkinColor].Price.ToString();
                confirmedItemsInstance.CategoriesInstance[0].ConfirmButton.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                confirmedItemsInstance.CategoriesInstance[0].ConfirmButton.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else if (Type == LoveRead_Backend.Look_EyeColor)
        {
            confirmedItemsInstance.CategoriesInstance[1].ConfirmButton.SetActive(!confirmedItemsInstance.CategoriesInstance[1].IsConfirmed);
            if (MainCharacterInstance.MainCharacterEyeInstance[LoveRead_Backend.SelectedEyeColor].Price > 0)
            {
                // check if already purchased
                confirmedItemsInstance.CategoriesInstance[1].ConfirmButton.transform.GetChild(1).GetChild(1).transform.GetComponent<TextMeshProUGUI>().text =
                    MainCharacterInstance.MainCharacterEyeInstance[LoveRead_Backend.SelectedEyeColor].Price.ToString();
                confirmedItemsInstance.CategoriesInstance[1].ConfirmButton.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                confirmedItemsInstance.CategoriesInstance[1].ConfirmButton.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        ManageConfirmButtons();
    }

    public void ConfirmedSelectedItem()
    {
        int tmp = confirmedItemsInstance.LastSelectedCategory;

        if (confirmedItemsInstance.CategoriesInstance[tmp].ConfirmButton.transform.GetChild(1).gameObject.activeSelf)
        {
            int Price = int.Parse(confirmedItemsInstance.CategoriesInstance[tmp].ConfirmButton.transform.GetChild(1).GetChild(1).transform.GetComponent<TextMeshProUGUI>().text);
            if (Purchased_Data_Instance.AvailableDiamonds >= Price)
            {
                confirmedItemsInstance.CategoriesInstance[1].IsConfirmed = true;
                confirmedItemsInstance.CategoriesInstance[tmp].ConfirmButton.SetActive(false);
                Purchased_Data_Instance.AvailableDiamonds = Purchased_Data_Instance.AvailableDiamonds - Price;
                DiamondText.text = Purchased_Data_Instance.AvailableDiamonds.ToString();
                ManagePurchasedItems();
            }
            else
            {
                NotEnoughDiamondsPopUp.OpenWindow();
            }
        }
        else
        {
            confirmedItemsInstance.CategoriesInstance[1].IsConfirmed = true;
            confirmedItemsInstance.CategoriesInstance[1].ConfirmButton.SetActive(false);
        }

        bool IsAllConfirmed = true;
        for(int i = 0; i < confirmedItemsInstance.CategoriesInstance.Length;i++)
        {
            if(!confirmedItemsInstance.CategoriesInstance[i].IsConfirmed)
            {
                IsAllConfirmed = false;
                break;
            }
        }
        ContinueToStory.SetActive(IsAllConfirmed);

    }
    // Hair
    public void Main_Hair_Button()
    {
        LookButton.GetComponent<Image>().color = MainButtonNormalColor;
        HairButton.GetComponent<Image>().color = MainButtonSelectedColor;
        ClothesButton.GetComponent<Image>().color = MainButtonNormalColor;
        LookPanel.SetActive(false);
        HairPanel.SetActive(true);
        ClothesPanel.SetActive(false);
        Sub_HairStyle_Button();
    }

    public void Sub_HairStyle_Button()
    {
        HairStyleButton.GetComponent<Image>().color = SubButtonSelectedColor;
        HairColorButton.GetComponent<Image>().color = SubButtonNormalColor;
        HairStyleScroll.SetActive(true);
        HairColorScroll.SetActive(false);
        confirmedItemsInstance.LastSelectedCategory = 2;
        ManageConfirmHair(LoveRead_Backend.Hair_Style);
        
    }

    public void Sub_HairColor_Button()
    {
        HairColorButton.GetComponent<Image>().color = SubButtonSelectedColor;
        HairStyleButton.GetComponent<Image>().color = SubButtonNormalColor;
        HairStyleScroll.SetActive(false);
        HairColorScroll.SetActive(true);
        confirmedItemsInstance.LastSelectedCategory = 3;
        ManageConfirmHair(LoveRead_Backend.Hair_Color);
    }

    public void ApplySelectedHairStyle(int HairStyleIndex)
    {
        LoveRead_Backend.SelectedHairStyle = HairStyleIndex;
        LoveRead_Backend.SelectedHairColor = 0;

        if (MainCharacterInstance.MainCharacterHairInstance[HairStyleIndex].HairStyleBackSprites[LoveRead_Backend.SelectedHairColor])
        {
            MainCharacterInstance.HairStyleBack.gameObject.SetActive(true);
            MainCharacterInstance.HairStyleBack.sprite =
                MainCharacterInstance.MainCharacterHairInstance[HairStyleIndex].HairStyleBackSprites[LoveRead_Backend.SelectedHairColor];
        }
        else
        {
            MainCharacterInstance.HairStyleBack.gameObject.SetActive(false);
        }

        MainCharacterInstance.HairStyle.sprite =
            MainCharacterInstance.MainCharacterHairInstance[HairStyleIndex].HairStyleSprites[LoveRead_Backend.SelectedHairColor];

        if (MainCharacterInstance.MainCharacterHairInstance[LoveRead_Backend.SelectedHairStyle].HairStyleShadowSprites[LoveRead_Backend.SelectedSkinColor])
        {
            MainCharacterInstance.HairStyleShadow.gameObject.SetActive(true);
            MainCharacterInstance.HairStyleShadow.sprite =
                MainCharacterInstance.MainCharacterHairInstance[LoveRead_Backend.SelectedHairStyle].HairStyleShadowSprites[LoveRead_Backend.SelectedSkinColor];
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

        foreach (Transform child in HairColorScrollContent)
        {
            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < MainCharacterInstance.MainCharacterHairInstance[HairStyleIndex].HairStyleSprites.Length; i++)
        {
            GameObject tmp = Instantiate(HairColorScrollContent.GetChild(0).gameObject, HairColorScrollContent);
            tmp.transform.GetChild(1).GetComponent<Image>().sprite =
                MainCharacterInstance.MainCharacterHairInstance[HairStyleIndex].MainCharacterHairColorInstance[i].HairStyleColorIcon;
            string tmpHairColor = i.ToString();
            tmp.name = "HairColor (" + i + ")";
            tmp.GetComponent<Button>().onClick.AddListener(() => ApplySelectedHairColor(tmpHairColor,"true"));
            tmp.SetActive(true);
        }
        Invoke("ApplySelectedHairColorDelayed", 0.1f);
        confirmedItemsInstance.CategoriesInstance[2].IsConfirmed = false;
        confirmedItemsInstance.LastSelectedCategory = 2;
        ManageConfirmHair(LoveRead_Backend.Hair_Style);
        ContinueToStory.SetActive(false);
    }

    void ApplySelectedHairColorDelayed()
    {
        ApplySelectedHairColor("0","false");
    }

   
    public void ApplySelectedHairColor(string HairColorIndex,string ManageConfirmButton)
    {
        LoveRead_Backend.SelectedHairColor = int.Parse(HairColorIndex);
        //print("Selected Hair Color : " + HairColorIndex);
        //print("Manage Confirm : " + ManageConfirmButton);
        if (MainCharacterInstance.MainCharacterHairInstance[LoveRead_Backend.SelectedHairStyle].HairStyleBackSprites[LoveRead_Backend.SelectedHairColor])
        {
            MainCharacterInstance.HairStyleBack.gameObject.SetActive(true);
            MainCharacterInstance.HairStyleBack.sprite =
                   MainCharacterInstance.MainCharacterHairInstance[LoveRead_Backend.SelectedHairStyle].HairStyleBackSprites[LoveRead_Backend.SelectedHairColor];
        }
        else
        {
            MainCharacterInstance.HairStyleBack.gameObject.SetActive(false);
        }

        MainCharacterInstance.HairStyle.sprite =
            MainCharacterInstance.MainCharacterHairInstance[LoveRead_Backend.SelectedHairStyle].HairStyleSprites[LoveRead_Backend.SelectedHairColor];


        int children = HairColorScrollContent.childCount;
        for (int i = 0; i < children - 1; i++)
        {
            if (i == LoveRead_Backend.SelectedHairColor)
            {
                HairColorScrollContent.GetChild(i + 1).GetChild(0).GetComponent<Image>().color = ScrollItemSelectedColor;
            }
            else
            {
                HairColorScrollContent.GetChild(i + 1).GetChild(0).GetComponent<Image>().color = ScrollItemNormalColor;
            }
        }

        if(ManageConfirmButton=="true")
        {
            confirmedItemsInstance.CategoriesInstance[3].IsConfirmed = false;
            ContinueToStory.SetActive(false);
            confirmedItemsInstance.LastSelectedCategory = 3;
            ManageConfirmHair(LoveRead_Backend.Hair_Color);
        }
        
    }

    void ManageConfirmHair(string Type)
    {

        if (Type == LoveRead_Backend.Hair_Style)
        {
            confirmedItemsInstance.CategoriesInstance[2].ConfirmButton.SetActive(!confirmedItemsInstance.CategoriesInstance[2].IsConfirmed);

            if (MainCharacterInstance.MainCharacterHairInstance[LoveRead_Backend.SelectedHairStyle].Price > 0)
            {
                // check if already purchased
                confirmedItemsInstance.CategoriesInstance[2].ConfirmButton.transform.GetChild(1).GetChild(1).transform.GetComponent<TextMeshProUGUI>().text =
                    MainCharacterInstance.MainCharacterHairInstance[LoveRead_Backend.SelectedHairStyle].Price.ToString();
                confirmedItemsInstance.CategoriesInstance[2].ConfirmButton.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                confirmedItemsInstance.CategoriesInstance[2].ConfirmButton.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        else if (Type == LoveRead_Backend.Hair_Color)
        {
            confirmedItemsInstance.CategoriesInstance[3].ConfirmButton.SetActive(!confirmedItemsInstance.CategoriesInstance[3].IsConfirmed);
            if (MainCharacterInstance.MainCharacterHairInstance[LoveRead_Backend.SelectedHairStyle].MainCharacterHairColorInstance[LoveRead_Backend.SelectedHairColor].Price > 0)
            {
                // check if already purchased
                confirmedItemsInstance.CategoriesInstance[3].ConfirmButton.transform.GetChild(1).GetChild(1).transform.GetComponent<TextMeshProUGUI>().text =
                    MainCharacterInstance.MainCharacterHairInstance[LoveRead_Backend.SelectedHairStyle].MainCharacterHairColorInstance[LoveRead_Backend.SelectedHairColor].Price.ToString();
                confirmedItemsInstance.CategoriesInstance[3].ConfirmButton.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                confirmedItemsInstance.CategoriesInstance[3].ConfirmButton.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        ManageConfirmButtons();
    }

    public void ManageConfirmButtons()
    {
        for(int i = 0; i< confirmedItemsInstance.CategoriesInstance.Length;i++)
        {
            confirmedItemsInstance.CategoriesInstance[i].ConfirmButton.SetActive(false);
        }
        int tmp = confirmedItemsInstance.LastSelectedCategory;
        confirmedItemsInstance.CategoriesInstance[tmp].ConfirmButton.SetActive(!confirmedItemsInstance.CategoriesInstance[tmp].IsConfirmed);
    }


    public void ManageContinueToStoryButton()
    {
        //if (IsSkinColorConfirmed && IsEyeColorConfirmed && IsHairStyleConfirmed && IsHairColorConfirmed)
        //{
        //    ContinueToStory.SetActive(true);
        //}
    }

    void ManagePurchasedItems()
    {
        if(confirmedItemsInstance.LastSelectedCategory ==0)
        {
            Purchased_Data_Instance.purchased_skintone.Add(LoveRead_Backend.SelectedSkinColor);
        }
        else if (confirmedItemsInstance.LastSelectedCategory == 1)
        {
            Purchased_Data_Instance.Purchased_eyeColor.Add(LoveRead_Backend.SelectedEyeColor);
        }
        else if (confirmedItemsInstance.LastSelectedCategory == 2)
        {
            AddPurchasedHairStyle();
        }
        else if (confirmedItemsInstance.LastSelectedCategory == 3)
        {
            AddPurchasedHairStyle();
            for (int i = 0; i < Purchased_Data_Instance.PurchasedHair.Count; i++)
            {
                if (Purchased_Data_Instance.PurchasedHair[i].purchased_hairstyle == LoveRead_Backend.SelectedHairStyle)
                {
                    Purchased_Data_Instance.PurchasedHair[i].purchased_haircolor.Add(LoveRead_Backend.SelectedHairColor);
                    break;
                }
            }
        }
        string _purchasedData = JsonUtility.ToJson(Purchased_Data_Instance);
        PlayerPrefs.SetString(LoveRead_Backend.PURCHASED_DATA_KEY, _purchasedData);
        ManagePurchasedData();  // This method will change price to 0 of purchased item in serialize class 
    }

    void AddPurchasedHairStyle()
    {
        bool IsHairstyleFound = false;
        for(int i = 0; i < Purchased_Data_Instance.PurchasedHair.Count;i++)
        {
            if(Purchased_Data_Instance.PurchasedHair[i].purchased_hairstyle==LoveRead_Backend.SelectedHairStyle)
            {
                IsHairstyleFound = true;
                break;
            }
        }
        if(!IsHairstyleFound)
        {

            purchased_hair purchased_Hair_tmp = new purchased_hair();
            purchased_Hair_tmp.purchased_hairstyle = LoveRead_Backend.SelectedHairStyle;
            List<int> purchased_haircolor_tmp = new List<int>();
            purchased_haircolor_tmp.Clear();
            purchased_Hair_tmp.purchased_haircolor = purchased_haircolor_tmp;
            Purchased_Data_Instance.PurchasedHair.Add(purchased_Hair_tmp);
        }
    }

    void ManagePurchasedData()
    {
        for (int i = 0; i < Purchased_Data_Instance.purchased_skintone.Count; i++)
        {
            MainCharacterInstance.MainCharacterBodyInstance[Purchased_Data_Instance.purchased_skintone[i]].Price = 0;
        }

        for (int i = 0; i < Purchased_Data_Instance.Purchased_eyeColor.Count; i++)
        {
            MainCharacterInstance.MainCharacterEyeInstance[Purchased_Data_Instance.Purchased_eyeColor[i]].Price = 0;
        }

        for (int i = 0; i < Purchased_Data_Instance.PurchasedHair.Count; i++)
        {
            MainCharacterInstance.MainCharacterHairInstance[Purchased_Data_Instance.PurchasedHair[i].purchased_hairstyle].Price = 0;
            for(int j = 0; j < Purchased_Data_Instance.PurchasedHair[i].purchased_haircolor.Count;j++)
            {
                int HairColorIndexTemp = Purchased_Data_Instance.PurchasedHair[i].purchased_haircolor[j];
                MainCharacterInstance.MainCharacterHairInstance[Purchased_Data_Instance.PurchasedHair[i].purchased_hairstyle]
                    .MainCharacterHairColorInstance[HairColorIndexTemp].Price = 0;
                
            }
        }

        SetDiamondKeyTexts();
    }


    public void GoToMainGameScene()
    {
        PlayScreenTransitionAnimation();
        StartCoroutine(GoToMainGameSceneDelayed());
    }


    IEnumerator GoToMainGameSceneDelayed()
    {
        yield return new WaitForSeconds(1);
        HideAllScreens();
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(LoveRead_Backend.MAIN_GAME_SCENE);
    }

    // Clothes
    public void Main_Cloth_Button()
    {
        LookButton.GetComponent<Image>().color = MainButtonNormalColor;
        HairButton.GetComponent<Image>().color = MainButtonNormalColor;
        ClothesButton.GetComponent<Image>().color = MainButtonSelectedColor;
        LookPanel.SetActive(false);
        HairPanel.SetActive(false);
        ClothesPanel.SetActive(true);
    }

    public void ApplySelectedCloth(int ClothIndex)
    {
        SelectedCloth = ClothIndex;
        MainCharacterInstance.Cloth.sprite = MainCharacterInstance.MainCharacterClothInstance[ClothIndex].ClothSprite;
        int children = ClothesScrollContent.childCount;
        for (int i = 0; i < children; i++)
        {
            if (i == ClothIndex)
            {
                ClothesScrollContent.GetChild(i).GetChild(0).GetComponent<Image>().color = ScrollItemSelectedColor;
            }
            else
            {
                ClothesScrollContent.GetChild(i).GetChild(0).GetComponent<Image>().color = ScrollItemNormalColor;
            }
        }
    }
    /************* CHARACTER CUSTOMISATION ENDS ************/
    /*******************************************************/


    /**************************************/
    /************* SHOP STARTS ************/
    public void OpenShopMenu()
    {
        StartCoroutine(OpenShopMenuCo());
    }

    IEnumerator OpenShopMenuCo()
    {
        PlayScreenTransitionAnimation();
        yield return new WaitForSeconds(1f);
        HideAllScreens();
        ShopScreen.SetActive(true);
    }

    public void CloseShopMenu()
    {
        StartCoroutine(CloseShopMenuCo());
    }

    IEnumerator CloseShopMenuCo()
    {
        PlayScreenTransitionAnimation();
        yield return new WaitForSeconds(1f);
        HideAllScreens();
        if(LastOpenedScreen)
        {
            LastOpenedScreen.SetActive(true);
        }
        //SettingScreen.SetActive(true);
    }


    public void BuyDiamonds(int quantity)
    {
        Purchased_Data_Instance.AvailableDiamonds = Purchased_Data_Instance.AvailableDiamonds + quantity;
        string _purchasedData = JsonUtility.ToJson(Purchased_Data_Instance);
        PlayerPrefs.SetString(LoveRead_Backend.PURCHASED_DATA_KEY, _purchasedData);
        SetDiamondKeyTexts();
    }

    public void BuyKeys(int quantity)
    {
        Purchased_Data_Instance.AvailableKeys = Purchased_Data_Instance.AvailableKeys + quantity;
        string _purchasedData = JsonUtility.ToJson(Purchased_Data_Instance);
        PlayerPrefs.SetString(LoveRead_Backend.PURCHASED_DATA_KEY, _purchasedData);
        SetDiamondKeyTexts();
    }

    void SetDiamondKeyTexts()
    {
        DiamondText.text = Purchased_Data_Instance.AvailableDiamonds.ToString();
        KeyText.text = Purchased_Data_Instance.AvailableKeys.ToString();
    }
    /************* SHOP ENDS ************/
    /**************************************/

    /****************************************************/
    /************* STORY/BOOK/CHAPTER STARTS ************/
    public void OpenStorySelectionScreen()
    {
        StartCoroutine(OpenStorySelectionScreenCo());
    }

    IEnumerator OpenStorySelectionScreenCo()
    {
        PlayScreenTransitionAnimation();
        yield return new WaitForSeconds(1f);
        HideAllScreens();
        StorySelectionScreen.SetActive(true);
    }

    public void Close_StorySelection()
    {
        StartCoroutine(Close_StorySelectionCo());
    }

    IEnumerator Close_StorySelectionCo()
    {
        PlayScreenTransitionAnimation();
        yield return new WaitForSeconds(1f);
        HideAllScreens();
        MainCharacterScreen.SetActive(true);
    }

    public void OpenBookSelectionScreen()
    {
        StartCoroutine(OpenBookSelectionScreenCo());
    }

    IEnumerator OpenBookSelectionScreenCo()
    {
        PlayScreenTransitionAnimation();
        yield return new WaitForSeconds(1f);
        HideAllScreens();
        BookSelectionScreen.SetActive(true);
        OnClickBook(0);
    }

    public void Close_BookSelection()
    {
        StartCoroutine(Close_BookSelectionCo());
    }

    IEnumerator Close_BookSelectionCo()
    {
        PlayScreenTransitionAnimation();
        yield return new WaitForSeconds(1f);
        HideAllScreens();
        StorySelectionScreen.SetActive(true);
    }

    public void OnClickBook(int bookIndex)
    {
        int children = BookSelectionContent.childCount;
        for (int i = 0; i < children; i++)
        {
            if (i == bookIndex)
            {
                BookSelectionContent.GetChild(i).GetChild(0).GetComponent<Image>().color = ScrollItemSelectedColor;
            }
            else
            {
                BookSelectionContent.GetChild(i).GetChild(0).GetComponent<Image>().color = ScrollItemNormalColor;
            }
        }
    }

    public void OnClickChapter(int chapterIndex)
    {
        int children = ChapterSelectionContent.childCount;
        for (int i = 0; i < children; i++)
        {
            if (i == chapterIndex)
            {
                //Debug.Log("Selected chapter - " + ChapterSelectionContent.GetChild(i).GetChild(0).gameObject.name);
            }
            else
            {
                //ChapterSelectionContent.GetChild(i).GetChild(0).GetComponent<Image>().color = ScrollItemNormalColor;
            }
        }
        OpenChapterModalWindow();
    }

    private void OpenChapterModalWindow()
    {
        ChapterModalWindowManager.OpenWindow();
    }
    private void CloseChapterModalWindow()
    {
        ChapterModalWindowManager.CloseWindow();
    }

    public void OnClickPlayNowChapter()
    {
        CloseChapterModalWindow();
        if (PlayerPrefs.HasKey(LoveRead_Backend.PlayerName))
        {
            Open_LetsChooseLookPopup();
        }
        else
        {
            OpenNameScreen();
        }
    }
    public void OnClickResumeChapter()
    {
        ChapterModalWindowManager.CloseWindow();
    }
    public void OnClickPlayAgainChapter()
    {
        ChapterModalWindowManager.CloseWindow();
        Open_LetsChooseLookPopup();
    }

    private void OpenNameScreen()
    {
        StartCoroutine(OpenNameScreenCO());
    }

    IEnumerator OpenNameScreenCO()
    {
        PlayScreenTransitionAnimation();
        yield return new WaitForSeconds(1f);
        HideAllScreens();
        PlayerNameScreen.SetActive(true);
    }

    public void OnClickPlayerNameContinue()
    {
        if (string.IsNullOrEmpty(PlayerNameInput.text))
        {
            return;
        }
        PlayerPrefs.SetString(LoveRead_Backend.PlayerName, PlayerNameInput.text);
        Open_LetsChooseLookPopup();
    }

    public void Open_LetsChooseLookPopup()
    {
        LetsChooseLookModalWindowManager.OpenWindow();
        //StartCoroutine(Open_LetsChooseLookPopupCo());
    }

    //IEnumerator Open_LetsChooseLookPopupCo()
    //{
    //    HideAllScreens();
    //    LetsChooseLookModalWindowManager.OpenWindow();
    //    yield return new WaitForSeconds(1f);
    //    LetsChooseLookModalWindowManager.CloseWindow();
    //    PlayScreenTransitionAnimation();
    //    yield return new WaitForSeconds(1f);
    //    OpenMainCharacterScreen();
    //}

    public void OnClick_LetsGo()
    {
        LetsChooseLookModalWindowManager.CloseWindow();
        StartCoroutine(OnClick_LetsGoCo());
    }

    IEnumerator OnClick_LetsGoCo()
    {
        HideAllScreens();
        PlayScreenTransitionAnimation();
        yield return new WaitForSeconds(1f);
        OpenMainCharacterScreen();
    }



    /************* STORY/BOOK/CHAPTER ENDS ************/
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
    public MainCharacterBody[] MainCharacterBodyInstance;
    public MainCharacterFace[] MainCharacterFaceInstance;
    public MainCharacterEye[] MainCharacterEyeInstance;
    public MainCharacterCloth[] MainCharacterClothInstance;
    public MainCharacterHair[] MainCharacterHairInstance;
}

[Serializable]
public class MainCharacterBody
{
    public int Price;
    public Sprite BodySprite;
}

[Serializable]
public class MainCharacterFace
{
    //public int Price;
    public Sprite FaceSprite;
}

[Serializable]
public class MainCharacterEye
{
    public int Price;
    public Sprite EyeSprite;
}

[Serializable]
public class MainCharacterCloth
{
    public int Price;
    public Sprite ClothSprite;
}

[Serializable]
public class MainCharacterHair
{
    public string HairName;
    public int Price;
    public Sprite[] HairStyleBackSprites;
    public Sprite[] HairStyleShadowSprites;
    public Sprite[] HairStyleSprites;
    public MainCharacterHairColor[] MainCharacterHairColorInstance;
}

[Serializable]
public class MainCharacterHairColor
{
    public int Price;
    public Sprite HairStyleColorIcon;
}

[Serializable]
public class ConfirmedItems
{
    public int LastSelectedCategory;
    public Categories[] CategoriesInstance;

    /*

    0-Skin Color
    1-Eye Color
    2-Hair Style
    3-Hair Color
    4-Cloth
    5-Accessaries
    6-Lipstics

     */

}

[Serializable]
public class Categories
{
    public bool IsConfirmed;
    public GameObject ConfirmButton;
}
