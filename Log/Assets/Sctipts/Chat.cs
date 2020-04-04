using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LOG;

public class Chat : MonoBehaviour
{
    [Tooltip("Is chat open when the scene starts")]
    public bool ChatIsOpen = false;

    [Tooltip("The log to which this chat is attached")]
    [SerializeField] Log log;

    [SerializeField] Image imgInputField;

    [SerializeField] TMP_InputField inputField;

    [Tooltip("Button to send a message")]
    [SerializeField] KeyCode chatSendKey = KeyCode.Return;

    [Space(10)]
    [SerializeField] string namePlayer = "Player";
    [Tooltip("Pause between messages to prevent spam")]
    [SerializeField] int freezeTime = 5;

    private Animation _animation;

    private float timeAfterBlocking;

    private bool canSend;

    void Start()
    {
        _animation = GetComponent<Animation>();

        if (ChatIsOpen)
            OpenChat();
        else
            CloseChat();

        canSend = true;
        timeAfterBlocking = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(chatSendKey) && ChatIsOpen)
        {
            Send();
        }
    }

    public void OpenChat()
    {
        _animation.Play("OpenChat");
    }

    public void CloseChat()
    {
        _animation.Play("CloseChat");
    }

    public void Send()
    {
        if (canSend)
        {
            if (inputField.text != "")
            {
                canSend = false;
                log.AddMessage(namePlayer, inputField.text);
                inputField.text = "";

                StartCoroutine(StopSpam(0.01f));
            }
        }
        else
        {
            // Redness of the chat during early sending.
            _animation.Play("ChatIsBlocked");
        }
    }

    // Pause between sending messages.
    IEnumerator StopSpam(float delay)
    {
        while (!canSend)
        {
            timeAfterBlocking += delay;
            if (timeAfterBlocking > freezeTime)
            {
                timeAfterBlocking = 0;
                canSend = true;
            }
            yield return new WaitForSeconds(delay);
        }
    }
}
