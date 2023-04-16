using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CQTacticsToolkit
{
    public class PlayerTeamContainer : TeamContainer
    {
        private static PlayerTeamContainer _instance;
        public static PlayerTeamContainer Instance { get { return _instance; } }
        private void Awake() {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
    }
}
