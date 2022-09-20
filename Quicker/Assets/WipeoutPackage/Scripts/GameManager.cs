using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private static Dictionary<Player, float> playerTime = new Dictionary<Player, float>();
    private static bool allPlayersReady = false;
    public static bool AllPlayersReady
    {
        get { return allPlayersReady; }
    }

    //list of players and their ready status.   
    public static Dictionary<Player, bool> playersReady = new Dictionary<Player, bool>();

    public static void SetPlayerTime(Player player)
    {
        if(!playerTime.ContainsKey(player))
        {
            playerTime.Add(player, RaceTimer.GetCurrentTime());
        }
    }
    public static void SetPlayerReadyStatus(Player player, bool isReady)
    {
        if(playersReady.ContainsKey(player))
        {
            playersReady[player] = isReady;
        }
        CheckAllPlayersAreReady();
    }

    public static void CheckAllPlayersAreReady()
    {
        foreach(KeyValuePair<Player, bool> player in playersReady)
        {
            if(player.Value == false)
            {
               allPlayersReady = false;
               return;
            }
        }
        allPlayersReady = true;
        StartTimer();
    }
    public static void StartTimer()
    {
        RaceTimer.StartTimer = true;
    }

    public static float GetPlayersCurrentTime(Player player)
    {
        float time;
        playerTime.TryGetValue(player, out time);
        return time;
    }
}
