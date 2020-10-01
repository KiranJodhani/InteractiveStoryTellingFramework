using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class MainCharacterCache : MonoBehaviour
{
    public static MainCharacterCache Instance;

    [Header("******* MAIN CHARACTER CACHE ********")]
    public TextAsset DummyResponse;
    public main_character main_character_instance;
    public DownloadClass DownloadClassInstance;
    
    public bool DoConvertOnly;


    [Header("******* DEVELOPR ONLY  ********")]
    public main_character main_character_instanceJSON;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        CreateFolderStucture();
    }

    void Start()
    {
        if(DoConvertOnly)
        {
            print(JsonUtility.ToJson(main_character_instanceJSON));
        }
        else
        {
            Invoke("InitMainCharacter", 1);
        }
    }

    void InitMainCharacter()
    {
        if (DummyResponse)
        {
            main_character_instance = JsonUtility.FromJson<main_character>(DummyResponse.text);
            StartCoroutine(DownloadMainCharacterImagesCo());
        }
        else
        {
            StartCoroutine(GetMainCharacterImagesCo());
        }
    }

    
    void CreateFolderStucture()
    {
        DownloadClassInstance.Root_Dir = Application.persistentDataPath + "/MainCharacter";
        try
        {
            if (!Directory.Exists(DownloadClassInstance.Root_Dir))
            {
                Directory.CreateDirectory(DownloadClassInstance.Root_Dir);
            }
        }
        catch (IOException ex)
        {
            print("Error Root : " + ex.Message);
        }

        for(int i = 0; i < DownloadClassInstance.items.Length;i++)
        {
            try
            {
                if (!Directory.Exists(DownloadClassInstance.Root_Dir+DownloadClassInstance.items[i].Dir))
                {
                    Directory.CreateDirectory(DownloadClassInstance.Root_Dir + DownloadClassInstance.items[i].Dir);
                }
            }
            catch (IOException ex)
            {
                print("Error : " + ex.Message);
            }
        }
    }

    IEnumerator GetMainCharacterImagesCo()
    {
        string RequestURL = LoveRead_Backend.MAIN_CHARACTER_IMAGES;
        Debug.Log("#### GET MAIN_CHARACTER_IMAGES REQUEST : " + RequestURL);
        var request = new UnityWebRequest(RequestURL, "GET");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("#### GET MAIN_CHARACTER_IMAGES RESPONSE : " + request.downloadHandler.text);
        main_character_instance = JsonUtility.FromJson<main_character>(request.downloadHandler.text);

        if (request.downloadHandler.text == "")
        {
            
        }
        else if (main_character_instance.code == "1")
        {
            StartCoroutine(DownloadMainCharacterImagesCo());
        }
        else if (main_character_instance.code == "0")
        {

        }
    }


    IEnumerator DownloadMainCharacterImagesCo()
    {
        UnityWebRequest www = new UnityWebRequest();
        string[] ImageURL_Array = new string[0];
        string[] LocalImages = new string[0];

        if (DownloadClassInstance.CurrentItem==0)
        {
            DownloadClassInstance.NumberOfImages = main_character_instance.body.Length;
            ImageURL_Array = main_character_instance.body[DownloadClassInstance.DownloadCounter].body_image.Split('/');
        }
        else if (DownloadClassInstance.CurrentItem == 1)
        {            
            DownloadClassInstance.NumberOfImages = main_character_instance.cloths.Length;
            if(DownloadClassInstance.items[DownloadClassInstance.CurrentItem].current_sub_item==0)
            {
                ImageURL_Array = main_character_instance.cloths[DownloadClassInstance.DownloadCounter].cloth_image.Split('/');
            }
            else if (DownloadClassInstance.items[DownloadClassInstance.CurrentItem].current_sub_item == 1)
            {
                ImageURL_Array = main_character_instance.cloths[DownloadClassInstance.DownloadCounter].cloth_icon.Split('/');
            }
        }
        LocalImages = Directory.GetFiles(DownloadClassInstance.Root_Dir + DownloadClassInstance.items[DownloadClassInstance.CurrentItem].Dir);
        bool IsImageFound = false;
        if (LocalImages.Length > 0)
        {
            for (int i = 0; i < LocalImages.Length; i++)
            {
                if (LocalImages[i].Contains(ImageURL_Array[ImageURL_Array.Length - 1]))
                {
                    www = UnityWebRequestTexture.GetTexture("file://" + LocalImages[i]);
                    IsImageFound = true;
                    break;
                }
                else
                {
                    IsImageFound = false;
                }
            }
        }
        else
        {
            IsImageFound = false;
        }

        if (!IsImageFound)
        {
            if (DownloadClassInstance.CurrentItem == 0)
            {
                www = UnityWebRequestTexture.GetTexture(main_character_instance.body[DownloadClassInstance.DownloadCounter].body_image);
            }
            else if (DownloadClassInstance.CurrentItem == 1)
            {
                if (DownloadClassInstance.items[DownloadClassInstance.CurrentItem].current_sub_item == 0)
                {
                    www = UnityWebRequestTexture.GetTexture(main_character_instance.cloths[DownloadClassInstance.DownloadCounter].cloth_image);
                }
                else if (DownloadClassInstance.items[DownloadClassInstance.CurrentItem].current_sub_item == 1)
                {
                    www = UnityWebRequestTexture.GetTexture(main_character_instance.cloths[DownloadClassInstance.DownloadCounter].cloth_icon);
                }
                
            }           
        }
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
            if (!IsImageFound)
            {
                File.WriteAllBytes(DownloadClassInstance.Root_Dir + DownloadClassInstance.items[DownloadClassInstance.CurrentItem].Dir +
ImageURL_Array[ImageURL_Array.Length - 1], www.downloadHandler.data);
            }

            if (DownloadClassInstance.CurrentItem == 0)
            {
                main_character_instance.body[DownloadClassInstance.DownloadCounter].body_sprite =
                     Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            }
            else if (DownloadClassInstance.CurrentItem == 1)
            {
                if (DownloadClassInstance.items[DownloadClassInstance.CurrentItem].current_sub_item == 0)
                {
                    main_character_instance.cloths[DownloadClassInstance.DownloadCounter].cloth_sprite =
     Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                }
                else if (DownloadClassInstance.items[DownloadClassInstance.CurrentItem].current_sub_item == 1)
                {
                    main_character_instance.cloths[DownloadClassInstance.DownloadCounter].cloth_icon_sprite =
     Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                }

            }
        }

        DownloadClassInstance.DownloadCounter++;
        if (DownloadClassInstance.DownloadCounter < DownloadClassInstance.NumberOfImages)
        {
            StartCoroutine(DownloadMainCharacterImagesCo());
        }
        else
        {
            if(DownloadClassInstance.items[DownloadClassInstance.CurrentItem].sub_item_length>1)
            {
                DownloadClassInstance.items[DownloadClassInstance.CurrentItem].current_sub_item++;
                if (DownloadClassInstance.items[DownloadClassInstance.CurrentItem].current_sub_item<
                    DownloadClassInstance.items[DownloadClassInstance.CurrentItem].sub_item_length)
                {
                    DownloadClassInstance.DownloadCounter=0;
                    StartCoroutine(DownloadMainCharacterImagesCo());
                }
            }
            else
            {
                DownloadClassInstance.DownloadCounter = 0;
                DownloadClassInstance.CurrentItem++;
                StartCoroutine(DownloadMainCharacterImagesCo());
            }
        }
    }

}


[Serializable]
public class DownloadClass
{
    public int CurrentItem;
    public int DownloadCounter;
    public int NumberOfImages;
    public string Root_Dir;
    public DownloadItem[] items;
}

[Serializable]
public class DownloadItem
{
    public string item_name;
    public int sub_item_length;
    public int current_sub_item;
    public string Dir;
}