using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that will handle the attack cards, that will have their main effect to be diverse and ACTIVATED IMMEDIATELY WHEN PLAYED
/// </summary>
public class WildCardController : CardController
{
    [field: Header("Wild Values")]
    [field: SerializeField] public AudioClip Sound { get; private set; }

    public override void ActivateCardEffect(GameObject card)
    {
        CardAudioManager.Instance.PlayAudioClip(Sound);
        TextVFXSpawner.Instance.RequestSpawnVFX(new Vector2(0,0), VFX_TYPE.WILD);
    }
}
