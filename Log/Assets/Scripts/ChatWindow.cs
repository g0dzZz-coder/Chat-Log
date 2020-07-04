using System.Collections;
using TMPro;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

namespace ChatForStrategy
{
    public class ChatWindow : MonoBehaviour
    {
        [SerializeField, Tooltip("Is chat open when the scene starts")]
        private bool chatIsOpen = false;
        public bool IsOpen { get => chatIsOpen; set => chatIsOpen = value; }

        [SerializeField, Tooltip("The log to which this chat is attached")]
        private Log log = null;

        [SerializeField]
        private TMP_InputField inputFieldComp = null;
        [SerializeField, Tooltip("Button to send a message")]
        private KeyCode chatSendKey = KeyCode.Return;

        [SerializeField]
        private Scrollbar scrollbar = null;

        [SerializeField]
        private Animation _animation = null;

        public void Awake()
        {
            Player.OnMessage += OnPlayerMessage;
        }

        private void Start()
        {
            if (chatIsOpen)
                OpenChat();
            else
                CloseChat();
        }

        private void Update()
        {
            if (Input.GetKeyDown(chatSendKey))
            {
                if (chatIsOpen)
                {
                    if (inputFieldComp.text == "")
                        CloseChat();
                    else
                        OnSend();
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

            EnableInputField();
        }

        public void CloseChat()
        {
            _animation.Play("CloseChat");
            chatIsOpen = false;

            DisableInputField();
        }

        public void OnSend()
        {
            if (inputFieldComp.text.Trim() == "")
                return;

            // Get our player.
            Player player = NetworkClient.connection.identity.GetComponent<Player>();

            // Send a message.
            player.CmdSend(inputFieldComp.text.Trim());

            inputFieldComp.text = "";

            EnableInputField();
        }

        private void OnPlayerMessage(Player player, string message)
        {
            //string prettyMessage = player.isLocalPlayer ?
            //    $"<color=red>{player.playerName}: </color> {message}" :
            //    $"<color=blue>{player.playerName}: </color> {message}";
            //AppendMessage(prettyMessage);

            string prettyMessage = $"{player.playerName}: {message}";

            if (player.isLocalPlayer)
                AppendMessage(prettyMessage, true);
            else
                AppendMessage(prettyMessage, false);

            Debug.Log(message);
        }

        private void EnableInputField()
        {
            inputFieldComp.readOnly = false;
            inputFieldComp.Select();
            inputFieldComp.ActivateInputField();
        }

        private void DisableInputField()
        {
            inputFieldComp.readOnly = true;
            inputFieldComp.DeactivateInputField();
        }

        internal void AppendMessage(string message, bool isLocal)
        {
            StartCoroutine(AppendAndScroll(message, isLocal));
        }

        private IEnumerator AppendAndScroll(string message, bool isLocal)
        {
            if (isLocal)
                log.AddMessage(message);
            else
                log.AddOpponentMessage(message);

            // It takes 2 frames for the UI to update?!?!
            yield return null;
            yield return null;

            // Slam the scrollbar down.
            scrollbar.value = 0;
        }
    }
}
