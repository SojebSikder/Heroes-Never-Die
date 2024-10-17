using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string conversationName;

    [SerializeField]
    public KeyValuePair<string, string>[] conversations;

    // public string name;

    // [TextArea(3, 10)]
    // public string[] sentences;

}
