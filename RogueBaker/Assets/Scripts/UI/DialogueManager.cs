﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI dialogueText;
    public Image characterImage;
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

    public void StartConvo(Dialogue dialogue)
    {
        anime.SetBool("openDialogue", true);
        characterImage.sprite = dialogue.characterImage.sprite;

        GameController.instance.ConversationActive = true;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        try
        {
            string sentence = sentences.Dequeue();

            StopAllCoroutines();
            StartCoroutine(Type(sentence));
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }

        
    }

    public void EndConvo()
    {
        GameController.instance.ConversationActive = false;
        GameController.instance.tutorialDone = true;
        ConvoFinished = true;

        if (!GameController.instance.rebellion)
        {
            GameController.instance.gameBegins = true;
        }

        anime.SetBool("openDialogue", false);
    }

    IEnumerator Type(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        if(sentences.Count == 0)
        {
            EndConvo();
        }

    }
}
