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
    private bool gameStarted = false;

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
        ShowStartMenu();
    }

    private void Update() { }

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
        gameStarted = true;
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
        SetGameActive(false);
    }

    public void ShowYouWon()
    {
        youWonPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        SetGameActive(false);
    }

    public void RestartGame()
    {
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