/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Common.Msg.Domain;

namespace wojilu.Common.Msg.Interface {

    public interface INotificationService {

        Notification GetById( int id );
        List<Notification> GetUnread( int receiverId, String receiverType, int count );
        DataPage<Notification> GetPage( int receiverId, String receiverType );
        int GetUnReadCount( int receiverId, String receiverType );

        void Read( int notificationId );
        void ReadAll( int receiverId, String receiverType );

        /// <summary>
        /// ���û�(User)����֪ͨ
        /// </summary>
        /// <param name="receiverId">����User��Id</param>
        /// <param name="msg">֪ͨ����</param>
        void send( int receiverId, String msg );

        /// <summary>
        /// ���û�(User)����֪ͨ
        /// </summary>
        /// <param name="receiverId">����User��Id</param>
        /// <param name="msg">֪ͨ����</param>
        /// <param name="type">NotificationType��ö��ֵ</param>
        void send( int receiverId, String msg, int type );

        /// <summary>
        /// ��ĳ��IMember������֪ͨ
        /// </summary>
        /// <param name="receiverId">�����û���Id</param>
        /// <param name="receiverType">�����ߵ�����Type.FullName������Site��User��</param>
        /// <param name="msg">֪ͨ����</param>
        /// <param name="type">NotificationType��ö��ֵ</param>
        void send( int receiverId, String receiverType, String msg, int type );

        void sendFriendRequest( int senderId, int receiverId, String msg );

        void cancelFriendRequest( int senderId, int receiverId );


    }

}
