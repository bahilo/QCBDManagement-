using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Structures;
using QCBDManagementGateway.QCBDServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using QCBDManagementCommon.Classes;
using System.Threading.Tasks;

namespace QCBDManagementGateway.Helper.ChannelHelper
{
    public static class ServiceHelper
    {
        
        //====================================================================================
        //===============================[ Agent ]===========================================
        //====================================================================================

        public static List<Agent> ArrayTypeToAgent(this AgentQCBDManagement[] agentQCBDManagementList)
        {
            object _lock = new object(); List<Agent> returnList = new List<Agent>();
            if (agentQCBDManagementList != null)
            {
                Parallel.ForEach(agentQCBDManagementList, (agentQCBD) =>
                {
                    Agent agent = new Agent();
                    agent.ID = agentQCBD.ID;
                    agent.FirstName = Utility.decodeBase64ToString(agentQCBD.FirstName);
                    agent.LastName = Utility.decodeBase64ToString(agentQCBD.LastName);
                    agent.Login = Utility.decodeBase64ToString(agentQCBD.Login);
                    agent.HashedPassword = Utility.decodeBase64ToString(agentQCBD.Password);
                    agent.Phone = Utility.decodeBase64ToString(agentQCBD.Phone);
                    agent.Status = Utility.decodeBase64ToString(agentQCBD.Status);
                    agent.ListSize = agentQCBD.ListSize;
                    agent.Email = Utility.decodeBase64ToString(agentQCBD.Email);
                    agent.Fax = Utility.decodeBase64ToString(agentQCBD.Fax);
                    agent.RoleList = agentQCBD.Roles.ArrayTypeToRole();

                    lock (_lock) returnList.Add(agent);
                });
            }
            return returnList;
        }

        public static AgentQCBDManagement[] AgentTypeToArray(this List<Agent> agentList)
        {
            int i = 0;
            AgentQCBDManagement[] returnQCBDArray = new AgentQCBDManagement[agentList.Count];
            if (agentList != null)
            {
                Parallel.ForEach(agentList, (agent) =>
                {
                    AgentQCBDManagement agentQCBD = new AgentQCBDManagement();
                    agentQCBD.ID = agent.ID;
                    agentQCBD.FirstName = Utility.encodeStringToBase64(agent.FirstName);
                    agentQCBD.LastName = Utility.encodeStringToBase64(agent.LastName);
                    agentQCBD.Login = Utility.encodeStringToBase64(agent.Login);
                    agentQCBD.Password = Utility.encodeStringToBase64(agent.HashedPassword);
                    agentQCBD.Phone = Utility.encodeStringToBase64(agent.Phone);
                    agentQCBD.Status = Utility.encodeStringToBase64(agent.Status);
                    agentQCBD.ListSize = agent.ListSize;
                    agentQCBD.Email = Utility.encodeStringToBase64(agent.Email);
                    agentQCBD.Fax = Utility.encodeStringToBase64(agent.Fax);

                    returnQCBDArray[i] = agentQCBD;
                    i++;
                });
            }
            return returnQCBDArray;
        }

        public static AgentFilterQCBDManagement AgentTypeToFilterArray(this Agent agent, string filterOperator)
        {
            AgentFilterQCBDManagement agentQCBD = new AgentFilterQCBDManagement();
            if (agent != null)
            {
                agentQCBD.ID = agent.ID;
                agentQCBD.FirstName = Utility.encodeStringToBase64(agent.FirstName);
                agentQCBD.LastName = Utility.encodeStringToBase64(agent.LastName);
                agentQCBD.Login = Utility.encodeStringToBase64(agent.Login);
                agentQCBD.Password = Utility.encodeStringToBase64(agent.HashedPassword);
                agentQCBD.Phone = Utility.encodeStringToBase64(agent.Phone);
                agentQCBD.Status = Utility.encodeStringToBase64(agent.Status);
                agentQCBD.ListSize = agent.ListSize;
                agentQCBD.Email = Utility.encodeStringToBase64(agent.Email);
                agentQCBD.Fax = Utility.encodeStringToBase64(agent.Fax);
                agentQCBD.Operator = filterOperator;
            }
            return agentQCBD;
        }

        //====================================================================================
        //===============================[ Statistic ]===========================================
        //====================================================================================

        public static List<Statistic> ArrayTypeToStatistic(this StatisticQCBDManagement[] statisticQCBDManagementList)
        {
            object _lock = new object(); List<Statistic> returnList = new List<Statistic>();
            if (statisticQCBDManagementList != null)
            {
                //Parallel.ForEach(statisticQCBDManagementList, (statisticQCBD) =>
                foreach(var statisticQCBD in statisticQCBDManagementList)
                {
                    Statistic statistic = new Statistic();
                    statistic.ID = statisticQCBD.ID;
                    int billId = 0;
                    int.TryParse(Utility.decodeBase64ToString(statisticQCBD.BillId), out billId);
                    statistic.BillId = billId;
                    statistic.Bill_date = Utility.convertToDateTime(Utility.decodeBase64ToString(statisticQCBD.Bill_date));
                    statistic.Company = Utility.decodeBase64ToString(statisticQCBD.Company);
                    statistic.Date_limit = Utility.convertToDateTime(Utility.decodeBase64ToString(statisticQCBD.Date_limit));
                    decimal income = 0;
                    decimal.TryParse(Utility.decodeBase64ToString(statisticQCBD.Income), out income);
                    statistic.Income = income;
                    double incomePercentage;
                    double.TryParse(Utility.decodeBase64ToString(statisticQCBD.Income_percent).Replace("%",""), out incomePercentage);
                    statistic.Income_percent = incomePercentage;
                    statistic.Pay_date = Utility.convertToDateTime(Utility.decodeBase64ToString(statisticQCBD.Pay_date));
                    decimal payReceived = 0;
                    decimal.TryParse(Utility.decodeBase64ToString(statisticQCBD.Pay_received).Split(new char[] { ' ' }).FirstOrDefault(), out payReceived);
                    statistic.Pay_received = payReceived;
                    decimal pricePurchaseTotal = 0;
                    decimal.TryParse(Utility.decodeBase64ToString(statisticQCBD.Price_purchase_total), out pricePurchaseTotal);
                    statistic.Price_purchase_total = pricePurchaseTotal;
                    statistic.Tax_value = statisticQCBD.Tax_value;
                    decimal total = 0;
                    decimal.TryParse(Utility.decodeBase64ToString(statisticQCBD.Total), out total);
                    statistic.Total = total;
                    decimal totalTaxIncluded;
                    decimal.TryParse(Utility.decodeBase64ToString(statisticQCBD.Total_tax_included), out totalTaxIncluded);
                    statistic.Total_tax_included = totalTaxIncluded;

                    lock (_lock) returnList.Add(statistic);
                }
                //);
            }
            return returnList;
        }

        public static StatisticQCBDManagement[] StatisticTypeToArray(this List<Statistic> statisticList)
        {
            int i = 0;
            StatisticQCBDManagement[] returnQCBDArray = new StatisticQCBDManagement[statisticList.Count];
            if (statisticList != null)
            {
                Parallel.ForEach(statisticList, (statistic) =>
                {
                    StatisticQCBDManagement statisticQCBD = new StatisticQCBDManagement();
                    statisticQCBD.ID = statistic.ID;
                    statisticQCBD.BillId = Utility.encodeStringToBase64(statistic.BillId.ToString());
                    statisticQCBD.Bill_date = Utility.encodeStringToBase64(statistic.Bill_date.ToString());
                    statisticQCBD.Company = Utility.encodeStringToBase64(statistic.Company);
                    statisticQCBD.Date_limit = Utility.encodeStringToBase64(statistic.Date_limit.ToString());
                    statisticQCBD.Income = Utility.encodeStringToBase64(statistic.Income.ToString());
                    statisticQCBD.Income_percent = Utility.encodeStringToBase64(statistic.Income_percent.ToString());
                    statisticQCBD.Pay_date = Utility.encodeStringToBase64(statistic.Pay_date.ToString());
                    statisticQCBD.Pay_received = Utility.encodeStringToBase64(statistic.Pay_received.ToString());
                    statisticQCBD.Price_purchase_total = Utility.encodeStringToBase64(statistic.Price_purchase_total.ToString());
                    statisticQCBD.Tax_value = statistic.Tax_value;
                    statisticQCBD.Total = Utility.encodeStringToBase64(statistic.Total.ToString());
                    statisticQCBD.Total_tax_included = Utility.encodeStringToBase64(statistic.Total_tax_included.ToString());

                    returnQCBDArray[i] = statisticQCBD;
                    i++;
                });
            }
            return returnQCBDArray;
        }

        public static StatisticFilterQCBDManagement StatisticTypeToFilterArray(this Statistic statistic, string filterOperator)
        {
            StatisticFilterQCBDManagement statisticQCBD = new StatisticFilterQCBDManagement();
            if (statistic != null)
            {
                statisticQCBD.ID = statistic.ID;
                statisticQCBD.Option = statistic.Option;
                statisticQCBD.BillId = Utility.encodeStringToBase64(statistic.BillId.ToString());
                statisticQCBD.Bill_date = Utility.encodeStringToBase64(statistic.Bill_date.ToString());
                statisticQCBD.Company = Utility.encodeStringToBase64(statistic.Company);
                statisticQCBD.Date_limit = Utility.encodeStringToBase64(statistic.Date_limit.ToString());
                statisticQCBD.Income = Utility.encodeStringToBase64(statistic.Income.ToString());
                statisticQCBD.Income_percent = Utility.encodeStringToBase64(statistic.Income_percent.ToString());
                statisticQCBD.Pay_date = Utility.encodeStringToBase64(statistic.Pay_date.ToString());
                statisticQCBD.Pay_received = Utility.encodeStringToBase64(statistic.Pay_received.ToString());
                statisticQCBD.Price_purchase_total = Utility.encodeStringToBase64(statistic.Price_purchase_total.ToString());
                statisticQCBD.Tax_value = statistic.Tax_value;
                statisticQCBD.Total = Utility.encodeStringToBase64(statistic.Total.ToString());
                statisticQCBD.Total_tax_included = Utility.encodeStringToBase64(statistic.Total_tax_included.ToString());
                statisticQCBD.Operator = filterOperator;
            }
            return statisticQCBD;
        }


        //====================================================================================
        //===============================[ Infos ]===========================================
        //====================================================================================

