using UnityEngine;

namespace _Game.Scripts.MainMechanics
{
    public class MoveCamera : MonoBehaviour {

        public Transform player;

        private void Update()
        {
            transform.position = player.transform.position;
        }
    }
}
