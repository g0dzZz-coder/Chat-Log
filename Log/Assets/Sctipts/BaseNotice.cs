using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace LOG
{
    [System.Serializable]
    public abstract class BaseNotice : MonoBehaviour
    {
        [Tooltip("Number by character, after which a line is added")]
        public int CharactersPerLine = 15;
    }
}