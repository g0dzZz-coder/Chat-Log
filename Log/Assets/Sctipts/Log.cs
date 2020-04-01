using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System;

namespace LOG
{
    public class Log : MonoBehaviour
    {
        #region Fields

        [Tooltip("Write messages to a file")]
        [SerializeField] bool logging;
        [Tooltip("true - use Timer.cs; false - use Time.deltaTime")]
        [SerializeField] bool ownTimer;

        [Header("Prefabs")]
        [SerializeField] GameObject prefabLogNotice;
        [SerializeField] GameObject prefabLogProcess;

        [Header("Chat")]
        [Tooltip("Is chat open?")]
        [SerializeField] bool chatIsOpen;
        [SerializeField] GameObject chat;
        [SerializeField] TextMeshProUGUI chatText;
        [Tooltip("Button to open / close chat")]
        [SerializeField] KeyCode chatKey;

        [Space(10)]

        [SerializeField] ScrollRect scrollRect;
        [SerializeField] Transform content;
        [SerializeField] Animation _animation;

        [Header("Colors:")]
        [SerializeField] Color colorMain;
        [SerializeField] Color colorWarning;
        [SerializeField] Color colorGoodNews;
        [SerializeField] Color colorMessages;
        [SerializeField] Color colorProcess;

        private float timer;

        #endregion

        #region Unity Metods

        void Start()
        {
            if (chatIsOpen)
                EnableChat();
            else
                DisableChat();

            AddWarning("[Game Started]");
        }

        void Update()
        {
            if (Input.GetKeyDown(chatKey))
            {
                if (chatIsOpen)
                    DisableChat();
                else
                    EnableChat();
            }

            if (!ownTimer)
            {
                timer += Time.deltaTime;
            }
        }

        #endregion

        #region Adding notices

        public void AddWarning(string text)
        {
            CreateNotice(text).GetComponent<Image>().color = colorWarning;
        }

        public void AddGoodNews(string text)
        {
            CreateNotice(text).GetComponent<Image>().color = colorGoodNews;
        }

        public void AddInformation(string text)
        {
            CreateNotice(text).GetComponent<Image>().color = colorMain;
        }

        public void AddMessage(string name, string text)
        {
            CreateNotice(name + ": " + text).GetComponent<Image>().color = colorMessages;
        }

        public void AddProcess(string text, float duration)
        {
            CreateProcess(text, duration).GetComponent<Image>().color = colorProcess;
        }

        #endregion

        #region Formation   

        // Filling out the notice.
        private GameObject CreateNotice(string text)
        {
            GameObject notice = Instantiate(prefabLogNotice) as GameObject;
            notice.transform.SetParent(content, false);                      

            if (ownTimer)
            {
                notice.GetComponent<Notice>().Init(text, Timer.result);
            }
            else
            {
                notice.GetComponent<Notice>().Init(text, FormattedTime);
            }

            ScrollBarDown();

            // If logging is enabled, then write to the file.
            if (logging)
                SaveNotice(notice);

            return notice;
        }

        // Filling process.
        private GameObject CreateProcess(string text, float duration)
        {
            GameObject process = Instantiate(prefabLogProcess) as GameObject;
            process.transform.SetParent(content, false);

            if (ownTimer)
            {
                process.GetComponent<Process>().Init(text, duration, Timer.result);
            }
            else
            {
                process.GetComponent<Process>().Init(text, duration, FormattedTime);
            }

            return process;
        }

        private void EnableChat()
        {
            chatIsOpen = true;
            _animation.Play("Expand Log");
        }

        private void DisableChat()
        {
            chatIsOpen = false;
            _animation.Play("Collapse Log");
        }

        // Forcibly lowers the scrollbar to the lower position.
        private void ScrollBarDown()
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0.0f;
        }

        private string FormattedTime
        {
            get
            {
                string minutes = Mathf.Floor(timer / 60).ToString("00");
                string seconds = Mathf.Floor(timer % 60).ToString("00");
                string time = string.Format("{0}:{1}", minutes, seconds);

                return time;
            }
        }

        #endregion

        #region Save to file

        // File to write.
        private string fileName = String.Format("{0}_{1}_{2}_{3}_{4}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute) + ".txt";

        // Logging a notice.
        private void SaveNotice(GameObject notice)
        {
            StreamWriter sw = new StreamWriter(Application.dataPath + "/Logs/" + fileName);
            sw.WriteLine(notice.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text + " " + notice.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            sw.Close();
        }

        #endregion
    }
}