using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UiAudioManager : MonoBehaviour
{
    public static UiAudioManager Instance { get; private set; }

    [Header("Компоненты вывода звука")]
    [Tooltip("Этот AudioSource настроен на 35% громкости для эффектов и 100% для реплик")]
    [SerializeField] private AudioSource sfxAudioSource;
    [Tooltip("Этот AudioSource настроен на 60% громкости и зациклен для фоновой музыки")]
    [SerializeField] private AudioSource musicAudioSource;

    [Header("Аудиоклипы интерфейса и игры")]
    [SerializeField] private AudioClip buttonClickClip;
    [SerializeField] private AudioClip robotDeathClip;

    [Header("Фоновая музыка (Громкость 60%)")]
    [SerializeField] private AudioClip sadMusicClip;
    [SerializeField] private AudioClip funMusicClip;

    [Header("Дорожки начала уровней (Громкость 100%)")]
    [Tooltip("Перетащи сюда аудиозаписи строго по порядку: элемент 0 = Уровень 1...")]
    [SerializeField] private AudioClip[] levelStartClips = new AudioClip[5];
    [SerializeField] private float delayBeforePlay = 0.5f;

    private bool[] playedIntros = new bool[5];

    private const float SFX_VOLUME = 0.35f;
    private const float INTRO_VOLUME = 1.00f;
    private const float MUSIC_VOLUME = 0.60f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (musicAudioSource != null)
            {
                musicAudioSource.loop = true;
                musicAudioSource.volume = MUSIC_VOLUME;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ManageBackgroundMusic(scene.name);

        if (scene.name == "MainMenu" || scene.name == "LevelSelect")
        {
            ResetIntroHistory();
            return;
        }

        if (scene.name.StartsWith("Level "))
        {
            string levelNumberString = scene.name.Replace("Level ", "");
            if (int.TryParse(levelNumberString, out int levelNumber))
            {
                int clipIndex = levelNumber - 1;

                if (clipIndex >= 0 && clipIndex < levelStartClips.Length)
                {
                    if (!playedIntros[clipIndex] && levelStartClips[clipIndex] != null)
                    {
                        playedIntros[clipIndex] = true;

                        StopAllCoroutines();
                        StartCoroutine(PlayLevelIntroRoutine(levelStartClips[clipIndex]));
                    }
                }
            }
        }
    }

    private void ManageBackgroundMusic(string sceneName)
    {
        AudioClip targetMusic = null;

        if (sceneName == "MainMenu" || sceneName == "LevelSelect" || sceneName == "Level 5")
        {
            targetMusic = sadMusicClip;
        }
        else if (sceneName == "Level 1" || sceneName == "Level 2" || sceneName == "Level 3" || sceneName == "Level 4")
        {
            targetMusic = funMusicClip;
        }

        if (musicAudioSource != null && musicAudioSource.clip != targetMusic)
        {
            musicAudioSource.clip = targetMusic;

            if (targetMusic != null)
            {
                musicAudioSource.volume = MUSIC_VOLUME;
                musicAudioSource.Play();
            }
            else
            {
                musicAudioSource.Stop();
            }
        }
    }

    private IEnumerator PlayLevelIntroRoutine(AudioClip clip)
    {
        yield return new WaitForSeconds(delayBeforePlay);

        if (sfxAudioSource != null)
        {
            sfxAudioSource.volume = INTRO_VOLUME;
            sfxAudioSource.PlayOneShot(clip);
        }
    }

    private void ResetIntroHistory()
    {
        for (int i = 0; i < playedIntros.Length; i++)
        {
            playedIntros[i] = false;
        }
    }

    public void PlayButtonClick()
    {
        if (sfxAudioSource != null && buttonClickClip != null)
        {
            sfxAudioSource.volume = SFX_VOLUME;
            sfxAudioSource.PlayOneShot(buttonClickClip);
        }
    }

    public void PlayRobotDeath()
    {
        if (sfxAudioSource != null && robotDeathClip != null)
        {
            sfxAudioSource.volume = SFX_VOLUME;
            sfxAudioSource.PlayOneShot(robotDeathClip);
        }
    }
}