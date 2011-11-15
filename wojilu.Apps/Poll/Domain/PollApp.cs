/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.AppBase.Interface;
using wojilu.ORM;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Poll.Domain {

    [Serializable]
    public class PollApp : ObjectBase<PollApp>, IApp, IAccessStatus {

        public int OwnerId { get; set; }
        public String OwnerUrl { get; set; }
        public String OwnerType { get; set; }

        [TinyInt]
        public int AccessStatus { get; set; }
        public DateTime Created { get; set; }

    }

}
