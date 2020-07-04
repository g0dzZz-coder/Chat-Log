using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace LOG
{
    public class Message : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI nameNoticeTextComp = null;
        public string Text => nameNoticeTextComp.text;

        [SerializeField]
        private TextMeshProUGUI timeTextComp = null;
        public string Time => timeTextComp.text;

        [SerializeField]
        private Image background = null;

        private float maxNameWidth = 0f;

        private RectTransform rect = null;

        private void Awake()
        {
            rect = gameObject.GetComponent<RectTransform>();

            // Get maximum line width of text.
            maxNameWidth = nameNoticeTextComp.GetComponent<RectTransform>().rect.width;
        }

        // Initialization.
        public void Init(string text, string time, Color color)
        {
            nameNoticeTextComp.text = text;
            timeTextComp.text = time;

            background.color = color;

            int countLines = 1;
            if (nameNoticeTextComp.preferredWidth > maxNameWidth)
                countLines = (int)Math.Ceiling(nameNoticeTextComp.preferredWidth / maxNameWidth);

            var rect1 = rect.rect;
            rect.sizeDelta = new Vector2(rect1.width, rect1.height * countLines);
        }

        // Initialization with a given number of lines.
        public void Init(string text, string time, Color color, int countLines)
        {
            nameNoticeTextComp.text = text;
            timeTextComp.text = time;

            background.color = color;

            var rect1 = rect.rect;
            rect.sizeDelta = new Vector2(rect1.width, rect1.height * countLines);
        }
    }
}