        public static List<Infos> ArrayTypeToInfos(this InfosQCBDManagement[] infosQCBDManagementList)
        {
            object _lock = new object(); List<Infos> returnList = new List<Infos>();
            if (infosQCBDManagementList != null)
            {
                Parallel.ForEach(infosQCBDManagementList, (infosQCBD) =>
                {
                    Infos infos = new Infos();
                    infos.ID = infosQCBD.ID;
                    infos.Name = Utility.decodeBase64ToString(infosQCBD.Name);
                    infos.Value = Utility.decodeBase64ToString(infosQCBD.Value);
                    lock (_lock) returnList.Add(infos);
                });
            }
            return returnList;
        }

        public static InfosQCBDManagement[] InfosTypeToArray(this List<Infos> infosList)
        {
            int i = 0;
            InfosQCBDManagement[] returnQCBDArray = new InfosQCBDManagement[infosList.Count];
            if (infosList != null)
            {
                Parallel.ForEach(infosList, (infos) =>
                {
                    if (infos != null)
                    {
                        InfosQCBDManagement infosQCBD = new InfosQCBDManagement();
                        infosQCBD.ID = infos.ID;
                        infosQCBD.Name = Utility.encodeStringToBase64(infos.Name);
                        infosQCBD.Value = Utility.encodeStringToBase64(infos.Value);
                        returnQCBDArray[i] = infosQCBD;
                        i++;
                    }

                });
            }
            return returnQCBDArray;
        }

        public static InfosFilterQCBDManagement InfosTypeToFilterArray(this Infos infos, string filterOperator)
        {
            InfosFilterQCBDManagement infosQCBD = new InfosFilterQCBDManagement();
            if (infos != null)
            {
                infosQCBD.ID = infos.ID;
                infosQCBD.Option = infos.Option;
                infosQCBD.Name = Utility.encodeStringToBase64(infos.Name);
                infosQCBD.Value = Utility.encodeStringToBase64(infos.Value);
                infosQCBD.Operator = Utility.encodeStringToBase64(filterOperator);
            }
            return infosQCBD;
        }
        
        //====================================================================================
        //===============================[ ActionRecord ]===========================================
        //====================================================================================

        public static List<ActionRecord> ArrayTypeToActionRecord(this ActionRecordQCBDManagement[] actionRecordQCBDManagementList)
        {
            object _lock = new object(); List<ActionRecord> returnList = new List<ActionRecord>();
            if (actionRecordQCBDManagementList != null)
            {
                Parallel.ForEach(actionRecordQCBDManagementList, (actionRecordQCBD) =>
                {
                    ActionRecord actionRecord = new ActionRecord();
                    actionRecord.ID = actionRecordQCBD.ID;
                    actionRecord.AgentId = actionRecordQCBD.AgentId;
                    actionRecord.Action = Utility.decodeBase64ToString(actionRecordQCBD.Action);
                    actionRecord.TargetId = actionRecordQCBD.TargetId;
                    actionRecord.TargetName = Utility.decodeBase64ToString(actionRecordQCBD.TargetName);
                    actionRecord.Date = Utility.convertToDateTime(Utility.decodeBase64ToString(actionRecordQCBD.Date));

                    lock (_lock) returnList.Add(actionRecord);
                });
            }
            return returnList;
        }

        public static ActionRecordQCBDManagement[] ActionRecordTypeToArray(this List<ActionRecord> actionRecordList)
        {
            int i = 0;
            ActionRecordQCBDManagement[] returnQCBDArray = new ActionRecordQCBDManagement[actionRecordList.Count];
            if (actionRecordList != null)
            {
                Parallel.ForEach(actionRecordList, (actionRecord) =>
                {
                    ActionRecordQCBDManagement actionRecordQCBD = new ActionRecordQCBDManagement();
                    actionRecordQCBD.ID = actionRecord.ID;
                    actionRecordQCBD.AgentId = actionRecord.AgentId;
                    actionRecordQCBD.Action = Utility.encodeStringToBase64(actionRecord.Action);
                    actionRecordQCBD.TargetId = actionRecord.TargetId;
                    actionRecordQCBD.TargetName = Utility.encodeStringToBase64(actionRecord.TargetName);
                    actionRecordQCBD.Date = Utility.encodeStringToBase64(actionRecord.Date.ToString());

                    returnQCBDArray[i] = actionRecordQCBD;
                    i++;
                });
            }
            return returnQCBDArray;
        }

        public static ActionRecordFilterQCBDManagement ActionRecordTypeToFilterArray(this ActionRecord actionRecord, string filterOperator)
        {
            ActionRecordFilterQCBDManagement actionRecordQCBD = new ActionRecordFilterQCBDManagement();
            if (actionRecord != null)
            {
                actionRecordQCBD.ID = actionRecord.ID;
                actionRecordQCBD.AgentId = actionRecord.AgentId;
                actionRecordQCBD.Action = Utility.encodeStringToBase64(actionRecord.Action);
                actionRecordQCBD.TargetId = actionRecord.TargetId;
                actionRecordQCBD.TargetName = Utility.encodeStringToBase64(actionRecord.TargetName);
                actionRecordQCBD.Date = Utility.encodeStringToBase64(actionRecord.Date.ToString());
                actionRecordQCBD.Operator = filterOperator;
            }
            return actionRecordQCBD;
        }

        //====================================================================================
        //===============================[ Role ]===========================================
        //====================================================================================
        
        public static List<Role> ArrayTypeToRole(this RoleQCBDManagement[] roleQCBDManagementList)
        {
            object _lock = new object(); List<Role> returnList = new List<Role>();
            if (roleQCBDManagementList != null)
            {
                //Parallel.ForEach(roleQCBDManagementList, (roleQCBD) =>
                foreach(var roleQCBD in roleQCBDManagementList)
                {
                    Role role = new Role();
                    role.ID = roleQCBD.ID;
                    role.Name = Utility.decodeBase64ToString(roleQCBD.Name);
                    role.ActionList = roleQCBD.Actions.ArrayTypeToAction();

                    lock (_lock) returnList.Add(role);
                }
                //);
            }
            return returnList;
        }

        public static RoleQCBDManagement[] RoleTypeToArray(this List<Role> roleList)
        {
            int i = 0;
            object _lock = new object();
            RoleQCBDManagement[] returnList = new RoleQCBDManagement[roleList.Count];
            if (roleList != null)
            {
                Parallel.ForEach(roleList, (role) =>
                {
                    RoleQCBDManagement roleQCBD = new RoleQCBDManagement();
                    roleQCBD.ID = role.ID;
                    roleQCBD.Name = Utility.encodeStringToBase64(role.Name);
                    roleQCBD.Actions = role.ActionList.ActionTypeToArray();

                    lock (_lock) returnList[i] = roleQCBD;
                    i++;
                });
            }
            return returnList;
        }

        public static RoleFilterQCBDManagement RoleTypeToFilterArray(this Role role, string filterOperator)
        {
            RoleFilterQCBDManagement roleQCBD = new RoleFilterQCBDManagement();
            if (role != null)
            {
                roleQCBD.ID = role.ID;
                roleQCBD.Name = Utility.encodeStringToBase64(role.Name);
                roleQCBD.Operator = filterOperator;
            }
            return roleQCBD;
        }



        //====================================================================================
        //===============================[ Role_action ]===========================================
        //====================================================================================

        public static List<Role_action> ArrayTypeToRole_action(this Role_actionQCBDManagement[] role_actionQCBDManagementList)
        {
            object _lock = new object(); List<Role_action> returnList = new List<Role_action>();
            if (role_actionQCBDManagementList != null)
            {
                Parallel.ForEach(role_actionQCBDManagementList, (role_actionQCBD) =>
                {
                    Role_action role_action = new Role_action();
                    role_action.ID = role_actionQCBD.ID;
                    role_action.ActionId = role_actionQCBD.ActionId;
                    role_action.RoleId = role_actionQCBD.RoleId;

                    lock (_lock) returnList.Add(role_action);
                });
            }
            return returnList;
        }

        public static Role_actionQCBDManagement[] Role_actionTypeToArray(this List<Role_action> role_actionList)
        {
            int i = 0;
            Role_actionQCBDManagement[] returnQCBDArray = new Role_actionQCBDManagement[role_actionList.Count];
            if (role_actionList != null)
            {
                Parallel.ForEach(role_actionList, (role_action) =>
                {
                    Role_actionQCBDManagement role_actionQCBD = new Role_actionQCBDManagement();
                    role_actionQCBD.ID = role_action.ID;
                    role_actionQCBD.ActionId = role_action.ActionId;
                    role_actionQCBD.RoleId = role_action.RoleId;

                    returnQCBDArray[i] = role_actionQCBD;
                    i++;
                });
            }
            return returnQCBDArray;
        }

        public static Role_actionFilterQCBDManagement Role_actionTypeToFilterArray(this Role_action role_action, string filterOperator)
        {
            Role_actionFilterQCBDManagement role_actionQCBD = new Role_actionFilterQCBDManagement();
            if (role_action != null)
            {
                role_actionQCBD.ID = role_action.ID;
                role_actionQCBD.ActionId = role_action.ActionId;
                role_actionQCBD.RoleId = role_action.RoleId;
                role_actionQCBD.Operator = filterOperator;
            }
            return role_actionQCBD;
        }


        //====================================================================================
        //===============================[ SecurityAction ]===========================================
        //====================================================================================

        public static List<QCBDManagementCommon.Entities.Action> ArrayTypeToAction(this ActionQCBDManagement[] actionQCBDManagementList)
        {
            object _lock = new object(); List<QCBDManagementCommon.Entities.Action> returnList = new List<QCBDManagementCommon.Entities.Action>();
            if (actionQCBDManagementList != null)
            {
                //Parallel.ForEach(actionQCBDManagementList, (actionQCBD) =>
                foreach(var actionQCBD  in actionQCBDManagementList)
                {
                    QCBDManagementCommon.Entities.Action action = new QCBDManagementCommon.Entities.Action();
                    action.ID = actionQCBD.ID;
                    action.Name = Utility.decodeBase64ToString(actionQCBD.Name);
                    var privilegeFoundList = new PrivilegeQCBDManagement[] { actionQCBD.Right }.ArrayTypeToPrivilege();
                    action.Right = (privilegeFoundList.Count > 0) ? privilegeFoundList.First() : new Privilege();

                    lock (_lock) returnList.Add(action);
                }
                //);
            }
            return returnList;
        }

