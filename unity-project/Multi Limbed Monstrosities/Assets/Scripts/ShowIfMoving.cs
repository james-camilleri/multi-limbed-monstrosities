using UnityEngine;

public class ShowIfMoving : MonoBehaviour
{
  private Vector3 position;
  void Start()
  {
    position = this.gameObject.transform.position;
    this.gameObject.GetComponent<MeshRenderer>().enabled = false;
  }

  // Update is called once per frame
  void Update()
  {
    if (this.gameObject.transform.position != position)
    {
      this.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
  }
}
