using UnityEngine;
using System.Collections;

using UnityEngine.UI;

/*

    Import AIML files within the StreamingAssets

*/


public class ChatWindowExamplePC : MonoBehaviour
{
    private ChatbotPC bot;
    public InputField inputField;
    public Text robotOutput;

    // Use this for initialization
    void Start()
    {
        bot = new ChatbotPC();
        bot.LoadBrain();
    }


    void OnDisable()
    {
        bot.SaveBrain();
    }


    /// <summary>
    /// Button to send the question to the robot
    /// </summary>
    public void SendQuestionToRobot()
    {
        if (string.IsNullOrEmpty(inputField.text) == false)
        {
            // Response Bot AIML
            var answer = bot.getOutput(inputField.text);
            // Response BotAIml in the Chat window
            robotOutput.text = answer;
            //
            inputField.text = string.Empty;
        }
    }


}
