/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Common.Msg.Enum;
using wojilu.Common.Msg.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Common.Msg.Interface;
using wojilu.Common.Comments;
using wojilu.Members.Interface;

namespace wojilu.Common.Msg.Service {

    public class NotificationService : INotificationService {

        public virtual void sendFriendRequest( int senderId, int receiverId, String msg ) {
            send( senderId, receiverId, typeof( User ).FullName, msg, NotificationType.Friend );
        }

        //private void send( String receiverIds, String msg, int type ) {
        //    int[] ids = cvt.ToIntArray( receiverIds );
        //    foreach (int receiverId in ids) {
        //        send( receiverId, msg, type );
        //    }
        //}

        /// <summary>
        /// ���û�(User)����֪ͨ
        /// </summary>
        /// <param name="receiverId">����User��Id</param>
        /// <param name="msg">֪ͨ����</param>
        public virtual void send( int receiverId, String msg ) {
            send( 0, receiverId, typeof( User ).FullName, msg, NotificationType.Normal );
        }

        /// <summary>
        /// ���û�(User)����֪ͨ
        /// </summary>
        /// <param name="receiverId">����User��Id</param>
        /// <param name="msg">֪ͨ����</param>
        /// <param name="type">NotificationType��ö��ֵ</param>
        public virtual void send( int receiverId, String msg, int type ) {
            send( 0, receiverId, typeof( User ).FullName, msg, type );
        }

        /// <summary>
        /// ��ĳ��IMember������֪ͨ
        /// </summary>
        /// <param name="receiverId">�����û���Id</param>
        /// <param name="receiverType">�����ߵ�����Type.FullName������Site��User��</param>
        /// <param name="msg">֪ͨ����</param>
        /// <param name="type">NotificationType��ö��ֵ</param>
        public virtual void send( int receiverId, String receiverType, String msg, int type ) {
            send( 0, receiverId, receiverType, msg, type );
        }

        private void send( int senderId, int receiverId, String receiverType, String msg, int type ) {


            Notification nf = new Notification();
            nf.Creator = new User( senderId );
            //nf.Receiver = receiver;
            nf.ReceiverId = receiverId;
            nf.ReceiverType = receiverType;

            nf.Msg = msg;
            nf.IsRead = 0;
            nf.Type = type;

            Result result = db.insert( nf );
            if (result.IsValid) {
                addNotificationCount( receiverId, receiverType );
            }
        }

        // User ������֪ͨ���ǻ����
        private void addNotificationCount( int receiverId, String receiverType ) {

            if (receiverType != typeof( User ).FullName) return;

            User receiver = User.findById( receiverId );
            if (receiver == null) throw new Exception( lang.get( "exReceiverNotFound" ) );

            receiver.NewNotificationCount++;
            receiver.update( "NewNotificationCount" );
        }


        public void cancelFriendRequest( int senderId, int receiverId ) {
            Notification f = db.find<Notification>( "Creator.Id=" + senderId + " and ReceiverId=" + receiverId ).first();
            if (f == null) return;

            db.delete( f );

            User receiver = db.findById<User>( receiverId );
            if (receiver == null) return;

            receiver.NewNotificationCount--;
            db.update( receiver, "NewNotificationCount" );
        }

        //----------------------------------------------------------------------------------------------------------------

        public virtual Notification GetById( int id ) {
            return db.findById<Notification>( id );
        }

        public virtual List<Notification> GetUnread( int receiverId, String receiverType, int count ) {
            return db.find<Notification>( "ReceiverId=" + receiverId + " and ReceiverType='" + receiverType + "' and IsRead=0" )
                .list( count );
        }

        public virtual DataPage<Notification> GetPage( int receiverId, String receiverType ) {
            return db.findPage<Notification>( "ReceiverId=" + receiverId + " and ReceiverType='" + receiverType + "'" );
        }

        public virtual int GetUnReadCount( int receiverId, String receiverType ) {
            return db.count<Notification>( "ReceiverId=" + receiverId + " and ReceiverType='" + receiverType + "' and IsRead=0" );
        }

        //----------------------------------------------------------------------------------------------------------------

        public virtual void Read( int notificationId ) {
            Notification nf = GetById( notificationId );

            if (nf == null) throw new Exception( lang.get( "exDataNotFound" ) );

            nf.IsRead = 1;
            nf.update( "IsRead" );

            subtractNotificationCount( nf );
        }

        private void subtractNotificationCount( Notification nf ) {

            if (nf.ReceiverType != typeof( User ).FullName) return;

            User user = User.findById( nf.ReceiverId );
            if (user.NewNotificationCount > 0) {
                user.NewNotificationCount--;
                user.update( "NewNotificationCount" );
            }

        }

        private void refuseFriend( Notification nf ) {
            throw new NotImplementedException();
        }

        public virtual void ReadAll( int receiverId, String receiverType ) {
            db.updateBatch<Notification>( "set IsRead=1", "ReceiverId=" + receiverId + " and ReceiverType='" + receiverType + "' and IsRead=0" );

            if (receiverType != typeof( User ).FullName) return;

            User user = User.findById( receiverId );
            if (user != null) {
                user.NewNotificationCount = 0;
                user.update( "NewNotificationCount" );
            }

        }

    }

}
