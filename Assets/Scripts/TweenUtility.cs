using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenUtility : MonoBehaviour
{
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 targetPosition;

    //a very basic animation utility
    private void OnEnable()
    {
        GetComponent<RectTransform>().DOAnchorPos(targetPosition, 0.4f).From(startPosition).SetEase(Ease.InCubic);
    }
}
