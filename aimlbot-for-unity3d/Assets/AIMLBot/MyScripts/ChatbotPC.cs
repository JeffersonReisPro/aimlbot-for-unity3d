using AIMLbot;
using System;
using System.IO;
using UnityEngine;


/*

    Import AIML files within the StreamingAssets

*/



public class ChatbotPC
{
    const string UserId = "consoleUser";
    public Bot AimlBot;
    public User myUser;

    public string pathToUserSettings;

    public ChatbotPC()
    {
        AimlBot = new Bot();
        myUser = new User(UserId, AimlBot);
        Initialize();
        if (Application.isEditor == false)
        {
            pathToUserSettings = Application.streamingAssetsPath + @"\Brain-Graphmaster.xml";
        }
        else
        {
            pathToUserSettings = Application.persistentDataPath + @"\Brain-Graphmaster.xml";
        }
    }

    // Loads all the AIML files in the \AIML folder         
    public void Initialize()
    {
        AimlBot.ChangeMyPath = Application.streamingAssetsPath;
        AimlBot.loadSettings();
        AimlBot.isAcceptingUserInput = false;
        AimlBot.loadAIMLFromFiles();
        AimlBot.isAcceptingUserInput = true;
    }

    // Given an input string, finds a response using AIMLbot lib
    public String getOutput(String input)
    {
        Request r = new Request(input, myUser, AimlBot);
        Result res = AimlBot.Chat(r);
        return (res.Output);
    }

    public void SaveBrain()
    {
        try
        {
            myUser.Predicates.DictionaryAsXML.Save(pathToUserSettings);
            Debug.Log("Brain saved");
        }
        catch (Exception e)
        {
            Debug.Log("Brain not saved");
            Debug.Log(e);
        }
    }


    public void LoadBrain()
    {
        try
        {
            myUser.Predicates.loadSettings(pathToUserSettings);
            Debug.Log("Brain loaded");
        }
        catch (Exception e)
        {
            Debug.Log("Brain not loaded");
            Debug.Log(e);
        }
    }

}