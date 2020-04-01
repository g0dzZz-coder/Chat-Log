using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace LOG
{
    public class Notice : BaseNotice
    {
        [SerializeField] TextMeshProUGUI txtNameNotice;
        [SerializeField] TextMeshProUGUI txtTime;

        [SerializeField] Animation _animation;

        void Start()
        {
            _animation.Play();
        }

        // Initialization.
        public void Init(string text, string time)
        {
            txtNameNotice.text = text;
            txtTime.text = time;

            RectTransform rect = gameObject.GetComponent<RectTransform>();
            int countLines = 1;
            for (int i = CharactersPerLine; i <= text.Length; i += CharactersPerLine)
            {
                if (i + 1 <= text.Length)
                    countLines++;
            }

            rect.sizeDelta = new Vector2(rect.rect.width, rect.rect.height * countLines);
        }

        // Initialization with a given number of lines.
        public void Init(string text, string time, int countLines)
        {
            txtNameNotice.text = text;
            txtTime.text = time;

            RectTransform rect = gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.rect.width, rect.rect.height * countLines);
        }
    }
}
