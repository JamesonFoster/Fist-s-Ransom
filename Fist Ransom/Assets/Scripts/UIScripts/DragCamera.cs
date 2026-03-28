using UnityEngine;

public class DragCamera : MonoBehaviour
{
    public float dragSpeed = 1f;
    public float minX;
    public float maxX;
    public string playerMarkerTag = "PlayerMarker";
    private Vector3 dragOrigin;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = dragOrigin - currentPos;

            Vector3 newPosition = transform.position + new Vector3(difference.x * dragSpeed, 0, 0);

            // Clamp camera position
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

            transform.position = Vector3.Lerp(transform.position, newPosition, 10f * Time.deltaTime);
        }
    }
    void Start()
    {
        StartCoroutine(SnapNextFrame());
    }  
    void SnapToPlayerAtStart()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag(playerMarkerTag);

        foreach (GameObject obj in markers)
        {
            if (obj.activeSelf)
            {
                Vector3 newPos = transform.position;
                newPos.x = obj.transform.position.x;

                // Clamp so it doesn't go out of bounds
                newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

                transform.position = newPos;
                return;
            }
        }
    }
    System.Collections.IEnumerator SnapNextFrame()
    {
        yield return null; //1 frame wait
        SnapToPlayerAtStart();
    }
}
