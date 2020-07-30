using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoveRead_Backend : MonoBehaviour
{
    public static string BASE_URL= "http://172.105.35.50/love_read/";
    public static string REGISTER = BASE_URL + "api/register";
    public static string LOGIN = BASE_URL + "api/login";
    public static string FORGOT_PASSWORD = BASE_URL + "api/forgotPassword";
    public static string userkey = "userkey";
    public static string RESEND_VARIFICATION = BASE_URL + "api/resendactivelink";
    public static int SelectedPlayerIndex=4;

    public static string SPLASH_SCENE= "0_SplashScene";
    public static string MAIN_MENU_SCENE= "1_MainMenuScene";
    public static string AUTHENTICATION_SCENE= "2_AuthenticationScene";
    
}

/*************************************************/
/************** LOGIN STARTS *************/

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
/************** LOGIN ENDS *************/
/*************************************************/



/*************************************************/
/************** REGISTER STARTS *************/

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

/************** REGISTER ENDS *************/
/*******************************************/


/*************************************************/
/************** RESEND LINK STARTS *************/

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

/************** RESEND LINK ENDS *************/
/*******************************************/
