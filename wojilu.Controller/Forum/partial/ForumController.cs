/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Apps.Forum.Domain;
using wojilu.Common.Onlines;
using wojilu.Common;

namespace wojilu.Web.Controller.Forum {

    public partial class ForumController : ControllerBase {
        
        private void bindAll( List<ForumBoard> categories, List<ForumLink> linkList ) {

            bindCategories( categories );

            load( "forumNewPosts", new TopController().List );

            bindLink( linkList );

            bindStats();

            bindMyPosts();

            //bindOnlineUsers( onlineUsers );
        }

        private void bindMyPosts() {

            set( "lnkMyTopic", to( new RecentController().MyTopic ) );
            set( "lnkMyPost", to( new RecentController().MyPost ) );

            set( "lnkPost", to( new RecentController().Post ) );
            set( "lnkTopic", to( new RecentController().Topic ) );
            set( "lnkPicked", to( new RecentController().Picked ) );
            set( "lnkReplies", to( new RecentController().Replies ) );
            set( "lnkViews", to( new RecentController().Views ) );

            set( "searchAction", to( new SearchController().Results ) );

        }


        private void bindCategories( List<ForumBoard> categories ) {
            IBlock fcategoryBlock = getBlock( "forumCategory" );

            foreach (ForumBoard board in categories) {

                List<ForumBoard> boards = getTree().GetChildren( board.Id );
                ctx.SetItem( "currentBoard", board );
                ctx.SetItem( "childForumBoards", boards );

                fcategoryBlock.Set( "childrenBoards", loadHtml( new BoardController().List ) );

                fcategoryBlock.Set( "adForumBoards", AdItem.GetAdById( AdCategory.ForumBoards ) );

                fcategoryBlock.Next();
            }
        }



        private void bindStats() {

            ForumApp forum = ctx.app.obj as ForumApp;

            ForumSetting s = forum.GetSettingsObj();
            set( "forum.IsHideStats_Style", s.IsHideStats == 1 ? "display:none" : "" );
            set( "forum.IsHideTop_Style", s.IsHideTop == 1 ? "display:none" : "" );
            set( "forum.IsHideOnline_Style", s.IsHideOnline == 1 ? "display:none" : "" );
            set( "forum.IsHideLink_Style", s.IsHideLink == 1 ? "display:none" : "" );


            String newUserLink = Link.T2( new wojilu.Web.Controller.Users.MainController().ListAll );
            String lastUserName = userService.GetLastUserName();
            set( "newUserLink", newUserLink );
            set( "newUserName", lastUserName );

            String lnkAll = t2( new wojilu.Web.Controller.Users.MainController().OnlineAll );
            String lnkMembers = t2( new wojilu.Web.Controller.Users.MainController().OnlineUser );

            set( "onlineLink", lnkAll );
            set( "onlineMemberLink", lnkMembers );
            
            set( "forum.VisitCount", forum.VisitCount );
            set( "forum.TodayVisitCount", forum.TodayVisitCount );
            set( "forum.YestodayPostCount", forum.YestodayPostCount );

            int userCount = userService.GetUserCount();
            set( "forum.MemberCount", userCount );

            set( "forum.TopicCount", forum.TopicCount );
            set( "forum.PostCount", forum.AllPostCount );
            set( "forum.TodayTopic", forum.TodayTopicCount );
            set( "forum.TodayPost", forum.AllTodayPostCount );
            set( "forum.PeakPostCount", forum.PeakPostCount );
            set( "forum.LastUpdatePostUrl", strUtil.Join( sys.Path.Root, forum.LastUpdatePostUrl ) );
            set( "forum.LastUpdatePostTitle", forum.LastUpdatePostTitle );
            //set( "forum.LastUpdateMemberUrl", memberUtil.GetUrlFull( forum.LastUpdateMemberUrl ) );
            set( "forum.LastUpdateMemberUrl", Link.ToUser( forum.LastUpdateMemberUrl ) );

            set( "forum.LastUpdateMemberName", forum.LastUpdateMemberName );
            set( "forum.LastUpdateTime", forum.LastUpdateTime );

        }

        private void bindLink( List<ForumLink> linkList ) {

            IBlock linkBlock = getBlock( "flinks" );
            foreach (ForumLink link in linkList) {
                linkBlock.Set( "flink.Name", link.Name );
                linkBlock.Set( "flink.Url", link.Url );
                linkBlock.Set( "flink.Logo", link.Logo );
                linkBlock.Next();
            }
        }

        private void bindOnlineUsers( List<OnlineUser> onlineUsers ) {

            IBlock onlineBlock = getBlock( "onlineUsers" );

            foreach (OnlineUser ol in onlineUsers) {

                onlineBlock.Set( "onlineUser.Name", ol.UserName );

                String lblValue = "【" + lang( "osInfo" ) + "】" + ol.Agent +
                    "\n【" + lang( "startTime" ) + "】" + ol.StartTime.ToString() +
                    "\n【" + lang( "lastActive" ) + "】" + ol.LastActive.ToString() +
                    "\n【" + lang( "clocation" ) + "】" + ol.Location;

                onlineBlock.Set( "onlineUser.Info", lblValue );
                if (ol.UserId > 0) {
                    onlineBlock.Set( "onlineUser.Url", ol.UserUrl );
                }
                else {
                    onlineBlock.Set( "onlineUser.Url", "javascript:void(0)" );
                }
                onlineBlock.Next();
            }
        }

        //------------------------------------------------------------------------------------

        private String getTadayTopics( int todayPosts ) {
            if (todayPosts > 0) {
                return string.Format( "<span class=\"note left5\">(" + alang( "today" ) + ":{0})</span>", todayPosts );
            }
            return string.Empty;
        }
    }
}

