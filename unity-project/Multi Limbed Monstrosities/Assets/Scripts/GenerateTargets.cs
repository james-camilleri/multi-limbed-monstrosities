using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTargets : MonoBehaviour
{

  public GameObject targetTemplate;

  public int numberOfTargets = 3;

  // public Vector3 targetArea = new Vector3(10, 10, 5);
  public Vector3 targetArea = new Vector3(0, 0, 0);

  private List<GameObject> targets = new List<GameObject>();

  private const float START_Z = 50f;

  // Start is called before the first frame update
  void Start()
  {
    for (int i = 0; i < numberOfTargets; i++)
    {
      Vector3 position = new Vector3(
          Random.Range(-targetArea.x, targetArea.x),
          Random.Range(-targetArea.y, targetArea.y),
          Random.Range(START_Z, START_Z + targetArea.z)
      );

      GameObject target = Instantiate(targetTemplate, position, Quaternion.identity);
      target.SetActive(true);
      targets.Add(target);
    }
  }

  // Update is called once per frame
  void Update()
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
