using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private List<GameObject> playablePlayers = new List<GameObject>();
    private List<GameObject> futurePlayablePlayers = new List<GameObject>();

    void Start()
    {
        playablePlayers.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        futurePlayablePlayers.AddRange(GameObject.FindGameObjectsWithTag("FuturePlayer"));
    }

    public void RemovePlayablePlayer(GameObject player)
    {
        if (playablePlayers.Contains(player))
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
        }
        else if (futurePlayablePlayers.Contains(player))
        {
            futurePlayablePlayers.Remove(player);
        }
        player.SetActive(false);
    }

    public void SelectNewPlayer()
    {
        //playablePlayers[0].GetComponent<Player>().enabled = true;
        GameManager.Instance.playerChar = playablePlayers[0];
    }

    // returns a bool indicating if there was anything available to possess.
    public bool GainNewPossession() {
        if (futurePlayablePlayers.Count < 1) {
            Debug.Log("Nothing left to possess!");
            return false;
        }

        GameObject newPossession = null;
        GameObject player = GameManager.Instance.playerChar;
        if (player == null) {
            newPossession = futurePlayablePlayers[0];
            futurePlayablePlayers.RemoveAt(0);
        }
        else
        {
            newPossession = GetClosestInList(player.transform, futurePlayablePlayers);
            futurePlayablePlayers.Remove(newPossession);
        }
        playablePlayers.Add(newPossession);

        Debug.Log("Possessed something new!");
        return true;
    }

    public GameObject GetClosestPlayer(Transform obj)
    {
        return GetClosestInList(obj, playablePlayers);
    }

    public GameObject GetClosestInList(Transform obj, List<GameObject> objList)
    {
        GameObject nearest = null;
        float smallestDistance = float.PositiveInfinity;
        if (objList.Count < 1)
        {
            return null;
        }
        foreach (GameObject player in objList)
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
