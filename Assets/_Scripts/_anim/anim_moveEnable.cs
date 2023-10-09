using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anim_moveEnable : OptimizedBehaviour
{
    private void OnEnable()
    {
        CachedTransform.DOMove(Vector3.down * 15f, 5f);
    }
}
