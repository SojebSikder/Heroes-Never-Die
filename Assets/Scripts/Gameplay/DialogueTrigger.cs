using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    // void Start()
    // {
    //     TriggerDialogue();
    // }

    public void TriggerDialogue(bool gameOver = false)
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, gameOver);
    }
}
