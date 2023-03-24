using System;
using System.Collections;
using System.Collections.Generic;
using TD.Entities;
using TD.Entities.Enemies;
using TD.Entities.Towers;
using TD.Entities.Towers.States;
using TMPro.EditorUtilities;
using UnityEngine;


namespace TD.EnemiesManager.EachEnemyIncomingAttackersLister
{
    [Serializable]
    public class IncomingAttackerData
    {
        public Transform attacker;
        public float timeInWaitingList = 0;
        public TargetedBehaviour targetedBehaviourOfTarget;

        public IncomingAttackerData(Transform _attacker, TargetedBehaviour _targetedBehaviourOfTarget)
        {
            attacker = _attacker;
            targetedBehaviourOfTarget = _targetedBehaviourOfTarget;
        }

        public IncomingAttackerData(Transform _attacker)
        {
            attacker = _attacker;
        }


        public void ResetTimeInWaitingList()
        {
            timeInWaitingList = 0;
        }

        public void UpdateTarget(TargetedBehaviour newTarget)
        {
            targetedBehaviourOfTarget = newTarget;
        }


    }


    public class EachEnemyIncomingAttackersLister : MonoBehaviour
    {

        public List<IncomingAttackerData> DataOfAllIncomingAttackers;
        private float maxTimeInWaitingList = 0.2f;

        private void OnEnable()
        {
            LockTargetState.OnTargetLock += UpdateListOfIncomingAttackersOfTargetOnTargetLock;
            ListOfTargets.OnTargetSwitch += UpdateListOfIncomingAttackersOfTargetOnTargetSwitch;
            StationaryState.OnTargetReset += UpdateListOfIncomingAttackersOfTargetOnTargetReset;

        }

        private void OnDisable()
        {
            LockTargetState.OnTargetLock -= UpdateListOfIncomingAttackersOfTargetOnTargetLock;
            ListOfTargets.OnTargetSwitch -= UpdateListOfIncomingAttackersOfTargetOnTargetSwitch;
            StationaryState.OnTargetReset -= UpdateListOfIncomingAttackersOfTargetOnTargetReset;
        }

        private void Update()
        {
            foreach(var dataOfIncomingAttacker in DataOfAllIncomingAttackers.ToArray())
            {
                dataOfIncomingAttacker.timeInWaitingList += Time.deltaTime;

                if (dataOfIncomingAttacker.timeInWaitingList >= maxTimeInWaitingList)
                {
                    if (Find(dataOfIncomingAttacker.attacker, dataOfIncomingAttacker.targetedBehaviourOfTarget.WaitingIncomingAttackers) != null)
                    {
                        if (Find(dataOfIncomingAttacker.attacker, dataOfIncomingAttacker.targetedBehaviourOfTarget.IncomingAttackers) == null)
                        {
                            dataOfIncomingAttacker.targetedBehaviourOfTarget.IncomingAttackers.Add(dataOfIncomingAttacker.attacker);
                            dataOfIncomingAttacker.targetedBehaviourOfTarget.WaitingIncomingAttackers.Remove(dataOfIncomingAttacker.attacker);
                        }
                    }

                }
                else
                {
                    if (Find(dataOfIncomingAttacker.attacker, dataOfIncomingAttacker.targetedBehaviourOfTarget.WaitingIncomingAttackers) != null)
                    {
                        if (Find(dataOfIncomingAttacker.attacker, dataOfIncomingAttacker.targetedBehaviourOfTarget.IncomingAttackers) == null)
                        {
                            TowerStateSwitcher towerStateSwitcher = dataOfIncomingAttacker.attacker.GetComponent<TowerStateSwitcher>();

                            if (towerStateSwitcher.CurrentTowerState == TowerState.Attacking)
                            {
                                dataOfIncomingAttacker.timeInWaitingList = maxTimeInWaitingList;
                            }
                        }
                    }
                }
            }
        }


