using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChatForStrategy
{
    public class ProcessComponent : MessageComponent
    {
        [SerializeField, Tooltip("Removal from the log after the end")]
        private bool autoDelete = true;

        [SerializeField]
        private TextMeshProUGUI timeLeftTextComp = null;

        [SerializeField]
        private Image barFillingImage = null;

        [SerializeField]
        private Animation _animation = null;
        [SerializeField]
        private RectTransform rectBar = null;

        private float curTime = 0f;
        private float duration = 0f;

        // Initialization.
        public void Init(Process process)
        {
            //base.Init(process);
            textMessage.text = process.Text;
            textTime.text = process.Time;
            background.color = process.Type.Color;

            duration = process.Duration;

            curTime = 0;

            int countLines = 1;
            if (textMessage.preferredWidth > maxTextWidth)
                countLines = (int)Math.Ceiling(textMessage.preferredWidth / maxTextWidth);

            rect.sizeDelta = new Vector2(rect.rect.width, textMessage.GetComponent<RectTransform>().rect.height * countLines + rectBar.rect.height);

            StartCoroutine(Filling());
        }

        // Bar fill animation.
        private IEnumerator Filling()
        {
            while (curTime < duration)
            {
                curTime += 0.1f;
                if (curTime > duration)
                    curTime = duration;

                timeLeftTextComp.text = (duration - curTime).ToString(CultureInfo.InvariantCulture);
                timeLeftTextComp.text = $"{(duration - curTime):0.0} s";
                barFillingImage.fillAmount = curTime / duration;

                yield return new WaitForSeconds(0.1f);
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
