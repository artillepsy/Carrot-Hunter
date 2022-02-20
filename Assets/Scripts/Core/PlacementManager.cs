using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class PlacementManager : MonoBehaviour
    {
        public static PlacementManager Inst;
        public readonly List<Vector2> dots;
        private void Awake()
        {
            Inst = this;
        }

        private void Start()
        {
            foreach (var dot in GameObject.FindGameObjectsWithTag("Dot"))
            {
                dots.Add(dot.transform.position);
            }
            
        }
    }
}