using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI textDisplay;
    public GameObject continueButton;
    private Animator anime;

    [Header("Stuff")]
    public float typingSpeed;
    public bool ConvoFinished;

    [HideInInspector]
    public Queue<string> sentences;

    private void Start()
    {
        sentences = new Queue<string>();
        anime = GetComponent<Animator>();

        anime.SetBool("openDialogue", false);
    }

    public void StartCovo(Dialogue dialogue)
    {
        anime.SetBool("openDialogue", true);

        GameController.instance.ConversationActive = true;

        
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

    }

    public void nextSentence()
    {
        continueButton.SetActive(false);

        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
        }
    }
}
