using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 1f;

//simple zoom functionality
    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        transform.position += transform.forward * scroll * scrollSpeed;

    }
}
