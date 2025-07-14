using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LunchSceneManager_Dummy : MonoBehaviour
{
    public List<LunchDogDummy> dogCubes; // 큐브 3개
    public List<FoodBowl> foodBowls;
    // public AudioSource bellSound;

    private bool triggered = false;

    void Start()
    {
        StartCoroutine(FillBowlsAfterDelay());
    }

    IEnumerator FillBowlsAfterDelay()
    {
        yield return new WaitForSeconds(3f); // 3초 대기

        foreach (var bowl in foodBowls)
        {
            bowl.Fill();
        }
    }

    void Update()
    {
        if (!triggered && foodBowls.TrueForAll(b => b.IsFilled))
        {
            triggered = true;
            // bellSound?.Play();

            foreach (var dog in dogCubes)
                dog.StartLunch();
        }
    }

    public void OnAllDogsFinished()
    {
        StartCoroutine(GoToNextScene());
    }

    private IEnumerator GoToNextScene()
    {
        yield return new WaitForSeconds(2f);
        yield return null;
    }
}
