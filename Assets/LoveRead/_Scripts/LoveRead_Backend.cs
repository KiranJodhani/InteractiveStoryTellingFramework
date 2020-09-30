using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoveRead_Backend : MonoBehaviour
{
    
    //***********  API *************
    public readonly static string BASE_URL= "http://172.105.35.50/love_read/";
    public readonly static string REGISTER = BASE_URL + "api/register";
    public readonly static string LOGIN = BASE_URL + "api/login";
    public readonly static string userkey = "userkey";
    public readonly static string MAIN_CHARACTER_IMAGES = BASE_URL + "main_character";

    //***********  SCENES *************
    public readonly static string SPLASH_SCENE= "0_SplashScene";
    public readonly static string MAIN_MENU_SCENE= "1_MainMenuScene";
    public readonly static string AUTHENTICATION_SCENE= "2_AuthenticationScene";
    public readonly static string MAIN_GAME_SCENE = "3_MainGameScene";


    //***********  CUSTOMISATION *************
    public readonly static string Look_EyeColor = "Look_EyeColor";
    public readonly static string Look_SkinColor = "Look_SkinColor";
    public readonly static string Hair_Style = "Hair_Style";
    public readonly static string Hair_Color = "Hair_Color";
    public readonly static string Cloth = "Cloth";
    public readonly static string Acc_lipstick = "Acc_lipstick";
    public readonly static string Acc_earrings = "Acc_earrings";
    public readonly static string Acc_glasses= "Acc_glasses";
    public readonly static string Acc_tattoos = "Acc_tattoos";

    public static int SelectedSkinColor=0;
    public static int SelectedEyeColor=0;
    public static int SelectedHairStyle=0;
    public static int SelectedHairColor = 0;
    public static int SelectedCloth = 0;
    public static int SelectedLipstick = 0;
    public static int SelectedEarrings = 0;
    public static int SelectedGlasses = 0;
    public static int SelectedTattoos = 0;

    public static string PlayerStoryProgress = "player_story_progress";
    public static string PlayerName = "player_name";

    //***********  SHOP *************
    public readonly static string PURCHASED_DATA_KEY = "PURCHASED_DATA_KEY";
}



/**********************************/
/********** LOGIN STARTS **********/
[Serializable]
public class login_request
{
    public string email;
    public string password;
}


[Serializable]
public class login_response
{
    public string status;
    public string message;
    public login_response_data data;
}

[Serializable]
public class login_response_data
{
    public string id;
    public string name;
    public string email;
    //public string username;
    public int is_varified;
}
/********** LOGIN ENDS **********/
/********************************/



/*************************************/
/********** REGISTER STARTS **********/
[Serializable]
public class register_request
{
    public string name;
    //public string username;
    public string email;
    public string password;
}


[Serializable]
public class register_response
{
    public string status;
    public string message;
    public register_response_data data;
}

[Serializable]
public class register_response_data
{
    public string id;
    public string name;
    //public string username;
    public string email;
    public string otp;
}

[Serializable]
public class verify_otp_request
{
    public string email;
    public string otp;
}

[Serializable]
public class verify_otp_response
{
    public string status;
    public string message;
}

[Serializable]
public class forgot_password_request
{
    public string email;
}

[Serializable]
public class forgot_password_response
{
    public string status;
    public string message;
    public forgot_password_response_data data;
}

[Serializable]
public class forgot_password_response_data
{
    public string id;
    //public string username;
    public string otp;
}
/********** REGISTER ENDS **********/
/***********************************/



/****************************************/
/********** RESEND LINK STARTS **********/
[Serializable]
public class resend_varification_request
{
    public string email;
}


[Serializable]
public class resend_varification_response
{
    public string status;
    public string message;
}
/********** RESEND LINK ENDS **********/
/**************************************/



/*******************************************/
/********** MAIN CHARACTER STARTS API **********/
[Serializable]
public class main_character
{
    public main_character_body[] body;
    //public main_character_hair[] hair;
    //public main_character_look look;
    //public main_character_cloth[] cloths;
    //public main_character_accessories[] accessories;
}

[Serializable]
public class main_character_body
{
    public string body_color;
    public string body_image;
    public Sprite body_sprite;
}

[Serializable]
public class main_character_look
{
    public main_character_look_skintone[] mc_look_skintone;
    public main_character_look_eyeColor[] mc_look_eyetone;
}

[Serializable]
public class main_character_look_skintone
{
    public string skintone_name;
    public string body_image;
    public string color_icon;
    public main_character_lipstick[] lipsticks;
}

[Serializable]
public class main_character_lipstick
{
    public string lipstick_name;
    public string[] lipstick_images;
    public string[] lipstick_icons;
}

[Serializable]
public class main_character_look_eyeColor
{
    public string eyecolor_name;
    public string eye_image;
    public string eye_icon;
}

[Serializable]
public class main_character_hair
{
    public string hair_name;
    public string[] hairstyle_back_images;
    public string[] hairstyle_shadow_images;
    public string[] hairstyle_images;
    public string[] hairstyle_color_icons;
    public string[] hairstyle_icons;
}

[Serializable]
public class main_character_cloth
{
    public string cloth_name;
    public string cloth_image;
    public string cloth_icon;
}

[Serializable]
public class main_character_accessories
{
    public string accessories_name;
    public string accessories_image;
    public string accessories_icon;
}
/********** MAIN CHARACTER ENDS API **********/
/*****************************************/


/****************************************/
/********** PURCHASED ITEMS STARTS **********/
[Serializable]
public class purchased_data
{
    public  int AvailableDiamonds;
    public  int AvailableKeys;
    public List<int> purchased_skintone = new List<int>();
    public List<int> Purchased_eyeColor = new List<int>();
    public List<purchased_hair> PurchasedHair = new List<purchased_hair>();
    public List<int> Purchased_clothes = new List<int>();
    public List<int> Purchased_lipsticks = new List<int>();
    public List<int> Purchased_earrings = new List<int>();
    public List<int> Purchased_glasses = new List<int>();
    public List<int> Purchased_tattoos = new List<int>();

}

[Serializable]
public class purchased_hair
{
    public int purchased_hairstyle;
    public List<int> purchased_haircolor = new List<int>();
}

/********** PURCHASED ITEMS ENDS **********/
/**************************************/


/**************************************/
/********** PURCHASED ITEMS ENDS *******/

[Serializable]
public class Chapter
{
    public string chapter_name;
    public ChapterScene[] ChapterScene_Instance;
}

[Serializable]
public class ChapterScene
{
    public string scene_name;
}

/********** PURCHASED ITEMS ENDS *******/
/**************************************/
