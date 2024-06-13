using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public GameObject continueButton;
    [SerializeField] private Image dialogueImageIcon;
    public Animator dialogueAnim;
    public float wordSpeed;
    public bool hasFinishedConversation;
    public bool isTyping;
    public bool hasStartedConversation;


    private string[] currentDialogueLines;
    public int currentLineIndex = 0;

    public void StartDialogue(string[] dialogueLines, Image image)
    {
        if (!hasStartedConversation)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.npcSound, 0.1f);
            dialogueText.text = "";
            hasStartedConversation = true;
            dialogueImageIcon = image;
            hasFinishedConversation = false;
            dialogueAnim.SetTrigger("show");
            currentDialogueLines = dialogueLines;
            currentLineIndex = 0;
            DisplayNextLine();
        }
    }

    public void DisplayNextLine()
    {
            SoundManager.Instance.PlaySound(SoundManager.Instance.npcSound, 0.1f, Random.Range(0, 0.05f));

        continueButton.SetActive(false);
        if (currentLineIndex >= currentDialogueLines.Length - 1)
        {
            hasFinishedConversation = true;
        }
        if (currentLineIndex < currentDialogueLines.Length - 1)
        {
            dialogueText.text = "";
            currentLineIndex++;
            StartCoroutine(Typing());
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        hasStartedConversation = false;
        hasFinishedConversation = true;
        isTyping = false;
        dialogueText.text = "";
        currentLineIndex = 0;
        dialogueAnim.SetTrigger("hide");
    }

    public IEnumerator Typing()
    {
        foreach (char letter in currentDialogueLines[currentLineIndex].ToCharArray())
        {
            dialogueText.text += letter;
            isTyping = true;
            if (dialogueText.text == currentDialogueLines[currentLineIndex])
            {
                isTyping = false;

                if (currentLineIndex == currentDialogueLines.Length - 1)
                {
                    continueButton.SetActive(false);
                }
                else
                {
                    continueButton.SetActive(true);
                }
            }
            yield return new WaitForSeconds(wordSpeed);
        }
    }
}
