using UnityEngine;
using System.Collections;
//
using System.Xml;
using System.Collections.Generic;

/*

    Import AIML files within the Resources

*/

public class ChatWindowExampleMobileWeb : MonoBehaviour
{

    private TextAsset[] aimlFiles;
    private List<string> aimlXmlDocumentListFileName = new List<string>();
    private List<XmlDocument> aimlXmlDocumentList = new List<XmlDocument>();
    //
    private TextAsset GlobalSettings, GenderSubstitutions, Person2Substitutions, PersonSubstitutions, Substitutions, DefaultPredicates, Splitters;
    //
    private ChatbotMobileWeb bot;
    public GUISkin myskin;
    private string messBox = "", messBoxAnswer = "", ask = "", user = "Me";
    private Rect windowRect;

    // Use this for initialization
    void Start()
    {
        bot = new ChatbotMobileWeb();
        LoadFilesFromConfigFolder();
        bot.LoadSettings(GlobalSettings.text, GenderSubstitutions.text, Person2Substitutions.text, PersonSubstitutions.text, Substitutions.text, DefaultPredicates.text, Splitters.text);
        TextAssetToXmlDocumentAIMLFiles();
        bot.loadAIMLFromXML(aimlXmlDocumentList.ToArray(), aimlXmlDocumentListFileName.ToArray());
        bot.LoadBrain();
    }


    // Update is called once per frame
    void Update()
    {


    }

    void OnGUI()
    {
        GUI.skin = myskin;

        GUI.Label(new Rect(10, 10, Screen.width, Screen.height), "Example Mobile and Web");

        // Width of the text box; This formula sets the width according to the screen size
        float W = (700 * Screen.height) / 800;
        // Height of the text box; This formula adjust the height according to the screen size
        float H = (210 * Screen.height) / 600;
        if (Screen.width < Screen.height)
        {
            windowRect = new Rect(0, Screen.height - H, Screen.width, H);
        }
        else {
            windowRect = new Rect(Screen.width / 2 - W / 2, Screen.height - H, W, H);
        }
        windowRect = GUI.Window(2, windowRect, windowFunc, "Chat");
    }

    private void windowFunc(int id)
    {
        // Question User
        GUILayout.Label(user + ": " + messBox);
        // Response bot
        GUILayout.Label("Bot: " + messBoxAnswer);
        //Skip a few lines to the box question becomes more below.
        //GUILayout.Label ("\n");
        //
        GUILayout.BeginHorizontal();
        // Where the player put the text
        ask = GUILayout.TextField(ask);
        //=================================================
        if (GUILayout.Button("Send", GUILayout.Width(75)))
        {
            messBox = ask;
            // Response Bot AIML
            var answer = bot.getOutput(ask);
            // Response BotAIml in the Chat window
            messBoxAnswer = answer;
            ask = "";
        }
        //==================================================
        GUILayout.EndHorizontal();
    }//close windowFunc


    void LoadFilesFromConfigFolder()
    {
        GlobalSettings = Resources.Load<TextAsset>("config/Settings");
        GenderSubstitutions = Resources.Load<TextAsset>("config/GenderSubstitutions");
        Person2Substitutions = Resources.Load<TextAsset>("config/Person2Substitutions");
        PersonSubstitutions = Resources.Load<TextAsset>("config/PersonSubstitutions");
        Substitutions = Resources.Load<TextAsset>("config/Substitutions");
        DefaultPredicates = Resources.Load<TextAsset>("config/DefaultPredicates");
        Splitters = Resources.Load<TextAsset>("config/Splitters");
    }

    void TextAssetToXmlDocumentAIMLFiles()
    {
        aimlFiles = Resources.LoadAll<TextAsset>("aiml");
        foreach (TextAsset aimlFile in aimlFiles)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(aimlFile.text);
            aimlXmlDocumentListFileName.Add(aimlFile.name);
            aimlXmlDocumentList.Add(xmlDoc);
        }
    }


    void OnDisable()
    {
        bot.SaveBrain();
    }


}
