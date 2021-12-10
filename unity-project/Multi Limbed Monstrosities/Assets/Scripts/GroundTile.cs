using UnityEngine;

public class GroundTile : MonoBehaviour
{

  GroundSpawner groundSpawner;
  [SerializeField] GameObject coinPrefab;
  [SerializeField] GameObject obstaclePrefab;

  private void Start()
  {
    groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
  }

  private void OnTriggerExit(Collider other)
  {
    groundSpawner.SpawnTile();
    Destroy(gameObject, 2);
  }

  //   private void Update()
  //   {
  //     this.gameObject.transform.position += Vector3.back * 0.1f;
  //   }
}
