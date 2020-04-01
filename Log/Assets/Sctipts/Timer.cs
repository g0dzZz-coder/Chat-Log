using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Timer : MonoBehaviour
{
    public int GetTime => sec;

    #region Fields

    public static bool stop;
    public static string result;

    [Tooltip("Run in Awake()")]
    [SerializeField] bool startAwake = true;

    [SerializeField] TextMeshProUGUI textOutput;

    private enum TimerMode { minSec = 0, sec = 1 };
    private TimerMode timerMode;

    private int startMin, startSec;
    private int min, sec;
    private string m, s;

    #endregion

    #region Unity Metods

    void Awake()
    {
        if (startAwake)
            stop = false;
        else
            stop = true;

        if (startMin > 0 && startMin <= 59)
            min = startMin;
        else
            startMin = 0;

        if (startSec > 0 && startSec <= 59)
            sec = startSec;
        else
            startSec = 0;
    }

    void Start()
    {
        startMin = 0;
        startSec = 0;
        timerMode = TimerMode.minSec;
        StartCoroutine(RepeatingFunction());
    }

    #endregion

    public void SwitchTimerMode()
    {
        switch (timerMode)
        {
            case TimerMode.minSec:
                timerMode = TimerMode.sec;
                break;

            case TimerMode.sec:
                timerMode = TimerMode.minSec;
                break;
        }
    }

    private void TimeCount()
    {
        if (timerMode == TimerMode.minSec)
        {
            if (sec > 59)
            {
                sec = 0;
                min++;
            }
        }

        CurrentTime();

        sec++;
    }

    private void CurrentTime()
    {
        if (sec < 10)
            s = "0" + sec;
        else
            s = sec.ToString();

        if (min < 10)
            m = "0" + min;
        else
            m = min.ToString();
    }

    private void OnGUI()
    {
        switch (timerMode)
        {
            case TimerMode.minSec:
                result = m + ":" + s;
                break;

            case TimerMode.sec:
                result = s;
                break;
        }

        textOutput.text = result;
    }   

    #region IEnumerators

    IEnumerator RepeatingFunction()
    {
        while (true)
        {
            if (!stop)
            {
                TimeCount();
                OnGUI();
            }
            yield return new WaitForSeconds(1);
        }
    }

    #endregion
}
