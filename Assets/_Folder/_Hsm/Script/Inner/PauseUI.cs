using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{
    public GameObject menu;
    public InputActionReference menuAction;

    private bool isPaused = false;

    private void Start()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        if(menuAction != null)
        {
            menuAction.action.performed += OnMenuButtonPressed;
        }
    }
    private void OnDisable()
    {
        if (menuAction != null)
        {
            menuAction.action.performed -= OnMenuButtonPressed;
        }
    }
    private void OnMenuButtonPressed(InputAction.CallbackContext ctx)
    {
        if (isPaused)
            CloseMenu();
        else
            OpenMenu();
    }
    public void OpenMenu()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void CloseMenu()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

}
