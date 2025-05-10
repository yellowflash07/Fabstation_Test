using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class MiniMapCameraController : MonoBehaviour
{
    [SerializeField] private GameObject product;
    [SerializeField] private GameObject boundingBox;
    [SerializeField] private float paddingPercent;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        Bounds bounds = CalculateBounds(product);

        boundingBox.transform.position = bounds.center;
        boundingBox.transform.localScale = bounds.size * 1.5f;
        boundingBox.transform.SetParent(product.transform);
    }

    void Update()
    {
        //bounding box of the product
        Bounds bounds = CalculateBounds(product);

        //follow the product
        transform.position = new Vector3(bounds.center.x, transform.position.y, bounds.center.z);

        //get the size
        var boundsSize = bounds.size;
        //calculate the diagonal of the bounding box
        float diagonal = Mathf.Sqrt(boundsSize.x * boundsSize.x + boundsSize.y * boundsSize.y + boundsSize.z * boundsSize.z);
        //orthographic size is half the diagonal
        cam.orthographicSize = diagonal / 2 + paddingPercent * diagonal;
        
    }

    public Bounds CalculateBounds(GameObject go)
    {
        var renderers = go.GetComponentsInChildren<Renderer>();
        Bounds bounds = renderers[0].bounds;
        foreach (var r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }
        return bounds;
    }
}