        public static ActionQCBDManagement[] ActionTypeToArray(this List<QCBDManagementCommon.Entities.Action> actionList)
        {
            int i = 0;
            object _lock = new object();
            ActionQCBDManagement[] returnList = new ActionQCBDManagement[actionList.Count];
            if (actionList != null)
            {
                Parallel.ForEach(actionList, (action) =>
                {
                    ActionQCBDManagement actionQCBD = new ActionQCBDManagement();
                    actionQCBD.ID = action.ID;
                    actionQCBD.Name = Utility.encodeStringToBase64(action.Name);
                    actionQCBD.Right = (new List<Privilege> { { action.Right } }.PrivilegeTypeToArray()).FirstOrDefault();

                    lock (_lock) returnList[i] = actionQCBD;
                    i++;
                });
            }
            return returnList;
        }

        public static ActionFilterQCBDManagement ActionTypeToFilterArray(this QCBDManagementCommon.Entities.Action action, string filterOperator)
        {
            ActionFilterQCBDManagement actionQCBD = new ActionFilterQCBDManagement();
            if (action != null)
            {
                actionQCBD.ID = action.ID;
                actionQCBD.Name = Utility.encodeStringToBase64(action.Name);
                actionQCBD.Operator = filterOperator;
            }
            return actionQCBD;
        }


        //====================================================================================
        //===============================[ Agent_role ]===========================================
        //====================================================================================

        public static List<Agent_role> ArrayTypeToAgent_role(this Agent_roleQCBDManagement[] agent_roleQCBDManagementList)
        {
            object _lock = new object(); List<Agent_role> returnList = new List<Agent_role>();
            if (agent_roleQCBDManagementList != null)
            {
                Parallel.ForEach(agent_roleQCBDManagementList, (agent_roleQCBD) =>
                {
                    Agent_role agent_role = new Agent_role();
                    agent_role.ID = agent_roleQCBD.ID;
                    agent_role.AgentId = agent_roleQCBD.AgentId;
                    agent_role.RoleId = agent_roleQCBD.RoleId;
                    agent_role.Date = Utility.convertToDateTime(Utility.decodeBase64ToString(agent_roleQCBD.Date));

                    lock (_lock) returnList.Add(agent_role);
                });
            }
            return returnList;
        }

        public static Agent_roleQCBDManagement[] Agent_roleTypeToArray(this List<Agent_role> agent_roleList)
        {
            int i = 0;
            Agent_roleQCBDManagement[] returnQCBDArray = new Agent_roleQCBDManagement[agent_roleList.Count];
            if (agent_roleList != null)
            {
                Parallel.ForEach(agent_roleList, (agent_role) =>
                {
                    Agent_roleQCBDManagement agent_roleQCBD = new Agent_roleQCBDManagement();
                    agent_roleQCBD.ID = agent_role.ID;
                    agent_roleQCBD.AgentId = agent_role.AgentId;
                    agent_roleQCBD.RoleId = agent_role.RoleId;
                    agent_roleQCBD.Date = Utility.encodeStringToBase64(agent_role.Date.ToString());

                    returnQCBDArray[i] = agent_roleQCBD;
                    i++;
                });
            }
            return returnQCBDArray;
        }

        public static Agent_roleFilterQCBDManagement Agent_roleTypeToFilterArray(this Agent_role agent_role, string filterOperator)
        {
            Agent_roleFilterQCBDManagement agent_roleQCBD = new Agent_roleFilterQCBDManagement();
            if (agent_role != null)
            {
                agent_roleQCBD.ID = agent_role.ID;
                agent_roleQCBD.AgentId = agent_role.AgentId;
                agent_roleQCBD.RoleId = agent_role.RoleId;
                agent_roleQCBD.Date = Utility.encodeStringToBase64(agent_role.Date.ToString());
                agent_roleQCBD.Operator = filterOperator;
            }
            return agent_roleQCBD;
        }

        //====================================================================================
        //===============================[ Privilege ]===========================================
        //====================================================================================

        public static List<Privilege> ArrayTypeToPrivilege(this PrivilegeQCBDManagement[] privilegeQCBDManagementList)
        {
            object _lock = new object(); List<Privilege> returnList = new List<Privilege>();
            if (privilegeQCBDManagementList != null)
            {
                //Parallel.ForEach(privilegeQCBDManagementList, (privilegeQCBD) =>
                foreach(var privilegeQCBD in privilegeQCBDManagementList)
                {
                    Privilege privilege = new Privilege();
                    if (privilegeQCBD != null)
                    {
                        privilege.ID = privilegeQCBD.ID;
                        privilege.Role_actionId = privilegeQCBD.Role_actionId;
                        privilege.IsWrite =Utility.convertToBoolean(Utility.decodeBase64ToString(privilegeQCBD._Write));
                        privilege.IsRead = Utility.convertToBoolean(Utility.decodeBase64ToString(privilegeQCBD._Read));
                        privilege.IsDelete = Utility.convertToBoolean(Utility.decodeBase64ToString(privilegeQCBD._Delete));
                        privilege.IsUpdate = Utility.convertToBoolean(Utility.decodeBase64ToString(privilegeQCBD._Update));
                        privilege.IsSendMail = Utility.convertToBoolean(Utility.decodeBase64ToString(privilegeQCBD.SendEmail));
                        privilege.Date = Utility.convertToDateTime(Utility.decodeBase64ToString(privilegeQCBD.Date));

                        lock (_lock) returnList.Add(privilege);
                    }                      
                    
                    
                }
                //);
            }
            return returnList;
        }

        public static PrivilegeQCBDManagement[] PrivilegeTypeToArray(this List<Privilege> privilegeList)
        {
            int i = 0;
            PrivilegeQCBDManagement[] returnQCBDArray = new PrivilegeQCBDManagement[privilegeList.Count];
            if (privilegeList != null)
            {
                Parallel.ForEach(privilegeList, (privilege) =>
                {
                    PrivilegeQCBDManagement privilegeQCBD = new PrivilegeQCBDManagement();
                    privilegeQCBD.ID = privilege.ID;
                    privilegeQCBD.Role_actionId = privilege.Role_actionId;
                    privilegeQCBD._Write = Utility.encodeStringToBase64((privilege.IsWrite)? "1" : "0");
                    privilegeQCBD._Read = Utility.encodeStringToBase64((privilege.IsRead) ? "1" : "0");
                    privilegeQCBD._Delete = Utility.encodeStringToBase64((privilege.IsDelete)? "1" : "0");
                    privilegeQCBD._Update = Utility.encodeStringToBase64((privilege.IsUpdate) ? "1" : "0");
                    privilegeQCBD.SendEmail = Utility.encodeStringToBase64((privilege.IsSendMail) ? "1" : "0");
                    privilegeQCBD.Date = Utility.encodeStringToBase64(privilege.Date.ToString("yyyy-MM-dd H:mm:ss"));
                    returnQCBDArray[i] = privilegeQCBD;
                    i++;
                });
            }
            return returnQCBDArray;
        }

        public static PrivilegeFilterQCBDManagement PrivilegeTypeToFilterArray(this Privilege privilege, string filterOperator)
        {
            PrivilegeFilterQCBDManagement privilegeQCBD = new PrivilegeFilterQCBDManagement();
            if (privilege != null)
            {
                privilegeQCBD.ID = privilege.ID;
                privilegeQCBD.Role_actionId = privilege.Role_actionId;
                privilegeQCBD._Write = Utility.encodeStringToBase64((privilege.IsWrite) ? "1" : "0");
                privilegeQCBD._Read = Utility.encodeStringToBase64((privilege.IsRead) ? "1" : "0");
                privilegeQCBD._Delete = Utility.encodeStringToBase64((privilege.IsDelete) ? "1" : "0");
                privilegeQCBD._Update = Utility.encodeStringToBase64((privilege.IsUpdate) ? "1" : "0");
                privilegeQCBD.SendEmail = Utility.encodeStringToBase64((privilege.IsSendMail) ? "1" : "0");
                privilegeQCBD.Date = Utility.encodeStringToBase64(privilege.Date.ToString("yyyy-MM-dd H:mm:ss"));
                privilegeQCBD.Operator = filterOperator;
            }
            return privilegeQCBD;
        }


        //====================================================================================
        //===============================[ Command ]===========================================
        //====================================================================================

        public static List<Command> ArrayTypeToCommand(this CommandsQCBDManagement[] CommandQCBDManagement)
        {            
            object _lock = new object(); List<Command> returnList = new List<Command>();
            if (CommandQCBDManagement != null)
            {
                Parallel.ForEach(CommandQCBDManagement, (CommandQCBD) =>
               {
                   Command Command = new Command();
                   Command.ID = CommandQCBD.ID;
                   Command.AgentId = CommandQCBD.AgentId;
                   Command.BillAddress = CommandQCBD.BillAddress;
                   Command.ClientId = CommandQCBD.ClientId;
                   Command.Comment1 = Utility.decodeBase64ToString(CommandQCBD.Comment1);
                   Command.Comment2 = Utility.decodeBase64ToString(CommandQCBD.Comment2);
                   Command.Comment3 = Utility.decodeBase64ToString(CommandQCBD.Comment3);
                   Command.Status = Utility.decodeBase64ToString(CommandQCBD.Status);
                   Command.Date = Utility.convertToDateTime(Utility.decodeBase64ToString(CommandQCBD.Date));
                   Command.DeliveryAddress = CommandQCBD.DeliveryAddress;
                   Command.Tax = Convert.ToDouble(CommandQCBD.Tax);

                   lock(_lock) returnList.Add(Command);
               });
            }
            return returnList;
        }

        public static CommandsQCBDManagement[] CommandTypeToArray(this List<Command> CommandList)
        {
            int i = 0;
            CommandsQCBDManagement[] returnQCBDArray = new CommandsQCBDManagement[CommandList.Count];
            if (CommandList != null)
            {
                Parallel.ForEach(CommandList, (Command) =>
               {
                   CommandsQCBDManagement CommandQCBD = new CommandsQCBDManagement();
                   CommandQCBD.ID = Command.ID;
                   CommandQCBD.AgentId = Command.AgentId;
                   CommandQCBD.BillAddress = Command.BillAddress;
                   CommandQCBD.ClientId = Command.ClientId;
                   CommandQCBD.Comment1 = Utility.encodeStringToBase64(Command.Comment1);
                   CommandQCBD.Comment2 = Utility.encodeStringToBase64(Command.Comment2);
                   CommandQCBD.Comment3 = Utility.encodeStringToBase64(Command.Comment3);
                   CommandQCBD.Status = Utility.encodeStringToBase64(Command.Status);
                   CommandQCBD.Date = Utility.encodeStringToBase64(Command.Date.ToString("yyyy-MM-dd H:mm:ss"));
                   CommandQCBD.DeliveryAddress = Command.DeliveryAddress;
                   CommandQCBD.Tax = Command.Tax;

                   returnQCBDArray[i] = CommandQCBD;
                   i++;
               });
            }
            return returnQCBDArray;
        }

