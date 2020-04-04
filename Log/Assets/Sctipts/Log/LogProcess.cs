using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LOG
{
    public class LogProcess : MonoBehaviour
    {
        public string Text => txtNameProcess.text;

        // Time when the process is complete.
        public string CompletionTime { get; private set; }

        [Tooltip("Removal from the log after the end")]
        [SerializeField] bool autoDelete = true;

        [Header("Text")]
        [SerializeField] TextMeshProUGUI txtNameProcess;
        [SerializeField] TextMeshProUGUI txtTime;
        [SerializeField] TextMeshProUGUI txt_timeLeft;

        [Header("Images")]
        [SerializeField] Image imgBackground;
        [SerializeField] Image imgBarFilling;       

        [Tooltip("Pause between bar updates")]
        [SerializeField, Range(0.1f, 1.0f)] float delay = 0.1f;

        [SerializeField] Animation _animation;
        [SerializeField] RectTransform rectBar;

        private RectTransform rect;
        private RectTransform rectNameProcess;
        private float maxNameWidth;
        private float curTime;
        private float duration;

        void Awake()
        {
            rect = gameObject.GetComponent<RectTransform>();
            rectNameProcess = txtNameProcess.GetComponent<RectTransform>();

            // Get maximum line width of text.
            maxNameWidth = rectNameProcess.rect.width;
        }

        // Initialization.
        public void Init(string text, float duration, string timeOfCreation, string completionTime, Color color)
        {
            txtNameProcess.text = text;
            txtTime.text = timeOfCreation;

            this.duration = duration;
            curTime = 0;

            CompletionTime = completionTime;

            imgBackground.color = color;

            int countLines = 1;
            if (txtNameProcess.preferredWidth > maxNameWidth)
                countLines = (int)Math.Ceiling(txtNameProcess.preferredWidth / maxNameWidth);

            rect.sizeDelta = new Vector2(rect.rect.width, rectNameProcess.rect.height * countLines + rectBar.rect.height);

            StartCoroutine(Filling());
        }

        // Initialization with a given number of lines.
        public void Init(string text, float duration, string time, Color color, int countLines)
        {
            txtNameProcess.text = text;
            txtTime.text = time;

            this.duration = duration;
            curTime = 0;

            imgBackground.color = color;

            rect.sizeDelta = new Vector2(rect.rect.width, rectNameProcess.rect.height * countLines + rectBar.rect.height);

            StartCoroutine(Filling());
        }

        // Bar fill animation.
        IEnumerator Filling()
        {
            while (curTime < duration)
            {
                curTime += delay;
                if (curTime > duration)
                    curTime = duration;

                txt_timeLeft.text = (duration - curTime).ToString();
                txt_timeLeft.text = String.Format("{0:0.0} s", (duration - curTime));
                imgBarFilling.fillAmount = curTime / duration;

                yield return new WaitForSeconds(delay);
            }

            // Removal of the process from the log.
            if (autoDelete)
            {
                _animation.Play("Delete Notice");

                while (_animation.IsPlaying("Delete Notice"))
                {
                    // Just wait.
                    yield return new WaitForFixedUpdate();
                }
                Destroy(gameObject);
            }
        }
    }
}
