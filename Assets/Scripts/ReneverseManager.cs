using Rene.Sdk;
using Rene.Sdk.Api.Game.Data;
using ReneVerse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ReneverseManager : MonoBehaviour
{
    public static string EmailHandler;

    // The TextMeshPro Inputfield
    public GameObject Email;
    // The Timer Text Field which would update every second
    public TextMeshProUGUI Timer;

    // The parent panet which contains both sign in and countdown panel
    public GameObject LogInPanel;
    // The Countdown panel which contains the timer
    public GameObject CountdownPanel;

    private bool LoginStatus = false; // Declaration of LoginStatus


    API ReneAPI;

    async Task ConnectUser()
    {
        ReneAPI = ReneAPIManager.API();
        EmailHandler = Email.GetComponent<TMP_InputField>().text;
        bool connected = await ReneAPI.Game().Connect(EmailHandler);
        Debug.Log(connected);
        if (!connected) return;
        StartCoroutine(ConnectReneService(ReneAPI));
    }

    private IEnumerator ConnectReneService(API reneApi)
    {
        CountdownPanel.SetActive(true);
        var counter = 30;
        var userConnected = false;
        var secondsToDecrement = 1;
        while (counter >= 0 && !userConnected)
        {
            Timer.text = counter.ToString();
            if (reneApi.IsAuthorized())
            {

                CountdownPanel.SetActive(false);
                LogInPanel.SetActive(false);


                // Will Fetch our assets from here


                userConnected = true;
                LoginStatus = true;
            }

            yield return new WaitForSeconds(secondsToDecrement);
            counter -= secondsToDecrement;
        }
        CountdownPanel.SetActive(false);
    }
}
