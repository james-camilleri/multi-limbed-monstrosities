using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTheFloor : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  private void Update()
  {
    this.gameObject.transform.position += Vector3.back * 0.1f;
  }
}
