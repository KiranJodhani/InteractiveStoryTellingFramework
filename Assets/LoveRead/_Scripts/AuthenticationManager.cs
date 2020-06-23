using System.Collections;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Michsky.UI.ModernUIPack;

public class AuthenticationManager : MonoBehaviour
{

    [Header("####### API INSTANCES ###########")]
    public login_request login_Request_Instance;
    public login_response login_Response_Instance;
    public register_request register_Request_Instance;
    public register_response register_Response_Instance;
    public forgot_password_request forgot_password_request_Instance;
    public forgot_password_response forgot_password_response_Instance;
    public resend_varification_request resend_varification_request_Instance;
    public resend_varification_response resend_varification_response_Instance;

    [Header("####### SCREENS ###########")]
    public GameObject MainMenuScreen;
    public GameObject ForgotPasswordScreen;

    [Header("####### LOGIN TEXTBOXES ###########")]
    public TMP_InputField username_InputField_login;
    public TMP_InputField password_InputField_login;

    [Header("####### REGISTER TEXTBOXES ###########")]
    public TMP_InputField name_InputField;
    public TMP_InputField username_InputField;
    public TMP_InputField email_InputField;
    public TMP_InputField password_InputField;
    public const string MatchEmailPattern =
      @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
      + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
      + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
      + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    [Header("####### FORGOT PASSWORD TEXTBOXES ###########")]
    public TMP_InputField username_InputField_fp;

    [Header("####### MODERN UI ###########")]
    public WindowManager WindowManagerInstance;
    public GameObject LoadingScreen;
    public NotificationManager NotificationManagerInstance;
    public Animator notificationAnimator;
    public TextMeshProUGUI NotificationText;

    public GameObject AlertWindow;
    public ModalWindowManager ModalWindowManagerInstance;
    public TextMeshProUGUI AlertWindowText;

    [Header("####### SCREEN TRANSITION ###########")]
    public GameObject ScreenTransitionAnimation;
    private string panelFadeIn = "Panel Open";


    void Start()
    {
        //if (PlayerPrefs.HasKey(LoveRead_Backend.userkey))
        //{
        //    PlayerName = PlayerPrefs.GetString(LoveRead_Backend.userkey);
        //    LoadMainMenu();
        //}
        //else
        //{
        //    LoginScreen.SetActive(true);
        //}
    }


    void ShowModalWindow(string msg)
    {
        ModalWindowManagerInstance.descriptionText = msg;
        AlertWindowText.text = msg;
        Invoke("ShowModalWindowDelayed", 0.1f);
    }

    void ShowModalWindowDelayed()
    {
        ModalWindowManagerInstance.OpenWindow();
    }

    public void OpenNotification(string msg,int length)
    {
        NotificationManagerInstance.description = msg;
        NotificationText.text = msg;
        HideLoadingScreen();
        Invoke("OpenNotificationDelayed", 0.1f);
        Invoke("CloseNotification", length);
        Invoke("HideNotificationScreen", length + 1);
    }
    void OpenNotificationDelayed()
    {
        NotificationManagerInstance.gameObject.SetActive(true);
        notificationAnimator.Play("In");
    }

    void CloseNotification()
    {
        notificationAnimator.Play("Out");
    }

    void HideNotificationScreen()
    {
        NotificationManagerInstance.gameObject.SetActive(true);
    }

    void LoadMainMenu()
    {
        //PlayerPrefs.SetString(LoveRead_Backend.userkey, PlayerName);
        //PlayerPrefs.Save();
        //SceneManager.LoadScene("MainMenuScene");
    }

    void HideAllScreen()
    {
        MainMenuScreen.SetActive(false);
        ForgotPasswordScreen.SetActive(false);
    }

    void ShowLoadingScreen()
    {
        LoadingScreen.SetActive(true);
    }

    void HideLoadingScreen()
    {
        LoadingScreen.SetActive(false);
    }


    public void OnClickLoginButton()
    {
        if(username_InputField_login.text=="" || username_InputField_login.text == null)
        {
            OpenNotification("Please Enter username or email",1);
            return;
        }

        if (password_InputField_login.text == "" || password_InputField_login.text == null)
        {
            OpenNotification("Please Enter password",1);
            return;
        }
        ShowLoadingScreen();
        login_Request_Instance.email = username_InputField_login.text;
        login_Request_Instance.password = password_InputField_login.text;
        string json = JsonUtility.ToJson(login_Request_Instance);
        StartCoroutine(LoginRequestCo(json));
    }

