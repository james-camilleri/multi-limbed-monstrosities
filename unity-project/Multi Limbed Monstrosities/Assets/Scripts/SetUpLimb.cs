using UnityEngine;

public class SetUpLimb : MonoBehaviour
{
  [Range(0, 3)]
  public int limbIndex;

  private Vector3 position;

  public ServerConfig config;

  void Start()
  {
    position = this.gameObject.transform.position;
    this.gameObject.GetComponent<MeshRenderer>().enabled = false;

    // config = GetComponent<ServerConfig>();
  }

  // Update is called once per frame
  void Update()
  {
    if (this.gameObject.transform.position != position)
    {
      // Debug.Log("limbTargets");
      // Debug.Log(limbIndex);
      // Debug.Log(config.limbTargets[limbIndex]);
      this.gameObject.GetComponent<MeshRenderer>().enabled = true;
      if (config.limbTargets[limbIndex] != null)
      {
        Debug.Log("things are happening");
        config.limbTargets[limbIndex].transform.position = this.gameObject.transform.position;
      }
    }
  }
}
