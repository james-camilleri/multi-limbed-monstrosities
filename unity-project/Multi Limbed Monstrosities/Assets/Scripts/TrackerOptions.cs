using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerOptions : MonoBehaviour
{
  public GameObject playerConfig;

  [HideInInspector]
  public int playerNumber;

  void Awake()
  {
    playerNumber = playerConfig.GetComponent<PlayerConfig>().playerNumber;
  }

  // Update is called once per frame
  void Update()
  {

  }
}
