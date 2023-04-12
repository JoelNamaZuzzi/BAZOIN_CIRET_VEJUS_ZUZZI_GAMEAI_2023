﻿using AI_BehaviorTree_AIGameUtility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AI_BehaviorTree_AIImplementation
{
    public class AIDecisionMaker
    {

        /// <summary>
        /// Ne pas supprimer des fonctions, ou changer leur signature sinon la DLL ne fonctionnera plus
        /// Vous pouvez unitquement modifier l'intérieur des fonctions si nécessaire (par exemple le nom)
        /// ComputeAIDecision en fait partit
        /// </summary>
        private int AIId = -1;
        public GameWorldUtils AIGameWorldUtils = new GameWorldUtils();
        BehaviorTree behavior = new BehaviorTree();

        // Ne pas utiliser cette fonction, elle n'est utile que pour le jeu qui vous Set votre Id, si vous voulez votre Id utilisez AIId
        public void SetAIId(int parAIId) { AIId = parAIId; }

        // Vous pouvez modifier le contenu de cette fonction pour modifier votre nom en jeu
        public string GetName() { return "CIRNO the STRONGEST"; }
        //Same as Initialize
        public void SetAIGameWorldUtils(GameWorldUtils parGameWorldUtils) { 
            AIGameWorldUtils = parGameWorldUtils;
            behavior.ID = AIId;
            //initialize your actions/parameters and such here
        }

        //Fin du bloc de fonction nécessaire (Attention ComputeAIDecision en fait aussi partit)


        private float BestDistanceToFire = 10.0f;
        public List<AIAction> ComputeAIDecision()
        {
            /*List<PlayerInformations> playerInfos = AIGameWorldUtils.GetPlayerInfosList();
            PlayerInformations myPlayerInfo = GetPlayerInfos(AIId, playerInfos);
            List<Action> myAction =  behavior.GetActions(myPlayerInfo);
            /*List<AIAction> actionList = new List<AIAction>();

            List<PlayerInformations> playerInfos = AIGameWorldUtils.GetPlayerInfosList();
            PlayerInformations target = null;
            foreach (PlayerInformations playerInfo in playerInfos)
            {
                if (!playerInfo.IsActive)
                    continue;

                if (playerInfo.PlayerId == AIId)
                    continue;

                target = playerInfo;
                break;
            }

            if (target == null)
                return actionList;

            PlayerInformations myPlayerInfo = GetPlayerInfos(AIId, playerInfos);
            if (myPlayerInfo == null)
                return actionList;

            if (Vector3.Distance(myPlayerInfo.Transform.Position, target.Transform.Position) < BestDistanceToFire)
            {
                AIActionStopMovement actionStop = new AIActionStopMovement();
                actionList.Add(actionStop);
            }
            else
            {
                AIActionMoveToDestination actionMove = new AIActionMoveToDestination();
                actionMove.Position = target.Transform.Position;
                actionList.Add(actionMove);
            }


            AIActionLookAtPosition actionLookAt = new AIActionLookAtPosition();
            actionLookAt.Position = target.Transform.Position;
            actionList.Add(actionLookAt);
            actionList.Add(new AIActionFire());*/

            List<AIAction> actionList = new List<AIAction>();

            return actionList;
        }

        public PlayerInformations GetPlayerInfos(int parPlayerId, List<PlayerInformations> parPlayerInfosList)
        {
            foreach (PlayerInformations playerInfo in parPlayerInfosList)
            {
                if (playerInfo.PlayerId == parPlayerId)
                    return playerInfo;
            }

            Assert.IsTrue(false, "GetPlayerInfos : PlayerId not Found");
            return null;
        }

    }

    public enum State
    {
        NOT_EXECUTED,
        SUCCESS,
        FAILURE,
        RUNNING
    }

    public class BehaviorTree
    {
        public int IDPlayer = -1;
        public Selector mySelector;

        public List<Action> actionToRealize;
        public List<AIAction> listAIActions;
        public BlackBoard myBlackBoard;

        public void getSelectorActions()
        {
            actionToRealize = mySelector.GetActions();
        }

        public void getAIActions()
        {
            foreach (var action in actionToRealize)
            {
                listAIActions.Add(action.GetAIAction());
            }
        }

        /*public List<Action> GetActions(PlayerInformations myPlayerInfo, List<PlayerInformations> playerInfos, BlackBoard theBlackBoard)

        {
            //Fait ton call en envoyant myPlayerInfo et the BlackBoard
        }*/
    }
    public class BlackBoard
    {
        public PlayerInformations playerTarget = null;
    }

    public class Noeud 
    {
        public State state;
        public List<Action> listActions;

        public List<Action> GetActions()
        {
            return listActions;
        }
    }
    public class Selector : Noeud
    {
        Action defaultAction;
        List<Sequencer> listSequencer;

        Selector()
        {
            listSequencer = new List<Sequencer>();
            state = State.NOT_EXECUTED;
        }

        /// <summary>
        /// lance le selector, qui va check l'état des Sequence et recuperer les actions a effectuer 
        /// </summary>
        /// <param name="myPlayerInfo"></param>
        public void LaunchSelector(PlayerInformations myPlayerInfo)
        {

            for (int i = 0; i < listSequencer.Count; i++)
            {
                listSequencer[i].LaunchSequencer(myPlayerInfo);
                if (listSequencer[i].state == State.SUCCESS)
                {
                    listActions = listSequencer[i].GetActions();
                    this.state = State.SUCCESS;
                    return;
                }
                if( i == listSequencer.Count)
                {
                    listActions = new List<Action>();
                    listActions.Add(defaultAction);
                    this.state = State.SUCCESS;
                    return;
                }
            }
        } 

        /// <summary>
        /// Ajoute une sequence a la liste des Sequence 
        /// </summary>
        /// <param name="s"></param>

       public void AddSequencer(Sequencer s)
        {
            listSequencer.Add(s);
        }

    }
    public class Sequencer : Noeud
    {
        public void addAction(Action a)
        {
            listActions.Add(a);
        }

        public void LaunchSequencer(PlayerInformations myPlayerInfo)
        {
            for (int i = 0; i< listActions.Count; i++)
            {
                if (listActions[i].GetState(myPlayerInfo) == State.FAILURE)
                {
                    listActions = new List<Action>();
                    return;
                }
                if (i == listActions.Count)
                {
                    this.state = State.SUCCESS;
                }

            }
        }
    }
    //Make AIAction a list, even if it's for only one
    public class Action : Noeud
    {
        public AIAction myAIAction;
        public virtual State GetState(PlayerInformations myPlayerInfo)
        {
            return state;
        }
        public virtual State GetState(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos)
        {
            return state;
        }
        public virtual State GetState(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard)
        {
            return state;
        }
        public virtual AIAction GetAIAction()
        {
            return myAIAction;
        }
        public virtual AIAction GetAIAction(BlackBoard theBlackBoard)
        {
            return myAIAction;
        }
    }
    public class ActionDash : Action
    {
        public override State GetState(PlayerInformations myPlayerInfo)
        {
            if (myPlayerInfo.IsDashAvailable)
            {
                state = State.SUCCESS;
            }
            else
            {
                state = State.FAILURE;
            }
            return state;
        }
    }
    public class ActionSetTarget : Action
    {
        public new AIActionLookAtPosition myAIAction = new AIActionLookAtPosition();
        PlayerInformations potentialTarget = null;
        //Ici on cherche tjr une nouvelle target, mais modifiable pour ne pas changer de target quand on en a une, ou en changer seulement si on a une distance specifique, etc...
        public override State GetState(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos)
        {
            potentialTarget = theBlackBoard.playerTarget;
            if (potentialTarget == null || !potentialTarget.IsActive)
            {
                foreach (PlayerInformations playerInfo in playerInfos)
                {
                    if (!playerInfo.IsActive)
                        continue;

                    else if (playerInfo.PlayerId == myPlayerInfo.PlayerId)
                        continue;
                    else
                    {
                        if (potentialTarget == null)
                        {
                            potentialTarget = playerInfo;
                        }
                        else if (Vector3.Distance(myPlayerInfo.Transform.Position, playerInfo.Transform.Position) < 
                            Vector3.Distance(myPlayerInfo.Transform.Position, potentialTarget.Transform.Position))
                        {
                            potentialTarget = playerInfo;
                        }
                    }
                }
            }
            return State.SUCCESS;
        }
        public override AIAction GetAIAction(BlackBoard theBlackBoard)
        {
            myAIAction.Position = potentialTarget.Transform.Position;
            theBlackBoard.playerTarget = potentialTarget;
            return myAIAction;
        }
    }
}
