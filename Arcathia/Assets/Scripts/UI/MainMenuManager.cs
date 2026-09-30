using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject modeSelectionPanel;

    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button customizeButton;
    public Button settingsButton;
    public Button quitButton;

    [Header("Mode Selection Buttons")]
    public Button campaignButton;
    public Button multiplayerButton;
    public Button backButton;

    [Header("Scene Build Settings Names")]
    public string campaignSceneName = "CampaignStage1";
    public string multiplayerLobbySceneName = "MultiplayerLobby";

    void Start()
    {
        // Set initial panel states
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (modeSelectionPanel != null) modeSelectionPanel.SetActive(false);

        // Bind Main Menu Button Events
        if (startButton != null) startButton.onClick.AddListener(OnStartButtonPressed);
        if (customizeButton != null) customizeButton.onClick.AddListener(OnCustomizeButtonPressed);
        if (settingsButton != null) settingsButton.onClick.AddListener(OnSettingsButtonPressed);
        if (quitButton != null) quitButton.onClick.AddListener(OnQuitButtonPressed);

        // Bind Mode Selection Button Events
        if (campaignButton != null) campaignButton.onClick.AddListener(OnCampaignButtonPressed);
        if (multiplayerButton != null) multiplayerButton.onClick.AddListener(OnMultiplayerButtonPressed);
        if (backButton != null) backButton.onClick.AddListener(OnBackButtonPressed);
    }

    #region Main Menu Actions

    private void OnStartButtonPressed()
    {
        // Hide Main Panel and show Mode Selection
      
        modeSelectionPanel.SetActive(true);
    }

    private void OnCustomizeButtonPressed()
    {
        // Placeholder for Customize Mode
        Debug.Log("[MainMenu] Customize Mode pressed - Feature coming soon.");
    }

    private void OnSettingsButtonPressed()
    {
        // Placeholder for Settings
        Debug.Log("[MainMenu] Settings pressed - Feature coming soon.");
    }

    private void OnQuitButtonPressed()
    {
        Debug.Log("[MainMenu] Quitting Application...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    #endregion

    #region Mode Selection Actions

    private void OnCampaignButtonPressed()
    {
        Debug.Log("[MainMenu] Starting Campaign Mode...");
        // Load single-player campaign scene
        if (!string.IsNullOrEmpty(campaignSceneName))
        {
            SceneManager.LoadScene(campaignSceneName);
        }
    }

    private void OnMultiplayerButtonPressed()
    {
        Debug.Log("[MainMenu] Entering Multiplayer Mode...");
        // Load multiplayer lobby or stage scene
        if (!string.IsNullOrEmpty(multiplayerLobbySceneName))
        {
            SceneManager.LoadScene(multiplayerLobbySceneName);
        }
    }

    private void OnBackButtonPressed()
    {
        // Return to main screen
        modeSelectionPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    #endregion
}