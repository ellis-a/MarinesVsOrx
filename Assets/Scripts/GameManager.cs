using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public MatchSettings MatchSettings;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one game manager in scene");
        }
        else
        {
            Instance = this;
        }
    }

    private const string PLAYER_ID_PREFIX = "Player ";
    private static Dictionary<string, Player> _players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string netId, Player player)
    {
        string playerId = PLAYER_ID_PREFIX + netId;
        _players.Add(playerId, player);
        player.transform.name = playerId;
    }

    public static void DeregisterPlayer(string playerId)
    {
        _players.Remove(playerId);
    }

    public static Player GetPlayer(string playerId)
    {
        return _players[playerId];
    }

    //void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(200,200,100, 500));
    //    GUILayout.BeginVertical();

    //    foreach (string playerId in _players.Keys)
    //    {
    //        GUILayout.Label(playerId + " - " + _players[playerId].transform.name);
    //    }

    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}
}
