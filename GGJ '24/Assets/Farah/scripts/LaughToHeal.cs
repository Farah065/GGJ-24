using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Windows.Speech;
using Debug = UnityEngine.Debug;

public class LaughToHeal : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, Action> keywordActions = new();

    public bool isLaughing;
    public float laughCooldown;

    public GameObject playerObj;
    Player Player;

    private ParticleSystem ps;

    void Start()
    {
        playerObj = GameObject.FindWithTag("Player");
        Player = playerObj.GetComponent<Player>();
        laughCooldown = 0;

        ps = GetComponent<ParticleSystem>();

        keywordActions.Add("A", Laugh);
        keywordActions.Add("H", Laugh);
        keywordActions.Add("HA", Laugh);
        keywordActions.Add("HAHA", Laugh);
        keywordActions.Add("HAHAHA", Laugh);
        keywordActions.Add("HAHAHAHA", Laugh);
        keywordActions.Add("HU", Laugh);
        keywordActions.Add("HUHU", Laugh);
        keywordActions.Add("HUHUHU", Laugh);
        keywordActions.Add("HUHUHUHU", Laugh);
        keywordActions.Add("HO", Laugh);
        keywordActions.Add("HOHO", Laugh);
        keywordActions.Add("HOHOHO", Laugh);
        keywordActions.Add("HOHOHOHO", Laugh);
        keywordActions.Add("HEE", Laugh);
        keywordActions.Add("HEEHEE", Laugh);
        keywordActions.Add("HEEHEEHEE", Laugh);
        keywordActions.Add("HEEHEEHEEHEE", Laugh);

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += OnKeywordsRecognized;
        keywordRecognizer.Start();
    }

    void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        keywordActions[args.text].Invoke();
    }

    void Laugh()
    {
        if(Player.hp > 0)
            StartCoroutine(Particle());
        if(laughCooldown <= 0 & Player.hp > 0)
            isLaughing = true;
    }

    void Update()
    {
        if (laughCooldown > 0)
        {
            laughCooldown -= Time.deltaTime;
        }

        if (isLaughing)
        {
            laughCooldown = 3;
            isLaughing = false;
            if (Player.hp < 8)
                Player.hp += 1;
        }
    }

    private IEnumerator Particle()
    {
        ps.Play();
        yield return new WaitForSeconds(2);
        ps.Stop();
    }
}
