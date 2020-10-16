using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorCallback : MonoBehaviour
{
    [SerializeField] UnityEvent[] events;

    public void SendEvent(int index)
    {
        events[index].Invoke();
    }
}
