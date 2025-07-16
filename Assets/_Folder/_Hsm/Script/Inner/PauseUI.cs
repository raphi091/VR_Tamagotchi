using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseUI : MonoBehaviour
{
    public GameObject menu;
    public GameObject pan;
    public InputActionReference menuAction;

   
    private bool isPaused = false;

    private void Update()
    {
        if (menuAction != null && menuAction.action != null && menuAction.action.triggered)
        {
            Debug.Log("MenuButton 눌림");
        }
    }

    private void Start()
    {
        menu.SetActive(false);
        pan.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        //inputActions?.Enable();
        if(menuAction != null)
        {
            menuAction.action.Enable();
            menuAction.action.performed += OnMenuButtonPressed;
        }
    }
    private void OnDisable()
    {
        //inputActions?.Disable();
        if (menuAction != null)
        {
            menuAction.action.performed -= OnMenuButtonPressed;
            menuAction.action.Disable();
        }
    }
    private void OnMenuButtonPressed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Menu");
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

    public void OnClickBGMControl()
    {
        Debug.Log("BGM On");
        pan.SetActive(true);

        //BGM 기능 추가
    }
    public void OnClickSave()
    {
        Debug.Log("Save On");
        pan.SetActive(false);
    }
    public void OnClickReturnToLobby()
    {
        Debug.Log("Lobby Go");
        pan.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("H_Lobby");
    }
    public void OnClickExit()
    {
        Debug.Log("Exit!");
        pan.SetActive(false);
        Application.Quit();
    }



}
