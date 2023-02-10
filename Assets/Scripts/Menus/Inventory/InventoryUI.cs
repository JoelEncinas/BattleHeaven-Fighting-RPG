using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class InventoryUI : MonoBehaviour
{
    // UI
    GameObject skillsContainer;
    [SerializeField] private List<Sprite> frameColors = new List<Sprite>();

    // Skill Description 
    GameObject skillIcon;
    GameObject skillRarityFrame;
    GameObject skillName;
    GameObject skillRarity;
    GameObject skillDescription;
    GameObject skillCategory;
    GameObject skillCategoryLore;
    Fighter player;

    // variables
    List<Transform> skillsGameObjectList = new List<Transform>();


    private void Awake()
    {
        player = PlayerUtils.FindInactiveFighter();
        skillsContainer = GameObject.FindGameObjectWithTag("SkillsContainer");
        skillIcon = GameObject.Find("SkillIcon");
        skillRarityFrame = GameObject.Find("SkillFrame");
        skillName = GameObject.Find("Text_Name");
        skillRarity = GameObject.Find("Text_Rarity");
        skillDescription = GameObject.Find("Text_Description");
        skillCategory = GameObject.Find("Text_Category");
        skillCategoryLore = GameObject.Find("Text_Category_Lore");

        AddSkillsGameObjectsToList();
        ShowOwnedSkills();

        //Whenever the user has not clicked anything
        SetDefaultSideBarInfo();
    }

    private void Start()
    {
        SceneFlag.sceneName = SceneNames.Inventory.ToString();
    }

    private void SetDefaultSideBarInfo(){
        SetSideBarSkillInfo(player.skills[0].skillName);
    }

    private void AddSkillsGameObjectsToList()
    {
        foreach (Transform skill in skillsContainer.transform) skillsGameObjectList.Add(skill);
    }

    //Be careful, the gameobject names have to match the skill names
    private void ShowOwnedSkills()
    {
        foreach (Transform skill in skillsGameObjectList)
        {
            //show skill on UI
            if (player.HasSkill(skill.gameObject.name)) ShowQuestionMarkOrSkill(skill, false, true);
            //show question mark on UI
            else ShowQuestionMarkOrSkill(skill, true, false);
        }
    }

    private void ShowQuestionMarkOrSkill(Transform skill, bool showQuestionMark, bool showSkill)
    {
        skill.GetChild(0).gameObject.SetActive(showQuestionMark);
        skill.GetChild(1).gameObject.SetActive(showSkill);
    }


    public void SetSideBarSkillInfo(string skillname)
    {
        Skill clickedSkill = player.skills.Where(skill => skill.skillName == skillname).ToList()[0];
        SetIcon(clickedSkill.icon);
        SetFrameColor(clickedSkill.rarity);
        skillName.GetComponent<TextMeshProUGUI>().text = clickedSkill.skillName;
        skillRarity.GetComponent<TextMeshProUGUI>().text = clickedSkill.rarity;
        skillDescription.GetComponent<TextMeshProUGUI>().text = clickedSkill.description;
        skillCategory.GetComponent<TextMeshProUGUI>().text = $"TYPE : {clickedSkill.category}";
        SkillCollection.SkillType skillEnumMember = (SkillCollection.SkillType)Enum.Parse(typeof(SkillCollection.SkillType), clickedSkill.category);
        skillCategoryLore.GetComponent<TextMeshProUGUI>().text = SkillCollection.skillCategoriesLore[skillEnumMember];
    }

    //OnSkillClicked
    public void GetSkillClicked()
    {
        string clickedSkillName = EventSystem.current.currentSelectedGameObject.name;
        if (player.HasSkill(clickedSkillName)) SetSideBarSkillInfo(clickedSkillName);
    }

    public void SetFrameColor(string rarity)
    {
        switch (rarity)
        {
            case "COMMON":
                skillRarityFrame.GetComponent<Image>().sprite = frameColors[0];
                break;
            case "RARE":
                skillRarityFrame.GetComponent<Image>().sprite = frameColors[1];
                break;
            case "EPIC":
                skillRarityFrame.GetComponent<Image>().sprite = frameColors[2];
                break;
            case "LEGENDARY":
                skillRarityFrame.GetComponent<Image>().sprite = frameColors[3];
                break;
        }
    }

    public void SetIcon(string iconNumber)
    {
        Sprite icon = Resources.Load<Sprite>("Icons/IconsSkills/" + iconNumber);
        skillIcon.GetComponent<Image>().sprite = icon;
    }
}
