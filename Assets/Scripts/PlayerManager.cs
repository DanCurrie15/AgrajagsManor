using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private List<GameObject> playablePlayers = new List<GameObject>();

    void Start()
    {
        playablePlayers.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }

    public void RemovePlayablePlayer(GameObject player)
    {
        playablePlayers.Remove(player);
        if (playablePlayers.Count > 0)
        {
            SelectNewPlayer();
        }
        else
        {
            GameManager.Instance.GameOver();
        }
        player.SetActive(false);
    }

    public void SelectNewPlayer()
    {
        playablePlayers[0].GetComponent<Player>().enabled = true;
    }

    public GameObject GetClosestPlayer(Transform obj)
    {
        GameObject nearest = null;
        float smallestDistance = float.PositiveInfinity;
        if (playablePlayers.Count < 1)
        {
            return null;
        }
        foreach (GameObject player in playablePlayers)
        {
            if (Vector3.Distance(player.transform.position, obj.position) < smallestDistance)
            {
                smallestDistance = Vector3.Distance(player.transform.position, obj.position);
                nearest = player;
            }
        }
        return nearest;
    }
}
