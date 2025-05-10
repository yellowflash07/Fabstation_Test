using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisable : MonoBehaviour
{
    [SerializeField] private GameObject uiCamera;

    //ui camera needs to be disabled and enabled to work
    //strange behavior
    private void Awake()
    {
        uiCamera.SetActive(false);
    }   

    void Start()
    {
        uiCamera.SetActive(true);
       
    }

}
