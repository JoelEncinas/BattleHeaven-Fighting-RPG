using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DeleteGame : MonoBehaviour
{
    // UI
    GameObject resetGame;

    private void Awake()
    {
        resetGame = GameObject.Find("Button_Delete_Confirmation");
        resetGame.GetComponent<Button>().onClick.AddListener(() => RestartGame());
    }

    public void RestartGame()
    {
        DeleteSaves();
        ResetAllPrefs();
        IGoToEntryPoint();
    }

    private void DeleteSaves()
    {
        //On Android when reading from Application.persistentDataPath we access a symlink at /storage/emulated/0....NFTGameMamecorp/files
        //If we want to delete the files at NFTGameMamecorp we have to get the files at the parent folder
        string[] filesAndroid = Directory.GetFiles(Path.GetDirectoryName(Application.persistentDataPath));
        foreach (var file in filesAndroid) File.Delete(file);

        // Debug.Log(Directory.GetFiles(Application.persistentDataPath)[0]);

        string[] filesPC =  Directory.GetFiles(Application.persistentDataPath);
        foreach (var file in filesPC) File.Delete(file);
    }

    private void ResetAllPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    private IEnumerator GoToEntryPoint()
    {
        StartCoroutine(SceneManagerScript.instance.FadeOut());
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SceneFlag.FADE_DURATION));
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNames.EntryPoint.ToString());
    }

    private void IGoToEntryPoint()
    {
        StartCoroutine(GoToEntryPoint());
    }
}
