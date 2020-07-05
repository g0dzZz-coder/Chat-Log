using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChatForStrategy;

public class SystemMessageExample : MonoBehaviour
{
    [SerializeField]
    private ChatWindow chat = null;

    public void AddNotification(string text)
    {
        chat.AddSystemMessage(text, 1);
    }

    public void AddWarning(string text)
    {
        chat.AddSystemMessage(text, -1);
    }

    public void AddProcess(string text)
    {
        chat.AddSystemMessage(text, 0, 10f);
    }
}