        public static CommandFilterQCBDManagement CommandTypeToFilterArray(this Command Command, string filterOperator)
        {
            CommandFilterQCBDManagement CommandQCBD = new CommandFilterQCBDManagement();
            if (Command != null)
            {
                CommandQCBD.ID = Command.ID;
                CommandQCBD.AgentId = Command.AgentId;
                CommandQCBD.BillAddress = Command.BillAddress;
                CommandQCBD.ClientId = Command.ClientId;
                CommandQCBD.Comment1 = Utility.encodeStringToBase64(Command.Comment1);
                CommandQCBD.Comment2 = Utility.encodeStringToBase64(Command.Comment2);
                CommandQCBD.Comment3 = Utility.encodeStringToBase64(Command.Comment3);
                CommandQCBD.Status = Utility.encodeStringToBase64(Command.Status);
                //CommandQCBD.Date = Utility.encodeStringToBase64(Command.Date.ToString("yyyy-MM-dd H:mm:ss"));
                CommandQCBD.DeliveryAddress = Command.DeliveryAddress;
                CommandQCBD.Tax = Command.Tax;
                CommandQCBD.Operator = filterOperator;
            }
            return CommandQCBD;
        }

        public static PdfQCBDManagement ParamCommandPdfTypeToArray(this ParamCommandToPdf paramCommandToPdf)
        {
            PdfQCBDManagement returnQCBDArray = new PdfQCBDManagement();
            if (!paramCommandToPdf.Equals(null))
            {
                returnQCBDArray.BillId = paramCommandToPdf.BillId;
                returnQCBDArray.CommandId = paramCommandToPdf.CommandId;                
            }
            return returnQCBDArray;
        }

        //====================================================================================
        //===============================[ Tax_command ]======================================
        //====================================================================================

        public static List<Tax_command> ArrayTypeToTax_command(this Tax_commandQCBDManagement[] Tax_commandQCBDManagementList)
        {
            object _lock = new object(); List<Tax_command> returnList = new List<Tax_command>();
            if (Tax_commandQCBDManagementList != null)
            {
                Parallel.ForEach(Tax_commandQCBDManagementList, (Tax_commandQCBD) =>
                {
                    Tax_command Tax_command = new Tax_command();
                    Tax_command.ID = Tax_commandQCBD.ID;
                    Tax_command.CommandId = Tax_commandQCBD.CommandId;
                    Tax_command.Date_insert = Utility.convertToDateTime(Utility.decodeBase64ToString(Tax_commandQCBD.Date_insert));
                    Tax_command.Target = Utility.decodeBase64ToString(Tax_commandQCBD.Target);
                    Tax_command.Tax_value = Convert.ToDouble(Tax_commandQCBD.Tax_value);
                    Tax_command.TaxId = Tax_commandQCBD.TaxId;

                    lock(_lock) returnList.Add(Tax_command);
                });
            }
            return returnList;
        }

        public static Tax_commandQCBDManagement[] Tax_commandTypeToArray(this List<Tax_command> Tax_commandList)
        {
            int i = 0;            
            Tax_commandQCBDManagement[] returnQCBDArray = new Tax_commandQCBDManagement[Tax_commandList.Count];
            if (Tax_commandList != null)
            {
                Parallel.ForEach(Tax_commandList, (Tax_command) =>
                {
                    Tax_commandQCBDManagement Tax_commandQCBD = new Tax_commandQCBDManagement();
                    Tax_commandQCBD.ID = Tax_command.ID;
                    Tax_commandQCBD.CommandId = Tax_command.CommandId;
                    Tax_commandQCBD.Date_insert = Utility.encodeStringToBase64(Tax_command.Date_insert.ToString("yyy-MM-dd H:mm:ss"));
                    Tax_commandQCBD.Target = Utility.encodeStringToBase64(Tax_command.Target);
                    Tax_commandQCBD.Tax_value = Tax_command.Tax_value;
                    Tax_commandQCBD.TaxId = Tax_command.TaxId;

                    returnQCBDArray[i] = Tax_commandQCBD;
                    i++;
                });
            }
            return returnQCBDArray;
        }

        public static Tax_commandFilterQCBDManagement Tax_commandTypeToFilterArray(this Tax_command Tax_command, string filterOperator)
        {
            Tax_commandFilterQCBDManagement Tax_commandQCBD = new Tax_commandFilterQCBDManagement();
            if (Tax_command != null)
            {
                Tax_commandQCBD.ID = Tax_command.ID;
                Tax_commandQCBD.CommandId = Tax_command.CommandId;
                Tax_commandQCBD.Date_insert = Utility.encodeStringToBase64(Tax_command.Date_insert.ToString("yyy-MM-dd H:mm:ss"));
                Tax_commandQCBD.Target = Utility.encodeStringToBase64(Tax_command.Target);
                Tax_commandQCBD.Tax_value = Tax_command.Tax_value;
                Tax_commandQCBD.TaxId = Tax_command.TaxId;
                Tax_commandQCBD.Operator = filterOperator;
            }
            return Tax_commandQCBD;
        }

        //====================================================================================
        //===============================[ Client ]===========================================
        //====================================================================================

        public static List<Client> ArrayTypeToClient(this ClientQCBDManagement[] ClientQCBDManagement)
        {
            object _lock = new object(); List<Client> returnList = new List<Client>();
            if (ClientQCBDManagement != null)
            {
                Parallel.ForEach(ClientQCBDManagement, (ClientQCBD) =>
                {
                    Client Client = new Client();
                    Client.ID = ClientQCBD.ID;
                    Client.FirstName = Utility.decodeBase64ToString(ClientQCBD.FirstName);
                    Client.LastName = Utility.decodeBase64ToString(ClientQCBD.LastName);
                    Client.AgentId = ClientQCBD.AgentId;
                    Client.Comment = Utility.decodeBase64ToString(ClientQCBD.Comment);
                    Client.Phone = Utility.decodeBase64ToString(ClientQCBD.Phone);
                    Client.Status = Utility.decodeBase64ToString(ClientQCBD.Status);
                    Client.Company = Utility.decodeBase64ToString(ClientQCBD.Company);
                    Client.Email = Utility.decodeBase64ToString(ClientQCBD.Email);
                    Client.Fax = Utility.decodeBase64ToString(ClientQCBD.Fax);
                    Client.CompanyName = Utility.decodeBase64ToString(ClientQCBD.CompanyName);
                    Client.CRN = Utility.decodeBase64ToString(ClientQCBD.CRN);
                    Client.MaxCredit = ClientQCBD.MaxCredit;
                    Client.Rib = Utility.decodeBase64ToString(ClientQCBD.Rib);
                    Client.PayDelay = ClientQCBD.PayDelay;

                    lock(_lock) returnList.Add(Client);
                });
            }
            return returnList;
        }

        public static ClientQCBDManagement[] ClientTypeToArray(this List<Client> ClientList)
        {
            int i = 0;
            ClientQCBDManagement[] returnQCBDArray = new ClientQCBDManagement[ClientList.Count];
            if (ClientList != null)
            {
                Parallel.ForEach(ClientList, (Client) =>
                {
                    ClientQCBDManagement ClientQCBD = new ClientQCBDManagement();
                    ClientQCBD.ID = Client.ID;
                    ClientQCBD.FirstName = Utility.encodeStringToBase64(Client.FirstName);
                    ClientQCBD.LastName = Utility.encodeStringToBase64(Client.LastName);
                    ClientQCBD.AgentId = Client.AgentId;
                    ClientQCBD.Comment = Utility.encodeStringToBase64(Client.Comment);
                    ClientQCBD.Phone = Utility.encodeStringToBase64(Client.Phone);
                    ClientQCBD.Status = Utility.encodeStringToBase64(Client.Status);
                    ClientQCBD.Company = Utility.encodeStringToBase64(Client.Company);
                    ClientQCBD.Email = Utility.encodeStringToBase64(Client.Email);
                    ClientQCBD.Fax = Utility.encodeStringToBase64(Client.Fax);
                    ClientQCBD.CompanyName = Utility.encodeStringToBase64(Client.CompanyName);
                    ClientQCBD.CRN = Utility.encodeStringToBase64(Client.CRN);
                    ClientQCBD.MaxCredit = Client.MaxCredit;
                    ClientQCBD.Rib = Utility.encodeStringToBase64(Client.Rib);
                    ClientQCBD.PayDelay = Client.PayDelay;

                    returnQCBDArray[i] = ClientQCBD;
                    i++;
                });
            }
            return returnQCBDArray;
        }
        public static ClientFilterQCBDManagement ClientTypeToFilterArray(this Client Client, string filterOperator)
        {
            ClientFilterQCBDManagement returnQCBDArray = new ClientFilterQCBDManagement();
            ClientFilterQCBDManagement ClientQCBD = new ClientFilterQCBDManagement();
            if (Client != null)
            {
                ClientQCBD.ID = Client.ID;
                ClientQCBD.FirstName = Utility.encodeStringToBase64(Client.FirstName);
                ClientQCBD.LastName = Utility.encodeStringToBase64(Client.LastName);
                ClientQCBD.AgentId = Client.AgentId;
                ClientQCBD.Comment = Utility.encodeStringToBase64(Client.Comment);
                ClientQCBD.Phone = Utility.encodeStringToBase64(Client.Phone);
                ClientQCBD.Status = Utility.encodeStringToBase64(Client.Status);
                ClientQCBD.Company = Utility.encodeStringToBase64(Client.Company);
                ClientQCBD.Email = Utility.encodeStringToBase64(Client.Email);
                ClientQCBD.Fax = Utility.encodeStringToBase64(Client.Fax);
                ClientQCBD.CompanyName = Utility.encodeStringToBase64(Client.CompanyName);
                ClientQCBD.CRN = Utility.encodeStringToBase64(Client.CRN);
                ClientQCBD.MaxCredit = Client.MaxCredit;
                ClientQCBD.Rib = Utility.encodeStringToBase64(Client.Rib);
                ClientQCBD.PayDelay = Client.PayDelay;
                ClientQCBD.Operator = filterOperator;
            }
            return ClientQCBD;
        }

