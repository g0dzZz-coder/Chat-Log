using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LOG;

public class Chat : MonoBehaviour
{
    [Tooltip("The log to which this chat is attached")]
    [SerializeField] Log log;
    [Tooltip("Your timer. If empty, then the built-in")]
    [SerializeField] Timer timer;

    [SerializeField] Image imgInputField;

    [SerializeField] TMP_InputField inputField;

    [Tooltip("Button to send a message")]
    [SerializeField] KeyCode chatKey;

    [Space(7)]
    [SerializeField] string namePlayer;
    [Tooltip("Pause between messages to prevent spam")]
    [SerializeField] int freezeTime;

    private float deltaTime;
    private bool canSend;

    private void Start()
    {
        canSend = true;
        deltaTime = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(chatKey))
        {
            Send();
        }
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
            StartCoroutine(CannotSend(0.05f));
    }

    // Redness of the chat during early sending.
    IEnumerator CannotSend(float delay)
    {
        Color curColor = imgInputField.color;
        Color tempColor = imgInputField.color;

        while (imgInputField.color.r < 0.5f)
        {
            imgInputField.color = new Color(curColor.r += 0.1f, curColor.g, curColor.b, curColor.a);
            yield return new WaitForSeconds(delay);
        }
        while (imgInputField.color.r > tempColor.r)
        {
            imgInputField.color = new Color(curColor.r -= 0.1f, curColor.g, curColor.b, curColor.a);
            yield return new WaitForSeconds(delay);
        }
    }

    // Pause between sending messages.
    IEnumerator StopSpam(float delay)
    {
        while (!canSend)
        {
            deltaTime += delay;
            if (deltaTime > freezeTime)
            {
                deltaTime = 0;
                canSend = true;
            }
            yield return new WaitForSeconds(delay);
        }
    }
}
