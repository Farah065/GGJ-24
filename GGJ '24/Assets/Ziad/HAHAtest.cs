using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using Debug = UnityEngine.Debug;

public class HAHAtest : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private KeywordRecognizer _keywordRecognizer;
    private Dictionary<string, Action> _keywordActions = new();

    public bool _isLaughing;
    public int LaughBufferLength = 1;
    public int LaughBufferCountdown;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

		_keywordActions.Add("A", Laugh);
		_keywordActions.Add("H", Laugh);
		_keywordActions.Add("HA", Laugh);
		_keywordActions.Add("HAH", Laugh);
		_keywordActions.Add("HU", Laugh);
		_keywordActions.Add("HUH", Laugh);
		_keywordActions.Add("HO", Laugh);
		_keywordActions.Add("HOH", Laugh);
		_keywordActions.Add("HEE", Laugh);

        _keywordRecognizer = new KeywordRecognizer(_keywordActions.Keys.ToArray(), ConfidenceLevel.Low);
        _keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        _keywordRecognizer.Start();
    }

    private void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        _keywordActions[args.text].Invoke();
    }

    // Update is called once per frame
    void Laugh()
    {
        _isLaughing = true;
        LaughBufferCountdown = LaughBufferLength;
        StopAllCoroutines();
		StartCoroutine(LaughBuffer());
	}

    IEnumerator LaughBuffer()
    {
        while (_isLaughing)
        {
			Debug.Log("LaughBuffer called");
			if (LaughBufferCountdown == 0)
			{
				_isLaughing = false;
				Debug.Log("Countdown is zero");
			}

			else
			{
				yield return new WaitForSeconds(1);
				LaughBufferCountdown--;
				Debug.Log("Countdown is " + LaughBufferCountdown);

			}
		}		
    }

    private void Update()
    {
		if (_isLaughing)
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
