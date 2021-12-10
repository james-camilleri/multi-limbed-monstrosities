using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterTarget : MonoBehaviour
{
  public ServerConfig config;
  void Start()
  {
    config.limbTargets.Add(this.gameObject);
    Shuffle(config.limbTargets);
  }

  void Shuffle(IList<GameObject> list)
  {
    int n = list.Count;
    while (n > 1)
    {
      n--;
      int k = Random.Range(0, n + 1);
      GameObject value = list[k];
      list[k] = list[n];
      list[n] = value;
    }
  }

}
