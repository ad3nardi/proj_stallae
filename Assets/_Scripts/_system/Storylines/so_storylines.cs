using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Storyline/Conversation")]
public class so_storylines : ScriptableObject
{
    [Header("Identifier")]
    public int sceneID;
    public int convoID;
    public bool useInputs = false;

    [Header("Text")]
    public List<string> _text = new List<string>();

    [Header("Image")]
    public List<Sprite> _image = new List<Sprite>();

    [Header("Settings")]
    public List<float> _displayTime = new List<float>();
}
