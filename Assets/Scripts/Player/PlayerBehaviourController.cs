using Core;
using UnityEngine;
using Behaviour = Core.Behaviour;

namespace Player
{
    public class PlayerBehaviourController : BaseBehaviourController, IAffectOnDamage
    {
        [SerializeField] private float onDamagedTime = 2f;
        private float _timeSinceDamaged = 0f;
        public void OnDamaged()
        {
            _timeSinceDamaged = onDamagedTime;
        }

        private void Update()
        {
            UpdateBehaviour();
        }

        protected override void UpdateBehaviour()
        {
            if (_timeSinceDamaged > 0)
            {
                _currentBehaviour = Behaviour.Damaged;
                _timeSinceDamaged -= Time.deltaTime;
            }
            else
            {
                _currentBehaviour = Behaviour.Normal;
            }

            NotifyTargetsIfNessesary();
        }
        
    }
}