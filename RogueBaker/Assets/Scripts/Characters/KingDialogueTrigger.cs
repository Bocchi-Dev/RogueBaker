using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingDialogueTrigger : MonoBehaviour
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
    public string[] commandDialogue;
    [TextArea(3, 10)]
    public string[] collectRemainingIngredientsDialogue;
    [TextArea(3, 10)]
    public string[] rebellionDialogue;
    [TextArea(3, 10)]
    public string[] defeatDialogue;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<DialogueManager>().ConvoFinished = false;
        alreadyTalked = false;
        convoStarted = false;

        if (GameController.instance.firstMeeting)
        {
            GameController.instance.firstMeeting = false;
            setDialogue(introductionDialogue);
        }
        else
        {
            if (GameController.instance.rebellion)
            {
                setDialogue(rebellionDialogue);
            }
            else
            {
                if (GameController.instance.inventoryFull)
                {
                    setDialogue(commandDialogue);
                }
                else
                {
                    setDialogue(collectRemainingIngredientsDialogue);
                }
            }
        } 
    }

    // Update is called once per frame
    void Update()
    {
        continueTalk();

        if (alreadyTalked)
        {
            if (GameController.instance.inventoryFull)
            {
                GameController.instance.emptyInventory();
            }
        }
    }

    void continueTalk()
    {
        if (!FindObjectOfType<DialogueManager>().ConvoFinished && Input.GetButtonDown("Attack"))
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();

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
}
