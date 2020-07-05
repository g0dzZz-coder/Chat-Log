using System.Collections;
using TMPro;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.IO;
using System;

namespace ChatForStrategy
{
    public class ChatWindow : MonoBehaviour
    {
        [SerializeField, Tooltip("Is chat open when the scene starts")]
        private bool chatIsOpen = false;

        [SerializeField, Tooltip("Write messages to a file")]
        private bool logging = false;

        [Header("UI")]
        [SerializeField]
        private TMP_InputField inputFieldComp = null;
        [SerializeField]
        private Transform content = null;
        [SerializeField]
        private Scrollbar scrollbar = null;
        [SerializeField]
        private Animation _animation = null;

        [SerializeField, Tooltip("Button to send a message")]
        private KeyCode chatSendKey = KeyCode.Return;

        [SerializeField]
        private TypesOfMessages messageTypes = new TypesOfMessages();

        private float timer = 0f;

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
                        OnSendMessage();
                }
                else
                {
                    OpenChat();
                }
            }

            UpdateTime();
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

        public void AddSystemMessage(string text, int effect, float duration = 0f)
        {
            if (effect < 0)
            {
                AppendMessage(text, messageTypes.Warning);
            }
            else if (effect > 0)
            {
                AppendMessage(text, messageTypes.Notification);
            }
            else
            {
                AppendMessage(text, messageTypes.Process, duration);
            }
        }

        public void OnSendMessage()
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
                AppendMessage(prettyMessage, messageTypes.Message);
            else
                AppendMessage(prettyMessage, messageTypes.OpponentMessage);

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

        internal void AppendMessage(string message, MessageType type, float duration = 0f)
        {
            StartCoroutine(AppendAndScroll(message, type, duration));
        }

        private IEnumerator AppendAndScroll(string message, MessageType type, float duration = 0f)
        {
            if (type == messageTypes.Process)
                CreateProcess(message, duration);
            else
                CreateMessage(message, type);

            // It takes 2 frames for the UI to update?!?!
            yield return null;
            yield return null;

            // Slam the scrollbar down.
            scrollbar.value = 0;
        }

        private void CreateMessage(string text, MessageType type)
        {
            Message message = new Message 
            { 
                Text = text, 
                Time = FormattedTime(timer), 
                Type = type 
            };

            var obj = Instantiate(message.Type.Prefab, content);
            var messageComp = obj.GetComponent<MessageComponent>();
            messageComp.Init(message);

            // If logging is enabled, then write to the file.
            if (logging)
                SaveMessage(message);
        }

        // Filling process.
        private void CreateProcess(string text, float duration)
        {
            Process process = new Process 
            { 
                Text = text, 
                Duration = duration, 
                Time = FormattedTime(timer), 
                CompletionTime = FormattedTime(timer + duration), 
                Type = messageTypes.Process 
            };

            var obj = Instantiate(process.Type.Prefab, content);
            var processComp = obj.GetComponent<ProcessComponent>();
            processComp.Init(process);

            // If logging is enabled, then write to the file.
            if (logging)
                SaveProcess(process);
        }

        private void UpdateTime()
        {
            timer += Time.deltaTime;
        }

        private string FormattedTime(float timer)
        {
            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = Mathf.Floor(timer % 60).ToString("00");
            string time = string.Format("{0}:{1}", minutes, seconds);

            return time;
        }

        #region Save to file

        // File to write.
        private static string fileName = string.Format("{0}_{1}_{2}_{3}_{4}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute) + ".txt";

        // Logging a notice.
        private void SaveMessage(Message message)
        {
            string line = message.Time + " - " + message.Text;

            StreamWriter sw = new StreamWriter(Application.dataPath + "/Logs/" + fileName);
            sw.WriteLine(line);
            sw.Close();
        }

        // Logging a process;
        private void SaveProcess(Process process)
        {
            string line = process.CompletionTime + " - " + process.Text;

            StreamWriter sw = new StreamWriter(Application.dataPath + "/Logs/" + fileName);
            sw.WriteLine(line);
            sw.Close();
        }

        #endregion
    }
}
