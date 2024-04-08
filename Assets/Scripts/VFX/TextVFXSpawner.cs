using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VFX_TYPE
{
    DAMAGE_RED,
    DAMAGE_BLUE,
    MISS,
    BUFF
}

[System.Serializable]
public struct VFXElement
{
    public VFX_TYPE Type;
    public Sprite VFX;
}

/// <summary>
/// Script that will spawn the text VFX for a card's effect.
/// Every VFX is referenced by the overhead enum, and this controller is a singleton in the scene. 
/// </summary>
public class TextVFXSpawner : MonoBehaviour
{
    #region SINGLETON DESIGN PATTERN
    public static TextVFXSpawner Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this);

        AssignDict();
    }
    #endregion

    #region VARIABLES
    [Header("VFX Elements")]
    [SerializeField] private GameObject _vfxPrefab;
    [SerializeField] private List<VFXElement> _vfxList;
    private Dictionary<VFX_TYPE, Sprite> _vfxDict;
    #endregion

    private void AssignDict()
    {
        _vfxDict = new Dictionary<VFX_TYPE, Sprite>();
        foreach(var vfx in _vfxList)
        {
            _vfxDict.Add(vfx.Type, vfx.VFX);
        }
    }

    /// <summary>
    /// Function called by external scripts (most likely CardController and its children OR ActorValuesController) that will request the spawn of a VFX at a certain position.
    /// </summary>
    /// <param name="spawnPos">Position where the VFX is to spawn</param>
    /// <param name="type">Type of the VFX</param>
    public void RequestSpawnVFX(Vector2 spawnPos, VFX_TYPE type)
    {
        if (!_vfxDict.ContainsKey(type))
        {
            Debug.LogError("[VFX SPAWNER] : dict does not contain requested VFX type");
            return;
        }
        StartCoroutine(SpawnNewVFX(spawnPos, type));
    }

    public IEnumerator SpawnNewVFX(Vector2 spawnPos, VFX_TYPE type)
    {
        GameObject newVfx = Instantiate(_vfxPrefab);
        _vfxPrefab.GetComponent<ParticleSystem>().textureSheetAnimation.SetSprite(0, _vfxDict[type]);
        newVfx.transform.position = spawnPos;
        yield return new WaitForSeconds(2f);
        Destroy(newVfx);
    }
}
