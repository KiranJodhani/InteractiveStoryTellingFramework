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

    //***********  CHAPTER *************
    public readonly static string ScreenType_Narration = "narration";
    public readonly static string ScreenType_MC_Speaking = "mc_speaking";
    public readonly static string ScreenType_OC_Speaking = "oc_speaking";
    public readonly static string ScreenType_MC_Thinking = "mc_thinking";
    public readonly static string ScreenType_OC_Thinking = "oc_thinking";
    public readonly static string ScreenType_Choice = "choice";
    public readonly static string ScreenType_Action = "action";
    public static int ChapterX_LastScreen = 0;
    public static int ChapterX_LastScene = 0;


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
    public string code;
    public main_character_body[] body;
    public main_character_cloth[] cloths;
    public main_character_accessories accessories;
    public main_character_eyes eyes;
    //public main_character_hair[] hair;
}

[Serializable]
public class main_character_body
{
    public string body_color;
    public string body_image;
    public string body_icon;
    public Sprite body_sprite;
    public Sprite body_icon_sprite;
    public int price;
}

[Serializable]
public class main_character_cloth
{
    public string cloth_name;
    public string cloth_image;
    public string cloth_icon;
    public Sprite cloth_sprite;
    public Sprite cloth_icon_sprite;
    public int price;
}

[Serializable]
public class main_character_accessories
{
    public earring[] earrings;
    public tattoo[] tattoos;
    public eye_glasses[] glasses;
    public lipstick_skin_tones[] skin_tones_lipsticks;
}

[Serializable]
public class earring
{
    public string earring_name;
    public string earring_image;
    public string earring_icon;
    public Sprite earring_sprite;
    public Sprite earring_icon_sprite;
    public int price;
}

[Serializable]
public class tattoo
{
    public string tattoo_name;
    public string tattoo_image;
    public string tattoo_icon;
    public Sprite tattoo_sprite;
    public Sprite tattoo_icon_sprite;
    public int price;
}

[Serializable]
public class eye_glasses
{
    public string glasses_name;
    public string glasses_image;
    public string glasses_icon;
    public Sprite glasses_sprite;
    public Sprite glasses_icon_sprite;
    public int price;
}

[Serializable]
public class lipstick_skin_tones
{
    public string skin_color;
    public lipstick[] lipsticks;
}

[Serializable]
public class lipstick
{
    public string lipstick_name;
    public string lipstick_icon;
    public Sprite lipstick_icon_sprite;
    public int price;
    public emotion[] emotions;
}

[Serializable]
public class emotion
{
    public string emotion_name;
    public string emotion_image;
    public Sprite emotion_image_sprite;
}

[Serializable]
public class main_character_eyes
{
    public basic_eyes[] basic_eyes;
    public rolling_eyes[] rolling_eyes;
}

[Serializable]
public class basic_eyes
{
    public string eyecolor_name;
    public string eye_image;
    public string eye_icon;
    public Sprite eye_sprite;
    public Sprite eye_icon_sprite;
    public int price;
}

[Serializable]
public class rolling_eyes
{
    public string eyecolor_name;
    public string eye_image;
    public string eye_icon;
    public Sprite eye_sprite;
    public Sprite eye_icon_sprite;
    public int price;
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
    public ChapterSceneScreen[] chapterSceneScreens;
}

[Serializable]
public class ChapterSceneScreen
{
    public string screenType;
    public string character_name;
    public string character_id;
    public string content;
    public string emotion;
    public string oc_body_type;  // 0 with cloth , 1 naked
    public choiceScreen choiceScreen;
    public actionScreen actionScreen;
}

[Serializable]
public class other_character
{
    public string name;
    public string id;
    public other_character_bodies[] bodies;
    public other_character_emotion[] emotions;
}

[Serializable]
public class other_character_bodies
{
    public string body_id;
    public string body_image;
    public Sprite body_sprite;
}

[Serializable]
public class other_character_emotion
{
    public string emotion_name;
    public string emotion_image;
    public Sprite emotion_sprite;
}


[Serializable]
public class choiceScreen
{
    public string choiceText;
    public choiceScreenOptions[] choiceScreenOptions;
}

[Serializable]
public class choiceScreenOptions
{
    public string optionText;
    public int price;
    public int targetScreenNumber;
}

[Serializable]
public class actionScreen
{
    public string actionText;
    public actionScreenOptions[] actionScreenOptions;
}

[Serializable]
public class actionScreenOptions
{
    public string optionText;
    public int price;
    public string image_url;
    public Sprite image_sprite;
    public int targetScreenNumber;
}
/********** PURCHASED ITEMS ENDS *******/
/**************************************/
