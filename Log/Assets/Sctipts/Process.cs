using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LOG
{
    public class Process : BaseNotice
    {
        [Tooltip("Removal from the log after the end")]
        [SerializeField] bool autoDelete;

        [SerializeField] TextMeshProUGUI txtNameProcess;
        [SerializeField] TextMeshProUGUI txtTime;
        [SerializeField] TextMeshProUGUI txt_timeLeft;

        [SerializeField] Image imgBarFilling;

        [Tooltip("Pause between bar updates")]
        [SerializeField, Range(0.1f, 1.0f)] float delay = 0.1f;

        [SerializeField] Animation _animation;

        private string nameProcess;
        private float curTime;
        private float duration;

        void Start()
        {
            _animation.Play("Add Notice");
        }

        // Initialization.
        public void Init(string text, float duration, string time)
        {
            txtNameProcess.text = text;
            txtTime.text = time;
            this.duration = duration;
            curTime = 0;

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
