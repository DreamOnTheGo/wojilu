/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.ORM;
using wojilu.Members.Users.Domain;

namespace wojilu.Common.Msg.Domain {

    [Serializable]
    public class Notification : ObjectBase<Notification> {

        public int ReceiverId { get; set; }

        /// <summary>
        /// �����ߵ����ͣ�ͨ����User��Ҳ������Site������IMember����
        /// </summary>
        public String ReceiverType { get; set; }

        public User Creator { get; set; }

        /// <summary>
        /// NotificationType ��ö��ֵ
        /// </summary>
        public int Type { get; set; }

        public int IsRead { get; set; }

        [LongText]
        public String Msg { get; set; }

        public DateTime Created { get; set; }

    }

}
