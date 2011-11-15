﻿using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Content.Domain;

namespace wojilu.Web.Controller.Content.Submit {

    public class SecurityController : ControllerBase {

        public override void CheckPermission() {
            ContentApp app = ctx.app.obj as ContentApp;
            if (app.GetSettingsObj().EnableSubmit == 0) {
                echo( "对不起，尚未开放投递功能" );
                return;
            }

            if (ctx.viewer.IsLogin == false) {
                redirectUrl( t2( new MainController().Login ) + "?returnUrl=" + ctx.url.EncodeUrl );
            }
        }

    }

}
