/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Members.Users.Domain;
using wojilu.Web.Controller.Users;

using wojilu.Apps.Content.Domain;
using wojilu.Apps.Content.Interface;
using wojilu.Common.Msg.Domain;
using wojilu.Apps.Content.Service;
using wojilu.Web.Controller.Content.Utils;

namespace wojilu.Web.Controller.Content.Binder {

    public class MyFeedbackBinderController : ControllerBase, ISectionBinder {

        public IContentCustomTemplateService ctService { get; set; }
        public MyFeedbackBinderController() {
            ctService = new ContentCustomTemplateService();
        }
        public void Bind( ContentSection section, IList serviceData ) {

            TemplateUtil.loadTemplate( this, section, ctService );

            //target( new FeedbackController().Create );
            set( "ActionLink", t2( new FeedbackController().Create ) );

            if (ctx.viewer.Id == ctx.owner.Id && ctx.owner.obj is User)
                set( "f.ListLink", t2( new FeedbackController().AdminList ) );
            else
                set( "f.ListLink", t2( new FeedbackController().List ) );

            String pwTip = string.Format( lang( "pwTip" ), Feedback.ContentLength );
            set( "pwTip", pwTip );


            IBlock block = getBlock( "list" );
            foreach (Feedback f in serviceData) {

                if (f.Creator == null) continue;

                block.Set( "f.UserName", f.Creator.Name );
                block.Set( "f.UserFace", f.Creator.PicSmall );
                block.Set( "f.UserLink", Link.ToMember( f.Creator ) );

                block.Set( "f.ReplyLink", t2( new FeedbackController().Reply, f.Id ) );

                block.Set( "f.Content", f.GetContent() );
                block.Set( "f.Created", cvt.ToTimeString( f.Created ) );

                block.Bind( "f", f );

                block.Next();
            }
        }





    }

}
