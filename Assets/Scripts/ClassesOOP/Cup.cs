using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class Cup 
{
    private static Cup instance = null;
    private Cup() { }

    public static Cup Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Cup();
            }
            return instance;
        }
    }

    private string _cupName;
    private bool _isActive;
    private bool _playerStatus;
    private string _round;
    private List<CupFighter> _participants;
    private Dictionary<string, Dictionary<string, Dictionary<string, string>>> _cupInfo;
    private bool _saveEnabled = false;

    public string cupName
    {
        get => _cupName;
        set
        {
            _cupName = value;
        }
    }
    public bool isActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
        }
    }
    public bool playerStatus
    {
        get => _playerStatus;
        set
        {
            _playerStatus = value;
        }
    }
    public string round
    {
        get => _round;
        set
        {
            _round = value;
        }
    }
    public List<CupFighter> participants
    {
        get => _participants;
        set
        {
            _participants = value;
        }
    }
    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> cupInfo
    {
        get => _cupInfo;
        set
        {
            _cupInfo = value;
        }
    }

    public bool saveEnabled
    {
        get => _saveEnabled; set
        {
            _saveEnabled = value;
        }
    }

    public void SetCupValues(string cupName, bool isActive, bool playerStatus, string round, List<CupFighter> participants, Dictionary<string, Dictionary<string, Dictionary<string, string>>> cupInfo)
    {
        this.cupName = cupName;
        this.isActive = isActive;
        this.playerStatus = playerStatus;
        this.round = round;
        this.participants = participants;
        this.cupInfo = cupInfo;
    }

    public void EnableSave()
    {
        this.saveEnabled = true;
    }

    public void SaveCup()
    {
        if (this.saveEnabled == true)
        {
            JsonDataManager.SaveData(JObject.FromObject(this), JsonDataManager.CupFileName);
        }
    }
}
