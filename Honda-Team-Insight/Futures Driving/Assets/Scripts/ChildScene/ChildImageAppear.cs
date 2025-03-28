using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChildImageAppear : MonoBehaviour
{
    [SerializeField] GameObject calendar;

    void Start()
    {
        calendar.SetActive(false);
    }

    public void ShowCalendar()
    {
        calendar.SetActive(true);
    }

    public void HideCalendar()
    {
        calendar.SetActive(false);
    }
}