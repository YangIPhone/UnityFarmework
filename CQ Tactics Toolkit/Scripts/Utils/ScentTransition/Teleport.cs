using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CQFramework.TransitionScene{
    public class Teleport : MonoBehaviour
    {
        [Header("要去的场景")]
        [SceneName]public string sceneToGo;
        [Header("要去的场景位置")]
        public Vector3 positionToGo;

        private void OnTriggerEnter2D(Collider2D other) {
            if(other.CompareTag("Player")){
                EventHandler.CallTransitionEvent(sceneToGo,positionToGo);
            }
        }
    }
}

