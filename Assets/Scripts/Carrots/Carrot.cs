using Core;
using UnityEngine;
using UnityEngine.Events;

namespace Carrots
{
    public class Carrot : MonoBehaviour, IAffectOnDamage
    {
        public static readonly UnityEvent OnCarrotExplose = new UnityEvent();
        public void OnDamaged()
        {
            OnCarrotExplose?.Invoke();
            Destroy(gameObject);
            
        }
    }
}