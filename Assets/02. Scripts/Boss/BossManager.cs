using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : Singleton<BossManager>
{
    [SerializeField] private List<Pattern> patterns;
    [SerializeField] private int pageNumber;

    protected override void Awake()
    {
        StartPattern(patterns[0]);
    }

    // ����(������) ���� �޼��� 
    public void StartPattern(Pattern pattern)
    {
        // ���� �б� ���� 
        StartCoroutine(ReadTileSets(pattern.tilesets));
    }

    // ���� �а� ���� �޼��� 
    private IEnumerator ReadTileSets(List<TileSetData> tilesets)
    {
        foreach (TileSetData tileset in tilesets)
        {
            // ��ٿ� ��� 
            yield return new WaitForSeconds(tileset.coolDownTime);

            // ���� ����
            if (tileset.useTrail)
            {
                // TODO - ���� �ڵ� �ۼ�
            }

            // Ÿ�� ���� ��ȯ
            if (tileset.useTile)
            {
                TileManager.Instance.SetTileType(tileset.tilePositions, tileset.type, tileset.startupTime, tileset.holdingTime);
            }
        }
    }
}
