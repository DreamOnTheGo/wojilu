/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.Web.Mvc;

namespace wojilu.Web.Controller.Admin.Apps.Forum {

    public class LayoutController : ControllerBase {

        public LayoutController() {
            HideLayout( typeof( Forum.LayoutController ) );
        }


    }

}
