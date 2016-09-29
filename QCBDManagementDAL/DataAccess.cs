using System;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementDAL.Core;
using QCBDManagementCommon.Entities;
using System.Threading.Tasks;
using System.Threading;

public class DataAccess: IDataAccessManager
{
    
    public IAgentManager DALAgent { get; set; }
    public IStatisticManager DALStatistic { get; set; }
    public ICommandManager DALCommand {get; set;}
    public IClientManager DALClient { get; set; }
    public IItemManager DALItem { get; set; }
    public IReferentialManager DALReferential { get; set; }
    public ISecurityManager DALSecurity { get; set; }
    public Func<double,double> ProgressBarFunc { get; set; }

    public DataAccess(
                        IAgentManager inDALAgent,
                        IClientManager inDALClient,
                        IItemManager inDALItem,
                        ICommandManager inDALCommande,
                        ISecurityManager inDALSecurity,
                        IStatisticManager inDALStatisitc,
                        IReferentialManager inDALReferential
        )    
    {
        this.DALAgent = inDALAgent;
        this.DALClient = inDALClient;
        this.DALCommand = inDALCommande;
        this.DALItem = inDALItem;
        this.DALStatistic = inDALStatisitc;
        this.DALReferential = inDALReferential;
        this.DALSecurity = inDALSecurity;
    }

    public void SetUserCredential(Agent authenticatedUser, bool isNewAgentAuthentication = false)
    {
        //ProgressBarFunc(-1);
        
        if (isNewAgentAuthentication)
        {
            Task.Factory.StartNew(() => {
                DALCommand.progressBarManagement(ProgressBarFunc);
                DALCommand.initializeCredential(authenticatedUser);
                //ProgressBarFunc(100);
            });
        }
        else
        {
            Task.Factory.StartNew(() =>
            {
                DALCommand.progressBarManagement(ProgressBarFunc);
                DALCommand.initializeCredential(authenticatedUser);
            }).ContinueWith((tsk)=> {
                Task.Factory.StartNew(() =>
                {
                    DALAgent.progressBarManagement(ProgressBarFunc);
                    DALAgent.initializeCredential(authenticatedUser);
                });

                Task.Factory.StartNew(() =>
                {
                    DALClient.progressBarManagement(ProgressBarFunc);
                    DALClient.initializeCredential(authenticatedUser);
                });

                Task.Factory.StartNew(() =>
                {
                    DALSecurity.progressBarManagement(ProgressBarFunc);
                    DALSecurity.initializeCredential(authenticatedUser);
                });

                Task.Factory.StartNew(() =>
                {
                    DALItem.progressBarManagement(ProgressBarFunc);
                    DALItem.initializeCredential(authenticatedUser);
                });

                Task tskReferential = Task.Factory.StartNew(() => {
                    DALReferential.progressBarManagement(ProgressBarFunc);
                    DALReferential.initializeCredential(authenticatedUser);
                });

                Thread main = new Thread(delegate () {
                    DALStatistic.progressBarManagement(ProgressBarFunc);
                    DALStatistic.initializeCredential(authenticatedUser);
                });
                main.SetApartmentState(ApartmentState.STA);
                main.Start();
            });
            
        }

        //Task.Factory.StartNew(() =>
        //{
        //    DALItem.progressBarManagement(ProgressBarFunc);
        //    DALItem.initializeCredential(authenticatedUser);
        //});


        /*
        
        Task.Factory.StartNew(() => {

            DALAgent.progressBarManagement(ProgressBarFunc);
            DALAgent.initializeCredential(authenticatedUser);

            DALCommand.progressBarManagement(ProgressBarFunc);
            DALCommand.initializeCredential(authenticatedUser);
            
        }).ContinueWith((task)=> {
            DALClient.progressBarManagement(ProgressBarFunc);
            DALClient.initializeCredential(authenticatedUser);

            DALSecurity.progressBarManagement(ProgressBarFunc);
            DALSecurity.initializeCredential(authenticatedUser);

            DALReferential.progressBarManagement(ProgressBarFunc);
            DALReferential.initializeCredential(authenticatedUser);
        });

        Task.Factory.StartNew(() =>
        {
            DALItem.progressBarManagement(ProgressBarFunc);
            DALItem.initializeCredential(authenticatedUser);
        });

        Thread main = new Thread(delegate ()
        {
            DALStatistic.progressBarManagement(ProgressBarFunc);
            DALStatistic.initializeCredential(authenticatedUser);
        });
        main.SetApartmentState(ApartmentState.STA);
        main.Start();
        */

    }

    public void Dispose()
    {
        this.DALAgent.Dispose();
        this.DALClient.Dispose();
        this.DALCommand.Dispose();
        this.DALItem.Dispose();
        this.DALStatistic.Dispose();
        this.DALReferential.Dispose();
        this.DALSecurity.Dispose();
    }
} /* end class DataAccess */
