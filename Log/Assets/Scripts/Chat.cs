using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LOG;

public class Chat : MonoBehaviour
{
    [SerializeField, Tooltip("Is chat open when the scene starts")]
    private bool chatIsOpen = false;
    public bool IsOpen { get => chatIsOpen; set => chatIsOpen = value; }

    [SerializeField, Tooltip("The log to which this chat is attached")]
    private Log log = null;

    [SerializeField]
    private TMP_InputField inputFieldComp = null;
    public string GetInputFieldText() { return inputFieldComp.text; }
    [SerializeField, Tooltip("Button to send a message")]
    private KeyCode chatSendKey = KeyCode.Return;

    [Space(10)]
    [SerializeField]
    private string namePlayer = "Player";
    [SerializeField]
    private string nameOpponent = "Opponent";

    [SerializeField]
    private bool useStopSpam = false;
    [SerializeField, Tooltip("Pause between messages to prevent spam")]
    private int freezeTime = 5;
    [SerializeField]
    private Animation _animation = null;

    private float timeAfterBlocking = 0f;
    private bool canSend = true;

    private Coroutine coroutine = null;
    private float delay = 0.1f;

    private void Start()
    {
        if (chatIsOpen)
            OpenChat();
        else
            CloseChat();

        canSend = true;
        timeAfterBlocking = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(chatSendKey))
        {
            if (chatIsOpen)
            {
                if (inputFieldComp.text == "")
                {
                    CloseChat();
                }
                else
                {
                    Send();
                }
            }
            else
            {
                OpenChat();
            }
        }
    }

    public void OpenChat()
    {
        _animation.Play("OpenChat");
        chatIsOpen = true;
        inputFieldComp.ActivateInputField();
    }

    public void CloseChat()
    {
        _animation.Play("CloseChat");
        chatIsOpen = false;
    }

    public void Send()
    {
        if (canSend)
        {
            log.AddMessage(namePlayer, inputFieldComp.text);
            inputFieldComp.text = "";

            if (useStopSpam)
            {
                canSend = false;
                StartStopSpamCoroutine(delay);
            }
        }
        else
        {
            // Redness of the chat during early sending.
            _animation.Play("ChatIsBlocked");
        }
    }

    private void StartStopSpamCoroutine(float delay)
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(StopSpam(delay));
        }
    }

    // Pause between sending messages.
    private IEnumerator StopSpam(float delay)
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

        coroutine = null;
    }
}
