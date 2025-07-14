using System.Collections;
using UnityEngine;

public class LunchDogDummy : MonoBehaviour
{
    public Transform waitPosition;
    public Transform bowlPosition;
    public GameObject goodText;
    public GameObject badText;
    public float eatingTime = 3f;

    public static int finishedCount = 0;

    void Start()
    {
        if (waitPosition != null)
            transform.position = waitPosition.position;

        // 처음엔 텍스트 꺼두기
        goodText?.SetActive(false);
        badText?.SetActive(false);
    }

    void Update()
    {
        if (goodText.activeSelf)
            goodText.transform.LookAt(Camera.main.transform);

        if (badText.activeSelf)
            badText.transform.LookAt(Camera.main.transform);
    }

    public void StartLunch()
    {
        StartCoroutine(LunchRoutine());
    }

    private IEnumerator LunchRoutine()
    {
        float speed = 1.5f;
        while (Vector3.Distance(transform.position, bowlPosition.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, bowlPosition.position, Time.deltaTime * speed);
            yield return null;
        }

        yield return new WaitForSeconds(eatingTime);

        if (Random.value > 0.3f) goodText.SetActive(true);
        else badText.SetActive(true);

        finishedCount++;
        if (finishedCount >= 3)
        {
            FindObjectOfType<LunchSceneManager_Dummy>().OnAllDogsFinished();
        }
    }
}
