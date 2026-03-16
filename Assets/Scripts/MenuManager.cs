// MenuManager.cs  (updated — restart skips the start screen)
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject youWonPanel;
    [SerializeField] private PlayerInputs playerInputs;

    private bool isPaused = false;

    // Static flag survives scene reloads — set to true when restarting
    private static bool s_skipStartMenu = false;

    public bool IsPaused => isPaused;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (s_skipStartMenu)
        {
            s_skipStartMenu = false; // reset for future restarts from main menu
            StartGame();
        }
        else
        {
            ShowStartMenu();
        }
    }

    private void SetGameActive(bool active)
    {
        Time.timeScale = active ? 1f : 0f;
        Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !active;
        if (playerInputs != null)
            playerInputs.enabled = active;
        if (active)
        {
            UIManager.Instance?.ShowCrosshair();
            UIManager.Instance?.ShowHUD();
        }
        else
        {
            UIManager.Instance?.HideCrosshair();
            UIManager.Instance?.HideHUD();
        }
    }

    private void ShowStartMenu()
    {
        startMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        youWonPanel.SetActive(false);
        SetGameActive(false);
    }

    public void StartGame()
    {
        startMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        youWonPanel.SetActive(false);
        isPaused = false;
        SetGameActive(true);
    }

    public void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);
        SetGameActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        SetGameActive(true);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);

        if (CheckpointManager.Instance != null && CheckpointManager.Instance.HasCheckpoint)
        {
            StartCoroutine(RespawnAfterDelay(1.5f));
        }
        else
        {
            SetGameActive(false);
        }
    }

    private System.Collections.IEnumerator RespawnAfterDelay(float delay)
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;

        gameOverPanel.SetActive(false);

        CheckpointManager.Instance.RespawnPlayer(GayManager.Instance.Player);
        SetGameActive(true);
    }

    public void ShowYouWon()
    {
        youWonPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        SetGameActive(false);
    }

    public void RestartGame()
    {
        s_skipStartMenu = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}