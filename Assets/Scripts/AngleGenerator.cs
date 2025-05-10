using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AngleGenerator : MonoBehaviour
{
    private enum AnglePoint
    {
        StartPoint,
        PointA,
        PointB
    }

    [SerializeField] private Image angleArc;
    [SerializeField] private LineRenderer angleLine;
    [SerializeField] private TextMeshProUGUI angleText;
    [SerializeField] private GameObject pointPrefab;
    private Vector3 startPoint;
    private Camera cam;
    private AnglePoint currPoint;
    private List<GameObject> editPoints = new List<GameObject>();
    private GameObject currentEdit = null;
    private Vector3 productHitPoint;
    private bool systemActive = false;

    void Start()
    {
        cam = Camera.main;
        currPoint = AnglePoint.StartPoint;
        angleArc.gameObject.SetActive(false);
        angleText.gameObject.SetActive(false);
    }

    void Update()
    {
        if(!systemActive) return;
        var inputMouse = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
        var mousePos = cam.ScreenToWorldPoint(inputMouse);
        mousePos.z = 0;
        //raycast
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider == null) return;

            if (hit.collider.CompareTag("Product"))
            {
                //"recording" the hit point on the product to move the point along the product
                productHitPoint = hit.point;
            }

            if (hit.collider.CompareTag("LinePoint") && editPoints.Count == 3)
            {
                //select the point to edit
                int index = editPoints.IndexOf(hit.collider.gameObject);
                if (index != -1)
                {
                    if (Input.GetMouseButton(0))
                    {
                        editPoints[index].GetComponent<Collider>().enabled = false;
                        currentEdit = editPoints[index];
                    }
                }
            }

            if (Input.GetMouseButtonDown(0) && editPoints.Count != 3)
            {
                AddPoint(hit.point);            
            }
        }
        
        //create new angle dynamically when any of the points are moved
        if (Input.GetMouseButton(0) && currentEdit != null)
        {
            currentEdit.transform.position = productHitPoint;
            int index = editPoints.IndexOf(currentEdit);
            angleLine.SetPosition(index, productHitPoint);
            CreateAngle(angleLine.GetPosition(0), angleLine.GetPosition(1), angleLine.GetPosition(2));
        }
        //reset
        if (Input.GetMouseButtonUp(0) && currentEdit != null)
        {
            currentEdit.GetComponent<Collider>().enabled = true;
            currentEdit = null;
        }
    }

    /// <summary>
    /// Add a point to the angle line. The first point is the start point, the second point is point A, and the third point is point B.
    /// Instantiates a prefab at the point position.
    /// </summary>
    /// <param name="position"></param>
    private void AddPoint(Vector3 position)
    {
        switch (currPoint)
        {
            case AnglePoint.StartPoint:
                angleLine.transform.position = position;
                startPoint = position;
                editPoints.Add(Instantiate(pointPrefab, startPoint, Quaternion.identity));
                currPoint = AnglePoint.PointA;
                break;
            case AnglePoint.PointA:
                angleLine.positionCount = 2;
                angleLine.SetPosition(0, startPoint + (Vector3.up * 0.01f));
                angleLine.SetPosition(1, position + (Vector3.up * 0.01f));
                currPoint = AnglePoint.PointB;
                editPoints.Add(Instantiate(pointPrefab, position, Quaternion.identity));
                break;
            case AnglePoint.PointB:
                angleLine.positionCount = 3;
                angleLine.SetPosition(2, position + (Vector3.up * 0.01f));
                editPoints.Add(Instantiate(pointPrefab, position, Quaternion.identity));
                CreateAngle(angleLine.GetPosition(0), angleLine.GetPosition(1), angleLine.GetPosition(2));
                break;
        }
    }

    private void CreateAngle(Vector3 start, Vector3 pointA, Vector3 pointB)
    {
        angleArc.transform.position = pointA;
        angleText.transform.position = pointA;
        angleArc.gameObject.SetActive(true);
        angleText.gameObject.SetActive(true);

       //face the camera
        Quaternion faceCam = Quaternion.LookRotation(cam.transform.forward, cam.transform.up);

        // 3) get screen-space directions
        Vector2 screenO = cam.WorldToScreenPoint(pointA);
        Vector2 screen1 = new Vector2(cam.WorldToScreenPoint(start).x, cam.WorldToScreenPoint(start).y) - screenO;
        Vector2 screen2 = new Vector2(cam.WorldToScreenPoint(pointB).x, cam.WorldToScreenPoint(pointB).y) - screenO;
        screen1.Normalize();
        screen2.Normalize();

        // signed angle from vector1 → vector2 around +Z
        float signedAngle = Vector2.SignedAngle(screen1, screen2);

        // compute start angle relative to Vector2.right (East)
        float startAngle = Vector2.SignedAngle(Vector2.right, screen1);

        // rotate the arc so that zero‐fill sits on screen1
        angleArc.transform.rotation = faceCam * Quaternion.Euler(0, 0, startAngle);

        // set fill direction & amount
        angleArc.fillClockwise = signedAngle < 0f;
        angleArc.fillAmount = Mathf.Abs(signedAngle) / 360f;

        angleText.text = Mathf.Round(Mathf.Abs(signedAngle)) + "°";
        angleText.transform.rotation = faceCam;
    }

    public void Clear()
    {
        angleArc.gameObject.SetActive(false);
        angleText.gameObject.SetActive(false);
        angleLine.positionCount = 0;
        foreach (var point in editPoints)
        {
            Destroy(point);
        }
        editPoints.Clear();
        currPoint = AnglePoint.StartPoint;
        currentEdit = null;
        angleLine.transform.position = Vector3.zero;
        startPoint = Vector3.zero;
        productHitPoint = Vector3.zero;

    }

    public void EnableSystem()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        angleArc.gameObject.SetActive(false);
        angleText.gameObject.SetActive(false);
        Clear();
        systemActive = true;
    }

    public void DisableSystem()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        Clear();
        systemActive = false;
    }
}