        public void UpdateListOfIncomingAttackersOfTargetOnTargetLock(Transform targetLocked, Transform attacker)
        {
            if (targetLocked == null)
                return;

            TargetedBehaviour targetedBehaviour = targetLocked.GetComponent<TargetedBehaviour>();
            if (Find(attacker, targetedBehaviour.WaitingIncomingAttackers) != null)
                return;

           
            if(GetDataOfIncomingAttacker(attacker) != null)
            {
                GetDataOfIncomingAttacker(attacker).ResetTimeInWaitingList();
                GetDataOfIncomingAttacker(attacker).UpdateTarget(targetedBehaviour);
                targetedBehaviour.WaitingIncomingAttackers.Add(GetDataOfIncomingAttacker(attacker).attacker);               
            }
            else
            {
                IncomingAttackerData incomingAttackerData = new IncomingAttackerData(attacker, targetedBehaviour);
                DataOfAllIncomingAttackers.Add(incomingAttackerData);
                targetedBehaviour.WaitingIncomingAttackers.Add(incomingAttackerData.attacker);
            }

            
            //Add(attacker, targetedBehaviour.IncomingAttackers);
            //AddToWaitingList(new IncomingAttackerData(attacker, targetedBehaviour.WaitingIncomingAttackers, targetedBehaviour.IncomingAttackers));
        }

        public void UpdateListOfIncomingAttackersOfTargetOnTargetSwitch(Transform previousTarget, Transform newTarget, Transform attacker)
        {
            
            if (previousTarget == null && newTarget == null)
                return;
            
            

            if(previousTarget != null && newTarget == null)
            {
               
                TargetedBehaviour targetedBehaviourPreviousTarget = previousTarget.GetComponent<TargetedBehaviour>();
                if (targetedBehaviourPreviousTarget != null)
                {
                    if(GetDataOfIncomingAttacker(attacker) != null)
                    {
                        GetDataOfIncomingAttacker(attacker).ResetTimeInWaitingList();
                    }

                    else if(GetDataOfIncomingAttacker(attacker) == null)
                    {
                        IncomingAttackerData incomingAttackerData = new IncomingAttackerData(attacker);
                        DataOfAllIncomingAttackers.Add(incomingAttackerData);
                    }

                    if (Find(attacker, targetedBehaviourPreviousTarget.WaitingIncomingAttackers) != null)
                    {
                        Remove(attacker, targetedBehaviourPreviousTarget.WaitingIncomingAttackers);                       
                    }

                    if (Find(attacker, targetedBehaviourPreviousTarget.IncomingAttackers) != null)
                    {
                        Remove(attacker, targetedBehaviourPreviousTarget.IncomingAttackers);
                    }
                    
                }
            }

            else if(previousTarget == null && newTarget != null)
            {
         
                TargetedBehaviour targetedBehaviourNewTarget = newTarget.GetComponent<TargetedBehaviour>();
                if (targetedBehaviourNewTarget != null)
                {
                    if (GetDataOfIncomingAttacker(attacker) != null)
                    {
                        GetDataOfIncomingAttacker(attacker).ResetTimeInWaitingList();
                        GetDataOfIncomingAttacker(attacker).UpdateTarget(targetedBehaviourNewTarget);
                    }

                    else if(GetDataOfIncomingAttacker(attacker) == null)
                    {
                        IncomingAttackerData incomingAttackerData = new IncomingAttackerData(attacker, targetedBehaviourNewTarget);
                        DataOfAllIncomingAttackers.Add(incomingAttackerData);
                    }

                    if (Find(attacker, targetedBehaviourNewTarget.WaitingIncomingAttackers) == null)
                    {
                        Add(attacker, targetedBehaviourNewTarget.WaitingIncomingAttackers);
                    }
                }
            }

            else if(previousTarget != null && newTarget != null)
            {
                

                if (GetDataOfIncomingAttacker(attacker) != null)
                {
                    GetDataOfIncomingAttacker(attacker).ResetTimeInWaitingList();
                }


                TargetedBehaviour targetedBehaviourPreviousTarget = previousTarget.GetComponent<TargetedBehaviour>();
                if (targetedBehaviourPreviousTarget != null)
                {
              
                    if (GetDataOfIncomingAttacker(attacker) == null)
                    {
                        IncomingAttackerData incomingAttackerData = new IncomingAttackerData(attacker, targetedBehaviourPreviousTarget);
                        DataOfAllIncomingAttackers.Add(incomingAttackerData);
                    }

                   

                    if (Find(attacker, targetedBehaviourPreviousTarget.WaitingIncomingAttackers) != null)
                    {
                        Remove(attacker, targetedBehaviourPreviousTarget.WaitingIncomingAttackers);

                    }


                    if (Find(attacker, targetedBehaviourPreviousTarget.IncomingAttackers) != null)
                    {
                        Remove(attacker, targetedBehaviourPreviousTarget.IncomingAttackers);
                    }

                }


                TargetedBehaviour targetedBehaviourNewTarget = newTarget.GetComponent<TargetedBehaviour>();
                if (targetedBehaviourNewTarget != null)
                {
                  
                    if (GetDataOfIncomingAttacker(attacker) == null)
                    {
                        IncomingAttackerData incomingAttackerData = new IncomingAttackerData(attacker, targetedBehaviourNewTarget);
                        DataOfAllIncomingAttackers.Add(incomingAttackerData);
                    }

                    else
                    {
                        GetDataOfIncomingAttacker(attacker).UpdateTarget(targetedBehaviourNewTarget);
                    }

                    if (Find(attacker, targetedBehaviourNewTarget.WaitingIncomingAttackers) == null)
                    {
                        Add(attacker, targetedBehaviourNewTarget.WaitingIncomingAttackers);
                    }
                }
            }
            
                     
        }

