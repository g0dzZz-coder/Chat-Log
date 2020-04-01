using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LOG;

public class AddProcess : MonoBehaviour
{
    [SerializeField] Log log;

    public void Click()
    {
        log.AddProcess("Process", 5.0f);
    }
}
