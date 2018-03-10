using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Facebook.MiniJSON;

public class FacebookManager
{
    private static FacebookManager _Instance;
    public static FacebookManager Instance
    {
        get
        {
            if(_Instance == null)
                _Instance = new FacebookManager();
            return _Instance;
        }
    }

    public event FacebookInitiatedHandler FacebookInitiated;
    public event FacebookDataRetrievedHandler FacebookDataRetrieved;

    public string PlayerName { get; private set; }

    public FacebookManager()
    {
        PlayerName = null;
    }

    public void Init()
    {
        FB.Init(OnInitComplete, OnHideUnity);
    }

    private void OnInitComplete()
    {
        if(FacebookInitiated != null)
            FacebookInitiated();

        if(FB.IsLoggedIn)
        {
            Debug.Log("WELCOME!!");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        Debug.Log("Is Game Showing? " + isGameShown);
    }

    public void Login()
    {
        if(!FB.IsLoggedIn)
            FB.Login("", LoginCallback);
    }

    private void PlayerNameRequestCallback(FBResult result)
    {
        Dictionary<string, object> json = Json.Deserialize(result.Text) as Dictionary<string, object>;
        
        if(result.Error != null)
        {
            Debug.Log("ERRO!!");
            return;
        }
        
        PlayerName = (string)json["first_name"];

        if(FacebookDataRetrieved != null)
            FacebookDataRetrieved();
    }

    private void LoginCallback(FBResult result)
    {
        Debug.Log("LOGIN CALLBACK");
        if (result.Error != null)
            Debug.Log("Error Response:\n" + result.Error);
        else if (!FB.IsLoggedIn)
        {
            Debug.Log("Login cancelled by Player");
        }
        else
        {
            FB.API("/me?fields=first_name", Facebook.HttpMethod.GET, PlayerNameRequestCallback);
            Debug.Log("Login was successful!");
        }
    }
}

public delegate void FacebookInitiatedHandler();
public delegate void FacebookDataRetrievedHandler();