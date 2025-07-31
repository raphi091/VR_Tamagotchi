using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject menu;
    public GameObject pan;
    public GameObject btn;
    public GameObject slider;

    [Header("Input Settings")]
    public InputActionReference menuAction;      // XR LeftHand Menu 버튼

    [Header("Controller Reference")]
    public Transform leftHandTransform;          // 왼손 컨트롤러 기준 Ray 쏘는 위치

    [Header("Sound")]
    public AudioClip clickbtn;

    private XRInput input;

    private bool isPaused = false;
    private bool canInteract = false;            // 상호작용 가능 여부

    private void Awake()
    {
        input = new XRInput();
    }

    private void Start()
    {
        menu.SetActive(false);
        pan.SetActive(false);
    }

    private void OnEnable()
    {
        if (menuAction != null)
        {
            menuAction.action.performed += OnMenuButtonPressed;
            menuAction.action.Enable(); // 여기에 꼭 넣어야 동작합니다!
        }
        canInteract = true;
        // Enable은 Start()에서 지연 후 실행
    }

    private void OnDisable()
    {
        if (menuAction != null) menuAction.action.performed -= OnMenuButtonPressed;

        menuAction?.action.Disable();
    }

    private void OnMenuButtonPressed(InputAction.CallbackContext ctx)
    {
        // 상호작용 불가능하면 무시
        if (!canInteract) return;

        // 버튼이 눌렸을 때만 실행 (떼어질 때는 무시)
        if (ctx.phase != InputActionPhase.Performed) return;

        if (isPaused) CloseMenu();
        else OpenMenu();
    }

    public void OpenMenu()
    {
        SoundManager.Instance.PlaySFX(clickbtn);

        if (menu != null) menu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        input.XRIUI.Enable();

        SetSelectedUIElement(btn);
    }

    public void CloseMenu()
    {
        if (menu != null) menu.SetActive(false);
        if (pan != null) pan.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        input.XRIUI.Disable();
        DataManager_J.instance.SaveSettings();
    }

    public void OnClickBGMControl()
    {
        SoundManager.Instance.PlaySFX(clickbtn);

        if (pan != null)
        {
            pan.SetActive(true);
            SetSelectedUIElement(slider);
        } 
    }

    // 강제로 모든 메뉴 닫기 (씬 전환 전 반드시 호출)
    private void ForceCloseAllMenus()
    {
        if (menu != null) menu.SetActive(false);
        if (pan != null) pan.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        canInteract = false; // 상호작용도 비활성화
    }

    public void OnClickReturnToLobby()
    {
        SoundManager.Instance.PlaySFX(clickbtn);

        ForceCloseAllMenus();
        SceneManager.LoadScene("H_Lobby");
    }

    public void OnClickExit()
    {
        SoundManager.Instance.PlaySFX(clickbtn);

        ForceCloseAllMenus();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetSelectedUIElement(GameObject element)
    {
        EventSystem.current.SetSelectedGameObject(element);
    }
}