using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected float speed;

    public Vector2 MoveWithLimited(Vector2 posMove)
    {
        posMove.x = posMove.x > GameKey.MAP_MAX_X ? GameKey.MAP_MAX_X : posMove.x;
        posMove.x = posMove.x < GameKey.MAP_MIN_X ? GameKey.MAP_MIN_X : posMove.x;
        posMove.y = posMove.y > GameKey.MAP_MAX_Y ? GameKey.MAP_MAX_Y : posMove.y;
        posMove.y = posMove.y < GameKey.MAP_MIN_Y ? GameKey.MAP_MIN_Y : posMove.y;
        return posMove;
    }
}
