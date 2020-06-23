using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{

    [Header("##### SETTINGS MENU #########")]
    public Animator SettingMenuAnimator;
    public GameObject SettingScreen;
    public bool IsOpened;
    public Button MenuButton;
    void Start()
    {
        
    }


    public void ToogleSettingMenu()
    {
        StartCoroutine(ManageButtonInteraction());
        if (IsOpened)
        {
            SettingMenuAnimator.SetBool("SettingMenuOut", true);
            SettingMenuAnimator.SetBool("SettingMenuIn", false);
            Invoke("DisableSettingScreen", 1.1f);
            IsOpened = false;
        }
        else
        {
            SettingScreen.SetActive(true);
            SettingMenuAnimator.SetBool("SettingMenuOut", false);
            SettingMenuAnimator.SetBool("SettingMenuIn", true);
            IsOpened = true;
        }
    }   

    IEnumerator ManageButtonInteraction()
    {
        MenuButton.interactable = false;
        yield return new WaitForSeconds(1.5f);
        MenuButton.interactable = true;
    }

    void DisableSettingScreen()
    {
        SettingScreen.SetActive(false);
    }

}
