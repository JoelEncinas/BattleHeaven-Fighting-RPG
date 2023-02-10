using UnityEngine;

public class CanvasRaycastBlocker : MonoBehaviour
{
    private CanvasGroup[] canvasList;

    private void Awake()
    {
        canvasList = FindObjectsOfType<CanvasGroup>();
    }

    private void Update()
    {
        if(SceneManagerScript.instance.hasFadingEnded)
            AllowRaycast();
        if (!SceneManagerScript.instance.hasFadingEnded)
            BlockRaycast();
    }

    private void BlockRaycast()
    {
        for(int i = 0; i < canvasList.Length; i++)
            if(canvasList[i] != null)
                canvasList[i].blocksRaycasts = false;
    }

    private void AllowRaycast()
    {
        for (int i = 0; i < canvasList.Length; i++)
            if (canvasList[i] != null)
                canvasList[i].blocksRaycasts = true;
    }
}
