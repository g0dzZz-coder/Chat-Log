using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChatForStrategy
{
    public class Process : MonoBehaviour
    {
        // Time when the process is complete.
        public string CompletionTime { get; private set; }

        [SerializeField, Tooltip("Removal from the log after the end")]
        private bool autoDelete = true;

        [Header("Text")]
        [SerializeField]
        private TextMeshProUGUI nameProcessTextComp = null;
        public string Text => nameProcessTextComp.text;

        [SerializeField]
        private TextMeshProUGUI timeTextComp = null;
        [SerializeField]
        private TextMeshProUGUI timeLeftTextComp = null;

        [Header("Images")]
        [SerializeField]
        private Image backgroundImage = null;
        [SerializeField]
        private Image barFillingImage = null;

        [Tooltip("Pause between bar updates")]
        [SerializeField, Range(0.1f, 1.0f)] float delay = 0.1f;

        [SerializeField]
        private Animation _animation = null;
        [SerializeField]
        private RectTransform rectBar = null;

        private RectTransform rect = null;
        private RectTransform rectNameProcess = null;
        private float maxNameWidth = 0f;
        private float curTime = 0f;
        private float duration = 0f;

        private void Awake()
        {
            rect = gameObject.GetComponent<RectTransform>();
            rectNameProcess = nameProcessTextComp.GetComponent<RectTransform>();

            // Get maximum line width of text.
            maxNameWidth = rectNameProcess.rect.width;
        }

        // Initialization.
        public void Init(string text, float duration, string timeOfCreation, string completionTime, Color color)
        {
            nameProcessTextComp.text = text;
            timeTextComp.text = timeOfCreation;

            this.duration = duration;
            curTime = 0;

            CompletionTime = completionTime;

            backgroundImage.color = color;

            int countLines = 1;
            if (nameProcessTextComp.preferredWidth > maxNameWidth)
                countLines = (int)Math.Ceiling(nameProcessTextComp.preferredWidth / maxNameWidth);

            rect.sizeDelta = new Vector2(rect.rect.width, rectNameProcess.rect.height * countLines + rectBar.rect.height);

            StartCoroutine(Filling());
        }

        // Initialization with a given number of lines.
        public void Init(string text, float duration, string time, Color color, int countLines)
        {
            nameProcessTextComp.text = text;
            timeTextComp.text = time;

            this.duration = duration;
            curTime = 0;

            backgroundImage.color = color;

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

                timeLeftTextComp.text = (duration - curTime).ToString(CultureInfo.InvariantCulture);
                timeLeftTextComp.text = $"{(duration - curTime):0.0} s";
                barFillingImage.fillAmount = curTime / duration;

                yield return new WaitForSeconds(delay);
            }

            // Removal of the process from the log.
            if (!autoDelete) 
                yield break;
            
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
