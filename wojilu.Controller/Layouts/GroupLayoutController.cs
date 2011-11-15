/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System.Collections;
using System.Collections.Generic;
using System.Text;

using wojilu.Web.Mvc;
using wojilu.Web.Url;

using wojilu.Members.Interface;
using wojilu.Members.Sites.Service;
using wojilu.Members.Sites.Domain;
using wojilu.Members.Users.Domain;
using wojilu.Members.Groups.Service;
using wojilu.Members.Groups.Domain;

using wojilu.Common.Skins;

using wojilu.Web.Controller.Forum.Utils;
using wojilu.Common.Menus.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Members.Groups.Interface;
using wojilu.Web.Controller.Groups.Admin;
using wojilu.Members.Sites.Interface;

namespace wojilu.Web.Controller.Layouts {

    public partial class GroupLayoutController : ControllerBase {

        public IMenuService groupMenuService { get; set; }
        public IMemberAppService groupAppService { get; set; }

        public SkinService skinService { get; set; }
        public IGroupService groupService { get; set; }
        public IGroupFriendService gfService { get; set; }
        public IMemberGroupService mgrService { get; set; }
        public ISiteSkinService siteSkinService { get; set; }

        public GroupLayoutController() {

            groupMenuService = new GroupMenuService();
            groupAppService = new GroupAppService();

            groupService = new GroupService();
            mgrService = new MemberGroupService();
            gfService = new GroupFriendService();
            skinService = new SkinService();
            siteSkinService = new SiteSkinService();
        }

        public override void Layout() {

            load( "topNav", new TopNavController().Index );

            bindSiteInfo();
            bindSiteSkin();
            bindSkin();

            Group group = ctx.owner.obj as Group;
            groupService.AddHits( group );

            IList list = groupMenuService.GetList( ctx.owner.obj );
            bindGroupNav( list );

            set( "group.Logo", group.LogoSmall ); // logo

            // 论坛属性(公开/半公开/秘密)
            set( "group.AccessStatusStr", group.GetAccessString() );
            set( "g.JoinTool", getJoinCmd( group ) );
            set( "g.Description", group.Description.Replace( "\n", "<br/>" ) );
            set( "g.MemberCount", group.MemberCount );
            set( "g.MemberList", t2( new Groups.MemberController().List ) );

            // 群组统计信息
            bindGroupStats( group ); 

            IList newMember = mgrService.GetNewMember( group.Id, 12 );
            bindNewMemberList( newMember );

            IList friends = gfService.GetFriends( group.Id, 8 );
            bindFriends( friends );

            set( "customSkinLink", to( new Groups.Admin.SkinController().CustomBg ) );

        }


        public void AdminLayout() {

            load( "topNav", new TopNavController().Index );
            load( "header", new TopNavController().Header );

            bindSiteInfo();
            bindSiteSkin();
            bindSkin();

            Group group = ctx.owner.obj as Group;
            set( "group.Logo", group.LogoSmall );

            bindGroupStats( group );
            bindGroupAdminHeader( group );
        }

        private void bindGroupAdminHeader( Group g ) {

            set( "lnkHome", Link.To( new Groups.Admin.MainController().Index ) );
            set( "lnkBaseInfo", Link.To( new Groups.Admin.MainController().Index ) );
            set( "lnkLogo", Link.To( new Groups.Admin.MainController().Logo ) );
            set( "lnkMember", Link.To( new Groups.Admin.MainController().Members, 0 ) );
            set( "lnkInvite", Link.To( new Groups.Admin.InviteController().Add ) );

            set( "lnkSkin", Link.To( new Groups.Admin.SkinController().My ) );

            set( "lnkLog", Link.To( new Groups.Admin.MainController().AdminLog ) );
            set( "lnkFriends", Link.To( new Groups.Admin.FriendController().Index ) );

            set( "lnkAppAdmin", Link.T2( ctx.owner.obj, new AppController().Index ) );
            set( "lnkMenuAdmin", Link.T2( ctx.owner.obj, new MenuController().Index ) );
            set( "urlList", Link.T2( ctx.owner.obj, new GLinkController().Index ) );

            IList apps = groupAppService.GetByMember( ctx.owner.Id );
            bindAppList( apps );
        }


    }

}
