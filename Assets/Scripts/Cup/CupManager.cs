using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CupManager : MonoBehaviour
{
    private void Awake()
    {
        if (File.Exists(JsonDataManager.getFilePath(JsonDataManager.CupFileName)))
            JsonDataManager.ReadCupFile();
        else
            CreateCupFile();
    }

    private void CreateCupFile()
    {
        string cupName = CupDB.CupNames.DIVINE.ToString(); ;
        string round = CupDB.CupRounds.QUARTERS.ToString();
        bool isActive = false;
        bool playerStatus = true;
        List<CupFighter> participants = GenerateParticipants();
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> cupInfo = GenerateCupInitialInfo();

        CupFactory.CreateCupInstance(cupName, isActive, playerStatus, round, participants, cupInfo);
        JObject cup = JObject.FromObject(Cup.Instance);
        JsonDataManager.SaveData(cup, JsonDataManager.CupFileName);
    }

    public void DeleteCupFile()
    {
        // TODO ADD IOS
        //On Android when reading from Application.persistentDataPath we access a symlink at /storage/emulated/0....NFTGameMamecorp/files
        //If we want to delete the files at NFTGameMamecorp we have to get the files at the parent folder
        string[] filesAndroid = Directory.GetFiles(Path.GetDirectoryName(Application.persistentDataPath));
        foreach (var file in filesAndroid) 
            if(file.Contains("cup"))
                File.Delete(file);

        // Debug.Log(Directory.GetFiles(Application.persistentDataPath)[0]);

        string[] filesPC = Directory.GetFiles(Application.persistentDataPath);
        foreach (var file in filesPC)
            if (file.Contains("cup"))
                File.Delete(file);
    }

    private List<CupFighter> GenerateParticipants()
    {
        Fighter player = PlayerUtils.FindInactiveFighter();

        // there will be 8 fighters per cup (7 + user)
        List<CupFighter> participants = new List<CupFighter>();

        CupFighter user = new CupFighter(0.ToString(), player.fighterName, player.species);
        participants.Add(user);

        for(int i = 1; i < 8; i++)
        {
            participants.Add(
                new CupFighter(
                    i.ToString(),
                    RandomNameGenerator.GenerateRandomName(),
                    GeneralUtils.GetRandomSpecies()
                )
            );
        }

        return participants;
    }

    private Dictionary<string, Dictionary<string, Dictionary<string, string>>> GenerateCupInitialInfo()
    {
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> cupInfo = 
            new Dictionary<string, Dictionary<string, Dictionary<string, string>>>
            {
                { CupDB.CupRounds.QUARTERS.ToString(), new Dictionary<string, Dictionary<string, string>>
                    {
                        { "1", new Dictionary<string, string>
                            {
                                { "matchId", "1"} , // match id
                                { "1", "0"} ,       // seed 1 player
                                { "2", "1"} ,       // seed 2 player
                                { "winner" , ""} ,  // winner id
                                { "loser" , ""}     // loser id
                            }
                        },
                        { "2", new Dictionary<string, string>
                            {
                                { "matchId", "2"} ,
                                { "3", "2"} ,
                                { "4", "3"} ,
                                { "winner" , ""} ,  
                                { "loser" , ""}
                            }
                        },
                        { "3", new Dictionary<string, string>
                            {
                                { "matchId", "3"} ,
                                { "5", "4"} ,
                                { "6", "5"} ,
                                { "winner" , ""} ,  
                                { "loser" , ""}
                            }
                        },
                        { "4", new Dictionary<string, string>
                            {
                                { "matchId", "4"} ,
                                { "7", "6"} ,
                                { "8", "7"} ,
                                { "winner" , ""} ,  
                                { "loser" , ""}
                            }
                        },
                    }
                }
            };

        return cupInfo;
    }

    public void SimulateQuarters(bool hasPlayerWon)
    {
        int random;
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> cupInfo = Cup.Instance.cupInfo;
        
        // match lists
        List<string> match1 = new List<string>
        {
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["1"]["1"],
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["1"]["2"],
        };

        List<string> match2 = new List<string>
        {
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["2"]["3"],
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["2"]["4"],
        };

        List<string> match3 = new List<string>
        {
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["3"]["5"],
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["3"]["6"],
        };

        List<string> match4 = new List<string>
        {
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["4"]["7"],
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["4"]["8"],
        };

        // simulate matches
        if (hasPlayerWon)
        {
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["1"]["winner"] = match1[0];
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["1"]["loser"] = match1[1];
        }
        else
        {
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["1"]["winner"] = match1[1];
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["1"]["loser"] = match1[0];
        }

        random = UnityEngine.Random.Range(0, match2.Count);
        cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["2"]["winner"] = match2[random];
        if (random == 0)
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["2"]["loser"] = match2[1];
        else
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["2"]["loser"] = match2[0];

        random = UnityEngine.Random.Range(0, match3.Count);
        cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["3"]["winner"] = match3[random];
        if (random == 0)
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["3"]["loser"] = match3[1];
        else
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["3"]["loser"] = match3[0];

        random = UnityEngine.Random.Range(0, match4.Count);
        cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["4"]["winner"] = match4[random];
        if (random == 0)
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["4"]["loser"] = match4[1];
        else
            cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["4"]["loser"] = match4[0];

        // save results
        Cup.Instance.round = CupDB.CupRounds.SEMIS.ToString(); ;

        Cup.Instance.cupInfo = cupInfo;
        Cup.Instance.SaveCup();

        GenerateCupSemisInfo();
    }

    public List<CupFighter> GenerateParticipantsBasedOnQuarters()
    {
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> _cupInfo = Cup.Instance.cupInfo;
        List<CupFighter> _participants = Cup.Instance.participants;

        List<CupFighter> semisParticipants = new List<CupFighter>();
        int matchesNumber = 4;

        foreach(CupFighter participant in _participants)
        {
            for(int matchCounter = 1; matchCounter < matchesNumber + 1; matchCounter++)
            {
                if (participant.id == _cupInfo[CupDB.CupRounds.QUARTERS.ToString()][matchCounter.ToString()]["winner"])
                    semisParticipants.Add(participant);
            }
        }

        return semisParticipants;
    }

    // call on combat end 
    private void GenerateCupSemisInfo()
    {
        Cup.Instance.cupInfo.Add(
            CupDB.CupRounds.SEMIS.ToString(), new Dictionary<string, Dictionary<string, string>>
            {
                { "5", new Dictionary<string, string>
                    {
                        { "matchId", "5"} ,
                        { "9", ""} ,
                        { "10", ""} ,
                        { "winner" , ""} ,
                        { "loser" , ""}
                    }
                },
                { "6", new Dictionary<string, string>
                    {
                        { "matchId", "6"} ,
                        { "11", ""} ,
                        { "12", ""} ,
                        { "winner" , ""} ,
                        { "loser" , ""}
                    }
                },
            });

        List<CupFighter> _participants = GenerateParticipantsBasedOnQuarters();
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> cupInfo = Cup.Instance.cupInfo;
        int participantsCounter = 0; 

        cupInfo[CupDB.CupRounds.SEMIS.ToString()]["5"]["9"] = _participants[participantsCounter].id;
        participantsCounter++;
        cupInfo[CupDB.CupRounds.SEMIS.ToString()]["5"]["10"] = _participants[participantsCounter].id;
        participantsCounter++;
        cupInfo[CupDB.CupRounds.SEMIS.ToString()]["6"]["11"] = _participants[participantsCounter].id;
        participantsCounter++;
        cupInfo[CupDB.CupRounds.SEMIS.ToString()]["6"]["12"] = _participants[participantsCounter].id;

        Cup.Instance.cupInfo = cupInfo;
        Cup.Instance.SaveCup();
    }

    public void SimulateSemis(bool hasPlayerWon)
    {
        int random;
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> cupInfo = Cup.Instance.cupInfo;
        
        // match lists
        List<string> match5 = new List<string>
        {
            cupInfo[CupDB.CupRounds.SEMIS.ToString()]["5"]["9"],
            cupInfo[CupDB.CupRounds.SEMIS.ToString()]["5"]["10"],
        };

        List<string> match6 = new List<string>
        {
            cupInfo[CupDB.CupRounds.SEMIS.ToString()]["6"]["11"],
            cupInfo[CupDB.CupRounds.SEMIS.ToString()]["6"]["12"],
        };

        // simulate matches
        if (hasPlayerWon)
        {
            cupInfo[CupDB.CupRounds.SEMIS.ToString()]["5"]["winner"] = match5[0];
            cupInfo[CupDB.CupRounds.SEMIS.ToString()]["5"]["loser"] = match5[1];
        }
        else
        {
            cupInfo[CupDB.CupRounds.SEMIS.ToString()]["5"]["winner"] = match5[1];
            cupInfo[CupDB.CupRounds.SEMIS.ToString()]["5"]["loser"] = match5[0];
        }

        random = UnityEngine.Random.Range(0, match6.Count);
        cupInfo[CupDB.CupRounds.SEMIS.ToString()]["6"]["winner"] = match6[random];
        if (random == 0)
            cupInfo[CupDB.CupRounds.SEMIS.ToString()]["6"]["loser"] = match6[1];
        else
            cupInfo[CupDB.CupRounds.SEMIS.ToString()]["6"]["loser"] = match6[0];

        // save results
        Cup.Instance.round = CupDB.CupRounds.FINALS.ToString(); ;

        Cup.Instance.cupInfo = cupInfo;
        Cup.Instance.SaveCup();

        GenerateCupFinalsInfo();
    }

    public List<CupFighter> GenerateParticipantsBasedOnSemis()
    {
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> _cupInfo = Cup.Instance.cupInfo;
        List<CupFighter> _participants = Cup.Instance.participants;

        List<CupFighter> semisParticipants = new List<CupFighter>();
        int matchesNumber = 2;
        matchesNumber += 5;

        foreach (CupFighter participant in _participants)
        {
            for (int matchCounter = 5; matchCounter < matchesNumber; matchCounter++)
            {
                if (participant.id == _cupInfo[CupDB.CupRounds.SEMIS.ToString()][matchCounter.ToString()]["winner"])
                    semisParticipants.Add(participant);
            }
        }

        return semisParticipants;
    }

    private void GenerateCupFinalsInfo()
    {
        Cup.Instance.cupInfo.Add(
            CupDB.CupRounds.FINALS.ToString(), new Dictionary<string, Dictionary<string, string>>
            {
                { "7", new Dictionary<string, string>
                    {
                        { "matchId", "7"} ,
                        { "13", ""} ,
                        { "14", ""} ,
                        { "winner" , ""} ,
                        { "loser" , ""}
                    }
                },
            });

        List<CupFighter> _participants = GenerateParticipantsBasedOnSemis();
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> cupInfo = Cup.Instance.cupInfo;
        int participantsCounter = 0;

        cupInfo[CupDB.CupRounds.FINALS.ToString()]["7"]["13"] = _participants[participantsCounter].id;
        participantsCounter++;
        cupInfo[CupDB.CupRounds.FINALS.ToString()]["7"]["14"] = _participants[participantsCounter].id;

        Cup.Instance.cupInfo = cupInfo;
        Cup.Instance.SaveCup();
    }

    public void SimulateFinals(bool hasPlayerWon)
    {
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> cupInfo = Cup.Instance.cupInfo;
        
        // matches lists
        List<string> match7 = new List<string>
        {
            cupInfo[CupDB.CupRounds.FINALS.ToString()]["7"]["13"],
            cupInfo[CupDB.CupRounds.FINALS.ToString()]["7"]["14"],
        };

        // simulate match
        if (hasPlayerWon)
        {
            cupInfo[CupDB.CupRounds.FINALS.ToString()]["7"]["winner"] = match7[0];
            cupInfo[CupDB.CupRounds.FINALS.ToString()]["7"]["loser"] = match7[1];
        }
        else
        {
            cupInfo[CupDB.CupRounds.FINALS.ToString()]["7"]["winner"] = match7[1];
            cupInfo[CupDB.CupRounds.FINALS.ToString()]["7"]["loser"] = match7[0];
        }

        // save cup won on profile
        ProfileData.SaveCups();

        // save results
        Cup.Instance.round = CupDB.CupRounds.END.ToString(); 

        Cup.Instance.cupInfo = cupInfo;
        Cup.Instance.SaveCup();
    }
}
