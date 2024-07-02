using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimeEvent : MonoBehaviour
{
    public UnityEvent ComboStartAct;
    public UnityEvent ComboCheckStartAct;
    public UnityEvent ComboCheckEndAct;


    public void ComboStart()
    {
        ComboStartAct?.Invoke();
    }
    public void ComboCheckStart()
    {
        ComboCheckStartAct?.Invoke();
    }
    public void ComboCheckEnd()
    {
        ComboCheckEndAct?.Invoke();
    }

}
