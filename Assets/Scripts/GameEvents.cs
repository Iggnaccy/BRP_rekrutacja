public static class GameEvents
{
    public delegate void OnEnemyKilled(IEnemy enemy, DamageType damageType);
    public static OnEnemyKilled EnemyKilled;
}

