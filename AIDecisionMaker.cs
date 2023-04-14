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
        BehaviorTree behavior;
        List<Noeud> listBTAction;


        ActionDash Ad;
        ActionSetTarget Ast;
        ActionMoveToTarget Amtt;
        ActionMoveToWhala Amtw;
        ActionFIRE Af;
        paralleleEnum paraEnum = paralleleEnum.FIRST;
        Parallele para;


        // Ne pas utiliser cette fonction, elle n'est utile que pour le jeu qui vous Set votre Id, si vous voulez votre Id utilisez AIId
        public void SetAIId(int parAIId) { 
            AIId = parAIId;
            

            behavior = new BehaviorTree();
            Ad = new ActionDash();
            Ast = new ActionSetTarget();
            Af = new ActionFIRE();
            Amtt = new ActionMoveToTarget();
            Amtw = new ActionMoveToWhala();
            behavior.IDPlayer = AIId;
            para = new Parallele(paraEnum);

            behavior.root.defaultAction = Ad;

            Sequencer Sequence1 = new Sequencer();
            behavior.root.AddSequencer(Sequence1);


            Sequence1.addAction(Ast);
            Sequence1.addAction(Amtt);



        }

        // Vous pouvez modifier le contenu de cette fonction pour modifier votre nom en jeu
        public string GetName() { return "CIRNO the STRONGEST"; }
        //Same as Initialize
        public void SetAIGameWorldUtils(GameWorldUtils parGameWorldUtils) { 
            AIGameWorldUtils = parGameWorldUtils;
            behavior.myBlackBoard.worldState = parGameWorldUtils;
            
            

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
            listBTAction = new List<Noeud>();




            behavior.root.Launch(GetPlayerInfos(behavior.IDPlayer, AIGameWorldUtils.GetPlayerInfosList()), behavior.myBlackBoard, AIGameWorldUtils.GetPlayerInfosList(), listBTAction);
            behavior.actionToRealize = listBTAction;
            
            actionList = behavior.getAIActions(GetPlayerInfos(behavior.IDPlayer, AIGameWorldUtils.GetPlayerInfosList()), behavior.myBlackBoard, AIGameWorldUtils.GetPlayerInfosList());         
            behavior.actionToRealize.Clear();
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
        public Selector root = new Selector();


        public List<Noeud> actionToRealize = new List<Noeud>();
        public BlackBoard myBlackBoard = new BlackBoard();


        public void getSelectorActions()
        {

          //  actionToRealize = root.GetActions();
        }

        public List<AIAction> getAIActions(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos)
        {
            List<AIAction> listAIActions = new List<AIAction>();
            foreach (var action in actionToRealize)
            {
                if (action.GetAIAction(theBlackBoard, playerInfos) != null)
                {
                    listAIActions.Add(action.GetAIAction(theBlackBoard, playerInfos));
                }
            }
            return listAIActions;
        }
    }
    public class BlackBoard
    {
        public int playerTarget = -1;
        public int potentialTargetID = -1;
        public Vector3 distance = new Vector3();
        public GameWorldUtils worldState;
    }

    public class Noeud 
    {
        public State state = State.NOT_EXECUTED;
        public List<Noeud> listActions = new List<Noeud>();
        public AIAction myAIAction;

        public List<Noeud> GetActions()
        {
            return listActions;
        }

        public virtual State Launch(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos, List<Noeud> listAction)
        {
            //Debug.LogError("Ton cast pu ");
            return this.state;
        }
        public virtual AIAction GetAIAction(BlackBoard theBlackBoard, List<PlayerInformations> playerInfos)
        {
                return myAIAction;
            
        }

    }
    public class Selector : Noeud
    {
        public Noeud defaultAction = new Noeud();
        public List<Noeud> listNoeud = new List<Noeud>();


        public Selector()
        {
            listNoeud = new List<Noeud>();
            state = State.NOT_EXECUTED;
        }

        public Selector(Noeud ActionDefault)
        {
            listNoeud = new List<Noeud>();
            this.defaultAction = ActionDefault;
            state = State.NOT_EXECUTED;
        }

        /// <summary>
        /// lance le selector, qui va check l'état des Sequence et recuperer les actions a effectuer 
        /// </summary>
        /// <param name="myPlayerInfo"></param>
        public override State Launch(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos, List<Noeud> listAction)
        {
            for (int i = 0; i < listNoeud.Count; i++)
            {
               
                listNoeud[i].Launch(myPlayerInfo, theBlackBoard, playerInfos, listAction);
                if (listNoeud[i].state == State.SUCCESS)
                {
                    listActions = listNoeud[i].GetActions();
                    this.state = State.SUCCESS;
                    
                    return this.state;
                } else
                if (listNoeud[i].state == State.RUNNING)
                {
                    this.state = State.RUNNING;
                    return this.state;
                }
                else
                {
                    listActions = new List<Noeud>();
                    listActions.Add(defaultAction);
                    this.state = State.SUCCESS;
                }
                if( i == listNoeud.Count)
                {
                    listActions = new List<Noeud>();
                    listActions.Add(defaultAction);
                    this.state = State.SUCCESS;
                    return this.state;
                }
            }
            return this.state;
        } 

        /// <summary>
        /// Ajoute une sequence a la liste des Sequence 
        /// </summary>
        /// <param name="s"></param>

       public void AddSequencer(Noeud s)
        {
            listNoeud.Add(s);
        }
    }
    public class Sequencer : Noeud
    {
        public List<Noeud> listNoeud = new List<Noeud>();

        public void addNoeud(Noeud a)
        {
            listNoeud.Add(a);
        }

        public override State Launch(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos, List<Noeud> listAction)
        {
            for (int i = 0; i< listNoeud.Count; i++)
            {
                if (listNoeud[i].Launch(myPlayerInfo, theBlackBoard, playerInfos, listAction) == State.FAILURE)
                {
                    listActions = new List<Noeud>();
                    state = State.FAILURE;
                    return this.state;
                }else
                if (listNoeud[i].state == State.RUNNING)
                {
                    this.state = State.RUNNING;
                    return this.state;
                } else
                {
                    listAction.Add(listNoeud[i]);
                    state = State.SUCCESS;
                }
                if (i == listNoeud.Count)
                {
                    this.state = State.SUCCESS;
                }
            }
            return this.state;
        }
    }

    public class ForcedSuccess : Noeud
    {
        List<Noeud> actions = new List<Noeud>();
        public override State Launch(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos, List<Noeud> listAction)
        {
            for (int i = 0; i < actions.Count; i++)
            {
               this.state = actions[i].Launch(myPlayerInfo, theBlackBoard, playerInfos, listAction);
            }
            return State.SUCCESS;
        }
    }

    public class ForcedFailed : Noeud
    {
        List<Noeud> actions = new List<Noeud>();
        public override State Launch(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos, List<Noeud> listAction)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                this.state = actions[i].Launch(myPlayerInfo, theBlackBoard, playerInfos, listAction);
            }
            return State.FAILURE;
        }
    }

   public enum paralleleEnum
    {
        FIRST,
        SECOND,
        BOTH
    }

    public class Parallele : Noeud
    {
        List<Noeud> actions = new List<Noeud>();
        State state1 = new State();
        State state2 = new State();
        paralleleEnum Categorie = new paralleleEnum();
        List<State> tabState = new List<State>();
        
        public void addAction(Noeud a)
        {
            actions.Add(a);
        }

       public Parallele(paralleleEnum p)
        {
            Categorie = p;
        }

        public override State Launch(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos, List<Noeud> listAction)
        {
            this.state1 = actions[0].Launch(myPlayerInfo, theBlackBoard, playerInfos, listAction);
            this.state2 = actions[1].Launch(myPlayerInfo, theBlackBoard, playerInfos, listAction);
           
            return getState(listAction);
        }

        public State getState(List<Noeud> listAction)
        {
            switch (Categorie)
            {
                case paralleleEnum.FIRST:
                    if(state1 == State.SUCCESS)
                    {
                        listAction.Add(actions[0]);
                        return State.SUCCESS;
                    }
                    if(state1 == State.FAILURE)
                    {
                        return State.FAILURE;
                    }
                    

                    break;
                case paralleleEnum.SECOND:
                    if (state2 == State.SUCCESS)
                    {
                        listAction.Add(actions[1]);
                        return State.SUCCESS;
                    }
                    if (state2 == State.FAILURE)
                    {
                        return State.FAILURE;
                    }
                    
                    break;
                case paralleleEnum.BOTH:
                    if (state1 == State.SUCCESS && state2 == State.SUCCESS)
                    {
                        listAction.Add(actions[0]);
                        listAction.Add(actions[1]);
                        return State.SUCCESS;
                    }
                    else
                    {
                        return State.FAILURE;
                    }
                    
                    break;
                default:
                    //Debug.LogError("Cheh");
                    break;
            }
            return State.RUNNING;
        }
    }



 
    public class ActionDash : Noeud
    {
        public new AIActionDash myAIAction = new AIActionDash();
        public override State Launch(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos, List<Noeud> listAction)
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
        public override AIAction GetAIAction(BlackBoard theBlackBoard, List<PlayerInformations> playerInfos)
        {
            myAIAction.Direction = new Vector3(90f, 0, 90f);
            return myAIAction;
        }
    }
    public class ActionSetTarget : Noeud
    {
        public new AIActionLookAtPosition myAIAction = new AIActionLookAtPosition();
        PlayerInformations potentialTarget;
        //Ici on cherche tjr une nouvelle target, mais modifiable pour ne pas changer de target quand on en a une, ou en changer seulement si on a une distance specifique, etc...
        public override State Launch(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos, List<Noeud> listAction)
        {
            potentialTarget = null;
            foreach (PlayerInformations playerInfo in playerInfos)
            {
                if (playerInfo.PlayerId == theBlackBoard.playerTarget)
                {
                    potentialTarget = playerInfo;
                }
            }
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
                    theBlackBoard.playerTarget = playerInfo.PlayerId;
                }
            }

            //Debug.LogError("target id" + theBlackBoard.playerTarget);
            return State.SUCCESS;
        }
        public override AIAction GetAIAction(BlackBoard theBlackBoard, List<PlayerInformations> playerInfos)
        {
            foreach (PlayerInformations playerInfo in playerInfos)
            {
                if (playerInfo.PlayerId == theBlackBoard.potentialTargetID)
                {
                    potentialTarget = playerInfo;
                }
            }
            myAIAction.Position = potentialTarget.Transform.Position;
            theBlackBoard.playerTarget = potentialTarget.PlayerId;
            return myAIAction;
        }
    }


    public class ActionSetLowHealthTarget : Action
    {
        public new AIActionLookAtPosition myAIAction = new AIActionLookAtPosition();
        PlayerInformations potentialTarget = null;
        //Ici on cherche tjr une nouvelle target, mais modifiable pour ne pas changer de target quand on en a une, ou en changer seulement si on a une distance specifique, etc...
        public override State Launch(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos)
        {
           
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
                        else if (playerInfo.CurrentHealth < potentialTarget.CurrentHealth)
                        {
                            potentialTarget = playerInfo;
                            theBlackBoard.playerTarget = playerInfo.PlayerId;
                        }
                    }
                }
            }
            myAIAction.Position = potentialTarget.Transform.Position;
            theBlackBoard.playerTarget = potentialTarget.PlayerId;
            return State.SUCCESS;
        }
    }
    public class ActionMoveToTarget : Noeud

    {
        public new AIActionMoveToDestination myAIAction = new AIActionMoveToDestination();
        PlayerInformations target = null;
        public override State Launch(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos, List<Noeud> listAction)
        {
            //Debug.LogError("target id" + theBlackBoard.playerTarget);
            foreach (PlayerInformations playerInfo in playerInfos)
            {
                if (playerInfo.PlayerId == theBlackBoard.playerTarget)
                {
                    target = playerInfo;
                }
            }
            if (theBlackBoard.playerTarget != -1 && target.IsActive)
            {
                return State.SUCCESS;
            }
            else
            {
                return State.FAILURE;
            }
        }
        public override AIAction GetAIAction(BlackBoard theBlackBoard, List<PlayerInformations> playerInfos)
        {
            myAIAction.Position = target.Transform.Position;
            return myAIAction;
        }
    }


    public class ActionMoveToWhala : Noeud
    {
        public new AIActionMoveToDestination myAIAction = new AIActionMoveToDestination();
        PlayerInformations target = null;
        public override State Launch(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos, List<Noeud> listAction)
        {
            
            if(Vector3.Distance(myPlayerInfo.Transform.Position , myAIAction.Position)< 1.5)
            {
                
                return State.RUNNING;
            }
            return State.SUCCESS;
        }
        public override AIAction GetAIAction(BlackBoard theBlackBoard, List<PlayerInformations> playerInfos)
        {
            myAIAction.Position = new Vector3(0,0,0) ;
            return myAIAction;
        }
    }

    public class ActionFIRE : Noeud
    {
        public new AIActionFire myAIAction = new AIActionFire();
        public override State Launch(PlayerInformations myPlayerInfo, BlackBoard theBlackBoard, List<PlayerInformations> playerInfos, List<Noeud> listAction)
        {
          return State.SUCCESS;
        }
        public override AIAction GetAIAction(BlackBoard theBlackBoard, List<PlayerInformations> playerInfos)
        {
            return myAIAction;
        }
    }


    
}
