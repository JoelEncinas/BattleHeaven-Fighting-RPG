using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    // UI
    public GameObject playerPrefab;
    public GameObject playerProfile;
    public GameObject playersContainer;

    // variables
    Fighter player;
    Dictionary<string, Dictionary<string, string>> usersDB;
    Dictionary<string, int> orderedDB;
    List<KeyValuePair<string, int>> newDict;
    const int DAYS_BETWEEN_UPDATE = 1;
    const int MINUTES_BETWEEN_USER_RANKING_UPDATE = 720; 

    // Player GameObject Structure
    // - List_Me
    //      - icon/level text
    //      - character_Bg
    //          - mask
    //              - character image
    //      - icon country
    //      - nickname
    //      - trophies text

    private void Awake()
    {
        playerProfile = GameObject.Find("List_Me_Player");
        playersContainer = GameObject.Find("Content");

        // user
        player = PlayerUtils.FindInactiveFighter();
        SetupPlayer();

        // ranking
        GetDB();

        if (LeaderboardDB.IsFirstTimeUsingDB())
            LeaderboardDB.GenerateBaseDB();

        GenerateDB();
        SetupUserPosition(GetUserPosition());
    }

    private void Update()
    {
        if (CanUpdateLeaderboard())
        {
            ResetLadder();
            LeaderboardDB.UpdateDB();
            UpdateDB();
            GenerateDB();
        }

        if (CheckForUserLadderUpdate())
        {
            SetNewUserRankingPositionTimestamp();
            SetUserPosition(GetTrophiesChange());
            SetupUserPosition(GetUserPosition());
        }
    }

    private void GetDB()
    {
        usersDB = LeaderboardDB.players;
    }

    private void GenerateDB()
    {
        OrderDB();
        int ranking = 1;
        int orderedDBkey;

        for(int i = 0; i < newDict.Count; i++)
        {
            orderedDBkey = int.Parse(newDict.ElementAt(i).Key);

            foreach (KeyValuePair<string, Dictionary<string, string>> user in usersDB)
            {
                if (int.Parse(user.Key) == orderedDBkey)
                {
                    SetupOtherPlayer(user, ranking);
                    Instantiate(playerPrefab, playersContainer.transform);
                    ranking++;
                }
            }
        }
    }

    private void OrderDB()
    {
        orderedDB = new Dictionary<string, int>();

        foreach (KeyValuePair<string, Dictionary<string, string>> user in usersDB)
            orderedDB.Add(user.Key, LeaderboardDB.GetUserTrophies(user.Key));

        newDict = orderedDB.OrderByDescending(user => user.Value).ToList();
    }

    private void UpdateDB()
    {
        PlayerPrefs.SetString("leaderboardUpdate", DateTime.Now.AddDays(DAYS_BETWEEN_UPDATE).ToBinary().ToString());
        PlayerPrefs.Save();
    }

    public bool CanUpdateLeaderboard()
    {
        if (!PlayerPrefs.HasKey("leaderboardUpdate"))
        {
            UpdateDB();
        }

        if (PlayerPrefs.HasKey("leaderboardUpdate"))
            return DateTime.Compare(DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("leaderboardUpdate"))), DateTime.Now) <= 0;

        return false;
    }

    private void SetupOtherPlayer(KeyValuePair<string, Dictionary<string, string>> user, int ranking)
    {
        playerPrefab.transform.GetChild(2).GetComponent<Image>().sprite = GetFlagByName(user.Value["country"]);
        playerPrefab.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = user.Value["name"];
        playerPrefab.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = LeaderboardDB.GetUserTrophies(user.Key).ToString();
        playerPrefab.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = MenuUtils.GetProfilePicture(user.Value["specie"]);

        if (ranking == 1)
        {
            playerPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
            playerPrefab.transform.GetChild(6).GetComponent<Image>().enabled = true;
            playerPrefab.transform.GetChild(6).GetComponent<RectTransform>().sizeDelta = new Vector2(124, 108);
            playerPrefab.transform.GetChild(6).GetComponent<Image>().sprite = Resources.Load<Sprite>("Medals/Medal_Gold");
        }

        if (ranking == 2)
        {
            playerPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
            playerPrefab.transform.GetChild(6).GetComponent<Image>().enabled = true;
            playerPrefab.transform.GetChild(6).GetComponent<RectTransform>().sizeDelta = new Vector2(111, 108);
            playerPrefab.transform.GetChild(6).GetComponent<Image>().sprite = Resources.Load<Sprite>("Medals/Medal_Silver");
        }
        if (ranking == 3)
        {
            playerPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
            playerPrefab.transform.GetChild(6).GetComponent<Image>().enabled = true;
            playerPrefab.transform.GetChild(6).GetComponent<RectTransform>().sizeDelta = new Vector2(82, 108);
            playerPrefab.transform.GetChild(6).GetComponent<Image>().sprite = Resources.Load<Sprite>("Medals/Medal_Bronze");
        }
        if (ranking > 3)
        {
            playerPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
            playerPrefab.transform.GetChild(6).GetComponent<Image>().enabled = false;
            playerPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ranking.ToString();
        }
    }

    private void SetupPlayer()
    {
        SetUpUserFlag(User.Instance.flag);
        SetupUserName();
        SetupUserTrophies();
        SetupUserSprite();
    }

    private void SetUpUserFlag(string flagName)
    {
        playerProfile.transform.GetChild(2).GetComponent<Image>().sprite = GetFlagByName(flagName);
    }

    private Sprite GetFlagByName(string flagName)
    {
        return Resources.Load<Sprite>("Flags/Icon_Flag_" + flagName);
    }

    private void SetupUserName()
    {
        playerProfile.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = player.fighterName;
    }

    private void SetupUserTrophies()
    {
        playerProfile.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = User.Instance.elo.ToString();
    }

    private void SetupUserSprite()
    {
        playerProfile.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = MenuUtils.GetProfilePicture(player.species);
    }

    private void SetupUserPosition(int newPosition)
    {
        playerProfile.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = newPosition.ToString();
    }

    private bool CheckForUserLadderUpdate()
    {
        if (PlayerPrefs.HasKey("userRankingTimestamp"))
            return DateTime.Compare(DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("userRankingTimestamp"))), DateTime.Now) <= 0;
        return false;
    }

    private void SetNewUserRankingPositionTimestamp()
    {
        PlayerPrefs.SetString("userRankingTimestamp", DateTime.Now.AddMinutes(MINUTES_BETWEEN_USER_RANKING_UPDATE).ToBinary().ToString());
        PlayerPrefs.Save();
    }

    private int GetUserPosition()
    {
        return PlayerPrefs.GetInt("userRankingPosition");
    }

    private void SetUserPosition(int newPosition)
    {
        PlayerPrefs.SetInt("userRankingPosition", newPosition);
        PlayerPrefs.Save();
    }

    private int GetUserCups()
    {
        return PlayerPrefs.GetInt("userCups");
    }

    private void SetUserCups(int newCups)
    {
        PlayerPrefs.SetInt("userCups", newCups);
        PlayerPrefs.Save();
    }

    private int GetTrophiesChange()
    {
        int lastTrophies = GetUserCups();
        int lastPosition = GetUserPosition();

        if(User.Instance.elo > lastTrophies)
        {
            SetUserCups(User.Instance.elo);
            SetUserPosition(lastPosition - GenerateRandomRankingPositionChange(2, 6));
            return GetUserPosition();
        }
        else if(User.Instance.elo < lastTrophies)
        {
            SetUserCups(User.Instance.elo);
            SetUserPosition(lastPosition + GenerateRandomRankingPositionChange(2, 6));
            return GetUserPosition();
        }
        else if(User.Instance.elo == lastTrophies)
        {
            return lastPosition;
        }

        return lastPosition;
    }

    private int GenerateRandomRankingPositionChange(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    private void ResetLadder()
    {
        List<Transform> users = new List<Transform>();

        for(int i = 0; i < playersContainer.transform.childCount; i++)
            users.Add(playersContainer.transform.GetChild(i));

        foreach (Transform user in users)
        {
            user.gameObject.SetActive(false);
        }
    }
}
