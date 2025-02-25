using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;


public class WorldUINickname : MonoBehaviour
{
    /// <summary>
    /// Make this always point towards the main camera and follow the target if there is one.
    /// If there is no target to follow, we destroy this object to clean up.
    /// </summary>

    public Text worldNicknameText;
    public Vector3 offset;

    [HideInInspector] public Transform target;

    //private VehicleEntity _car;

    //public void SetKart( VehicleEntity car )
    //{
    //    _car = car;
    //    target = car.Rigidbody.InterpolationTarget;
    //}

    private void Update()
    {
        //if(_car == null) return;

        //var username = "";

        //if(_car.TryGetComponent<ARCarController>( out var carComponent ))
        //    username = carComponent.RoomUser.Username.Value;
        //else if(_car.TryGetComponent<BikeController>( out var motorcycleComponent ))
        //    username = motorcycleComponent.RoomUser.Username.Value;

        //worldNicknameText.text = username;

        //var lobbyUser = _car.Controller.RoomUser;

        //if(lobbyUser != null)
        //{
        //    worldNicknameText.text = lobbyUser.Username.Value;
        //}
    }

    private void LateUpdate()
    {
        if(target)
        {
            transform.SetPositionAndRotation( target.position + offset, Camera.main.transform.rotation );
        }
        else
        {
            StartCoroutine( WaitAndDestroy() );
        }
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds( 3 );
        if(target != null && !target.Equals( null ))
        {
            //continue following the target
            yield return null;
        }
        else //there has been no target to follow for 3 seconds so Destroy this:
        {
            Destroy( gameObject );
        }
    }
}

