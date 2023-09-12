using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Script for having a typewriter effect for UI - Version 2
// Prepared by Nick Hwang (https://www.youtube.com/nickhwang)
// Want to get creative? Try a Unicode leading character(https://unicode-table.com/en/blocks/block-elements/)
// Copy Paste from page into Inspector

public class ui_typewriter : OptimizedBehaviour
{
    [SerializeField] TextMeshProUGUI tmpProText;
    string writer;
    [SerializeField] private Coroutine coroutine;

    [SerializeField] float delayBeforeStart = 0f;
    [SerializeField] float timeBtwChars = 0.1f;
    [SerializeField] string leadingChar = "";
    [SerializeField] bool leadingCharBeforeDelay = false;
    [Space(10)][SerializeField] private bool startOnEnable = false;

    [Header("Collision-Based")]
    [SerializeField] private bool clearAtStart = false;
    [SerializeField] private bool startOnCollision = false;
    enum options { clear, complete }
    [SerializeField] options collisionExitOptions;

    // Use this for initialization
    void Awake()
    {
        if (tmpProText != null)
        {
            writer = tmpProText.text;
        }
    }

    void Start()
    {
        if (!clearAtStart) return;

        if (tmpProText != null)
        {
            tmpProText.text = "";
        }
    }

    private void OnEnable()
    {
        if (startOnEnable) StartTypewriter();
    }

    private void OnColliderEnter(Collider col)
    {
        if (startOnCollision)
        {
            StartTypewriter();
        }
    }

    private void OnColliderExit(Collider other)
    {
        if (collisionExitOptions == options.complete)
        {

            if (tmpProText != null)
            {
                tmpProText.text = writer;
            }
        }
        // clear
        else
        {
            if (tmpProText != null)
            {
                tmpProText.text = "";
            }
        }

        StopAllCoroutines();
    }

    private void StartTypewriter()
    {
        StopAllCoroutines();

        if (tmpProText != null)
        {
            tmpProText.text = "";

            StartCoroutine("TypeWriterTMP");
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator TypeWriterTMP()
    {
        tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);
        for (char c = char.MinValue; c < writer.Length; c++)
        {
            if (tmpProText.text.Length > 0)
            {
                tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - leadingChar.Length);
            }
            tmpProText.text += c;
            tmpProText.text += leadingChar;
            yield return new WaitForSeconds(timeBtwChars);
        }

        if (leadingChar != "")
        {
            tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - leadingChar.Length);
        }
    }
}
