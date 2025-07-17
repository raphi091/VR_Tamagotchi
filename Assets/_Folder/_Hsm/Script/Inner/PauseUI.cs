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
    private bool canInteract = false;            // 상호작용 가능 여부

    private void Start()
    {
        // 강제로 모든 UI 초기화
        ForceInitializeUI();

        // LineRenderer 설정
        if (debugRay != null)
        {
            debugRay.positionCount = 2;
        }

        // 씬 로드 후 잠시 대기하여 Input 충돌 방지
        StartCoroutine(EnableInteractionAfterDelay());

        if (enableDebugLogs) Debug.Log("[PauseUI] 초기화 완료");
    }

    private IEnumerator EnableInteractionAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // 0.5초 대기
        canInteract = true;

        // Input Action 활성화
        menuAction?.action.Enable();
        clickAction?.action.Enable();

        if (enableDebugLogs) Debug.Log("[PauseUI] 상호작용 활성화");
    }

    private void OnEnable()
    {
        if (menuAction != null) menuAction.action.performed += OnMenuButtonPressed;
        if (clickAction != null) clickAction.action.performed += OnClickButtonPressed;

        // Enable은 Start()에서 지연 후 실행
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
            // positionCount 안전성 체크
            if (debugRay.positionCount < 2)
            {
                debugRay.positionCount = 2;
            }

            debugRay.SetPosition(0, leftHandTransform.position);
            debugRay.SetPosition(1, leftHandTransform.position + leftHandTransform.forward * 10f);
        }
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

    private void OnClickButtonPressed(InputAction.CallbackContext ctx)
    {
        if (!canInteract) return;
        if (!isPaused) return;

        if (ctx.phase != InputActionPhase.Performed) return;

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
        if (leftHandTransform == null) return;

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

    // 강제로 모든 메뉴 닫기 (씬 전환 전 반드시 호출)
    private void ForceCloseAllMenus()
    {
        if (menu != null) menu.SetActive(false);
        if (pan != null) pan.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        canInteract = false; // 상호작용도 비활성화

        if (enableDebugLogs) Debug.Log("[PauseUI] 모든 메뉴 강제 닫힘");
    }

    // UI 강제 초기화 (Start에서 호출)
    private void ForceInitializeUI()
    {
        if (menu != null)
        {
            menu.SetActive(false);
            if (enableDebugLogs) Debug.Log("[PauseUI] 메뉴 강제 비활성화");
        }

        if (pan != null)
        {
            pan.SetActive(false);
            if (enableDebugLogs) Debug.Log("[PauseUI] 패널 강제 비활성화");
        }

        Time.timeScale = 1f;
        isPaused = false;
        canInteract = false;

        if (enableDebugLogs) Debug.Log("[PauseUI] UI 강제 초기화 완료");
    }

    // 점심씬으로 이동하는 메서드 (필요하면 추가)
    public void OnClickGoToLunchScene()
    {
        if (enableDebugLogs) Debug.Log("[PauseUI] 점심씬으로 이동");
        ForceCloseAllMenus();
        SceneManager.LoadScene("H_Lunch"); // 실제 씬 이름으로 변경
    }

    // 외부씬으로 이동하는 메서드 (필요하면 추가)
    public void OnClickGoToOutdoorScene()
    {
        if (enableDebugLogs) Debug.Log("[PauseUI] 외부씬으로 이동");
        ForceCloseAllMenus();
        SceneManager.LoadScene("H_Outdoor"); // 실제 씬 이름으로 변경
    }

    public void OnClickReturnToLobby()
    {
        if (enableDebugLogs) Debug.Log("[PauseUI] 로비로 이동");
        ForceCloseAllMenus();
        SceneManager.LoadScene("H_Lobby");
    }

    public void OnClickExit()
    {
        if (enableDebugLogs) Debug.Log("[PauseUI] 종료 클릭");
        ForceCloseAllMenus();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}