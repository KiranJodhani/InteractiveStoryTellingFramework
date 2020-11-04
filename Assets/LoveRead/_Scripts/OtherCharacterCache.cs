using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherCharacterCache : MonoBehaviour
{
    public other_character[] other_Characters;


    public static OtherCharacterCache Instance;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
