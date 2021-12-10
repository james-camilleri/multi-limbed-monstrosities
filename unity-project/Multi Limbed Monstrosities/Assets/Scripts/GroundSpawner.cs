using UnityEngine;

public class GroundSpawner : MonoBehaviour
{

  [SerializeField] GameObject groundTile;
  Vector3 nextSpawnPoint;

  GameObject lastTile;

  public void SpawnTile()
  {
    Vector3 spawnPoint = new Vector3(0, -1.5f, 0);
    if (lastTile != null)
    {
      spawnPoint = lastTile.transform.position + Vector3.forward * 5;
    }

    GameObject newTile = Instantiate(groundTile, spawnPoint, Quaternion.identity);
    newTile.transform.parent = this.gameObject.transform.parent;
    lastTile = newTile;

    // nextSpawnPoint = newTile.transform.GetChild(1).transform.position;
  }

  private void Start()
  {
    for (int i = 0; i < 15; i++)
    {
      SpawnTile();
    }
  }
}
