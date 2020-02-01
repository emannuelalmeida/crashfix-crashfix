using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Actors
{
    public class Fixer : PlayerActor
    {
        public Fixer()
        {

        }

        protected override void UpdateCheering()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateIdle()
        {
            if(Input.GetAxis("Vertical") > 0.65f)
            {
                TryMove(Position.X, Position.Y + 1);
            }
            if (Input.GetAxis("Vertical") < -0.65f)
            {
                TryMove(Position.X, Position.Y - 1);
            }
            if (Input.GetAxis("Horizontal") > 0.65f)
            {
                TryMove(Position.X + 1, Position.Y);
            }
            if (Input.GetAxis("Horizontal") < -0.65f)
            {
                TryMove(Position.X - 1, Position.Y);
            }
        }

        protected override void UpdateInteracting()
        {
            Position interactPosition = Position;
            switch (facing)
            {
                case Facing.UP:
                    interactPosition.Y--;
                    break;
                case Facing.DOWN:
                    interactPosition.Y++;
                    break;
                case Facing.LEFT:
                    interactPosition.X--;
                    break;
                case Facing.RIGHT:
                    interactPosition.X++;
                    break;
            }

            mapManager.FixTile(Position.X, Position.Y);
        }

        protected override void UpdateMoving()
        {
            var targetVector = mapManager.GetTilePosition(targetPosition.X, targetPosition.Y);
            transform.position = Vector3.MoveTowards(transform.position, targetVector, this.movementSpeed);

            if(Vector3.Distance(targetVector, transform.position) < 0.01f)
            {
                playerState = PlayerState.IDLE;
                transform.position = targetVector;
                mapManager.OnStepTile(targetPosition.X, targetPosition.Y, this);
            }
        }
    }
}
