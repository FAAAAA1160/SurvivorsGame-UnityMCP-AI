using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[BurstCompile]
public partial struct EnemySpawnSystem : ISystem
{
    private EntityQuery _spawnerQuery;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _spawnerQuery = new EntityQueryBuilder(Unity.Collections.Allocator.Temp)
            .WithAll<EnemySpawner>()
            .Build(ref state);

        state.RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 获取玩家位置
        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        if (playerEntity == Entity.Null) return;

        LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
        float deltaTime = SystemAPI.Time.DeltaTime;

        // 使用EntityCommandBufferSystem
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        // 创建主随机数生成器
        var masterRandom = Random.CreateFromIndex((uint)state.WorldUnmanaged.Time.ElapsedTime);

        // 遍历所有生成器
        foreach (var (spawner, spawnerEntity) in
                 SystemAPI.Query<RefRW<EnemySpawner>>().WithEntityAccess())
        {
            spawner.ValueRW.Timer += deltaTime;

            // 检查是否可以刷新
            if (spawner.ValueRO.Timer >= spawner.ValueRO.SpawnInterval)
            {
                // 计算本次可以刷新的最大数量
                int availableSlots = spawner.ValueRO.MaxEnemies - spawner.ValueRO.CurrentEnemies;
                int spawnCount = math.min(spawner.ValueRO.SpawnBatchCount, availableSlots);

                if (spawnCount <= 0) continue; // 没有可用的刷新槽位

                spawner.ValueRW.Timer = 0;

                // 批量生成敌人
                for (int i = 0; i < spawnCount; i++)
                {
                    GenerateEnemy(
                        ecb,
                        spawner.ValueRO.EnemyPrefab,
                        playerTransform.Position,
                        spawner.ValueRO.SpawnRadius,
                        spawnerEntity,
                        i,
                        ref masterRandom,
                        ref state
                    );

                    spawner.ValueRW.CurrentEnemies++;
                }
            }
        }
    }

    [BurstCompile]
    private void GenerateEnemy(
        EntityCommandBuffer ecb,
        Entity enemyPrefab,
        float3 playerPos,
        float spawnRadius,
        Entity spawnerEntity,
        int spawnIndex,
        ref Random random,
        ref SystemState state)
    {
        if (enemyPrefab == Entity.Null) return;

        // 生成敌人实体
        Entity enemy = ecb.Instantiate(enemyPrefab);

        // 生成随机位置
        float3 spawnPos = GetRandomSpawnPosition(
            playerPos,
            spawnRadius,
            spawnerEntity,
            spawnIndex,
            ref random,
            ref state
        );

        // 设置位置
        ecb.SetComponent(enemy, LocalTransform.FromPosition(spawnPos));
    }

    [BurstCompile]
    private float3 GetRandomSpawnPosition(
        float3 playerPos,
        float spawnRadius,
        Entity spawnerEntity,
        int spawnIndex,
        ref Random random,
        ref SystemState state)
    {
        const float MIN_DISTANCE = 2.5f;  // 敌人间最小间距
        const int MAX_ATTEMPTS = 5;       // 位置尝试次数

        uint randomSeed = random.NextUInt() +
                         (uint)spawnerEntity.Index +
                         (uint)spawnIndex +
                         (uint)(state.WorldUnmanaged.Time.ElapsedTime * 1000);

        var localRandom = Random.CreateFromIndex(randomSeed);

        for (int attempt = 0; attempt < MAX_ATTEMPTS; attempt++)
        {
            // 生成随机角度和距离
            float angle = localRandom.NextFloat(0f, 360f) * math.PI / 180f;
            float distance = localRandom.NextFloat(10f, spawnRadius);

            float3 spawnPos = new float3(
                playerPos.x + math.cos(angle) * distance,
                0,
                playerPos.z + math.sin(angle) * distance
            );

            // 简单位置检查（可选）
            bool tooClose = false;

            foreach (var enemyTransform in
                     SystemAPI.Query<RefRO<LocalTransform>>().WithAll<EnemyTag>())
            {
                if (math.distance(spawnPos, enemyTransform.ValueRO.Position) < MIN_DISTANCE)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose) return spawnPos;

            // 准备下一次尝试
            localRandom = Random.CreateFromIndex(localRandom.NextUInt());
        }

        // 如果所有尝试都失败，返回最后一次计算的位置
        float finalAngle = localRandom.NextFloat(0f, 360f) * math.PI / 180f;
        float finalDistance = localRandom.NextFloat(10f, spawnRadius);

        return new float3(
            playerPos.x + math.cos(finalAngle) * finalDistance,
            0,
            playerPos.z + math.sin(finalAngle) * finalDistance
        );
    }
}