using UnityEngine;

public class SoulEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject InteractionPanelObject;
    [SerializeField] private GameObject ActionsPanelObject;
    [SerializeField] private SpriteRenderer EnemySpriteRenderer;

    private SpawnPoint _enemyPosition;
    private EnemyDefinition _enemyDefinition;

    public void SetupEnemy(EnemyDefinition enemyDefinition, SpawnPoint spawnPoint)
    {
        EnemySpriteRenderer.sprite = enemyDefinition.Sprite;
        _enemyDefinition = enemyDefinition;
        _enemyPosition = spawnPoint;
        gameObject.SetActive(true);
    }

    public SpawnPoint GetEnemyPosition()
    {
        return _enemyPosition;
    }

    public GameObject GetEnemyObject()
    {
        return this.gameObject;
    }

    private void ActiveCombatWithEnemy()
    {
        ActiveInteractionPanel(false);
        ActiveActionPanel(true);
    }

    private void ActiveInteractionPanel(bool active)
    {
        InteractionPanelObject.SetActive(active);
    }

    private void ActiveActionPanel(bool active)
    {
        ActionsPanelObject.SetActive(active);
    }

    private void UseBow()
    {
        // USE BOW
        GameEvents.EnemyKilled?.Invoke(this, DamageType.BOW);
    }

    private void UseSword()
    {
        GameEvents.EnemyKilled?.Invoke(this, DamageType.SWORD);
        // USE SWORD
    }

    public void SelectMe()
    {
        if(InteractionPanelObject.activeSelf)
            InteractionPanelObject.GetComponentInChildren<UnityEngine.UI.Button>().Select();
        else if(ActionsPanelObject.activeSelf)
            ActionsPanelObject.GetComponentInChildren<UnityEngine.UI.Button>().Select();
    }

    #region OnClicks

    public void Combat_OnClick()
    {
        ActiveCombatWithEnemy();
    }

    public void Bow_OnClick()
    {
        UseBow();
    }

    public void Sword_OnClick()
    {
        UseSword();
    }

    #endregion
    public int GetScore(DamageType damageType)
    {
        if(damageType == _enemyDefinition.Weakness)
            return (int)(_enemyDefinition.Score * 1.5f);
        return _enemyDefinition.Score;
    }

}


public interface IEnemy
{
    SpawnPoint GetEnemyPosition();
    GameObject GetEnemyObject();
    int GetScore(DamageType damageType);
    void SelectMe();
}

public enum DamageType
{
    BOW,
    SWORD
}
