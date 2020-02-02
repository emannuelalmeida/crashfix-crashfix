using Assets.Actors;
using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Events;

public abstract class PlayerActor : MonoBehaviour
{
    public Position Position { get; private set; }
    protected Position targetPosition;
    protected Vector3 targetVector;
    private readonly Vector3 movementOffset = new Vector3(-0.8f, 0, -0.8f);

    protected MapManager mapManager;

    private PlayerState _playerState;
    public readonly UnityEvent<PlayerState> PlayerStateEvents = new PlayerStateEvent();
    protected PlayerState playerState
    {
        get { return _playerState; }
        set
        {
            _playerState = value;
            PlayerStateEvents.Invoke(value);
        }
    }

    protected Facing facing;

    public float movementSpeed = 0.1f;
    protected float sensitivity = 0.15f;

    public void Initialize(MapManager mapManager, Position startPosition)
    {
        this.Position = startPosition;
        this.targetPosition = startPosition;
        this.mapManager = mapManager;
        playerState = PlayerState.IDLE;
        facing = Facing.DOWN;
    }

    protected abstract void UpdateIdle();

    protected virtual void UpdateMoving()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetVector, this.movementSpeed);

        if (Math.Abs(Vector3.Distance(targetVector, transform.position)) < 0.01f)
        {
            playerState = PlayerState.IDLE;
            transform.position = targetVector;
            Position.X = targetPosition.X;
            Position.Y = targetPosition.Y;
            mapManager.CheckVictoryCondition();
        }
    }

    protected abstract void UpdateInteracting();
    protected abstract void UpdateCheering();
    protected abstract void UpdateDefeat();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (mapManager.IsItVictory())
            playerState = PlayerState.CHEERING;
        else if (mapManager.IsItGameOver())
            playerState = PlayerState.DEFEAT;
        else switch (playerState)
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
            case PlayerState.DEFEAT:
                UpdateDefeat();
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
            targetVector = mapManager.GetTilePosition(targetPosition.X, targetPosition.Y);
            targetVector.y = this.transform.position.y;
            targetVector += movementOffset;
        }

        UpdateRotation();
    }

    private void UpdateRotation()
    {
        switch (facing)
        {
            case Facing.UP:
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                break;
            case Facing.DOWN:
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case Facing.LEFT:
                transform.localRotation = Quaternion.Euler(0, 270, 0);
                break;
            case Facing.RIGHT:
                transform.localRotation = Quaternion.Euler(0, 90, 0);
                break;
            default:
                break;
        }
    }
}
