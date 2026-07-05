using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Экраны меню")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelsPanel;

    [Header("Экран Сюжетного Вступления")]
    [SerializeField] private CanvasGroup introCanvasGroup;
    [SerializeField] private float introFadeDuration = 1.5f;
    [SerializeField] private float introDisplayTime = 3f;

    [Header("Кнопки уровней (всего 5 штук)")]
    [SerializeField] private Button[] levelButtons;

    void Start()
    {
        mainMenuPanel.SetActive(true);
        levelsPanel.SetActive(false);

        if (introCanvasGroup != null)
        {
            introCanvasGroup.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        if (UiAudioManager.Instance != null) UiAudioManager.Instance.PlayButtonClick();

        if (introCanvasGroup != null)
        {
            StartCoroutine(IntroSequence());
        }
        else
        {
            SceneManager.LoadScene("Level 1");
        }
    }

    private IEnumerator IntroSequence()
    {
        mainMenuPanel.SetActive(false);

        introCanvasGroup.alpha = 0f;
        introCanvasGroup.gameObject.SetActive(true);

        float time = 0;
        while (time < introFadeDuration)
        {
            time += Time.deltaTime;
            introCanvasGroup.alpha = Mathf.Lerp(0f, 1f, time / introFadeDuration);
            yield return null;
        }
        introCanvasGroup.alpha = 1f;

        yield return new WaitForSeconds(introDisplayTime);

        time = 0;
        while (time < introFadeDuration)
        {
            time += Time.deltaTime;
            introCanvasGroup.alpha = Mathf.Lerp(1f, 0f, time / introFadeDuration);
            yield return null;
        }
        introCanvasGroup.alpha = 0f;

        SceneManager.LoadScene("Level 1");
    }

    public void OpenLevelsMenu()
    {
        if (UiAudioManager.Instance != null) UiAudioManager.Instance.PlayButtonClick();

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

        if (UiAudioManager.Instance != null) UiAudioManager.Instance.PlayButtonClick();

        mainMenuPanel.SetActive(true);
        levelsPanel.SetActive(false);
    }

    public void LoadSpecificLevel(string levelName)
    {

        if (UiAudioManager.Instance != null) UiAudioManager.Instance.PlayButtonClick();

        SceneManager.LoadScene(levelName);
    }

    public void ResetProgress()
    {
      
        if (UiAudioManager.Instance != null) UiAudioManager.Instance.PlayButtonClick();

        PlayerPrefs.DeleteKey("ReachedLevel");

        mainMenuPanel.SetActive(false);
        levelsPanel.SetActive(true);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = (i == 0); 
        }
    }
}