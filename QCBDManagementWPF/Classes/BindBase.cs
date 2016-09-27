using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Enum;
using QCBDManagementWPF.Interfaces;
using QCBDManagementWPF.Models;
using QCBDManagementWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace QCBDManagementWPF.Classes
{
    public class BindBase : INotifyPropertyChanged, IState, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected Startup _startup;
        protected ConfirmationViewModel _dialog;

        public BindBase()
        {

        }
        
        protected virtual void setProperty<P>(
            ref P member,
            P val,
            [CallerMemberName]
            string propertyName = null)
        {
            /*if (object.Equals(member,val))
                return;*/

            member = val;
            
            onPropertyChange(propertyName);
        }

        public Startup Startup
        {
            get { return _startup; }
            set { setProperty(ref _startup, value, "Startup"); }
        }

        public ConfirmationViewModel Dialog
        {
            get { return _dialog; }
            set { setProperty(ref _dialog, value, "Dialog"); }
        }

        protected void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }                
        }


        public void Handle(Context context, Func<object, object> page)
        {
            var prev = context.PreviousState;
            context.PreviousState = context.NextState;
            context.NextState = prev;
            page(context.NextState);
        }

        public bool securityCheck(EAction action, ESecurity right)
        {            
            if (_startup != null)
            {
                Agent agent = _startup.Bl.BlSecurity.GetAuthenticatedUser();
                if (agent.RoleList != null)           
                    foreach (var role in agent.RoleList)
                    {
                        var actionFound = role.ActionList.Where(x => x.Name.Equals(action.ToString())).FirstOrDefault();
                        if (actionFound != null)
                        {
                            switch (right)
                            {
                                case ESecurity._Delete:
                                    return actionFound.Right.IsDelete;
                                case ESecurity._Read:
                                    return actionFound.Right.IsRead;
                                case ESecurity._Update:
                                    return actionFound.Right.IsUpdate;
                                case ESecurity._Write:
                                    return actionFound.Right.IsWrite;
                                case ESecurity.SendEmail:
                                    return actionFound.Right.IsSendMail;
                                default:
                                    return false;
                            }
                        }
                    }
            }  
            return false;
        }

        public virtual void Dispose()
        {
            
        }

        /*protected async void getFtpCredential(out string login, out string password)
        {
            var infosLoginFTP = (await _startup.Bl.BlReferential.searchInfos(new QCBDManagementCommon.Entities.Infos { Name = "ftp_login" }, "OR")).FirstOrDefault() ?? new Infos();
            var infosPasswordFTP = (await _startup.Bl.BlReferential.searchInfos(new QCBDManagementCommon.Entities.Infos { Name = "ftp_password" }, "OR")).FirstOrDefault() ?? new Infos();
            login = infosLoginFTP.Value;
            password = infosPasswordFTP.Value;
        }*/


    }
}
