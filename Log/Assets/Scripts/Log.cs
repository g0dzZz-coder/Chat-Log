using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System;
using System.Collections.Generic;

namespace ChatForStrategy
{
    [Serializable]
    public struct MessageType
    {
        public string Name;
        public Color Color;
    }

    public class Log : MonoBehaviour
    {
        #region Fields

        [Tooltip("Write messages to a file")]
        [SerializeField]
        private bool logging = false;
        [Tooltip("(Only with logging enabled) If true, then the log is saved along the path Users / Username / AppData / LocalLow / CompanyName / ProjectName /")]
        [SerializeField]
        private bool usePersistentDataPath = false;

        [Header("Prefabs")]
        [SerializeField]
        private Message prefabMessage = null;
        [SerializeField]
        private Process prefabProcess = null;

        [Space(10)]
        [SerializeField, Tooltip("Leave blank if you do not need to display the current time.")]
        private TextMeshProUGUI timerTextComp = null;
        [SerializeField]
        private ScrollRect scrollRect = null;
        [SerializeField]
        private Transform content = null;

        [Header("Types:")]
        [SerializeField]
        private List<MessageType> messageTypes = new List<MessageType>();

        private float timer = 0f;

        #endregion

        #region Unity Metods

        private void Awake()
        {
            InitDefaultTypes();
        }

        private void Start()
        {
            AddWarning("Game Started");
        }

        private void Update()
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            timer += Time.deltaTime;

            if (timerTextComp != null)
                timerTextComp.text = FormattedTime(timer);
        }

        #endregion

        #region Adding notices

        public void AddNotification(string text)
        {
            CreateMessage(text, messageTypes[0]);
        }

        public void AddWarning(string text)
        {
            CreateMessage(text, messageTypes[1]);
        }

        public void AddGoodNews(string text)
        {
            CreateMessage(text, messageTypes[2]);
        }

        public void AddMessage(string text)
        {
            CreateMessage(text, messageTypes[3]);
        }

        public void AddOpponentMessage(string text)
        {
            CreateMessage(text, messageTypes[4]);
        }

        public void AddProcess(string text, float duration)
        {
            CreateProcess(text, duration, messageTypes[5]);
        }

        #endregion

        #region Formation   

        private void InitDefaultTypes()
        {
            messageTypes.Add(new MessageType { Name = "Notificaton", Color = Color.yellow });
            messageTypes.Add(new MessageType { Name = "Warning", Color = Color.red });
            messageTypes.Add(new MessageType { Name = "GoodNews", Color = Color.green });
            messageTypes.Add(new MessageType { Name = "PlayerMessage", Color = Color.green });
            messageTypes.Add(new MessageType { Name = "OpponentMessage", Color = Color.red });
            messageTypes.Add(new MessageType { Name = "Progress", Color = Color.blue });
        }

        // Filling out the notice.
        private Message CreateMessage(string text, MessageType type)
        {
            var message = Instantiate(prefabMessage, content);

            message.Init(text, FormattedTime(timer), type.Color);

            ScrollBarDown();

            // If logging is enabled, then write to the file.
            if (logging)
                SaveMessage(message);

            return message;
        }

        // Filling process.
        private Process CreateProcess(string text, float duration, MessageType type)
        {
            var process = Instantiate(prefabProcess, content);

            process.Init(text, duration, FormattedTime(timer), FormattedTime(timer + duration), type.Color);

            ScrollBarDown();

            // If logging is enabled, then write to the file.
            if (logging)
                SaveProcess(process);

            return process;
        }

        // Forcibly lowers the scrollbar to the lower position.
        private void ScrollBarDown()
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0.0f;
        }

        private string FormattedTime(float timer)
        {
            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = Mathf.Floor(timer % 60).ToString("00");
            string time = string.Format("{0}:{1}", minutes, seconds);

            return time;
        }

        #endregion

        #region Save to file

        // File to write.
        private static string fileName = String.Format("{0}_{1}_{2}_{3}_{4}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute) + ".txt";

        // Logging a notice.
        private void SaveMessage(Message message)
        {
            string line = message.Time + " - " + message.Text;

            if (usePersistentDataPath)
            {
                StreamWriter swPersistent = new StreamWriter(Application.persistentDataPath + fileName);
                swPersistent.WriteLine(line);
                swPersistent.Close();
            }
            else
            {
                StreamWriter sw = new StreamWriter(Application.dataPath + "/Logs/" + fileName);
                sw.WriteLine(line);
                sw.Close();
            }
        }

        // Logging a process;
        private void SaveProcess(Process process)
        {
            string line = process.CompletionTime + " - " + process.Text;

            if (usePersistentDataPath)
            {
                StreamWriter swPersistent = new StreamWriter(Application.persistentDataPath + fileName);
                swPersistent.WriteLine(line);
                swPersistent.Close();
            }
            else
            {
                StreamWriter sw = new StreamWriter(Application.dataPath + "/Logs/" + fileName);
                sw.WriteLine(line);
                sw.Close();
            }
        }

        #endregion
    }
}