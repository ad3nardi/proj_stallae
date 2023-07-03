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
    public Color normalColor, highlightedColor;


    public int CheckRadialMenu(float x, float y)
    {
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
                    _selectedOption = i;
                    _highlightObj.transform.rotation = Quaternion.Euler(0, 0, i * -(360 / optionAmnt));
                }
                else
                {
                    _options[i].color = normalColor;
                }
            }
            return _selectedOption;

        }
        else
            return 0;

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
