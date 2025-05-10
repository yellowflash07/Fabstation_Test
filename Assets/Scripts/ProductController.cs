using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductController : MonoBehaviour
{
    //rotate the product 90 degrees
    public void Rotate90()
    {
        //get the "true" center of the object
        Bounds bound = transform.GetChild(0).GetComponent<Renderer>().bounds;
        transform.RotateAround(bound.center, Vector3.right, 90f);
    }

    public void Rotate180()
    {
        Bounds bound = transform.GetChild(0).GetComponent<Renderer>().bounds;
        transform.RotateAround(bound.center, Vector3.forward, 180f);
    }
}
