using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NicknameUI : MonoBehaviour
{
    public WorldUINickname nicknamePrefab;

    //private readonly Dictionary<VehicleEntity, WorldUINickname> _kartNicknames =
    //    new Dictionary<VehicleEntity, WorldUINickname>();

    private void Awake()
    {
        EnsureAllTexts();

        //VehicleEntity.OnCarSpawned += SpawnNicknameText;
        //VehicleEntity.OnCarDespawned += DespawnNicknameText;
    }

    private void OnDestroy()
    {
        //VehicleEntity.OnCarSpawned -= SpawnNicknameText;
        //VehicleEntity.OnCarDespawned -= DespawnNicknameText;
    }

    private void EnsureAllTexts()
    {
        // we need to make sure that any karts that spawned before the callback was subscribed, are registered
        //var karts = VehicleEntity.Cars;
        //foreach(var kart in karts.Where( kart => !_kartNicknames.ContainsKey( kart ) ))
        //{
        //    SpawnNicknameText( kart );
        //}
    }

    //private void SpawnNicknameText( VehicleEntity kart )
    //{
    //    // we dont want to see our own name tag - dont spawn
    //    if(kart.Object.IsValid && kart.Object.HasInputAuthority)
    //        return;

    //    var obj = Instantiate( nicknamePrefab, this.transform );
    //    obj.SetKart( kart );

    //    _kartNicknames.Add( kart, obj );
    //}

    //private void DespawnNicknameText( VehicleEntity kart )
    //{
    //    if(!_kartNicknames.ContainsKey( kart ))
    //        return;

    //    var text = _kartNicknames[ kart ];
    //    Destroy( text.gameObject );

    //    _kartNicknames.Remove( kart );
    //}
}
