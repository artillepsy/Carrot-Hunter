using System.Collections.Generic;
using System.Linq;
using Enemy;
using UnityEngine;

namespace Core
{
    public abstract class BaseBehaviourController : MonoBehaviour
    {
        protected Behaviour _currentBehaviour;
        private Behaviour _prevBehaviour;
        private List<IOnBehaviourChange> _callbackTargets;
        
        protected abstract void UpdateBehaviour();
        
        private void Awake()
        {
            _callbackTargets = GetComponents<IOnBehaviourChange>().ToList();
            _currentBehaviour = Behaviour.Normal;
            _prevBehaviour = _currentBehaviour;
        }
        
        protected void NotifyTargetsIfNessesary()
        {
            if (_prevBehaviour == _currentBehaviour) return;
            foreach (var target in _callbackTargets)
            {
                target.OnBehaviourChange(_currentBehaviour);
            }
            _prevBehaviour = _currentBehaviour;
        }
    }
}