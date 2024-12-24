using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Assets.Scripts.Enemy
{
    public class EnemyStateMachine
    {
        public EnemyState currentState { get; private set; }

        public void InitializeState(EnemyState state)
        {
            currentState = state;
            currentState.Enter();
        }

        public void ChangeState(EnemyState state)
        {
            if (state == currentState)
                return;

            currentState.Exit();
            currentState = state;
            currentState.Enter();
        }
    }
}
