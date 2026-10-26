using UnityEngine;

[CreateAssetMenu(fileName = "Stage01", menuName = "ZombieFPS/ZombieSpawnTable")]
public class ZombieSpawnTable : ScriptableObject
{
    public int StageID = 1;
    public ZombieSpawnData[] ZombieSpawnDataList;
}