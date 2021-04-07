using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuCamera : MonoBehaviour
{
    // Our Level prefabs are stored in this array
    public GameObject[] levelLoad;

    public GameObject titleObj;
    public GameObject loggingInObj;
    public GameObject playFabErrorObj;
    public GameObject menuObj;
    public GameObject promptObj;
    public GameObject optionsObj;
    public GameObject playfabManagerObject;

    public GameObject musicManager;

    private PlayFabManager playfabManager;

    public float rotationSpeed;

    public float gravityPower;

    public InputField usernameIn;

    private AudioSource buttonPress;

    private Vector3 target_rotation;
    private bool isFacingSky = true;
    private bool isFacingUp = false;
    
    // Start current level index at -1 for the main menu
    private int currentLevelIndex = -1;

    private string path;

    private static bool musicIsPlaying = false;

    private float lastButtonPress = 0.0f;

    void Start()
    {
        buttonPress = GetComponent<AudioSource>();
        path = Application.persistentDataPath + "/PleaseDontModifyThisPleaseDontMakeMeWriteAnEncryptionAlgorithm.txt";
        Physics.gravity = new Vector3(0.0f, gravityPower, 0.0f);
        Cursor.visible = true;
        Time.timeScale = 1.0f;
        playfabManager = playfabManagerObject.GetComponent<PlayFabManager>();
        DontDestroyOnLoad(musicManager);
        if(!musicIsPlaying)
        {
            musicManager.GetComponent<AudioSource>().Play();
            musicIsPlaying = true;
        }
    }

    public bool isLookingAtBirds()
    {
        return isFacingSky;
    }

    void Update()
    {
        // Rotate towards the desired rotation which gets updated in the Rotate functions
        //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, target_rotation, Time.deltaTime * rotationSpeed);

        transform.eulerAngles = new Vector3(
            Mathf.LerpAngle(transform.eulerAngles.x, target_rotation.x, rotationSpeed * Time.deltaTime),
            Mathf.LerpAngle(transform.eulerAngles.y, target_rotation.y, rotationSpeed * Time.deltaTime),
            Mathf.LerpAngle(transform.eulerAngles.z, target_rotation.z, rotationSpeed * Time.deltaTime)
        );
    }

    public void WillCauseError()
    {
        int[] haha = new int[2];
        int errorline = haha[5];
    }


    public void MainMenuValidateLogin()
    {
        // Validate login
        if(playfabManager.IsPlayerLoggedIn())
        {
            ValidateUsernameInitialized();
        }
        // Player is NOT logged in; login to Playfab
        else
        {
            playfabManager.Login();
            EnableLoginLoading();
        }
    }
    
    public void ValidateUsernameInitialized()
    {
        // If a username is already initialized, it's buisness as usual
        if(File.Exists(path))
        {
            DisableTitleCard();
            EnableMenu();
            Rotate180Degrees();
        }
        // No username file exists; prompt the user.
        else
        {
            DisableTitleCard();
            EnableUsernamePrompt();
        }
    }

    public void WriteUsernameToFile()
    {
        string username = usernameIn.text;
        if(!File.Exists(path))
        {
            File.WriteAllText(path, username);
        }
    }

    public void Rotate180Degrees()
    {
        if(isFacingSky)
        {
            target_rotation = new Vector3(0.0f, 180.0f, 0.0f);
            isFacingSky = false;
        }
        else
        {
            target_rotation = new Vector3(0.0f, 0.0f, 0.0f);
            isFacingSky = true;
        }
    }

    public void RotateUpAndBack()
    {
        if(isFacingUp)
        {
            target_rotation = new Vector3(0.0f, 0.0f, 0.0f);
            isFacingUp = false;
        }
        else
        {
            target_rotation = new Vector3(-90f, 0.0f, 0.0f);
            isFacingUp = true;
        }
    }

    // Finds the level in the scene at the specified index and destroy it
    public void DestroyLevelAtGivenIndex(int index)
    {
        string levelToDestroy = "Level" + index.ToString() + "(Clone)";
        GameObject objectToDestroy = GameObject.Find(levelToDestroy);
        Destroy(objectToDestroy);
    }

    // Instaniates the level in the scene at the specified index
    public void SpawnLevelAtGivenIndex(int index)
    {
        GameObject info = Instantiate(levelLoad[index], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        ScoreboardUpdater scoreboard = menuObj.transform.Find("Scoreboard").GetComponent<ScoreboardUpdater>();
        scoreboard.ClearScoreboard();
        scoreboard.EnableLoadingIcon();

        // Grab the LevelInformation from the instaniated object
        LevelInformation level = info.GetComponent<LevelInformation>();

        // Update the main menu and static class
        UpdateMainMenuUI(level);
        UpdateStaticClass(level);

        // Have scoreboard display information for specified level
        playfabManager.GetLeaderboard(level.associatedScoreboard);
    }

    // Updates the main menu components with the level information
    public void UpdateMainMenuUI(LevelInformation level)
    {
        // Use transform.Find to get the text from the menu UI
        Text titleText = menuObj.transform.Find("LevelInfo/TitlePanel/TitleText").gameObject.GetComponent<Text>();
        Text levelDescriptionText = menuObj.transform.Find("LevelInfo/LevelDescription/LevelDescriptionText").gameObject.GetComponent<Text>();

        // Update text components appropriately
        titleText.text = level.levelTitle;
        titleText.fontSize = level.titleFontSize;
        levelDescriptionText.text = level.levelDescription;
    }

    public void UpdateStaticClass(LevelInformation level)
    {
        // Static class gets updated with the level information
        DifficultyStatic.difficulty = level.difficulty;
        DifficultyStatic.trashChance = level.percentChanceForGarbage;
        DifficultyStatic.fallingSpeed = level.fallingSpeed;
        DifficultyStatic.playfabScoreboard = level.associatedScoreboard;
        DifficultyStatic.foodSpawnRate = level.foodSpawnRate;

        // Log for sanity check
        Debug.Log("DIFFICULTY: " + DifficultyStatic.difficulty + " TRASH CHANCE: " + DifficultyStatic.trashChance);
        Debug.Log("WILL SEND TO: " + DifficultyStatic.playfabScoreboard);
    }

    // Called by the >> button to step through the levels array
    public void NextDifficulty()
    {
        // Allow progression of difficulty after short delay to fix bug with scoreboard
        if(Time.time > lastButtonPress + 0.2f)
        {
            // We start at -1, so don't try to find something at the -1 index
            if(currentLevelIndex != -1)
            {
                DestroyLevelAtGivenIndex(currentLevelIndex);
            }

            // incr the current level index
            ++currentLevelIndex;

            // if we've gone over the amount of indexes in the array, set it back to 0 so it's circular
            if(currentLevelIndex == levelLoad.Length)
            {
                currentLevelIndex = 0;
            }
            SpawnLevelAtGivenIndex(currentLevelIndex);
            lastButtonPress = Time.time;
        }
    }

    public void PreviousDifficulty()
    {
        if(Time.time > lastButtonPress + 0.2f)
        {
            // Destroy the level at the current index
            DestroyLevelAtGivenIndex(currentLevelIndex);

            // decr the current level index
            --currentLevelIndex;

            // if we've gone negative with the indexes, wrap around so it's circular
            if(currentLevelIndex < 0)
            {
                currentLevelIndex = levelLoad.Length - 1;
            }
            SpawnLevelAtGivenIndex(currentLevelIndex);
            lastButtonPress = Time.time;
        }
    }

    public void StartGame()
    {
        if(currentLevelIndex == 0)
        {
            SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        } 
    }

    // PLAYFAB STUFF

    public void EnablePlayfabManager()
    {
        playfabManagerObject.SetActive(true);
    }

    public void UpdateDisplayName()
    {
        playfabManagerObject.GetComponent<PlayFabManager>().UpdateDisplayName(usernameIn.text);
    }
    
    // Is called upon successful login by PlayFabManager.
    public void SuccessfulLogin()
    {
        ValidateUsernameInitialized();
        DisableLoginLoading();
    }

    // Is called upon a failure to login
    public void LoginError()
    {
        EnableError();
        DisableLoginLoading();
    }

    // MENU BUTTONS

    public void ButtonPressSFX()
    {
        buttonPress.pitch = Random.Range(0.70f, 1.30f);
        buttonPress.Play();
    }

    public void DisableLoginLoading()
    {
        loggingInObj.SetActive(false);
    }

    public void EnableLoginLoading()
    {
        loggingInObj.SetActive(true);
    }

    public void EnableError()
    {
        playFabErrorObj.SetActive(true);
    }

    public void DisableTitleCard()
    {
        titleObj.SetActive(false);
    }

    public void EnableTitleCard()
    {
        titleObj.SetActive(true);
    }

    public void DisableMenu()
    {
        menuObj.SetActive(false);
    }

    public void EnableMenu()
    {
        menuObj.SetActive(true);
        NextDifficulty();
    }

    public void DisableUsernamePrompt()
    {
        promptObj.SetActive(false);
    }

    public void EnableUsernamePrompt()
    {
        promptObj.SetActive(true);
    }

    public void EnableOptions()
    {
        optionsObj.SetActive(true);
    }

    public void DisableOptions()
    {
        optionsObj.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleVSync(bool on)
    {
        if(on)
        {
            QualitySettings.vSyncCount = 1;
            Debug.Log("VSync On");
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            Debug.Log("VSync Off");
        }
    }

    public void TrackpadMode(bool check)
    {
        DifficultyStatic.trackpadMode = check;
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality, true);
    }
}
