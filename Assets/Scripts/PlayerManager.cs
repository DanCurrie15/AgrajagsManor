using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private List<GameObject> playablePlayers = new List<GameObject>();

    public GameObject deadPlayer;

    void Start()
    {
        playablePlayers.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }

    public void RemovePlayablePlayer(GameObject player)
    {
        playablePlayers.Remove(player);
        SelectNewPlayer();
        Instantiate(deadPlayer, player.transform.position, Quaternion.identity);
        player.SetActive(false);
    }

    public void SelectNewPlayer()
    {
        playablePlayers[0].GetComponent<Player>().enabled = true;
    }
}
