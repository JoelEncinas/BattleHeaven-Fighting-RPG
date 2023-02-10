using TMPro;
using UnityEngine;

public class Profile : MonoBehaviour
{
    // UI
    public GameObject playerLevelSlider;
    public GameObject playerExpGO;
    GameObject peakElo;
    GameObject cups;
    GameObject nCombats;
    GameObject winrate;
    GameObject wins;
    GameObject loses;
    GameObject userName;
    GameObject characterProfilePicture;
    GameObject userIcon;

    // Components
    Fighter player;

    void Awake()
    {
        player = PlayerUtils.FindInactiveFighter();

        // UI
        peakElo = GameObject.Find("Trophies_Text_Value");
        cups = GameObject.Find("Cups_Text_Value");
        nCombats = GameObject.Find("NCombats_Text_Value");
        winrate = GameObject.Find("winrate");
        wins = GameObject.Find("wins");
        loses = GameObject.Find("loses");
        userName = GameObject.Find("Text_UserName");
        characterProfilePicture = GameObject.Find("Character_Picture");
        userIcon = GameObject.Find("UserIconImage");

        LoadStats();
    }

    private void LoadStats()
    {
        MenuUtils.SetLevelSlider(playerExpGO, playerLevelSlider, player.level, player.experiencePoints);
        MenuUtils.DisplayLevelIcon(player.level, GameObject.Find("Levels_Profile"));
        MenuUtils.SetName(userName, player.fighterName);
        MenuUtils.SetProfilePicture(characterProfilePicture);
        MenuUtils.SetProfileUserIcon(userIcon);

        peakElo.GetComponent<TextMeshProUGUI>().text = User.Instance.peakElo.ToString();
        cups.GetComponent<TextMeshProUGUI>().text = User.Instance.cups.ToString();
        winrate.GetComponent<TextMeshProUGUI>().text = GetWinrate().ToString() + " %";
        nCombats.GetComponent<TextMeshProUGUI>().text = (User.Instance.wins + User.Instance.loses).ToString();
        wins.GetComponent<TextMeshProUGUI>().text = User.Instance.wins.ToString();
        loses.GetComponent<TextMeshProUGUI>().text = User.Instance.loses.ToString();

    }

    private float GetWinrate()
    {
        int gamesPlayed = User.Instance.wins + User.Instance.loses;
        return gamesPlayed == 0 ? 0 : Mathf.Floor((float)User.Instance.wins / (gamesPlayed) * 100);
    }
}
