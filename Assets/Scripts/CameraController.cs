using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] Transform player;

    void LateUpdate() {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
