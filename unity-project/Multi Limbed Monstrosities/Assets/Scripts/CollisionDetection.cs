using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
  void OnTriggerEnter(Collider collider)
  {
    if (collider.gameObject.tag == "Limb")
    {
      this.gameObject.SetActive(false);
    }
  }
}
