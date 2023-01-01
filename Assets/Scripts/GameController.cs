using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameController : MonoBehaviour
{
    public Text timeField;
    public Text wordToFindField;
    public GameObject[] hangMan;
    public GameObject winText;
    public GameObject loseText;
    public GameObject resetButton;
    private float time;
    private string[] wordsLocal = { "MATT", "JOANNE", "ROBERT", "MARRY JANE" };
    private string[] words = File.ReadAllLines(@"Assets/words.txt");
    private string chosenWord;
    private string hiddenWord;
    private int fails;
    private bool gameEnd;

    // Start is called before the first frame update
    void Start()
    {

        chosenWord = words[Random.Range(0, words.Length)];
        for (int i = 0; i < chosenWord.Length; i++)
        {
            char letter = chosenWord[i];
            if (char.IsWhiteSpace(letter))
            {
                hiddenWord += " ";
            }
            else
            {
                hiddenWord += "_";
            }
        }
        wordToFindField.text = hiddenWord;
        Debug.Log("Word is " + chosenWord);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameEnd)
        {
            time += Time.deltaTime;
            timeField.text = time.ToString();
        }
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode.ToString().Length == 1)
        {
            string pressedLetter = e.keyCode.ToString();
            Debug.Log(pressedLetter);

            if (chosenWord.Contains(pressedLetter))
            {
                int pressedIndex = chosenWord.IndexOf(pressedLetter);
                while(pressedIndex != -1)
                {
                    hiddenWord = hiddenWord.Substring(0, pressedIndex) + pressedLetter + hiddenWord.Substring(pressedIndex + 1);
                    Debug.Log("hidden change : " + hiddenWord);

                    chosenWord = chosenWord.Substring(0, pressedIndex) + "_" + chosenWord.Substring(pressedIndex + 1);
                    Debug.Log("chosen change : " + chosenWord);

                    pressedIndex = chosenWord.IndexOf(pressedLetter);
                }
                wordToFindField.text = hiddenWord;
            }
            //add a hangman body part
            else
            {
                hangMan[fails].SetActive(true);
                fails++;
            }
            //Case lost game
            if(fails == hangMan.Length)
            {
                loseText.SetActive(true);
                resetButton.SetActive(true);
                gameEnd = true;
            }
            //Case won game
            if (!hiddenWord.Contains("_"))
            {
                gameEnd = true;
                winText.SetActive(true);
                resetButton.SetActive(true);
            }
        }
    }
}
