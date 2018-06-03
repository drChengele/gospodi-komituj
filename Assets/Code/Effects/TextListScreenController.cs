using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextListScreenController : MonoBehaviour {

    [SerializeField] int maxLines;
    [SerializeField] Text targetTextObj;
    public List<string> texts = new List<string>();

    public void AddText(string text) {
        texts.Add(text);
        if (texts.Count > maxLines) texts.RemoveAt(0);
        targetTextObj.text = string.Join("\r\n", texts);
    }

    private void Start() {
        InvokeRepeating("AddRandomText", 1f, 0.5f);
    }

    void AddRandomText() {
        var strs = new[] {
          "Hello world",
          "Bounty inreased",
          "ULTRAAAA",
          "Enemy Destroyed"
        };

        var str = strs[UnityEngine.Random.Range(0, strs.Length)];

        AddText(str);
    }

}