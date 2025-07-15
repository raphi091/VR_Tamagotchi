using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float floatDistance = 1f;       // 얼마나 위로 떠오를지
    public float floatDuration = 1.5f;     // 전체 지속 시간
    public float fadeOutDelay = 0.3f;      // 얼마나 후에 투명해질지

    private TextMeshPro textMesh;
    private Color startColor;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        startColor = textMesh.color;
    }

    void OnEnable()
    {
        StartCoroutine(AnimateFloating());
    }

    private System.Collections.IEnumerator AnimateFloating()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * floatDistance;

        float timer = 0f;

        while (timer < floatDuration)
        {
            timer += Time.deltaTime;
            float t = timer / floatDuration;

            // 위치 이동
            transform.position = Vector3.Lerp(startPos, endPos, t);

            // 일정 시간 지나면 점점 투명하게
            if (t > fadeOutDelay)
            {
                float fadeT = (t - fadeOutDelay) / (1f - fadeOutDelay);
                Color newColor = startColor;
                newColor.a = Mathf.Lerp(1f, 0f, fadeT);
                textMesh.color = newColor;
            }

            yield return null;
        }

        gameObject.SetActive(false); // 끝나면 꺼버리기
    }
}
