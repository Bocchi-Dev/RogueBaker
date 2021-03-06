﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingDialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject invisibleWall;

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

        if (GameController.instance.firstMeetingKing)
        {
            GameController.instance.firstMeetingKing = false;
            setDialogue(introductionDialogue);
            invisibleWall.SetActive(true);
        }
        else
        {
            if (GameController.instance.rebellion)
            {
                setDialogue(rebellionDialogue);
                invisibleWall.SetActive(true);
                GameController.instance.bossFightTime = true;
            }
            else
            {
                if (GameController.instance.inventoryFull)
                {
                    setDialogue(commandDialogue);
                    invisibleWall.SetActive(true);
                    GameController.instance.strengthenKing();
                }
                else
                {
                    setDialogue(collectRemainingIngredientsDialogue);
                    invisibleWall.SetActive(false);
                }
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
            FindObjectOfType<AudioManager>().Play("King");
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
                invisibleWall.SetActive(false);

                GameController.instance.timerPanel.SetActive(true);
                GameController.instance.taskbar.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (!alreadyTalked)
            {
                if (GameController.instance.inventoryFull)
                {
                    GameController.instance.emptyInventory();
                    GameController.instance.resetTimer();
                }
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
