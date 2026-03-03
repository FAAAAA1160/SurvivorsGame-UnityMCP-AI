using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    [Header("刷新配置")]
    public GameObject EnemyPrefab;
    public float SpawnInterval = 0.5f;
    public int MaxEnemies = 50;
    public float SpawnRadius = 20f;
    [Tooltip("每次刷新生成多少个敌人（默认为1）")]
    public int SpawnBatchCount = 1;  // 新增
    [Tooltip("敌人类别（0=默认，可用于随机选择不同类型）")]
    public int EnemyType = 0;        // 新增，为后续多类型敌人预留

    class Baker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            if (authoring.EnemyPrefab == null)
            {
                Debug.LogWarning("EnemyPrefab is not assigned in EnemySpawnerAuthoring!");
                return;
            }

            Entity enemyPrefabEntity = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic);

            AddComponent(entity, new EnemySpawner
            {
                EnemyPrefab = enemyPrefabEntity,
                SpawnInterval = authoring.SpawnInterval,
                Timer = 0f,
                MaxEnemies = authoring.MaxEnemies,
                CurrentEnemies = 0,
                SpawnRadius = authoring.SpawnRadius,
                SpawnBatchCount = math.max(1, authoring.SpawnBatchCount),  // 确保至少为1
                EnemyTypes = authoring.EnemyType
            });
        }
    }
}