using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private UnityAction onClickAction;
    [SerializeField] private Button button;
    [SerializeField] private Image selected;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        text.text = gameObject.name;
        //simple animation
        button.onClick.AddListener(() =>
        {
            transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 10, 1);
        });
    }

    // This method is called when the button is clicked
    public void Select(bool isSelected)
    {
        selected.enabled = isSelected;
        if(isSelected)
        {
            onClickAction?.Invoke();    
        }
    }

    // This method is used to set the action to be performed when the button is clicked
    public void SetButtonAction(Action action)
    {
        button.onClick.AddListener(() => action?.Invoke());
    }
}
