using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimeEvent : MonoBehaviour
{
    public UnityEvent ComboCheckStartAct;
    public UnityEvent ComboCheckEndAct;

    public void ComboCheckStart()
    {
        ComboCheckStartAct?.Invoke();
    }
    public void ComboCheckEnd()
    {
        ComboCheckEndAct?.Invoke();
    }
}
