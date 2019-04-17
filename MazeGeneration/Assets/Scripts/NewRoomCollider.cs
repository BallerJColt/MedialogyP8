using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRoomCollider : MonoBehaviour
{
    public NotePadScript notePad;
    Collider triggerCollider;
    Valve.VR.InteractionSystem.Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Valve.VR.InteractionSystem.Player>();
        triggerCollider = player.headCollider;
    }


    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        
        if (collider == triggerCollider)
        {
            notePad.NotepadEnterNewRoom();
        }

    }
}
