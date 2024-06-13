using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public string[] dialogue;
    public DialogueManager dialogueManager;
    public Image dialogueImage;
    private bool playerIsClose = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            dialogueManager.EndDialogue();
        }
    }

    private void Update()
    {
        if (playerIsClose && Input.GetKeyDown(KeyCode.Space))
        {
            if (!dialogueManager.hasStartedConversation || dialogueManager.hasFinishedConversation)
            {
                StartDialogue();
            }
            else if (!dialogueManager.isTyping)
            {
                dialogueManager.DisplayNextLine();
            }
        }
    }

    private void StartDialogue()
    {
        dialogueManager.StartDialogue(dialogue, dialogueImage);
    }


}
