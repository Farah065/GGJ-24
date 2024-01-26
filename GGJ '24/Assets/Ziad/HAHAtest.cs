using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Windows.Speech;
using Debug = UnityEngine.Debug;

public class HAHAtest : MonoBehaviour
{
	SpriteRenderer spriteRenderer;
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, Action> keywordActions = new();

    public bool isLaughing;
    public int laughBufferLength = 1;
    public int laughBufferCountdown;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

		keywordActions.Add("A", Laugh);
		keywordActions.Add("H", Laugh);
		keywordActions.Add("HA", Laugh);
		keywordActions.Add("HAH", Laugh);
		keywordActions.Add("HU", Laugh);
		keywordActions.Add("HUH", Laugh);
		keywordActions.Add("HO", Laugh);
		keywordActions.Add("HOH", Laugh);
		keywordActions.Add("HEE", Laugh);

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        keywordActions[args.text].Invoke();
    }

    // Update is called once per frame
    void Laugh()
    {
        isLaughing = true;
        laughBufferCountdown = laughBufferLength;
        StopAllCoroutines();
		StartCoroutine(LaughBuffer());
	}

    IEnumerator LaughBuffer()
    {
        while (isLaughing)
        {
			Debug.Log("LaughBuffer called");
			if (laughBufferCountdown == 0)
			{
				isLaughing = false;
				Debug.Log("Countdown is zero");
			}

			else
			{
				yield return new WaitForSeconds(1);
				laughBufferCountdown--;
				Debug.Log("Countdown is " + laughBufferCountdown);

			}
		}		
    }

    void Update()
    {
	    spriteRenderer.color = isLaughing ? Color.red : Color.white;
    }
}
