/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Text;

using wojilu.Members.Users.Domain;
using System.Collections.Generic;
using wojilu.Common.AppBase;

namespace wojilu.Common.Pages.Domain {

    public class OpenStatus {

        /// <summary>
        /// �رձ༭
        /// </summary>
        public static readonly int Close = 0;

        /// <summary>
        /// ��ȫ���ţ��κ�ע���û������Ա༭
        /// </summary>
        public static readonly int Open = 1;

        /// <summary>
        /// ֻ�б���ѡ����(�༭)���Ա༭
        /// </summary>
        public static readonly int Editor = 2;
    }

    [Serializable]
    public class PageCategory : ObjectBase<PageCategory>, ISort {

        public int OwnerId { get; set; }
        public String OwnerType { get; set; }
        public String OwnerUrl { get; set; }

        public User Creator { get; set; }

        public int OrderId { get; set; }
        public int ParentId { get; set; }

        public String Name { get; set; }
        public String Description { get; set; }
        public String Logo { get; set; }
        public DateTime Created { get; set; }
        public int DataCount { get; set; }

        public int IsShowWiki { get; set; } // �Ƿ���ʾwiki����ͳ����Ϣ
        public int OpenStatus { get; set; } // ����״̬����OpenStatus
        public String EditorIds { get; set; } // ����༭���û��������� OpenStatus==2 ��ʱ��������


        public void updateOrderId() {
            this.update();
        }

    }

}
