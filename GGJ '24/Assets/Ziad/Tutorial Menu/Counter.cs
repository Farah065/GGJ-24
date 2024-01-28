using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public GameObject EventSystem;
    TMP_Text counterText;
    int counter = -1;

    void Start()
    {
        counterText = GetComponent<TMP_Text>();
    }
    void Update()
    {
        counter = EventSystem.GetComponent<LaughToStart>().LaughCount;
        counterText.SetText("Laughs Detected: " + counter);
    }
}