        public void UpdateListOfIncomingAttackersOfTargetOnTargetReset(Transform targetReseted, Transform attacker)
        {
            TargetedBehaviour targetedBehaviour = targetReseted.GetComponent<TargetedBehaviour>();
            if (targetReseted == null)
                return;

         
            if (GetDataOfIncomingAttacker(attacker) != null)
            {
                GetDataOfIncomingAttacker(attacker).ResetTimeInWaitingList();
            }

            else if (GetDataOfIncomingAttacker(attacker) == null)
            {
                IncomingAttackerData incomingAttackerData = new IncomingAttackerData(attacker, targetedBehaviour);
                DataOfAllIncomingAttackers.Add(incomingAttackerData);
            }


           /* if (Find(attacker, targetedBehaviour.WaitingIncomingAttackers) != null)
            {
                Remove(attacker, targetedBehaviour.WaitingIncomingAttackers);
            }

            if (Find(attacker, targetedBehaviour.IncomingAttackers) != null)
            {
                Remove(attacker, targetedBehaviour.IncomingAttackers);

               
            }*/


        }

        public void Add(Transform attacker, List<Transform> enemyIncomingAttackersList)
        {
            enemyIncomingAttackersList.Add(attacker);
        }

        public void Remove(Transform attacker, List<Transform> enemyIncomingAttackersList)
        {
            enemyIncomingAttackersList.Remove(attacker);
        }

        /*public void RemoveFromWaitingList(IncomingAttackerData data)
        {
            data.waitingList.Remove(data.attacker);
        }

        public void AddToWaitingList(IncomingAttackerData data)
        {
            data.waitingList.Add(data.attacker);
        }*/

        public Transform Find(Transform attacker, List<Transform> enemyIncomingAttackersList)
        {
            foreach(var incomingAttacker in enemyIncomingAttackersList)
            {
                if (incomingAttacker != attacker)
                    continue;

                return attacker;
            }

            return null;
        }

        public IncomingAttackerData GetDataOfIncomingAttacker(Transform attacker)
        {
            foreach(var dataOfIncomingAttacker in DataOfAllIncomingAttackers)
            {
                if (dataOfIncomingAttacker.attacker != attacker)
                    continue;

                return dataOfIncomingAttacker;
            }

            return null;
        }



        
    }
}