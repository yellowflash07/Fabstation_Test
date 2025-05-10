using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RawImage rawImage;    // The map
    [SerializeField] private Camera rtCamera;    // camera rendering the map
    [SerializeField] private Camera mainCamera;  // the world camera

    public void OnPointerClick(PointerEventData eventData)
    {
       // get the local point in the RawImage
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rawImage.rectTransform,
            eventData.position,
            eventData.pressEventCamera, 
            out Vector2 localPoint
        );

        // Normalize
        Rect rect = rawImage.rectTransform.rect;
        float u = (localPoint.x - rect.x) / rect.width;
        float v = (localPoint.y - rect.y) / rect.height;      

        // scale to texture size
        RenderTexture rt = rtCamera.targetTexture;
        float px = u * rt.width;
        float py = v * rt.height;

        // Build a ray from the RT camera through that pixel
        Ray ray = rtCamera.ScreenPointToRay(new Vector3(px, py, 0));

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 destination = hit.point;
            Vector3 camPos = mainCamera.transform.position;
            Vector3 destPos = new Vector3(destination.x,camPos.y,destination.z);
            mainCamera.transform.DOMove(destPos, 0.2f).SetEase(Ease.OutCubic); //smooth movement
        }
        else
        {
            // if we don't hit anything, we can use a plane to get the destination
            Plane ground = new Plane(Vector3.up, Vector3.zero);
            if (ground.Raycast(ray, out float enter))
            {
                Vector3 destination = ray.GetPoint(enter);
                Vector3 camPos = mainCamera.transform.position;
                Vector3 destPos = new Vector3(destination.x, camPos.y, destination.z);
                mainCamera.transform.DOMove(destPos, 0.2f).SetEase(Ease.OutCubic);
            } 
        }

    }
}
