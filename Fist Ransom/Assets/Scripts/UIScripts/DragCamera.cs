using UnityEngine;

public class DragCamera : MonoBehaviour
{
    public float dragSpeed = 1f;
    public float minX;
    public float maxX;

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
}
