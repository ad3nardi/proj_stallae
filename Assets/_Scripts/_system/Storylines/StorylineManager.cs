using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class StorylineManager : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] private so_storylines _curConvo;

    [Header("Settings")]
    [SerializeField] private int _curConvoID;
    [SerializeField] private float _txtSpeed = 0.1f;
    [SerializeField] private bool _useInputs = false;
    [SerializeField] private float _convoTimer = 0f;
    [SerializeField] private float _convoTime = 0f;

    [SerializeField] private int _lineIndex;

    [Header("UI Elements")]
    [SerializeField] private OptimizedBehaviour _convoObj;
    [SerializeField] private TextMeshProUGUI _convoTextBox;
    [SerializeField] private Image _convoImage;

    private void Awake()
    {
        _convoObj.CachedGameObject.SetActive(true);
        _convoTextBox.text = string.Empty;
        _convoTime = 0f;

        _convoObj.CachedGameObject.SetActive(false);
        _curConvo = null;


    }
    void Start()
    {

        if (_convoTextBox != null)
        {
            _convoTextBox.text = "";
        }
    }
    private void OnEnable()
    {

    }
    private void Update()
    {
        if(_convoTime > 0f)
            _convoTime -= Time.deltaTime;

        if (!_useInputs && _curConvo != null)
        {
            if(_convoTime <= 0)
            {
                if (_convoTextBox.text == _curConvo._text[_lineIndex])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    _convoTextBox.text = _curConvo._text[_lineIndex];
                }
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void PlayConvo(so_storylines convo)
    {
        _convoObj.CachedGameObject.SetActive(true);
        _lineIndex = 0;
        _curConvo = convo;
        _convoImage.sprite = convo._image[_lineIndex];
        _convoTime = convo._displayTime[_lineIndex];

        StartCoroutine(TypeLine());
    }
    private void NextLine()
    {
        if(_lineIndex+1 < _curConvo._text.Count)
        {
            _lineIndex++;
            _convoImage.sprite = _curConvo._image[_lineIndex];
            _convoTime = _curConvo._displayTime[_lineIndex];
            _convoTextBox.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            CloseConvo();
        }
    }
    public void CloseConvo()
    {
        _curConvo = null;
        _convoImage.sprite = null;
        _convoObj.CachedGameObject.SetActive(false);

    }

    IEnumerator TypeLine()
    {
        //Type each char 1 by 1
        /*
        for (int z = 0; z < _curConvo._text[_lineIndex].Length; z++)
        {
            char[] c = _curConvo._text[_lineIndex].ToCharArray();
            _convoTextBox.text += c[z];
            yield return new WaitForSeconds(_txtSpeed);


        }
        */
        foreach (char c in _curConvo._text[_lineIndex].ToCharArray())
        {
            _convoTextBox.text += c;
            yield return new WaitForSeconds(_txtSpeed);

        }
    }


}
