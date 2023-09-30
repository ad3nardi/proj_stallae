using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class gui_radialMenu : OptimizedBehaviour
{
    [Header("Plugins")]
    [SerializeField] Vector2 _mousePos;
    [SerializeField] GameObject _highlightObj;

    [Header("Selection Behaviour")]
    public int _selectedOption;
    [SerializeField] List<TextMeshProUGUI> _options;
    public Color normalColor, highlightedColor, unactiveColour;
    private bool[] _activeOptions = new bool[6];

    private void GetInfo(ITargetable iTarget)
    {

        GetTargetInfo(iTarget, iTarget.GetUnitHealth(), iTarget.GetActive(), iTarget.GetHP());
    }

    public void GetTargetInfo(ITargetable target, float shipHP, bool[] actSS, float[] ssHP)
    {
        _activeOptions = actSS;
    }

    public int CheckRadialMenu(ITargetable iTarget, float x, float y)
    {
        GetInfo(iTarget);
        _mousePos = new Vector2(x - Screen.width / 2, y - Screen.height / 2);
        _mousePos.Normalize();
        int optionAmnt = _options.Count;

        if (_mousePos != Vector2.zero)
        {
            float angle = Mathf.Atan2(_mousePos.y, -_mousePos.x) / Mathf.PI;
            angle *= 180;
            angle -= 360/ optionAmnt;
            if (angle < 0)
            {
                angle += 360;
            }
            for (int i = 0; i < optionAmnt; i++)
            {
                if (angle > i * (360 / optionAmnt) && angle < (i + 1) * (360 / optionAmnt))
                {
                    _options[i].color = highlightedColor;
                    
                    if (_activeOptions[i] == false)
                    {
                        _options[i].color = unactiveColour;
                        _selectedOption = i;
                        for (int j = 0; j < 6; j++)
                        {
                            _selectedOption = j;
                            if (_activeOptions[j] == false)
                            {
                                continue;
                            }
                            else
                                break;
                        }
                    }
                    else
                    {
                        _selectedOption = i;
                        _highlightObj.transform.rotation = Quaternion.Euler(0, 0, i * -(360 / optionAmnt));
                    } 
                }
                else
                {
                    _options[i].color = normalColor;
                    if (_activeOptions[i] == false)
                    {
                        _options[i].color = unactiveColour;
                    }
                }
            }
            return _selectedOption;
        }
        else
        {
            return 0;
        }

        /*
        //INPUT ACTION HERE - i.e On Release
        switch (_selectedOption)
        {
            case 0:
                SwitchOne();
                break;
            case 1:
                SwitchTwo();
                break;
        }*/
    }

        /*
    private void SwitchOne()
    {

    }
    private void SwitchTwo()
    {

    }
    private void SwitchThree()
    {

    }
    private void SwitchFour()
    {

    }
    private void SwitchFive()
    {

    }
        */
}
