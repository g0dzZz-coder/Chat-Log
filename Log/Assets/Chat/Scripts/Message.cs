using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChatForStrategy
{
    public class Message
    {
        public string Text;
        public string Time;
        public MessageType Type;
    }

    public class Process : Message
    {
        public float Duration;
        public string CompletionTime;
    }

    [System.Serializable]
    public struct TypesOfMessages
    {
        public MessageType Message;
        public MessageType OpponentMessage;

        public MessageType Notification;
        public MessageType Warning;
        public MessageType Process;
    }

    [System.Serializable]
    public class MessageType
    {
        public Color Color;
        public GameObject Prefab;
    }
}
