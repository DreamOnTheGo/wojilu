/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections;
using System.Text;
using wojilu.ORM;
using wojilu.Members.Users.Domain;
using wojilu.Common.Comments;
using wojilu.Common.Feeds.Domain;
using wojilu.Common.Feeds.Service;
using wojilu.Web.Mvc;
using wojilu.Serialization;
using System.Collections.Generic;
using wojilu.Common.Msg.Service;
using wojilu.Common.Msg.Enum;

namespace wojilu.Common.Pages.Domain {

    [Serializable]
    public class PageComment : ObjectBase<PageComment>, IComment {

        public int AppId { get; set; }

        [Column( Length = 50 )]
        public String Author { get; set; }
        public User Member { get; set; }

        public int ParentId { get; set; }
        public int RootId { get; set; }

        [Column( Length = 20 )]
        public String Title { get; set; }
        [LongText]
        public String Content { get; set; }

        public int Replies { get; set; }

        public DateTime Created { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        public Type GetTargetType() {
            return typeof( Page );
        }

        public void AddFeedInfo( String lnkTarget ) {

            Feed myfeed = new Feed();

            myfeed.Creator = this.Member;
            myfeed.DataType = this.GetTargetType().FullName;

            String tt = "{*actor*} ���������� {*target*}";

            myfeed.TitleTemplate = tt;
            myfeed.TitleData = getTitleData( lnkTarget );

            myfeed.BodyGeneral = strUtil.ParseHtml( this.Content, 50 );

            new FeedService().publishUserAction( myfeed );

        }

        private String getTitleData( String lnkPost ) {

            Page data = Page.findById( this.RootId );

            String target = string.Format( "<a href=\"{0}\">{1}</a>", lnkPost, data.Title );

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add( "target", target );
            return JSON.DicToString( dic );
        }

        public void AddNotification( String lnkTarget ) {

            Page post = Page.findById( this.RootId );
            if (post == null) return;

            int receiverId = post.OwnerId;

            // �Լ��Ļظ����ø��Լ���֪ͨ
            if (this.Member != null && (this.Member.Id == receiverId)) return;

            String msg = this.Author + " ���������� <a href=\"" + lnkTarget + "\">" + post.Title + "</a>";

            NotificationService nfService = new NotificationService();
            nfService.send( receiverId, post.OwnerType, msg, NotificationType.Comment );

            sendToEditors( lnkTarget, post, nfService );

        }

        // �����й����߷���֪ͨ
        private void sendToEditors( String lnkTarget, Page post, NotificationService nfService ) {
            List<int> editorIds = GetEditorIds( post.Id );
            foreach (int eId in editorIds) {
                if (this.Member != null && (this.Member.Id == eId)) continue;
                String cmsg = this.Author + " ���������������ҳ�� <a href=\"" + lnkTarget + "\">" + post.Title + "</a>";
                nfService.send( eId, cmsg );
            }
        }


        private List<int> GetEditorIds( int pageId ) {

            List<PageHistory> list = PageHistory.find( "PageId=" + pageId ).list();

            List<int> users = new List<int>();
            foreach (PageHistory ph in list) {
                if (users.Contains( ph.EditUser.Id )) continue;
                users.Add( ph.EditUser.Id );
            }

            return users;
        }


    }

}
