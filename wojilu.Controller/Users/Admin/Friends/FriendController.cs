/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Members.Users.Service;
using wojilu.Members.Users.Domain;
using wojilu.Common.Onlines;
using wojilu.Members.Users.Interface;
using wojilu.Serialization;

namespace wojilu.Web.Controller.Users.Admin.Friends {

    public class FriendController : ControllerBase {

        public IFriendService friendService { get; set; }
        public IUserService userService { get; set; }
        public IFollowerService followService { get; set; }
        public IBlacklistService blacklistService { get; set; }

        public FriendController() {
            friendService = new FriendService();
            userService = new UserService();
            followService = new FollowerService();
            blacklistService = new BlacklistService();
        }

        public override void Layout() {

            set( "f.AddLink", to( Add ) );
            set( "categoryLink", to( new FriendCategoryController().List ) );
            set( "allLink", to( List, 0 ) );
            set( "blacklistLink", to( new BlacklistController().Index ) );

            List<FriendCategory> categories = FriendCategory.GetByOwner( ctx.owner.Id );
            bindCategories( categories );


        }

        public void SelectBox() {

            List<FriendShip> ulist = friendService.GetFriendsAll( ctx.owner.Id );
            set( "friendAll", getUserNames( ulist ) );

            List<FriendCategory> categories = FriendCategory.GetByOwner( ctx.owner.Id );
            bindSelectCategories( categories, ulist );


            DataPage<User> friends = friendService.GetFriendsPage( ctx.owner.Id, 50 );
            bindList( "list", "f", friends.Results );
            set( "page", friends.PageBar );


        }

        private String getUserNames( List<FriendShip> ulist ) {

            String strUsers = "";
            foreach (FriendShip ship in ulist) {

                User u;
                if (ship.User.Id == ctx.owner.Id)
                    u = ship.Friend;
                else
                    u = ship.User;

                strUsers += u.Name + ",";
            }
            strUsers = strUsers.TrimEnd( ',' );

            return strUsers;
        }

        private void bindSelectCategories( List<FriendCategory> categories, List<FriendShip> ulist ) {

            IBlock block = getBlock( "categories" );
            foreach (FriendCategory c in categories) {
                block.Set( "c.Name", c.Name );

                List<FriendShip> fslist = getByCategory( ulist, c );
                block.Set( "c.FriendList", getUserNames( fslist ) );

                block.Next();
            }
        }

        private List<FriendShip> getByCategory( List<FriendShip> ulist, FriendCategory c ) {

            List<FriendShip> results = new List<FriendShip>();
            foreach (FriendShip ship in ulist) {

                if (ship.User.Id == ctx.owner.Id && ship.CategoryId == c.Id) {

                    results.Add( ship );

                }
                else if (ship.Friend.Id == ctx.owner.Id && ship.CategoryIdFriend == c.Id) {
                    results.Add( ship );
                }

            }

            return results;
        }

        //---------------------------------------------------------------------------

        public void Search() {
            run( new Users.MainController().Search );
        }

        public void List( int categoryId ) {
            DataPage<FriendShip> list = friendService.GetPageByCategory( ctx.owner.Id, categoryId, 20 );
            bindFriendList( list, ctx.owner.Id );

            // �޸����
            List<FriendCategory> categories = FriendCategory.GetByOwner( ctx.owner.Id );
            dropList( "FriendCategory", categories, "Name=Id", 0 );
            set( "saveCategoryLink", to( SaveCategory ) );
            set( "categoryLink", to( new FriendCategoryController().List ) );

        }

        private void bindCategories( List<FriendCategory> categories ) {
            IBlock block = getBlock( "categories" );
            foreach (FriendCategory c in categories) {
                block.Set( "c.Name", c.Name );
                block.Set( "c.Link", to( List, c.Id ) );
                block.Next();
            }

            dropList( "FriendCategory", categories, "Name=Id", 0 );
        }

        [HttpPost, DbTransaction]
        public void SaveCategory() {

            int friendId = ctx.PostInt( "friendId" );
            int categoryId = ctx.PostInt( "categoryId" );
            String friendDescription = strUtil.CutString( ctx.Post( "friendDescription" ), 250 );

            if (categoryId <= 0) {
                echoError( "��ѡ�����򴴽��µķ���" );
                return;
            }

            friendService.UpdateCategory( ctx.owner.Id, friendId, categoryId, friendDescription );

            FriendCategory f = db.findById<FriendCategory>( categoryId );

            Dictionary<String, Object> dic = new Dictionary<string, object>();
            dic.Add( "Id", f.Id );
            dic.Add( "FriendId", friendId );
            dic.Add( "Name", f.Name );
            dic.Add( "Description", friendDescription );
            dic.Add( "IsValid", true );

            echoJson( JsonString.Convert( dic ) );
        }

        public void Add() {
            target( Create );
        }

        public void FollowingList() {
            DataPage<User> list = followService.GetFollowingPage( ctx.owner.Id );
            bindFriendList( list );
        }

