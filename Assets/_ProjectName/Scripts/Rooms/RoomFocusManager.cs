using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class RoomFocusManager : MonoBehaviour
{
    public static RoomFocusManager Instance;

    private CinemachineCamera cinemachineCamera;

    private HashSet<RoomMaskController> roomsPlayerInside = new();
    private RoomMaskController currentRoom;

    private void Awake()
    {
        Instance = this;
        cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();
    }

    public void RegisterEnter(RoomMaskController room)
    {
        roomsPlayerInside.Add(room);

        // 🔥 Si ya había un cuarto activo, no cambiamos nada
        if (currentRoom != null)
            return;

        // Si no había ninguno activo, este será el primero
        SetCurrentRoom(room);
    }

    public void RegisterExit(RoomMaskController room)
    {
        roomsPlayerInside.Remove(room);

        // Si el que salió no es el actual, no hacemos nada
        if (currentRoom != room)
            return;

        if (roomsPlayerInside.Count == 0)
        {
            currentRoom.LightOff();
            currentRoom = null;
            return;
        }

        // Si queda solo uno, ese es el nuevo activo
        foreach (var remainingRoom in roomsPlayerInside)
        {
            SetCurrentRoom(remainingRoom);
            break;
        }
    }

    private void SetCurrentRoom(RoomMaskController newRoom)
    {
        if (currentRoom != null)
            currentRoom.LightOff();

        currentRoom = newRoom;

        currentRoom.LightOn();
        cinemachineCamera.Follow = currentRoom.transform;
    }
}