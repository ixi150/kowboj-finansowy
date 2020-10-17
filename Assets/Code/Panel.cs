using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] GameObject[] activeObjects;

    Panel[] panels;

    private void Awake()
    {
        panels = transform.parent.GetComponentsInChildren<Panel>(true);
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        foreach (var o in activeObjects)
        {
            o.SetActive(active);
        }

        if (active)
        {
            foreach (var panel in panels)
            {
                if (panel != this)
                {
                    panel.SetActive(false);
                }
            }
        }
    }
}
