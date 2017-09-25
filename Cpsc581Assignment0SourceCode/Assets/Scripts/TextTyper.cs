using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTyper : MonoBehaviour
{

    public float letterPause;
    public AudioClip Sound;
    public GUIText TextGUI;
    public bool IsTyping;

    void Start()
    {
        TextGUI = gameObject.GetComponent<GUIText>();
        IsTyping = false;
    }

    // Use this for initialization
    public void DisplayText(string message)
    {
        if (!IsTyping)
        {
            TextGUI.text = "";
            StartCoroutine(TypeText(message));
        }
    }

    IEnumerator TypeText(string message)
    {
        IsTyping = true;
        foreach (char letter in message.ToCharArray())
        {
            TextGUI.text += letter;
            yield return new WaitForSeconds(letterPause);
        }
        yield return new WaitForSeconds(1.0f);
        IsTyping = false;
    }
}