        //====================================================================================
        //===============================[ Contact ]===========================================
        //====================================================================================

        public static List<Contact> ArrayTypeToContact(this ContactQCBDManagement[] ContactQCBDManagement)
        {
            object _lock = new object(); List<Contact> returnList = new List<Contact>();
            if (ContactQCBDManagement != null)
            {
                Parallel.ForEach(ContactQCBDManagement, (ContactQCBD) =>
               {
                   Contact Contact = new Contact();
                   Contact.ID = ContactQCBD.ID;
                   Contact.Cellphone = Utility.decodeBase64ToString(ContactQCBD.Cellphone);
                   Contact.ClientId = ContactQCBD.ClientId;
                   Contact.Comment = Utility.decodeBase64ToString(ContactQCBD.Comment);
                   Contact.Email = Utility.decodeBase64ToString(ContactQCBD.Email);
                   Contact.Phone = Utility.decodeBase64ToString(ContactQCBD.Phone);
                   Contact.Fax = Utility.decodeBase64ToString(ContactQCBD.Fax);
                   Contact.Firstname = Utility.decodeBase64ToString(ContactQCBD.Firstname);
                   Contact.LastName = Utility.decodeBase64ToString(ContactQCBD.LastName);
                   Contact.Position = Utility.decodeBase64ToString(ContactQCBD.Position);

                   lock(_lock) returnList.Add(Contact);
               });
            }
            return returnList;
        }

        public static ContactQCBDManagement[] ContactTypeToArray(this List<Contact> ContactList)
        {
            int i = 0;
            ContactQCBDManagement[] returnQCBDArray = new ContactQCBDManagement[ContactList.Count];
            if (ContactList != null)
            {
                Parallel.ForEach(ContactList, (Contact) =>
               {
                   ContactQCBDManagement ContactQCBD = new ContactQCBDManagement();
                   ContactQCBD.ID = Contact.ID;
                   ContactQCBD.Position = Utility.encodeStringToBase64(Contact.Position);
                   ContactQCBD.LastName = Utility.encodeStringToBase64(Contact.LastName);
                   ContactQCBD.Firstname = Utility.encodeStringToBase64(Contact.Firstname);
                   ContactQCBD.Comment = Utility.encodeStringToBase64(Contact.Comment);
                   ContactQCBD.Phone = Utility.encodeStringToBase64(Contact.Phone);
                   ContactQCBD.ClientId = Contact.ClientId;
                   ContactQCBD.Cellphone = Utility.encodeStringToBase64(Contact.Cellphone);
                   ContactQCBD.Email = Utility.encodeStringToBase64(Contact.Email);
                   ContactQCBD.Fax = Utility.encodeStringToBase64(Contact.Fax);

                   returnQCBDArray[i] = ContactQCBD;
                   i++;
               });
            }
            return returnQCBDArray;
        }

        public static ContactFilterQCBDManagement ContactTypeToFilterArray(this Contact Contact, string filterOperator)
        {
            ContactFilterQCBDManagement returnQCBDArray = new ContactFilterQCBDManagement();
            ContactFilterQCBDManagement ContactQCBD = new ContactFilterQCBDManagement();
            if (Contact != null)
            {
                ContactQCBD.ID = Contact.ID;
                ContactQCBD.Position = Utility.encodeStringToBase64(Contact.Position);
                ContactQCBD.LastName = Utility.encodeStringToBase64(Contact.LastName);
                ContactQCBD.Firstname = Utility.encodeStringToBase64(Contact.Firstname);
                ContactQCBD.Comment = Utility.encodeStringToBase64(Contact.Comment);
                ContactQCBD.Phone = Utility.encodeStringToBase64(Contact.Phone);
                ContactQCBD.ClientId = Contact.ClientId;
                ContactQCBD.Cellphone = Utility.encodeStringToBase64(Contact.Cellphone);
                ContactQCBD.Email = Utility.encodeStringToBase64(Contact.Email);
                ContactQCBD.Fax = Utility.encodeStringToBase64(Contact.Fax);
                ContactQCBD.Operator = filterOperator;
            }
            return ContactQCBD;
        }



        //====================================================================================
        //===============================[ Address ]===========================================
        //====================================================================================

        public static List<Address> ArrayTypeToAddress(this AddressQCBDManagement[] AddressQCBDManagement)
        {
            object _lock = new object(); List<Address> returnList = new List<Address>();
            if (AddressQCBDManagement != null)
            {
                Parallel.ForEach(AddressQCBDManagement, (AddressQCBD) =>
               {
                   Address Address = new Address();
                   Address.ID = AddressQCBD.ID;
                   Address.AddressName = Utility.decodeBase64ToString(AddressQCBD.Address);
                   Address.ClientId = AddressQCBD.ClientId;
                   Address.Comment = Utility.decodeBase64ToString(AddressQCBD.Comment);
                   Address.Email = Utility.decodeBase64ToString(AddressQCBD.Email);
                   Address.Phone = Utility.decodeBase64ToString(AddressQCBD.Phone);
                   Address.CityName = Utility.decodeBase64ToString(AddressQCBD.CityName);
                   Address.Country = Utility.decodeBase64ToString(AddressQCBD.Country);
                   Address.LastName = Utility.decodeBase64ToString(AddressQCBD.LastName);
                   Address.FirstName = Utility.decodeBase64ToString(AddressQCBD.FirstName);
                   Address.Name = Utility.decodeBase64ToString(AddressQCBD.Name);
                   Address.Name2 = Utility.decodeBase64ToString(AddressQCBD.Name2);
                   Address.Postcode = Utility.decodeBase64ToString(AddressQCBD.Postcode);

                   lock(_lock) returnList.Add(Address);
               });
            }
            return returnList;
        }

        public static AddressQCBDManagement[] AddressTypeToArray(this List<Address> AddressList)
        {
            int i = 0;
            AddressQCBDManagement[] returnQCBDArray = new AddressQCBDManagement[AddressList.Count];
            if (AddressList != null)
            {
                Parallel.ForEach(AddressList, (Address) =>
               {
                   AddressQCBDManagement AddressQCBD = new AddressQCBDManagement();
                   AddressQCBD.ID = Address.ID;
                   AddressQCBD.Address = Utility.encodeStringToBase64(Address.AddressName);
                   AddressQCBD.ClientId = Address.ClientId;
                   AddressQCBD.Comment = Utility.encodeStringToBase64(Address.Comment);
                   AddressQCBD.Email = Utility.encodeStringToBase64(Address.Email);
                   AddressQCBD.Phone = Utility.encodeStringToBase64(Address.Phone);
                   AddressQCBD.CityName = Utility.encodeStringToBase64(Address.CityName);
                   AddressQCBD.Country = Utility.encodeStringToBase64(Address.Country);
                   AddressQCBD.LastName = Utility.encodeStringToBase64(Address.LastName);
                   AddressQCBD.FirstName = Utility.encodeStringToBase64(Address.FirstName);
                   AddressQCBD.Name = Utility.encodeStringToBase64(Address.Name);
                   AddressQCBD.Name2 = Utility.encodeStringToBase64(Address.Name2);
                   AddressQCBD.Postcode = Utility.encodeStringToBase64(Address.Postcode);

                   returnQCBDArray[i] = AddressQCBD;
                   i++;
               });
            }
            return returnQCBDArray;
        }

        public static AddressFilterQCBDManagement AddressTypeToFilterArray(this Address Address, string filterOperator)
        {
            AddressFilterQCBDManagement returnQCBDArray = new AddressFilterQCBDManagement();
            AddressFilterQCBDManagement AddressQCBD = new AddressFilterQCBDManagement();
            if (Address != null)
            {
                AddressQCBD.ID = Address.ID;
                AddressQCBD.Address = Utility.encodeStringToBase64(Address.AddressName);
                AddressQCBD.ClientId = Address.ClientId;
                AddressQCBD.Comment = Utility.encodeStringToBase64(Address.Comment);
                AddressQCBD.Email = Utility.encodeStringToBase64(Address.Email);
                AddressQCBD.Phone = Utility.encodeStringToBase64(Address.Phone);
                AddressQCBD.CityName = Utility.encodeStringToBase64(Address.CityName);
                AddressQCBD.Country = Utility.encodeStringToBase64(Address.Country);
                AddressQCBD.LastName = Utility.encodeStringToBase64(Address.LastName);
                AddressQCBD.FirstName = Utility.encodeStringToBase64(Address.FirstName);
                AddressQCBD.Name = Utility.encodeStringToBase64(Address.Name);
                AddressQCBD.Name2 = Utility.encodeStringToBase64(Address.Name2);
                AddressQCBD.Postcode = Utility.encodeStringToBase64(Address.Postcode);
                AddressQCBD.Operator = filterOperator;
            }
            return AddressQCBD;
        }


        //====================================================================================
        //===============================[ Bill ]===========================================
        //====================================================================================

        public static List<Bill> ArrayTypeToBill(this BillQCBDManagement[] BillQCBDManagementList)
        {
            object _lock = new object(); List<Bill> returnList = new List<Bill>();
            if (BillQCBDManagementList != null)
            {
                //Parallel.ForEach(BillQCBDManagementList, (BillQCBD) =>
                foreach(var BillQCBD in BillQCBDManagementList)
               {
                   Bill Bill = new Bill();
                   Bill.ID = BillQCBD.ID;
                   Bill.ClientId = BillQCBD.ClientId;
                   Bill.CommandId = BillQCBD.CommandId;
                   Bill.Comment1 = Utility.decodeBase64ToString(BillQCBD.Comment1);
                   Bill.Comment2 = Utility.decodeBase64ToString(BillQCBD.Comment2);
                   Bill.Date = Utility.convertToDateTime(Utility.decodeBase64ToString(BillQCBD.Date));
                   Bill.DateLimit = Utility.convertToDateTime(Utility.decodeBase64ToString(BillQCBD.DateLimit));
                   Bill.Pay = BillQCBD.Pay;
                   Bill.PayDate = Utility.convertToDateTime(Utility.decodeBase64ToString(BillQCBD.DatePay));
                   Bill.PayMod = Utility.decodeBase64ToString(BillQCBD.PayMod);
                   Bill.PayReceived = BillQCBD.PayReceived;

                   lock (_lock) returnList.Add(Bill);
               }
                //);
            }
            return returnList;
        }

