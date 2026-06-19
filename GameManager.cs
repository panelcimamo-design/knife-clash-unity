using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [System.Serializable]
    public class Weapon
    {
        public string name;
        public int damage;
        public float fireRate;
        public int ammo;
        public int maxAmmo;
        public float reloadTime;
        public Mesh model;
        public Material material;
    }

    [System.Serializable]
    public class Agent
    {
        public string name;
        public int health;
        public float speed;
        public Ability[] abilities;
        public Mesh model;
        public Material[] skins;
    }

    [System.Serializable]
    public class Ability
    {
        public string name;
        public float cooldown;
        public int cost;
        public string description;
    }

    [Header("Game Settings")]
    public int roundDuration = 120;
    public int maxRounds = 13;
    public int teamSize = 5;
    public GameMode currentMode = GameMode.Unrated;

    [Header("Weapons")]
    public Weapon[] weapons = new Weapon[8];

    [Header("Agents")]
    public Agent[] agents = new Agent[4];

    [Header("Teams")]
    public Team[] teams = new Team[2];

    [Header("UI")]
    public Canvas gameCanvas;
    public Text scoreText;
    public Text timerText;
    public Text agentNameText;
    public Text weaponNameText;
    public Image weaponIcon;
    public Image agentIcon;

    [Header("Lighting")]
    public Light mainLight;
    public Light[] environmentLights;

    [Header("Audio")]
    public AudioSource bgmSource;
    public AudioClip gameplayMusic;
    public AudioClip[] weaponSounds;
    public AudioClip[] voiceLines;

    private float roundTime;
    private int currentRound = 1;
    private bool isRoundActive = false;
    private Player[] allPlayers;

    public enum GameMode
    {
        Unrated,
        Competitive,
        Deathmatch,
        Spike
    }

    [System.Serializable]
    public class Team
    {
        public string name;
        public Color color;
        public int score;
        public Player[] players;
    }

    [System.Serializable]
    public class Player
    {
        public string name;
        public Agent selectedAgent;
        public Weapon selectedWeapon;
        public int health;
        public int credits;
        public int kills;
        public int deaths;
        public int assists;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InitializeWeapons();
        InitializeAgents();
        SetupLighting();
    }

    void Start()
    {
        StartRound();
        PlayGameplayMusic();
    }

    void InitializeWeapons()
    {
        weapons = new Weapon[8]
        {
            new Weapon { name = "Karambit", damage = 40, fireRate = 1.5f, ammo = 30, maxAmmo = 120, reloadTime = 2.1f },
            new Weapon { name = "Butterfly Knife", damage = 35, fireRate = 1.2f, ammo = 30, maxAmmo = 120, reloadTime = 2.0f },
            new Weapon { name = "Phantom", damage = 39, fireRate = 9.75f, ammo = 30, maxAmmo = 120, reloadTime = 2.5f },
            new Weapon { name = "Vandal", damage = 40, fireRate = 9.75f, ammo = 25, maxAmmo = 100, reloadTime = 2.25f },
            new Weapon { name = "Classic", damage = 22, fireRate = 6.75f, ammo = 30, maxAmmo = 120, reloadTime = 1.5f },
            new Weapon { name = "Sheriff", damage = 55, fireRate = 2.0f, ammo = 6, maxAmmo = 24, reloadTime = 2.2f },
            new Weapon { name = "Operator", damage = 150, fireRate = 0.6f, ammo = 5, maxAmmo = 20, reloadTime = 3.0f },
            new Weapon { name = "Ares", damage = 30, fireRate = 13.33f, ammo = 50, maxAmmo = 200, reloadTime = 2.8f }
        };
    }

    void InitializeAgents()
    {
        agents = new Agent[4]
        {
            new Agent { name = "Sage", health = 100, speed = 5.75f },
            new Agent { name = "Phoenix", health = 100, speed = 5.75f },
            new Agent { name = "Jett", health = 100, speed = 5.75f },
            new Agent { name = "Reyna", health = 100, speed = 5.75f }
        };
    }

    void SetupLighting()
    {
        if (mainLight == null)
        {
            GameObject lightObj = new GameObject("Main Light");
            mainLight = lightObj.AddComponent<Light>();
            mainLight.type = LightType.Directional;
            mainLight.intensity = 1.2f;
            mainLight.color = Color.white;
            lightObj.transform.rotation = Quaternion.Euler(45, 45, 0);
        }

        environmentLights = new Light[4];
        Color[] lightColors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow };

        for (int i = 0; i < 4; i++)
        {
            GameObject lightObj = new GameObject($"Environment Light {i}");
            environmentLights[i] = lightObj.AddComponent<Light>();
            environmentLights[i].type = LightType.Point;
            environmentLights[i].intensity = 0.8f;
            environmentLights[i].range = 15f;
            environmentLights[i].color = lightColors[i];

            float angle = (i / 4f) * 360f;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * 8f;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * 8f;
            lightObj.transform.position = new Vector3(x, 5, z);
        }

        RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.5f);
    }

    void StartRound()
    {
        roundTime = roundDuration;
        isRoundActive = true;
        UpdateUI();
    }

    void Update()
    {
        if (!isRoundActive)
            return;

        roundTime -= Time.deltaTime;
        if (roundTime <= 0)
        {
            EndRound();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (timerText != null)
            timerText.text = $"{Mathf.Max(0, Mathf.RoundToInt(roundTime))}s";

        if (scoreText != null)
            scoreText.text = $"{teams[0].score} - {teams[1].score}";
    }

    void EndRound()
    {
        isRoundActive = false;
        currentRound++;

        if (currentRound > maxRounds)
        {
            EndGame();
        }
        else
        {
            Invoke("StartRound", 5f);
        }
    }

    void EndGame()
    {
        Debug.Log("Game Ended!");
    }

    void PlayGameplayMusic()
    {
        if (bgmSource != null && gameplayMusic != null)
        {
            bgmSource.clip = gameplayMusic;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlayWeaponSound(int weaponIndex)
    {
        if (weaponIndex < weaponSounds.Length && weaponSounds[weaponIndex] != null)
        {
            AudioSource.PlayClipAtPoint(weaponSounds[weaponIndex], transform.position);
        }
    }

    public Weapon GetWeapon(int index)
    {
        return index >= 0 && index < weapons.Length ? weapons[index] : null;
    }

    public Agent GetAgent(int index)
    {
        return index >= 0 && index < agents.Length ? agents[index] : null;
    }
}
