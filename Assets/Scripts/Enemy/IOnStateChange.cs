using Core;

namespace Enemy
{
    public interface IOnStateChange
    {
        public void OnStateChange(State newState);
    }
}