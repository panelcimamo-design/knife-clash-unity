using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject agentSelectPanel;
    public GameObject weaponSelectPanel;
    public GameObject gameModesPanel;
    public GameObject settingsPanel;

    [Header("Buttons")]
    public Button playButton;
    public Button settingsButton;
    public Button exitButton;
    public Button[] agentButtons;
    public Button[] weaponButtons;
    public Button[] gameModeButtons;
    public Button backButton;

    [Header("UI Elements")]
    public Text titleText;
    public Image agentPreview;
    public Image weaponPreview;
    public Text descriptionText;

    [Header("Audio")]
    public AudioSource uiAudioSource;
    public AudioClip buttonClickSound;
    public AudioClip menuBGM;

    private int selectedAgent = 0;
    private int selectedWeapon = 0;
    private GameManager.GameMode selectedMode = GameManager.GameMode.Unrated;

    void Start()
    {
        SetupButtons();
        ShowMainMenu();
        PlayMenuMusic();
    }

    void SetupButtons()
    {
        playButton.onClick.AddListener(ShowGameModes);
        settingsButton.onClick.AddListener(ShowSettings);
        exitButton.onClick.AddListener(ExitGame);
        backButton.onClick.AddListener(ShowMainMenu);

        for (int i = 0; i < agentButtons.Length; i++)
        {
            int index = i;
            agentButtons[i].onClick.AddListener(() => SelectAgent(index));
        }

        for (int i = 0; i < weaponButtons.Length; i++)
        {
            int index = i;
            weaponButtons[i].onClick.AddListener(() => SelectWeapon(index));
        }

        for (int i = 0; i < gameModeButtons.Length; i++)
        {
            int index = i;
            gameModeButtons[i].onClick.AddListener(() => SelectGameMode(index));
        }
    }

    void ShowMainMenu()
    {
        PlayButtonSound();
        HideAllPanels();
        mainMenuPanel.SetActive(true);
        titleText.text = "VALORANT";
    }

    void ShowGameModes()
    {
        PlayButtonSound();
        HideAllPanels();
        gameModesPanel.SetActive(true);
        titleText.text = "SELECT GAME MODE";
    }

    void SelectGameMode(int mode)
    {
        PlayButtonSound();
        selectedMode = (GameManager.GameMode)mode;
        ShowAgentSelect();
    }

    void ShowAgentSelect()
    {
        HideAllPanels();
        agentSelectPanel.SetActive(true);
        titleText.text = "SELECT AGENT";
    }

    void SelectAgent(int agent)
    {
        PlayButtonSound();
        selectedAgent = agent;
        
        GameManager.Agent selectedAgentData = GameManager.Instance.GetAgent(agent);
        if (selectedAgentData != null)
        {
            descriptionText.text = selectedAgentData.name;
        }
        
        ShowWeaponSelect();
    }

    void ShowWeaponSelect()
    {
        HideAllPanels();
        weaponSelectPanel.SetActive(true);
        titleText.text = "SELECT WEAPON";
    }

    void SelectWeapon(int weapon)
    {
        PlayButtonSound();
        selectedWeapon = weapon;
        
        GameManager.Weapon selectedWeaponData = GameManager.Instance.GetWeapon(weapon);
        if (selectedWeaponData != null)
        {
            descriptionText.text = $"{selectedWeaponData.name} - Damage: {selectedWeaponData.damage}";
        }
        
        StartGame();
    }

    void ShowSettings()
    {
        PlayButtonSound();
        HideAllPanels();
        settingsPanel.SetActive(true);
        titleText.text = "SETTINGS";
    }

    void StartGame()
    {
        PlayButtonSound();
        SceneManager.LoadScene("GameScene");
    }

    void ExitGame()
    {
        PlayButtonSound();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void HideAllPanels()
    {
        mainMenuPanel.SetActive(false);
        gameModesPanel.SetActive(false);
        agentSelectPanel.SetActive(false);
        weaponSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    void PlayMenuMusic()
    {
        if (uiAudioSource != null && menuBGM != null)
        {
            uiAudioSource.clip = menuBGM;
            uiAudioSource.loop = true;
            uiAudioSource.Play();
        }
    }

    void PlayButtonSound()
    {
        if (uiAudioSource != null && buttonClickSound != null)
        {
            AudioSource.PlayClipAtPoint(buttonClickSound, transform.position);
        }
    }
}
