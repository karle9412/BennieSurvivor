using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private Camera spawnCamera;

    [SerializeField, Min(0.01f)] private float spawnMargin = 0.1f;
    [SerializeField, Min(0.01f)] private float spawnInterval = 5f;

    private float nextSpawnTime = 0f;

    private void Update()
    {
        if (enemyPrefab == null)
            return;

        if (player == null || !player.gameObject.activeInHierarchy)
            return;

        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (Time.time < nextSpawnTime)
            return;

        Enemy enemy = Instantiate(enemyPrefab, GetSpawnPosition(), Quaternion.identity);

        enemy.SetTarget(player);

        nextSpawnTime = Time.time + spawnInterval;
    }

    private Vector3 GetSpawnPosition()
    {
        float x = 0f;
        float y = 0f;

        //정수 Random.Range의 마지막 값은 사용하지 않음.
        int side = Random.Range(0, 4);

        switch(side)
        {
            case 0: //왼쪽
                x = -spawnMargin;
                y = Random.value;
                break;

            case 1: //오른쪽
                x = 1f + spawnMargin;
                y = Random.value;
                break;

            case 2: //아래쪽
                x = Random.value;
                y = -spawnMargin;
                break;

            case 3: //위쪽
                x = Random.value;
                y = 1f + spawnMargin;
                break;
        }

        //현재처럼 카메라가 회전 없이 +Z 방향을 보는 2D 기준
        float distanceFromCamera = player.position.z - spawnCamera.transform.position.z;

        Vector3 position = spawnCamera.ViewportToWorldPoint(new Vector3(x, y, distanceFromCamera));

        position.z = player.position.z;
        return position;
    }
}