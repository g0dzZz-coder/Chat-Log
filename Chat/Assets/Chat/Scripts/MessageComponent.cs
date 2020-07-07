using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace ChatForStrategy
{
    public class MessageComponent : MonoBehaviour
    {
        [SerializeField]
        protected TextMeshProUGUI textMessage = null;
        [SerializeField]
        protected TextMeshProUGUI textTime = null;
        [SerializeField]
        protected Image background = null;

        protected float maxTextWidth = 0f;

        protected RectTransform rect = null;

        public virtual void Awake()
        {
            rect = gameObject.GetComponent<RectTransform>();

            // Get maximum line width of text.
            maxTextWidth = textMessage.GetComponent<RectTransform>().rect.width;
        }

        // Initialization.
        public void Init(Message message)
        {
            textMessage.text = message.Text;
            textTime.text = message.Time;
            background.color = message.Type.Color;

            int countLines = 1;
            if (textMessage.preferredWidth > maxTextWidth)
            {
                countLines = (int)Math.Ceiling(textMessage.preferredWidth / maxTextWidth);
            }

            rect.sizeDelta = new Vector2(rect.rect.width, rect.rect.height * countLines);
        }
    }
}
