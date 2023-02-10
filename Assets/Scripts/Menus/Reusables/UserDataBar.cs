using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class UserDataBar : MonoBehaviour
{
    public GameObject gold;
    public GameObject gems;
    public GameObject energy;
    public GameObject timerContainer;
    public GameObject timer;
    public GameObject playerNameGO;

    IEnumerator Start()
    {
        Fighter player = PlayerUtils.FindInactiveFighter();
        RefreshCurrency();
        MenuUtils.SetName(playerNameGO, player.fighterName);

        //Update timer, user energy, energy number on databar and battle button each second
        while (!EnergyManager.UserHasMaxEnergy())
        {
            MenuUtils.DisplayEnergyCountdown(timerContainer, timer);
            EnergyManager.RefreshEnergyBasedOnCountdown();

            // refresh currency
            RefreshCurrency();

            if (SceneManager.GetActiveScene().name == SceneNames.MainMenu.ToString())
            {
                GameObject.Find("MainMenuManager").GetComponent<MainMenu>().SetBattleButton();
            }
            yield return new WaitForSeconds(1f);
        }

        timerContainer.SetActive(false);
    }

    private void Update()
    {
        RefreshCurrency();
    }

    private void RefreshCurrency()
    {
        MenuUtils.SetGold(gold);
        MenuUtils.SetGems(gems);
        MenuUtils.SetEnergy(energy);
    }
}