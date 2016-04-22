using UnityEngine;
using System.Collections;

/*

    Import AIML files within the StreamingAssets

*/


public class ChatWindowExamplePC : MonoBehaviour
{
    private ChatbotPC bot;
    public GUISkin myskin;
    private string messBox = "", messBoxAnswer = "", ask = "", user = "Me";
    private Rect windowRect;

    // Use this for initialization
    void Start()
    {
        bot = new ChatbotPC();
        bot.LoadBrain();
    }

    void OnGUI()
    {
        GUI.skin = myskin;

        GUI.Label(new Rect(10, 10, Screen.width, Screen.height), "Example PC/Desktop");

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

    void OnDisable()
    {
        bot.SaveBrain();
    }

}
