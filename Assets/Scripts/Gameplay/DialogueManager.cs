using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    private bool gameOver = false;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;

    // private Queue<string> sentences;
    private Queue<KeyValuePair<string, string>> sentences;

    // Use this for initialization
    void Start()
    {
        // sentences = new Queue<string>();
        sentences = new Queue<KeyValuePair<string, string>>();
    }

    public void StartDialogue(Dialogue dialogue, bool gameOver = false)
    {
        animator.SetBool("IsOpen", true);
        this.gameOver = gameOver;

        // nameText.text = dialogue.name;

        sentences.Clear();

        foreach (KeyValuePair<string, string> conversation in dialogue.conversations)
        {
            sentences.Enqueue(new KeyValuePair<string, string>(conversation.Key, conversation.Value));
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue(gameOver);
            return;
        }

        KeyValuePair<string, string> sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(KeyValuePair<string, string> sentence)
    {
        nameText.text = sentence.Key;
        dialogueText.text = "";
        foreach (char letter in sentence.Value.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue(bool gameOver = false)
    {
        animator.SetBool("IsOpen", false);

        if (gameOver)
        {
            // GameManager.Instance.GoMainMenu();
            SceneManager.LoadScene("GameoverMenu");
        }
    }
}
