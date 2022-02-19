namespace Enemy
{
    public interface IOnEnemyStateChange
    {
        public void OnStateChange(EnemyState newState);
    }
}