using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextListScreenController : MonoBehaviour {

    [SerializeField] int maxLines;
    [SerializeField] Text targetTextObj;
    [SerializeField] string[] sourceTexts;
    
    List<string> texts = new List<string>();

    public void AddText(string text) {
        texts.Add(text);
        if (texts.Count > maxLines) texts.RemoveAt(0);
        targetTextObj.text = string.Join("\r\n", texts);
    }

    private void Start() {
        InvokeRepeating("AddRandomText", 1f, 0.5f);
    }

    void AddRandomText() {        
        var str = sourceTexts[UnityEngine.Random.Range(0, sourceTexts.Length)];
        AddText(str);
    }

}