        public void More() {

            int userId = ctx.owner.Id;

            List<int> friendIds = friendService.FindFriendsIdList( userId );

            List<User> flist = friendService.FindFriendsByFriends( userId, 20 );
            List<User> usertops = userService.GetRankedToMakeFriends( 20, friendIds );
            List<User> onlines = GetRecentToMakeFriends( userId, 20, friendIds );

            bindFriends( flist, "flist" );
            bindFriends( usertops, "top" );
            bindFriends( onlines, "online" );
        }

        public List<User> GetRecentToMakeFriends( int userId, int count, List<int> friendIds ) {
            List<OnlineUser> all = OnlineService.GetAll();

            String ids = "";
            int icount = 1;
            foreach (OnlineUser info in all) {

                if (icount > count) break;

                if (info.UserId > 0 && info.UserId != userId && friendIds.Contains( info.UserId ) == false) {
                    ids += info.UserId + ",";
                    count++;
                }
            }
            ids = ids.TrimEnd( ',' );

            if (strUtil.IsNullOrEmpty( ids )) return new List<User>();
            return db.find<User>( "Id in (" + ids + ")" ).list();
        }


        private void bindFriends( List<User> friends, String blockName ) {
            IBlock block = getBlock( blockName );
            foreach (User user in friends) {
                block.Set( "user.Name", user.Name );
                block.Set( "user.Face", user.PicSmall );
                block.Set( "user.Link", Link.ToMember( user ) );
                block.Set( "user.AddLink", Link.T2( user, new Users.FriendController().AddFriend, user.Id ) );
                block.Set( "user.FollowLink", Link.T2( user, new Users.FriendController().AddFollow, user.Id ) );
                block.Next();
            }
        }

        [HttpPost, DbTransaction]
        public void Create() {
            String target = ctx.Post( "Name" );
            if (strUtil.IsNullOrEmpty( target )) {
                errors.Add( lang( "exUserName" ) );
                run( Add );
                return;
            }
            User friend = userService.GetByName( target );
            if (friend == null) {
                errors.Add( lang( "exUser" ) );
                run( Add );
                return;
            }

            Result result = friendService.AddFriend( ctx.owner.Id, friend.Id, strUtil.CutString( ctx.Post( "Msg" ), 100 ) );
            if (result.HasErrors) {
                echoError( result );
            }
            else {
                echoRedirect( lang( "addFriendDone" ), to( List, 0 ) );
            }
        }

        [HttpDelete, DbTransaction]
        public void Delete( int id ) {
            friendService.DeleteFriend( ctx.owner.obj.Id, id );
            echoRedirectPart( lang( "opok" ) );
        }

        [HttpDelete, DbTransaction]
        public void DeleteFollowing( int id ) {
            followService.DeleteFollow( ctx.owner.obj.Id, id );
            echoRedirectPart( lang( "opok" ) );
        }

        private void bindFriendList( DataPage<User> list ) {
            IBlock block = getBlock( "list" );
            List<User> friends = list.Results;
            foreach (User user in friends) {
                block.Set( "m.Name", user.Name );
                block.Set( "m.FaceFull", user.PicMedium );
                block.Set( "m.UrlFull", Link.ToMember( user ) );
                block.Set( "m.DeleteUrl", to( Delete, user.RealId ) );
                block.Set( "m.DeleteFollowingLink", to( DeleteFollowing, user.RealId ) );

                block.Set( "m.CreateTime", user.Created );
                block.Set( "m.LastLoginTime", user.LastLoginTime );
                block.Next();
            }
            set( "page", list.PageBar );
        }

        private void bindFriendList( DataPage<FriendShip> list, int userId ) {

            IBlock block = getBlock( "list" );
            List<FriendShip> friends = list.Results;

            List<FriendCategory> cats = FriendCategory.GetByOwner( userId );

            foreach (FriendShip ship in friends) {

                if (ship.User == null || ship.Friend == null) continue;

                User friend;
                FriendCategory category;
                String description;

                if (ship.User.Id == userId) {
                    friend = ship.Friend;
                    category = getCategory( cats, ship.CategoryId );
                    description = ship.Description;
                }
                else {
                    friend = ship.User;
                    category = getCategory( cats, ship.CategoryIdFriend );
                    description = ship.DescriptionFriend;
                }

                String HideClass = "";
                if (friend.Id <= 0) HideClass = "hide";

                block.Set( "m.HideClass", HideClass );

                block.Set( "m.Id", friend.RealId );
                block.Set( "m.Name", friend.Name );

                block.Set( "m.FaceFull", friend.PicMedium );
                block.Set( "m.UrlFull", Link.ToMember( friend ) );
                block.Set( "m.DeleteUrl", to( Delete, friend.RealId ) );
                block.Set( "m.DeleteFollowingLink", to( DeleteFollowing, friend.RealId ) );

                block.Set( "m.CreateTime", friend.Created );
                block.Set( "m.LastLoginTime", friend.LastLoginTime );

                block.Set( "m.CategoryName", category == null ? lang( "none" ) : category.Name );
                block.Set( "m.CategoryId", category == null ? 0 : category.Id );

                block.Set( "m.Description", description );

                block.Next();
            }
            set( "page", list.PageBar );
        }

        private FriendCategory getCategory( List<FriendCategory> cats, int categoryId ) {
            if (categoryId <= 0) return null;
            foreach (FriendCategory c in cats) {
                if (c.Id == categoryId) return c;
            }
            return null;
        }


    }
}