        public static BillQCBDManagement[] BillTypeToArray(this List<Bill> BillList)
        {
            int i = 0;
            BillQCBDManagement[] returnQCBDArray = new BillQCBDManagement[BillList.Count];
            if (BillList != null)
            {
                //Parallel.ForEach(BillList, (Bill) =>
                foreach(var Bill in BillList)
               {
                   BillQCBDManagement BillQCBD = new BillQCBDManagement();
                   BillQCBD.ID = Bill.ID;
                   BillQCBD.ClientId = Bill.ClientId;
                   BillQCBD.CommandId = Bill.CommandId;
                   BillQCBD.Comment1 = Utility.encodeStringToBase64(Bill.Comment1);
                   BillQCBD.Comment2 = Utility.encodeStringToBase64(Bill.Comment2);
                   BillQCBD.Date = Utility.encodeStringToBase64(Bill.Date.ToString("yyyy-MM-dd H:mm:ss"));
                   BillQCBD.DateLimit = Utility.encodeStringToBase64(Bill.DateLimit.ToString("yyyy-MM-dd H:mm:ss"));
                   BillQCBD.Pay = Bill.Pay;
                   BillQCBD.DatePay = Utility.encodeStringToBase64(Bill.PayDate.ToString("yyyy-MM-dd H:mm:ss"));
                   BillQCBD.PayMod = Utility.encodeStringToBase64(Bill.PayMod);
                   BillQCBD.PayReceived = Bill.PayReceived;

                   returnQCBDArray[i] = BillQCBD;
                   i++;
               }
               //);
            }
            return returnQCBDArray;
        }

        public static BillFilterQCBDManagement BillTypeToFilterArray(this Bill Bill, string filterOperator)
        {
            BillFilterQCBDManagement BillQCBD = new BillFilterQCBDManagement();
            if (Bill != null)
            {
                BillQCBD.ID = Bill.ID;
                BillQCBD.ClientId = Bill.ClientId;
                BillQCBD.CommandId = Bill.CommandId;
                BillQCBD.Comment1 = Utility.encodeStringToBase64(Bill.Comment1);
                BillQCBD.Comment2 = Utility.encodeStringToBase64(Bill.Comment2);
                BillQCBD.Date = Utility.encodeStringToBase64(Bill.Date.ToString("yyyy-MM-dd H:mm:ss"));
                BillQCBD.DateLimit = Utility.encodeStringToBase64(Bill.DateLimit.ToString("yyyy-MM-dd H:mm:ss"));
                BillQCBD.Pay = Bill.Pay;
                BillQCBD.DatePay = Utility.encodeStringToBase64(Bill.PayDate.ToString("yyyy-MM-dd H:mm:ss"));
                BillQCBD.PayMod = Utility.encodeStringToBase64(Bill.PayMod);
                BillQCBD.PayReceived = Bill.PayReceived;
                BillQCBD.Operator = filterOperator;
            }
            return BillQCBD;
        }

        //====================================================================================
        //===============================[ Delivery ]===========================================
        //====================================================================================

        public static List<Delivery> ArrayTypeToDelivery(this DeliveryQCBDManagement[] DeliveryQCBDManagementList)
        {
            object _lock = new object(); List<Delivery> returnList = new List<Delivery>();
            if (DeliveryQCBDManagementList != null)
            {
                Parallel.ForEach(DeliveryQCBDManagementList, (DeliveryQCBD) =>
               {
                   Delivery Delivery = new Delivery();
                   Delivery.ID = DeliveryQCBD.ID;
                   Delivery.BillId = DeliveryQCBD.BillId;
                   Delivery.CommandId = DeliveryQCBD.CommandId;
                   Delivery.Date = Utility.convertToDateTime(Utility.decodeBase64ToString(DeliveryQCBD.Date));
                   Delivery.Package = DeliveryQCBD.Package;
                   Delivery.Status = Utility.decodeBase64ToString(DeliveryQCBD.Status);

                   lock(_lock) returnList.Add(Delivery);
               });
            }
            return returnList;
        }

        public static DeliveryQCBDManagement[] DeliveryTypeToArray(this List<Delivery> DeliveryList)
        {
            int i = 0;
            DeliveryQCBDManagement[] returnQCBDArray = new DeliveryQCBDManagement[DeliveryList.Count];
            if (DeliveryList != null)
            {
                Parallel.ForEach(DeliveryList, (Delivery) =>
               {
                   DeliveryQCBDManagement DeliveryQCBD = new DeliveryQCBDManagement();
                   DeliveryQCBD.ID = Delivery.ID;
                   DeliveryQCBD.BillId = Delivery.BillId;
                   DeliveryQCBD.CommandId = Delivery.CommandId;
                   DeliveryQCBD.Date = Utility.encodeStringToBase64(Delivery.Date.ToString("yyyy-MM-dd H:mm:ss"));
                   DeliveryQCBD.Package = Delivery.Package;
                   DeliveryQCBD.Status = Utility.encodeStringToBase64(Delivery.Status);

                   returnQCBDArray[i] = DeliveryQCBD;
                   i++;
               });
            }
            return returnQCBDArray;
        }

        public static DeliveryFilterQCBDManagement DeliveryTypeToFilterArray(this Delivery Delivery, string filterOperator)
        {
            DeliveryFilterQCBDManagement DeliveryQCBD = new DeliveryFilterQCBDManagement();
            if (Delivery != null)
            {
                DeliveryQCBD.ID = Delivery.ID;
                DeliveryQCBD.BillId = Delivery.BillId;
                DeliveryQCBD.CommandId = Delivery.CommandId;
                DeliveryQCBD.Date = Utility.encodeStringToBase64(Delivery.Date.ToString("yyyy-MM-dd H:mm:ss"));
                DeliveryQCBD.Package = Delivery.Package;
                DeliveryQCBD.Status = Utility.encodeStringToBase64(Delivery.Status);
                DeliveryQCBD.Operator = filterOperator;
            }
            return DeliveryQCBD;
        }

        //====================================================================================
        //================================[ Command_item ]====================================
        //====================================================================================

        public static List<Command_item> ArrayTypeToCommand_item(this Command_itemQCBDManagement[] Command_itemQCBDManagementList)
        {
            object _lock = new object(); List<Command_item> returnList = new List<Command_item>();
            if (Command_itemQCBDManagementList != null)
            {
                Parallel.ForEach(Command_itemQCBDManagementList, (Command_itemQCBD) =>
               {
                   Command_item Command_item = new Command_item();
                   Command_item.ID = Command_itemQCBD.ID;
                   Command_item.CommandId = Command_itemQCBD.CommandId;
                   Command_item.Comment_Purchase_Price = Utility.decodeBase64ToString(Command_itemQCBD.Comment_Purchase_Price);
                   Command_item.Item_ref = Utility.decodeBase64ToString(Command_itemQCBD.Item_ref);
                   Command_item.Order = Command_itemQCBD.Order;
                   Command_item.Price = Command_itemQCBD.Price;
                   Command_item.Price_purchase = Command_itemQCBD.Price_purchase;
                   Command_item.Quantity = Command_itemQCBD.Quantity;
                   Command_item.Quantity_current = Command_itemQCBD.Quantity_current;
                   Command_item.Quantity_delivery = Command_itemQCBD.Quantity_delivery;
                    /*int resInt = 0;
                    decimal resDec = 0.0m;
                    int.TryParse(Command_itemQCBD.ID, out resInt);
                    Command_item.ID = resInt;
                    int.TryParse(Command_itemQCBD.CommandId, out resInt);
                    Command_item.CommandId = resInt;
                    Command_item.Comment_Purchase_Price = Command_itemQCBD.Comment_Purchase_Price;
                    Command_item.Items_ref = Command_itemQCBD.Item_ref;
                    int.TryParse(Command_itemQCBD.Order, out resInt);
                    Command_item.Order = resInt;
                    decimal.TryParse(Command_itemQCBD.Price, out resDec);
                    Command_item.Price = resDec;
                    decimal.TryParse(Command_itemQCBD.Price_purchase, out resDec);
                    Command_item.Price_purchase = resDec;
                    int.TryParse(Command_itemQCBD.Quantity, out resInt);
                    Command_item.Quantity = resInt;
                    int.TryParse(Command_itemQCBD.Quantity_current, out resInt);
                    Command_item.Quantity_current = resInt;
                    int.TryParse(Command_itemQCBD.Quantity_delivery, out resInt);
                    Command_item.Quantity_delivery = resInt;*/


                   lock(_lock) returnList.Add(Command_item);
               });
            }
            return returnList;
        }

        public static Command_itemQCBDManagement[] Command_itemTypeToArray(this List<Command_item> Command_itemList)
        {
            int i = 0;
            Command_itemQCBDManagement[] returnQCBDArray = new Command_itemQCBDManagement[Command_itemList.Count];
            if (Command_itemList != null)
            {
                Parallel.ForEach(Command_itemList, (Command_item) =>
               {
                   Command_itemQCBDManagement Command_itemQCBD = new Command_itemQCBDManagement();
                   Command_itemQCBD.ID = Command_item.ID;
                   Command_itemQCBD.CommandId = Command_item.CommandId;
                   Command_itemQCBD.Comment_Purchase_Price = Utility.encodeStringToBase64(Command_item.Comment_Purchase_Price);
                   Command_itemQCBD.Item_ref = Utility.encodeStringToBase64(Command_item.Item_ref);
                   Command_itemQCBD.Order = Command_item.Order;
                   Command_itemQCBD.Price = Command_item.Price;
                   Command_itemQCBD.Price_purchase = Command_item.Price_purchase;
                   Command_itemQCBD.Quantity = Command_item.Quantity;
                   Command_itemQCBD.Quantity_current = Command_item.Quantity_current;
                   Command_itemQCBD.Quantity_delivery = Command_item.Quantity_delivery;

                   returnQCBDArray[i] = Command_itemQCBD;
                   i++;
               });
            }
            return returnQCBDArray;
        }

