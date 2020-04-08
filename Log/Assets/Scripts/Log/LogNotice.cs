using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace LOG
{
    public class LogNotice : MonoBehaviour
    {
        public string Text => txtNameNotice.text;
        public string Time => txtTime.text;

        [SerializeField] TextMeshProUGUI txtNameNotice;
        [SerializeField] TextMeshProUGUI txtTime;
        [SerializeField] Image background;

        private float maxNameWidth;

        private RectTransform rect;

        void Awake()
        {
            rect = gameObject.GetComponent<RectTransform>();

            // Get maximum line width of text.
            maxNameWidth = txtNameNotice.GetComponent<RectTransform>().rect.width;
        }

        // Initialization.
        public void Init(string text, string time, Color color)
        {
            txtNameNotice.text = text;
            txtTime.text = time;

            background.color = color;

            int countLines = 1;
            if (txtNameNotice.preferredWidth > maxNameWidth)
                countLines = (int)Math.Ceiling(txtNameNotice.preferredWidth / maxNameWidth);
            
            rect.sizeDelta = new Vector2(rect.rect.width, rect.rect.height * countLines);
        }

        // Initialization with a given number of lines.
        public void Init(string text, string time, Color color, int countLines)
        {
            txtNameNotice.text = text;
            txtTime.text = time;

            background.color = color;

            rect.sizeDelta = new Vector2(rect.rect.width, rect.rect.height * countLines);
        }
    }
}
