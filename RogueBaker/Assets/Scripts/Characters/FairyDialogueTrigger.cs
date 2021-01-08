using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyDialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    bool playerInRange;
    public bool alreadyTalked = false;
    public bool convoStarted = false;
    public bool firstMeeting = true;

    [Header("Different Dialogue")]
    [TextArea(3, 10)]
    public string[] introductionDialogue;
    [TextArea(3, 10)]
    public string[] reviveDialogue;
    [TextArea(3, 10)]
    public string[] removedFrogDialogue;
    [TextArea(3, 10)]
    public string[] notEnoughRupeesDialogue;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<DialogueManager>().ConvoFinished = false;
        alreadyTalked = false;
        convoStarted = false;

        if (GameController.instance.firstMeetingFairy)
        {
            GameController.instance.firstMeetingFairy = false;
            setDialogue(introductionDialogue);
        }
        else
        {
            if (GameController.instance.playerDead)
            {
                setDialogue(reviveDialogue);
                GameController.instance.playerDead = false;
            }

            if(GameController.instance.rupeeAmount < 4)
            {
                setDialogue(notEnoughRupeesDialogue);
            }

            if(GameController.instance.rupeeAmount >= 4)
            {
                setDialogue(removedFrogDialogue);
                GameController.instance.removeTimer();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        continueTalk();
    }

    void continueTalk()
    {
        if (!FindObjectOfType<DialogueManager>().ConvoFinished && Input.GetButtonDown("Attack") && playerInRange)
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
            FindObjectOfType<AudioManager>().Play("Fairy");
            if (FindObjectOfType<DialogueManager>().ConvoFinished)
            {
                alreadyTalked = true;
            }
        }
    }

    void startTalk()
    {
        if (!FindObjectOfType<DialogueManager>().ConvoFinished)
        {
            playerInRange = true;
            if (!convoStarted)
            {
                FindObjectOfType<DialogueManager>().StartConvo(dialogue);
                convoStarted = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!alreadyTalked)
            {
                startTalk();
            }
        }
    }

    private void setDialogue(string[] setDialogue)
    {
        FindObjectOfType<DialogueManager>().sentences.Clear();
        for (int c = 0; c < dialogue.sentences.Length; c++)
        {
            dialogue.sentences[c] = "";
        }
        dialogue.sentences = new string[setDialogue.Length];

        for (int c = 0; c < setDialogue.Length; c++)
        {
            dialogue.sentences[c] = setDialogue[c];
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
