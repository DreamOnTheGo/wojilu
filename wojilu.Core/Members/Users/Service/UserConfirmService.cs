/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using wojilu.Members.Users.Domain;
using wojilu.Members.Users.Enum;
using wojilu.Members.Users.Interface;
using wojilu.Common.Money.Service;
using wojilu.Common.Money.Interface;
using wojilu.Common.Money.Domain;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Interface;

namespace wojilu.Members.Users.Service {

    public class UserConfirmService : IUserConfirmService {

        public IUserIncomeService userIncomeService { get; set; }
        public ICurrencyService currencyService { get; set; }
        public IMessageService msgService { get; set; }

        public UserConfirmService() {
            userIncomeService = new UserIncomeService();
            currencyService = new CurrencyService();
            msgService = new MessageService();
        }

        //public virtual void AddConfirm( String code ) {
        //}

        public virtual Result CanSend( User user ) {

            int maxMinutes = config.Instance.Site.UserSendConfirmEmailInterval;

            Result result = new Result();
            UserConfirm ac = db.find<UserConfirm>( "User.Id=" + user.Id+" order by Id desc" ).first();
            if (ac == null) return result;

            if (DateTime.Now.Subtract( ac.Created ).Minutes < maxMinutes) {

                result.Add( string.Format( "{0} ����֮�ڣ����ֻ�ܷ���һ��", maxMinutes ) );

                return result;

            }

            return result;
        }

        public virtual User Valid( String code ) {


            string[] arrItem = code.Split( '_' );
            if (arrItem.Length != 2) {
                return null;
            }

            int userId = cvt.ToInt( arrItem[0] );
            if (userId <= 0) {
                return null;
            }

            User user = db.findById<User>( userId );
            if (user == null) {
                return null;
            }

            String guid = arrItem[1];

            UserConfirm ac = db.find<UserConfirm>( "User.Id=:userId and Code=:code" )
                .set( "userId", userId )
                .set( "code", guid )
                .first();

            if (ac == null) return null;

            user.IsEmailConfirmed = EmailConfirm.Confirmed;

            db.update( user, "IsEmailConfirmed" );
            db.delete( ac );

            addIncomeAndMsg( user );



            return user;

        }

        private void addIncomeAndMsg( User user ) {

            int actionId = 18;

            userIncomeService.AddIncome( user, actionId );

            KeyIncomeRule rule = currencyService.GetKeyIncomeRulesByAction( actionId ); // ��ȡ��ǰ����action������������ȡ�������Ļ��ң���Ҳ����ʹ�� GetRulesByAction(actionId) ��ȡ�������л��ҵ��������

            int creditValue = rule.Income; // �����ֵ
            String creditName = rule.CurrencyName; // ���ҵ����ơ������ǻ�ȡ�����Ļ��ҡ�

            userIncomeService.AddIncome( user, actionId ); // ���û���������

            String msgTitle = "��л�������ʼ�";
            String msgBody = string.Format( "{0}��<br/>���ã�<br/>��л�������ʼ�������˻��{1}��������{2}��<br/>��ӭ�������룬лл��", user.Name, creditName, creditValue );
            msgService.SiteSend( msgTitle, msgBody, user ); // ���û�����վ��˽��
        }


        public virtual void AddConfirm( UserConfirm uc ) {
            db.insert( uc );
        }
    }

}
