using Assets.Actors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerActor : MonoBehaviour
{
    public Position Position { get; private set; }
    protected Position targetPosition;

    protected MapManager mapManager;

    protected PlayerState playerState;
    protected Facing facing;

    public float movementSpeed = 0.1f;

    public void Initialize(MapManager mapManager, Position startPosition)
    {
        this.Position = startPosition;
        this.targetPosition = startPosition;
        this.mapManager = mapManager;
        playerState = PlayerState.IDLE;
        facing = Facing.DOWN;
    }

    protected abstract void UpdateIdle();
    protected abstract void UpdateMoving();
    protected abstract void UpdateInteracting();
    protected abstract void UpdateCheering();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (mapManager.TimeRemaining.TotalSeconds <= 0)
            playerState = PlayerState.DEFEAT;

        switch (playerState)
        {
            case PlayerState.IDLE:
                UpdateIdle();
                break;
            case PlayerState.MOVING:
                UpdateMoving();
                break;
            case PlayerState.INTERACTING:
                UpdateInteracting();
                break;
            case PlayerState.CHEERING:
                UpdateCheering();
                break;
            default:
                throw new NotSupportedException("Invalid state");
        }
    }

    protected void TryMove(int x, int y)
    {
        if (x > Position.X)
            facing = Facing.RIGHT;
        else if (x < Position.X)
            facing = Facing.LEFT;
        else if (y < Position.Y)
            facing = Facing.UP;
        else if (y > Position.Y)
            facing = Facing.DOWN;

        if (mapManager.CanMove(x, y))
        {
            playerState = PlayerState.MOVING;
            targetPosition = new Position(x, y);
        }
    }
}
