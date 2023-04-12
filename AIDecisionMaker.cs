using AI_BehaviorTree_AIGameUtility;
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
        public int ID = -1;
        public Selector mySelector;
        List<int> BlackBoard = new List<int>();
        /*public List<Action> GetActions(PlayerInformations myPlayerInfo)
        {
            //Fait ton call en envoyant myPlayerInfo
        }*/
    }

    public class Noeud 
    {
        public State state;
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

        public List<Action> LaunchSelector()
        {
            List<Action> returnActions = new List<Action>();

            foreach (Sequencer sequencer in listSequencer)
            {
                sequencer.LaunchSequencer();
            }

            return returnActions;
        } 


       public void AddSequencer(Sequencer s)
        {
            listSequencer.Add(s);
        }

    }
    public class Sequencer : Noeud
    {
        List<Action> listAction;
        public void addAction(Action a)
        {
            listAction.Add(a);
        }

        public List<Action> LaunchSequencer()
        {
            List<Action> returnActions = new List<Action>();

            foreach (Action action in listAction)
            {
                if (action.GetState() == State.FAILURE)
                {

                }
            }

            return returnActions;
        }
    }
    public class Action : Noeud
    {
        public AIAction myAIAction;
        public virtual State GetState(PlayerInformations myPlayerInfo)
        {
            return state;
        }
        public virtual AIAction GetAIAction()
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
    public class ActionLook : Action
    {
        /*public override State GetState(PlayerInformations myPlayerInfo)
        {
            if(myPlayerInfo.)
        }*/
    }
}
