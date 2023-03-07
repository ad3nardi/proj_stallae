using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class formation_renderer : MonoBehaviour
{
    private formation_base _formation;
    public formation_base Formation
    {
        get
        {
            if (_formation == null) _formation = GetComponent<formation_base>();
            return _formation;
        }
        set => _formation = value;
    }

    [SerializeField] private Vector3 _unitGizmoSize;
    [SerializeField] private Color _gizmoColor;

    public void OnDrawGizmos()
    {
        if (Formation == null || Application.isPlaying) return;

        Gizmos.color = _gizmoColor;

        foreach (var pos in Formation.EvaluatePoints())
        {
            Gizmos.DrawCube(transform.position + pos + new Vector3(0, _unitGizmoSize.y * 0.5f, 0), _unitGizmoSize);
        }
    }
}