        public static Command_itemFilterQCBDManagement Command_itemTypeToFilterArray(this Command_item Command_item, string filterOperator)
        {
            Command_itemFilterQCBDManagement Command_itemQCBD = new Command_itemFilterQCBDManagement();
            if (Command_item != null)
            {
                Command_itemQCBD.ID = Command_item.ID;
                Command_itemQCBD.CommandId = Command_item.CommandId;
                Command_itemQCBD.Comment_Purchase_Price = Utility.encodeStringToBase64(Command_item.Comment_Purchase_Price);
                Command_itemQCBD.Item_ref = Utility.encodeStringToBase64(Command_item.Item_ref);
                Command_itemQCBD.Order = Command_item.Order;
                Command_itemQCBD.Price = Command_item.Price;
                Command_itemQCBD.Price_purchase = Command_item.Price_purchase;
                Command_itemQCBD.Quantity = Command_item.Quantity;
                Command_itemQCBD.Quantity_current = Command_item.Quantity_current;
                Command_itemQCBD.Quantity_delivery = Command_item.Quantity_delivery;
                Command_itemQCBD.Operator = filterOperator;
            }
            return Command_itemQCBD;
        }
        

        //====================================================================================
        //==================================[ Tax ]===========================================
        //====================================================================================

        public static List<Tax> ArrayTypeToTax(this TaxQCBDManagement[] TaxQCBDManagementList)
        {
            object _lock = new object(); List<Tax> returnList = new List<Tax>();
            if (TaxQCBDManagementList != null)
            {
                Parallel.ForEach(TaxQCBDManagementList, (TaxQCBD) =>
               {
                   Tax Tax = new Tax();
                   Tax.ID = TaxQCBD.ID;
                   Tax.Tax_current = TaxQCBD.Tax_current;
                   Tax.Type = Utility.decodeBase64ToString(TaxQCBD.Type);
                   Tax.Value = Convert.ToDouble(TaxQCBD.Value);
                   Tax.Date_insert = Utility.convertToDateTime(Utility.decodeBase64ToString(TaxQCBD.Date_insert));
                   Tax.Comment = Utility.decodeBase64ToString(TaxQCBD.Comment);

                   lock(_lock) returnList.Add(Tax);
               });
            }
            return returnList;
        }

        public static TaxQCBDManagement[] TaxTypeToArray(this List<Tax> TaxList)
        {
            int i = 0;
            TaxQCBDManagement[] returnQCBDArray = new TaxQCBDManagement[TaxList.Count];
            if (TaxList != null)
            {
                Parallel.ForEach(TaxList, (Tax) =>
               {
                   TaxQCBDManagement TaxQCBD = new TaxQCBDManagement();
                   TaxQCBD.ID = Tax.ID;
                   TaxQCBD.Tax_current = Tax.Tax_current;
                   TaxQCBD.Type = Utility.encodeStringToBase64(Tax.Type);
                   TaxQCBD.Value = Tax.Value;
                   TaxQCBD.Date_insert = Utility.encodeStringToBase64(Tax.Date_insert.ToString("yyyy-MM-dd H:mm:ss"));
                   TaxQCBD.Comment = Utility.encodeStringToBase64(Tax.Comment);

                   returnQCBDArray[i] = TaxQCBD;
                   i++;
               });
            }
            return returnQCBDArray;
        }

        public static TaxFilterQCBDManagement TaxTypeToFilterArray(this Tax Tax, string filterOperator)
        {
            TaxFilterQCBDManagement TaxQCBD = new TaxFilterQCBDManagement();
            if (Tax != null)
            {
                TaxQCBD.ID = Tax.ID;
                TaxQCBD.Tax_current = Tax.Tax_current;
                TaxQCBD.Type = Utility.encodeStringToBase64(Tax.Type);
                TaxQCBD.Value = Tax.Value;
                TaxQCBD.Date_insert = Utility.encodeStringToBase64(Tax.Date_insert.ToString("yyyy-MM-dd H:mm:ss"));
                TaxQCBD.Comment = Utility.encodeStringToBase64(Tax.Comment);
                TaxQCBD.Operator = filterOperator;
            }
            return TaxQCBD;
        }



        //====================================================================================
        //===============================[ Provider_item ]===========================================
        //====================================================================================

        public static List<Provider_item> ArrayTypeToProvider_item(this Provider_itemQCBDManagement[] Provider_itemQCBDManagementList)
        {
            object _lock = new object(); List<Provider_item> returnList = new List<Provider_item>();
            if (Provider_itemQCBDManagementList != null)
            {
                Parallel.ForEach(Provider_itemQCBDManagementList, (Provider_itemQCBD) =>
               {
                   Provider_item Provider_item = new Provider_item();
                   Provider_item.ID = Provider_itemQCBD.ID;
                   Provider_item.Item_ref = Utility.decodeBase64ToString(Provider_itemQCBD.Item_ref);
                   Provider_item.Provider_name = Utility.decodeBase64ToString(Provider_itemQCBD.Provider_name);

                   lock(_lock) returnList.Add(Provider_item);
               });
            }
            return returnList;
        }

        public static Provider_itemQCBDManagement[] Provider_itemTypeToArray(this List<Provider_item> Provider_itemList)
        {
            int i = 0;
            Provider_itemQCBDManagement[] returnQCBDArray = new Provider_itemQCBDManagement[Provider_itemList.Count];
            if (Provider_itemList != null)
            {
                Parallel.ForEach(Provider_itemList, (Provider_item) =>
               {
                   Provider_itemQCBDManagement Provider_itemQCBD = new Provider_itemQCBDManagement();
                   Provider_itemQCBD.ID = Provider_item.ID;
                   Provider_itemQCBD.Item_ref = Utility.encodeStringToBase64(Provider_item.Item_ref);
                   Provider_itemQCBD.Provider_name = Utility.encodeStringToBase64(Provider_item.Provider_name);

                   returnQCBDArray[i] = Provider_itemQCBD;
                   i++;
               });
            }
            return returnQCBDArray;
        }

        public static Provider_itemFilterQCBDManagement Provider_itemTypeToFilterArray(this Provider_item Provider_item, string filterOperator)
        {
            Provider_itemFilterQCBDManagement Provider_itemQCBD = new Provider_itemFilterQCBDManagement();
            if (Provider_item != null)
            {
                Provider_itemQCBD.ID = Provider_item.ID;
                Provider_itemQCBD.Item_ref = Utility.encodeStringToBase64(Provider_item.Item_ref);
                Provider_itemQCBD.Provider_name = Utility.encodeStringToBase64(Provider_item.Provider_name);
                Provider_itemQCBD.Operator = filterOperator;
            }
            return Provider_itemQCBD;
        }

        //====================================================================================
        //===============================[ Provider ]===========================================
        //====================================================================================

        public static List<Provider> ArrayTypeToProvider(this ProviderQCBDManagement[] ProviderQCBDManagementList)
        {
            object _lock = new object(); List<Provider> returnList = new List<Provider>();
            if (ProviderQCBDManagementList != null)
            {
                Parallel.ForEach(ProviderQCBDManagementList, (ProviderQCBD) =>
                //foreach(var ProviderQCBD in ProviderQCBDManagementList)
                {
                   Provider Provider = new Provider();
                   Provider.ID = ProviderQCBD.ID;
                   Provider.Name = Utility.decodeBase64ToString(ProviderQCBD.Name);
                   Provider.Source = ProviderQCBD.Source;

                   lock(_lock) returnList.Add(Provider);
               });
            }
            return returnList;
        }

        public static ProviderQCBDManagement[] ProviderTypeToArray(this List<Provider> ProviderList)
        {
            int i = 0;
            ProviderQCBDManagement[] returnQCBDArray = new ProviderQCBDManagement[ProviderList.Count];
            if (ProviderList != null)
            {
                Parallel.ForEach(ProviderList, (Provider) =>
               {
                   ProviderQCBDManagement ProviderQCBD = new ProviderQCBDManagement();
                   ProviderQCBD.ID = Provider.ID;
                   ProviderQCBD.Name = Utility.encodeStringToBase64(Provider.Name);
                   ProviderQCBD.Source = Provider.Source;

                   returnQCBDArray[i] = ProviderQCBD;
                   i++;
               });
            }
            return returnQCBDArray;
        }

        public static ProviderFilterQCBDManagement ProviderTypeToFilterArray(this Provider Provider, string filterOperator)
        {
            ProviderFilterQCBDManagement ProviderQCBD = new ProviderFilterQCBDManagement();
            if (Provider != null)
            {
                ProviderQCBD.ID = Provider.ID;
                ProviderQCBD.Name = Utility.encodeStringToBase64(Provider.Name);
                ProviderQCBD.Source = Provider.Source;
                ProviderQCBD.Operator = filterOperator;
            }
            return ProviderQCBD;
        }

        //====================================================================================
        //===============================[ Item ]===========================================
        //====================================================================================

        public static List<Item> ArrayTypeToItem(this ItemQCBDManagement[] ItemQCBDManagementList)
        {
            object _lock = new object(); List<Item> returnList = new List<Item>();
            if (ItemQCBDManagementList != null)
            {
                Parallel.ForEach(ItemQCBDManagementList, (ItemQCBD) =>
               {
                   Item Item = new Item();
                   Item.ID = ItemQCBD.ID;
                   Item.Comment = Utility.decodeBase64ToString(ItemQCBD.Comment);
                   Item.Erasable = Utility.decodeBase64ToString(ItemQCBD.Erasable);
                   Item.Name = Utility.decodeBase64ToString(ItemQCBD.Name);
                   Item.Price_purchase = ItemQCBD.Price_purchase;
                   Item.Number_of_sale = ItemQCBD.Number_of_sale;
                   Item.Price_sell = ItemQCBD.Price_sell;
                   Item.Ref = Utility.decodeBase64ToString(ItemQCBD.Ref);
                   Item.Type_sub = Utility.decodeBase64ToString(ItemQCBD.Type_sub);
                   int intConverted = 0;
                   if(int.TryParse(Utility.decodeBase64ToString(ItemQCBD.Source.ToString()), out intConverted))
                        Item.Source = intConverted;
                   Item.Type = Utility.decodeBase64ToString(ItemQCBD.Type);

                   lock(_lock) returnList.Add(Item);
               });
            }
            return returnList;
        }

