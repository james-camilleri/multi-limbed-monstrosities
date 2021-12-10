using System.Collections.Generic;
using UnityEngine;

public class ServerConfig : MonoBehaviour
{
  public bool isServer = false;

  public GameObject targetTemplate;

  public int numberOfTargets = 3;

  public Vector3 targetArea = new Vector3(0, 0, 0);

  private List<GameObject> targets = new List<GameObject>();

  public List<GameObject> limbTargets = new List<GameObject>();

  private const float START_Z = 50f;

  // Start is called before the first frame update
  void Start()
  {
    GenerateTargets(isServer);
  }

  // Update is called once per frame
  void Update()
  {
    if (isServer)
    {
      MoveTargets();
    }
  }

  void GenerateTargets(bool isServer)
  {
    for (int i = 0; i < numberOfTargets; i++)
    {
      Vector3 position = isServer ? new Vector3(
          Random.Range(-targetArea.x, targetArea.x),
          Random.Range(-targetArea.y, targetArea.y),
          Random.Range(START_Z, START_Z + targetArea.z)
      ) : new Vector3(0, 0, 0);

      GameObject target = Instantiate(targetTemplate, position, Quaternion.identity);
      target.SetActive(true);

      if (isServer)
      {
        var sender = target.GetComponent<MqttPropertySender>();
        sender._topic = "mlm/t/" + i;
        sender.enabled = true;
      }
      else
      {
        var receiver = target.GetComponent<MqttEventReceiver>();
        receiver.enabled = true;
      }
      targets.Add(target);
    }
  }

  void MoveTargets()
  {
    targets.ForEach(target =>
    {
      Vector3 currentPosition = target.transform.position;
      target.transform.position = new Vector3(
          currentPosition.x,
          currentPosition.y,
          currentPosition.z - 0.1f
      );
    });
  }
}
