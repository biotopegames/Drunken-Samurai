using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public string[] dialogue;
    public DialogueManager dialogueManager;
    public Image dialogueImage;
    public string npcName;
    private bool playerIsClose = false;
    [SerializeField] private Patrolling patrolling;

    void Start()
    {
        if (GetComponent<Patrolling>() != null)
        {
            patrolling = GetComponent<Patrolling>();
        }
    }

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
        if(patrolling != null && dialogueManager.hasStartedConversation)
        {
            patrolling.isTalking = true;
        }



        if (playerIsClose && Input.GetKeyDown(KeyCode.E))
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

        // Check if NPC is in dialogue
        if (patrolling != null && dialogueManager.hasStartedConversation)
        {
            patrolling.StopPatrol();
        }

        if (patrolling != null && !dialogueManager.hasStartedConversation || patrolling != null && dialogueManager.hasFinishedConversation)
        {
            patrolling.StartPatrol();
        }
    }

    private void StartDialogue()
    {
        dialogueManager.StartDialogue(dialogue, npcName, dialogueImage);
    }
}
