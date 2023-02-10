using UnityEngine;

public class ChooseFirstFighter : MonoBehaviour
{
    // scripts
    ChooseFirstFighterUI chooseFirstFighterUI;

    private void Awake()
    {
        chooseFirstFighterUI = GameObject.Find("UIManager").GetComponent<ChooseFirstFighterUI>();
    }

    public void OnSelectFighter()
    {
        switch (transform.name)
        {
            case "Container_Fighter_Left":
                SetFighter(GameObject.Find("Fighter_Left").GetComponent<FighterSkinData>());
                chooseFirstFighterUI.EnableFighterHighlight("left");
                break;
            case "Container_Fighter_Mid":
                SetFighter(GameObject.Find("Fighter_Mid").GetComponent<FighterSkinData>());
                chooseFirstFighterUI.EnableFighterHighlight("mid");
                break;
            case "Container_Fighter_Right":
                SetFighter(GameObject.Find("Fighter_Right").GetComponent<FighterSkinData>());
                chooseFirstFighterUI.EnableFighterHighlight("right");
                break;
        }
    }

    public void MoveToNextState()
    {
        switch (FirstPlayTempData.state.ToString())
        {
            case "FIGHTER":
                chooseFirstFighterUI.ChooseFighter();
                break;
            case "NAME":
                chooseFirstFighterUI.CheckName();
                break;
            case "COUNTRY":
                chooseFirstFighterUI.CheckFlag();
                break;
        }
    }

    public void MoveToPreviousState()
    {
        switch (FirstPlayTempData.state.ToString())
        {
            case "NAME":
                chooseFirstFighterUI.BackToChooseFighter();
                break;
            case "COUNTRY":
                chooseFirstFighterUI.BackToName();
                break;
        }
    }

    private void SetFighter(FighterSkinData fighterSkin)
    {
        FirstPlayTempData.skinName = fighterSkin.skinName;
        FirstPlayTempData.species = fighterSkin.species;
    }

    // used on set focus on input
    public void ResetRegexText()
    {
        chooseFirstFighterUI.regexText.gameObject.SetActive(false);
        chooseFirstFighterUI.regexText.text = "";
    }

    public void GetFlagClicked()
    {
        if (FirstPlayTempData.firstFlag)
            FirstPlayTempData.lastFlag = FirstPlayTempData.countryFlag;
        else
            FirstPlayTempData.firstFlag = true;

        chooseFirstFighterUI.EnableCheckOnFlag(transform.name);
        FirstPlayTempData.countryFlag = transform.name;

        if (FirstPlayTempData.lastFlag != "")
            if (FirstPlayTempData.lastFlag != FirstPlayTempData.countryFlag)
                chooseFirstFighterUI.DisableCheckOnFlag(FirstPlayTempData.lastFlag);

        FirstPlayTempData.lastFlag = FirstPlayTempData.countryFlag;
    }

    public void DisableFlagError()
    {
        chooseFirstFighterUI.flagErrorOk.SetActive(false);
    }
}