        public static ItemQCBDManagement[] ItemTypeToArray(this List<Item> ItemList)
        {
            int i = 0;
            ItemQCBDManagement[] returnQCBDArray = new ItemQCBDManagement[ItemList.Count];
            if (ItemList != null)
            {
                Parallel.ForEach(ItemList, (Item) =>
               {
                   ItemQCBDManagement ItemQCBD = new ItemQCBDManagement();
                   ItemQCBD.ID = Item.ID;
                   ItemQCBD.Comment = Utility.encodeStringToBase64(Item.Comment);
                   ItemQCBD.Erasable = Utility.encodeStringToBase64(Item.Erasable);
                   ItemQCBD.Name = Utility.encodeStringToBase64(Item.Name);
                   ItemQCBD.Price_purchase = Item.Price_purchase;
                   ItemQCBD.Number_of_sale = Item.Number_of_sale;
                   ItemQCBD.Price_sell = Item.Price_sell;
                   ItemQCBD.Ref = Utility.encodeStringToBase64(Item.Ref);
                   ItemQCBD.Type_sub = Utility.encodeStringToBase64(Item.Type_sub);
                   ItemQCBD.Source = Utility.encodeStringToBase64(Item.Source.ToString());
                   ItemQCBD.Type = Utility.encodeStringToBase64(Item.Type);

                   returnQCBDArray[i] = ItemQCBD;
                   i++;
               });
            }
            return returnQCBDArray;
        }

        public static ItemFilterQCBDManagement ItemTypeToFilterArray(this Item Item, string filterOperator)
        {
            ItemFilterQCBDManagement ItemQCBD = new ItemFilterQCBDManagement();
            if (Item != null)
            {
                ItemQCBD.ID = Item.ID;
                ItemQCBD.Comment = Utility.encodeStringToBase64(Item.Comment);
                ItemQCBD.Erasable = Utility.encodeStringToBase64(Item.Erasable);
                ItemQCBD.Name = Utility.encodeStringToBase64(Item.Name);
                ItemQCBD.Price_purchase = Item.Price_purchase;
                ItemQCBD.Price_sell = Item.Price_sell;
                ItemQCBD.Number_of_sale = Item.Number_of_sale;
                ItemQCBD.Option = Item.Option;
                ItemQCBD.Ref = Utility.encodeStringToBase64(Item.Ref);
                ItemQCBD.Type_sub = Utility.encodeStringToBase64(Item.Type_sub);
                ItemQCBD.Source = Utility.encodeStringToBase64(Item.Source.ToString());
                ItemQCBD.Type = Utility.encodeStringToBase64(Item.Type);
                ItemQCBD.Operator = filterOperator;
            }
            return ItemQCBD;
        }




        //====================================================================================
        //===============================[ Item_delivery ]===========================================
        //====================================================================================
        

        public static List<Item_delivery> ArrayTypeToItem_delivery(this Item_deliveryQCBDManagement[] Item_deliveryQCBDManagementList)
        {
            object _lock = new object(); List<Item_delivery> returnList = new List<Item_delivery>();
            if (Item_deliveryQCBDManagementList != null)
            {
                Parallel.ForEach(Item_deliveryQCBDManagementList, (Item_deliveryQCBD) =>
               {
                   Item_delivery Item_delivery = new Item_delivery();
                   Item_delivery.ID = Item_deliveryQCBD.ID;
                   Item_delivery.DeliveryId = Item_deliveryQCBD.DeliveryId;
                   Item_delivery.Item_ref = Utility.decodeBase64ToString(Item_deliveryQCBD.Item_ref);
                   Item_delivery.Quantity_delivery = Item_deliveryQCBD.Quantity_delivery;

                   lock(_lock) returnList.Add(Item_delivery);
               });
            }
            return returnList;
        }

        public static Item_deliveryQCBDManagement[] Item_deliveryTypeToArray(this List<Item_delivery> Item_deliveryList)
        {
            int i = 0;
            Item_deliveryQCBDManagement[] returnQCBDArray = new Item_deliveryQCBDManagement[Item_deliveryList.Count];
            if (Item_deliveryList != null)
            {
                Parallel.ForEach(Item_deliveryList, (Item_delivery) =>
               {
                   Item_deliveryQCBDManagement Item_deliveryQCBD = new Item_deliveryQCBDManagement();
                   Item_deliveryQCBD.ID = Item_delivery.ID;
                   Item_deliveryQCBD.DeliveryId = Item_delivery.DeliveryId;
                   Item_deliveryQCBD.Item_ref = Utility.encodeStringToBase64(Item_delivery.Item_ref);
                   Item_deliveryQCBD.Quantity_delivery = Item_delivery.Quantity_delivery;

                   returnQCBDArray[i] = Item_deliveryQCBD;
                   i++;
               });
            }
            return returnQCBDArray;
        }

        public static Item_deliveryFilterQCBDManagement Item_deliveryTypeToFilterArray(this Item_delivery Item_delivery, string filterOperator)
        {
            Item_deliveryFilterQCBDManagement Item_deliveryQCBD = new Item_deliveryFilterQCBDManagement();
            if (Item_delivery != null)
            {
                Item_deliveryQCBD.ID = Item_delivery.ID;
                Item_deliveryQCBD.DeliveryId = Item_delivery.DeliveryId;
                Item_deliveryQCBD.Item_ref = Utility.encodeStringToBase64(Item_delivery.Item_ref);
                Item_deliveryQCBD.Quantity_delivery = Item_delivery.Quantity_delivery;
                Item_deliveryQCBD.Operator = filterOperator;
            }
            return Item_deliveryQCBD;
        }



        //====================================================================================
        //===============================[ Tax_item ]===========================================
        //====================================================================================


        public static List<Tax_item> ArrayTypeToTax_item(this Tax_itemQCBDManagement[] Tax_itemQCBDManagementList)
        {
            object _lock = new object(); List<Tax_item> returnList = new List<Tax_item>();
            if (Tax_itemQCBDManagementList != null)
            {
                Parallel.ForEach(Tax_itemQCBDManagementList, (Tax_itemQCBD) =>
                {
                    Tax_item Tax_item = new Tax_item();
                    Tax_item.ID = Tax_itemQCBD.ID;
                    Tax_item.Item_ref = Utility.decodeBase64ToString(Tax_itemQCBD.Item_ref);
                    Tax_item.Tax_type = Utility.decodeBase64ToString(Tax_itemQCBD.Tax_type);
                    Tax_item.TaxId = Tax_itemQCBD.TaxId;
                    Tax_item.Tax_value = Tax_itemQCBD.Tax_value;

                    lock (_lock) returnList.Add(Tax_item);
                });
            }
            return returnList;
        }

        public static Tax_itemQCBDManagement[] Tax_itemTypeToArray(this List<Tax_item> Tax_itemList)
        {
            int i = 0;
            object _lock = new object();
            Tax_itemQCBDManagement[] returnQCBDArray = new Tax_itemQCBDManagement[Tax_itemList.Count];
            if (Tax_itemList != null)
            {
                Parallel.ForEach(Tax_itemList, (Tax_item) =>
                {
                    Tax_itemQCBDManagement Tax_itemQCBD = new Tax_itemQCBDManagement();
                    Tax_itemQCBD.ID = Tax_item.ID;
                    Tax_itemQCBD.Item_ref = Utility.encodeStringToBase64(Tax_item.Item_ref);
                    Tax_itemQCBD.Tax_type = Utility.encodeStringToBase64(Tax_item.Tax_type);
                    Tax_itemQCBD.TaxId = Tax_item.TaxId;
                    Tax_itemQCBD.Tax_value = Tax_item.Tax_value;

                    lock (_lock) returnQCBDArray[i] = Tax_itemQCBD;
                    i++;
                });
            }
            return returnQCBDArray;
        }

        public static Tax_itemFilterQCBDManagement Tax_itemTypeToFilterArray(this Tax_item Tax_item, string filterOperator)
        {
            Tax_itemFilterQCBDManagement Tax_itemQCBD = new Tax_itemFilterQCBDManagement();
            if (Tax_item != null)
            {
                Tax_itemQCBD.ID = Tax_item.ID;
                Tax_itemQCBD.Item_ref = Utility.encodeStringToBase64(Tax_item.Item_ref);
                Tax_itemQCBD.Tax_type = Utility.encodeStringToBase64(Tax_item.Tax_type);
                Tax_itemQCBD.TaxId = Tax_item.TaxId;
                Tax_itemQCBD.Tax_value = Tax_item.Tax_value;
                Tax_itemQCBD.Operator = filterOperator;
            }
            return Tax_itemQCBD;
        }


        //====================================================================================
        //===============================[ Auto_ref ]===========================================
        //====================================================================================

        public static List<Auto_ref> ArrayTypeToAuto_ref(this Auto_refsQCBDManagement[] Auto_refQCBDManagementList)
        {
            object _lock = new object(); List<Auto_ref> returnList = new List<Auto_ref>();
            if (Auto_refQCBDManagementList != null)
            {
                Parallel.ForEach(Auto_refQCBDManagementList, (Auto_refQCBD) =>
                {
                    Auto_ref Auto_ref = new Auto_ref();
                    Auto_ref.ID = Auto_refQCBD.ID;
                    Auto_ref.RefId = Auto_refQCBD.RefId;

                    lock (_lock) returnList.Add(Auto_ref);
                });
            }
            return returnList;
        }

        public static Auto_refsQCBDManagement[] Auto_refTypeToArray(this List<Auto_ref> Auto_refList)
        {
            int i = 0;
            Auto_refsQCBDManagement[] returnQCBDArray = new Auto_refsQCBDManagement[Auto_refList.Count];
            if (Auto_refList != null)
            {
                Parallel.ForEach(Auto_refList, (Auto_ref) =>
               {
                   Auto_refsQCBDManagement Auto_refQCBD = new Auto_refsQCBDManagement();
                   Auto_refQCBD.ID = Auto_ref.ID;
                   Auto_refQCBD.RefId = Auto_ref.RefId;

                   returnQCBDArray[i] = Auto_refQCBD;
                   i++;
               });
            }
            return returnQCBDArray;
        }

        public static Auto_refsFilterQCBDManagement Auto_refTypeToFilterArray(this Auto_ref Auto_ref, string filterOperator)
        {
            Auto_refsFilterQCBDManagement Auto_refQCBD = new Auto_refsFilterQCBDManagement();
            if (Auto_ref != null)
            {
                Auto_refQCBD.ID = Auto_ref.ID;
                Auto_refQCBD.RefId = Auto_ref.RefId;
                Auto_refQCBD.Operator = filterOperator;
            }
            return Auto_refQCBD;
        }
    }


}
