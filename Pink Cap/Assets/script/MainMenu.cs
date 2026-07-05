using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Экраны меню")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelsPanel;

    [Header("Кнопки уровней (всего 5 штук)")]
    [SerializeField] private Button[] levelButtons;

    void Start()
    {
        
        ShowMainMenu();
    }

    

    public void StartGame()
    {
        
        SceneManager.LoadScene("Level 1"); 
    }

    public void OpenLevelsMenu()
    {
        mainMenuPanel.SetActive(false);
        levelsPanel.SetActive(true);


        int reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);


        for (int i = 0; i < levelButtons.Length; i++)
        {
            
            int levelNumber = i + 1;

            if (levelNumber <= reachedLevel)
            {
                levelButtons[i].interactable = true;  
            }
            else
            {
                levelButtons[i].interactable = false; 
            }
        }
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        levelsPanel.SetActive(false);
    }



    public void LoadSpecificLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }


    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("ReachedLevel");
        OpenLevelsMenu(); 
    }
}