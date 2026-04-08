using UnityEngine;
using UnityEngine.InputSystem;
using TD.Core;

namespace TD.Input {
    public class InputManager : MonoBehaviour {
        [SerializeField] private LayerMask enemyMask;

        public void OnClick() {
            // Create a ray from the camera through the mouse position
            var mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit, 10000, enemyMask)) {
                // 'hit.collider' contains the collider of the object hit
                Unit enemyObj = hit.collider.gameObject.GetComponent<Unit>();
                enemyObj.TakeDamage(enemyObj.hp);
            }
        }
    }

}