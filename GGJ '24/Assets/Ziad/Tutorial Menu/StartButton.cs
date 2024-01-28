using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    Button button;
    Image image;
    public Sprite unactivated;
    public Sprite activated;
    public GameObject EventSystem;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        int counter = EventSystem.GetComponent<LaughToStart>().LaughCount;
        if (counter >= 3)
        {
            button.interactable = true;
            image.sprite = activated;
        }
        else
        {
            button.interactable = false;
            image.sprite = unactivated;
        }
    }
}
