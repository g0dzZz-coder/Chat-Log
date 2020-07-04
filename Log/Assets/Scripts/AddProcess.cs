using UnityEngine;
using ChatForStrategy;

// Added for example.
public class AddProcess : MonoBehaviour
{
    [SerializeField]
    private Log log = null;

    [SerializeField]
    private string nameProcess = null;
    [SerializeField]
    private float duration = 10f;

    public void Click()
    {
        log.AddProcess(nameProcess, duration);
    }
}
