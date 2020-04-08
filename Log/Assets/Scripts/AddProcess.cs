using UnityEngine;
using LOG;

// Added for example.
public class AddProcess : MonoBehaviour
{
    [SerializeField] Log log;

    [SerializeField] string nameProcess;
    [SerializeField] float duration;

    public void Click()
    {
        log.AddProcess(nameProcess, duration);
    }
}
