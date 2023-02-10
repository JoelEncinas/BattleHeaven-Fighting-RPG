using System.IO;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class EntryPoint : MonoBehaviour
{
    // UI 
    GameObject loadingBarGO;
    TextMeshProUGUI loadingText;
    Slider loadingBar;
    TextMeshProUGUI tipText;

    // fighter
    public static GameObject fighterGameObject;

    private void Awake()
    {
        loadingBarGO = GameObject.Find("Slider_LoadingBar");
        loadingText = loadingBarGO.GetComponentInChildren<TextMeshProUGUI>();
        loadingBar = loadingBarGO.GetComponent<Slider>();
        tipText = GameObject.Find("TipText").GetComponentInChildren<TextMeshProUGUI>();

        ResetBar();
    }

    private void StartMusic(){
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            SetupMusic(PlayerPrefs.GetFloat("musicVolume"));
            SetupSFX(PlayerPrefs.GetFloat("soundsVolume"));
        } else
        {
            InitDefaultVolume();
        }
        AudioSource audioSource = FindObjectOfType<AudioManager>().GetComponent<AudioSource>();
        if (!audioSource.isPlaying) audioSource.Play();
    }

    private void InitDefaultVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", 5f);
        PlayerPrefs.SetFloat("soundsVolume", 5f);
        PlayerPrefs.Save();
    }

    IEnumerator Start()
    {
        StartMusic();
        HideFighter();
        GenerateTip();

        StartCoroutine(SceneManagerScript.instance.FadeIn());
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SceneFlag.FADE_DURATION));

        // --- Enable this for loading effect ---
        int randomLoadingAnimation = Random.Range(1, 4);

        switch (randomLoadingAnimation)
        {
            case 1:
                StartCoroutine(FakeDelay1());
                break;
            case 2:
                StartCoroutine(FakeDelay2());
                break;
            case 3:
                StartCoroutine(FakeDelay3());
                break;
        }

        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(3.5f));

        bool saveFilesFound = File.Exists(JsonDataManager.getFilePath(JsonDataManager.UserFileName)) &&
            File.Exists(JsonDataManager.getFilePath(JsonDataManager.FighterFileName));

        if (saveFilesFound)
        {
            JsonDataManager.ReadUserFile();
            JsonDataManager.ReadFighterFile();

            if(User.Instance.firstTime)
            {
                StartCoroutine(SceneManagerScript.instance.FadeOut());
                yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SceneFlag.FADE_DURATION));
                UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNames.Combat.ToString());
            } 
            else
            {
                StartCoroutine(SceneManagerScript.instance.FadeOut());
                yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SceneFlag.FADE_DURATION));
                UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNames.MainMenu.ToString());
            }
        }

        else
        {
            StartCoroutine(SceneManagerScript.instance.FadeOut());
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SceneFlag.FADE_DURATION));
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNames.ChooseFirstFighter.ToString());
        }

        Notifications.InitiateCardsUnseen();
        SceneFlag.sceneName = SceneNames.EntryPoint.ToString();
    }

    private void ResetBar()
    {
        // set up bar
        loadingText.text = "0%";
        loadingBar.value = 0f;
    }

    IEnumerator FakeDelay1()
    {
        loadingText.text = "0%";
        loadingBar.value = 0f;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(1f));

        loadingText.text = "30%";
        loadingBar.value = 0.3f;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(1.25f));

        loadingText.text = "70%";
        loadingBar.value = 0.7f;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(0.5f));

        loadingText.text = "100%";
        loadingBar.value = 1f;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(0.25f));
    }

    IEnumerator FakeDelay2()
    {
        loadingText.text = "0%";
        loadingBar.value = 0f;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(0.75f));

        loadingText.text = "15%";
        loadingBar.value = 0.15f;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(0.75f));

        loadingText.text = "60%";
        loadingBar.value = 0.6f;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(1.25f));

        loadingText.text = "100%";
        loadingBar.value = 1f;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(0.25f));
    }

    IEnumerator FakeDelay3()
    {
        loadingText.text = "0%";
        loadingBar.value = 0f;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(0.25f));

        loadingText.text = "40%";
        loadingBar.value = 0.4f;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(1f));

        loadingText.text = "85%";
        loadingBar.value = 0.85f;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(0.75f));

        loadingText.text = "100%";
        loadingBar.value = 1f;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(1f));
    }

    private void HideFighter()
    {
        fighterGameObject = GameObject.Find("Fighter");
        fighterGameObject.SetActive(false);
    }

    private void GenerateTip()
    {
        tipText.text = Tips.tips[Random.Range(0, Tips.tips.Count)];
    }

    public void SetupMusic(float value)
    {
        FindObjectOfType<AudioManager>().ChangeVolume("V", value / 10);
    }

    public void SetupSFX(float value)
    {
        FindObjectOfType<AudioManager>().ChangeVolume("S", value / 10);
    }
}