    IEnumerator LoginRequestCo(string Bodyjson)
    {
        Debug.Log("#### LOGIN REQUEST : " + Bodyjson);
        var request = new UnityWebRequest(LoveRead_Backend.LOGIN, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(Bodyjson);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send();
        Debug.Log("#### LOGIN RESPONSE: " + request.downloadHandler.text);
        login_Response_Instance = JsonUtility.FromJson<login_response>(request.downloadHandler.text);
        if (request.downloadHandler.text == "")
        {
            OpenNotification("Server Error", 1);
        }
        else if (login_Response_Instance.status == "true")
        {
            HideAllScreen();
            OpenNotification(login_Response_Instance.message, 2);
        }
        else if (login_Response_Instance.status == "false")
        {
            OpenNotification(login_Response_Instance.message, 2);
            if (login_Response_Instance.data.is_varified==0  && login_Response_Instance.data.email != ""
                    && login_Response_Instance.data.email != null)
            {
                ShowModalWindow(login_Response_Instance.message+". \nIf you wish to get new link Please click on Resend button.");
            }
        }
    }

    public void OnClickRegisterButton()
    {
        HideAllScreen();
        name_InputField.text = "";
        username_InputField.text = "";
        email_InputField.text = "";
        password_InputField.text = "";
    }

    public void OnClickSignUpButton()
    {

        if (username_InputField.text == "" || username_InputField.text == null)
        {
            OpenNotification("Please Enter username",1);
            return;
        }

        if (name_InputField.text == "" || name_InputField.text == null)
        {
            OpenNotification("Please Enter name",1);
            return;
        }

        if (email_InputField.text == "" || email_InputField.text == null)
        {
            OpenNotification("Please Enter email",1);
            return;
        }
        else
        {
            if (!validateEmail(email_InputField.text))
            {
                OpenNotification("Please Enter valid email",1);
                return;
            }
        }

        if (password_InputField.text == "" || password_InputField.text == null)
        {
            OpenNotification("Please Enter password",1);
            return;
        }

        ShowLoadingScreen();
        register_Request_Instance.username = username_InputField.text;
        register_Request_Instance.name = name_InputField.text;
        register_Request_Instance.email = email_InputField.text;
        register_Request_Instance.password = password_InputField.text;
        string json = JsonUtility.ToJson(register_Request_Instance);
        StartCoroutine(RegisterRequestCo(json));
    }


    IEnumerator RegisterRequestCo(string Bodyjson)
    {
        Debug.Log("#### REGISTRATION REQUEST : " + Bodyjson);
        var request = new UnityWebRequest(LoveRead_Backend.REGISTER, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(Bodyjson);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.Send();
        Debug.Log("#### PROFILE Response: " + request.downloadHandler.text);
        register_Response_Instance = JsonUtility.FromJson<register_response>(request.downloadHandler.text);
        if (request.downloadHandler.text == "")
        {
            OpenNotification("Server Error",1);
        }
        else if (register_Response_Instance.status == "true")
        {
            OpenNotification(register_Response_Instance.message,3);
        }
        else if (register_Response_Instance.status == "false")
        {
            OpenNotification(register_Response_Instance.message,2);
        }
    }
    public static bool validateEmail(string email)
    {
        if (email != null)
        {
            return Regex.IsMatch(email, MatchEmailPattern);
        }
        else
        {
            return false;
        }
    }
    
    public void OnClickForgotPassword()
    {
        PlayScreenTransitionAnimation();
        Invoke("OpenForgotPasswordScreen", 1);
    }

    void OpenForgotPasswordScreen()
    {
        HideAllScreen();
        ForgotPasswordScreen.SetActive(true);
        username_InputField_fp.text = "";
    }


    public void OnClickResendButton()
    {
        ShowLoadingScreen();
        resend_varification_request_Instance.email = username_InputField_login.text;
        string json = JsonUtility.ToJson(resend_varification_request_Instance);
        StartCoroutine(ResendVarificationCo(json));
    }

    IEnumerator ResendVarificationCo(string Bodyjson)
    {
        Debug.Log("#### RESEND VARIFICATION REQUEST : " + Bodyjson);
        var request = new UnityWebRequest(LoveRead_Backend.RESEND_VARIFICATION, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(Bodyjson);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("#### RESEND VARIFICATION RESPONSE: " + request.downloadHandler.text);
        resend_varification_response_Instance = JsonUtility.FromJson<resend_varification_response>(request.downloadHandler.text);
        if (request.downloadHandler.text == "")
        {
            OpenNotification("Server Error", 1);
        }
        else if (resend_varification_response_Instance.status == "true")
        {
            OpenNotification(resend_varification_response_Instance.message, 2);
        }
        else if (resend_varification_response_Instance.status == "false")
        {
            OpenNotification(resend_varification_response_Instance.message, 2);
        }
    }


    public void FP_OnClickSubmitButton()
    {
        if (username_InputField_fp.text == "" || username_InputField_fp.text == null)
        {
            OpenNotification("Please Enter username or Email",1);
            return;
        }
        else
        {
            ShowLoadingScreen();
            forgot_password_request_Instance.email = username_InputField_fp.text;
            string json = JsonUtility.ToJson(forgot_password_request_Instance);
            StartCoroutine(FP_OnClickSubmitButtonCo(json));
        }
    }

    IEnumerator FP_OnClickSubmitButtonCo(string Bodyjson)
    {
        Debug.Log("#### FORGET PASSWORD REQUEST : " + Bodyjson);
        var request = new UnityWebRequest(LoveRead_Backend.FORGOT_PASSWORD, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(Bodyjson);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("#### FORGET PASSWORD RESPONSE: " + request.downloadHandler.text);
        forgot_password_response_Instance = JsonUtility.FromJson<forgot_password_response>(request.downloadHandler.text);
        if (request.downloadHandler.text == "")
        {
            OpenNotification("Server Error", 1);
        }
        else if (forgot_password_response_Instance.status == "true")
        {
            OpenNotification(forgot_password_response_Instance.message, 2);
        }
        else if (forgot_password_response_Instance.status == "false")
        {
            OpenNotification(forgot_password_response_Instance.message, 2);
        }
    }

    public void OnClickBackFromMainMenuScreen()
    {
        HideAllScreen();
    }

    public void FP_BackButton()
    {
        PlayScreenTransitionAnimation();
        Invoke("OpenMainMenu", 1);
    }

    void OpenMainMenu()
    {
        HideAllScreen();
        MainMenuScreen.SetActive(true);
        WindowManagerInstance.OpenPanel("Register");
    }

    public void PlayScreenTransitionAnimation()
    {
        ScreenTransitionAnimation.SetActive(true);
        ScreenTransitionAnimation.GetComponent<Animator>().Play("");
        ScreenTransitionAnimation.GetComponent<Animator>().Play(panelFadeIn);
        Invoke("ResetScreenTransition", 2f);
    }

    void ResetScreenTransition()
    {
        ScreenTransitionAnimation.SetActive(false);
    }

}
