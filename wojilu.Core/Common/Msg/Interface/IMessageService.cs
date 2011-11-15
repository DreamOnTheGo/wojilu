/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Members.Users.Domain;
using wojilu.Common.Msg.Domain;

namespace wojilu.Common.Msg.Interface {


    public interface IMessageService {

        /// <summary>
        /// ����վ��������û�����վ�ڶ���
        /// </summary>
        /// <param name="title">���ű���</param>
        /// <param name="body">��������</param>
        /// <param name="receiver">������</param>
        void SiteSend( String title, String body, User receiver );

        /// <summary>
        /// ����վ�����������û�����վ�ڶ���
        /// </summary>
        /// <param name="title">���ű���</param>
        /// <param name="body">��������</param>
        /// <param name="receiver">�������б�</param>
        void SiteSend( String title, String body, List<User> receivers );

        /// <summary>
        /// ����վ�ڶ���
        /// </summary>
        /// <param name="sender">������User</param>
        /// <param name="rawReceiver">�����˵��û���������û����ö��ŷֿ���</param>
        /// <param name="msgTitle">���ű���</param>
        /// <param name="msgBody">��������</param>
        /// <returns></returns>
        Result SendMsg( User sender, String rawReceiver, String msgTitle, String msgBody );

        Result SendMsg( User sender, String rawReceiver, String msgTitle, String msgBody, int replyId, int[] attachmentIds );
        

        void ReadMsg( Message msg );

        Boolean AdminByAction( String action, User member, String choice );

        DataPage<Message> FindPageAll( int receiverId );
        DataPage<MessageData> FindPageSent( int senderId );

        DataPage<Message> FindPageTrash( int receiverId );
        DataPage<Message> GetNewMsg( int receiverId );

        Message GetById( int receiverId, int id );
        MessageData GetDataById( int senderId, int id );
        List<Message> GetNewMsg( int receiverId, int count );
        MessageStats GetStats( User owner );

        void CheckSiteMsg( User user );


        DataPage<Message> SearchByUser( int receiverId, string senderName );
        DataPage<MessageData> SearchByReceiver( int senderId, string receiverName );

        Message GetPrevMsg( int receiverId, int msgId );
        Message GetNextMsg( int receiverId, int msgId );

        void DeleteToTrash( Message msg );
    }

}

