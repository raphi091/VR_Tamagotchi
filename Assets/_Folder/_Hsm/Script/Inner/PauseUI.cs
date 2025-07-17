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

    [Header("Input Settings")]
    public InputActionReference menuAction;      // XR LeftHand Menu 버튼
    public InputActionReference clickAction;     // XR LeftHand B 버튼 (SecondaryButton)

    [Header("Controller Reference")]
    public Transform leftHandTransform;          // 왼손 컨트롤러 기준 Ray 쏘는 위치

    [Header("Raycast")]
    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;

    [Header("Debug")]
    public bool enableDebugLogs = true;
    public LineRenderer debugRay;                // Ray 시각화용 (선택)

    private bool isPaused = false;

    private void Start()
    {
        if (menu != null) menu.SetActive(false);
        if (pan != null) pan.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (enableDebugLogs) Debug.Log("[PauseUI] 초기화 완료");
    }

    private void OnEnable()
    {
        if (menuAction != null) menuAction.action.performed += OnMenuButtonPressed;
        if (clickAction != null) clickAction.action.performed += OnClickButtonPressed;

        menuAction?.action.Enable();
        clickAction?.action.Enable();
    }

    private void OnDisable()
    {
        if (menuAction != null) menuAction.action.performed -= OnMenuButtonPressed;
        if (clickAction != null) clickAction.action.performed -= OnClickButtonPressed;

        menuAction?.action.Disable();
        clickAction?.action.Disable();
    }

    private void Update()
    {
        if (debugRay != null && leftHandTransform != null)
        {
            debugRay.SetPosition(0, leftHandTransform.position);
            debugRay.SetPosition(1, leftHandTransform.position + leftHandTransform.forward * 10f);
        }
    }

    private void OnMenuButtonPressed(InputAction.CallbackContext ctx)
    {
        if (isPaused) CloseMenu();
        else OpenMenu();
    }

    private void OnClickButtonPressed(InputAction.CallbackContext ctx)
    {
        if (!isPaused) return;
        TryRaycastUI();
    }

    public void OpenMenu()
    {
        if (menu != null) menu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        if (enableDebugLogs) Debug.Log("[PauseUI] 메뉴 열림");
    }

    public void CloseMenu()
    {
        if (menu != null) menu.SetActive(false);
        if (pan != null) pan.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (enableDebugLogs) Debug.Log("[PauseUI] 메뉴 닫힘");
    }

    private void TryRaycastUI()
    {
        Vector2 screenPoint;

        // 월드 → 스크린 포인트 변환
        if (Camera.main != null)
        {
            screenPoint = Camera.main.WorldToScreenPoint(leftHandTransform.position + leftHandTransform.forward * 5f);
        }
        else
        {
            screenPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        }

        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = screenPoint
        };

        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(pointerData, results);

        foreach (var result in results)
        {
            Button btn = result.gameObject.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.Invoke();
                if (enableDebugLogs) Debug.Log($"[PauseUI] 버튼 클릭됨: {btn.name}");
                return;
            }
        }

        if (enableDebugLogs) Debug.Log("[PauseUI] Raycast 대상 없음");
    }


    public void OnClickBGMControl()
    {
        if (enableDebugLogs) Debug.Log("[PauseUI] BGM 설정 클릭");
        if (pan != null) pan.SetActive(true);
    }

    public void OnClickSave()
    {
        if (enableDebugLogs) Debug.Log("[PauseUI] 저장 클릭");
        if (pan != null) pan.SetActive(false);
        // 저장 로직 구현 필요
    }

    public void OnClickReturnToLobby()
    {
        if (enableDebugLogs) Debug.Log("[PauseUI] 로비로 이동");
        if (pan != null) pan.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("H_Lobby");
    }

    public void OnClickExit()
    {
        if (enableDebugLogs) Debug.Log("[PauseUI] 종료 클릭");
        if (pan != null) pan.SetActive(false);
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
