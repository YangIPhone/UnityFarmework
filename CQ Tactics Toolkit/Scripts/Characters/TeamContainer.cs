using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CQTacticsToolkit
{
    public class TeamContainer : MonoBehaviour
    {
        public List<Character> characters;

        void Start()
        {
            characters = GetComponentsInChildren<Character>().ToList();
        }
    }
}
