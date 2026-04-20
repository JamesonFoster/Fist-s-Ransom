using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
    public IEnumerator DirectionalShake(Vector3 direction, float distance, float duration)
    {
        Vector3 originalPos = transform.localPosition;
        Vector3 targetPos = originalPos + direction.normalized * distance;

        float elapsed = 0f;

        // Move outward
        while (elapsed < duration / 2f)
        {
            transform.localPosition = Vector3.Lerp(originalPos, targetPos, elapsed / (duration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        // Move back
        while (elapsed < duration / 2f)
        {
            transform.localPosition = Vector3.Lerp(targetPos, originalPos, elapsed / (duration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}