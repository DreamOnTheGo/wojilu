/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Web.Mvc;
using wojilu.Apps.Reader.Interface;
using wojilu.Apps.Reader.Service;
using wojilu.Apps.Reader.Domain;
using wojilu.Members.Users.Domain;

namespace wojilu.Web.Controller.Reader {

    public class LayoutController : ControllerBase {

        public IFeedCategoryService categoryService { get; set; }
        public ISubscriptionService subscribeService { get; set; }

        public LayoutController() {
            categoryService = new FeedCategoryService();
            subscribeService = new SubscriptionService();
        }

        public override void Layout() {

            IBlock adminblock = getBlock( "admin" );
            if (canAdmin()) {
                adminblock.Set( "categoryAdmin", to( new Admin.FeedCategoryController().List ) );
                adminblock.Set( "addSubscription", to( new Admin.SubscriptionController().Index ) );
                adminblock.Next();
            }

            set( "rssHome", to( new ReaderController().Index ) );

            IBlock cblock = getBlock( "categoryList" );
            List<FeedCategory> categories = categoryService.GetByApp( ctx.app.Id );
            
            List<Subscription> slist = subscribeService.GetByApp( ctx.app.Id );

            foreach (FeedCategory category in categories) {
                cblock.Set( "category.Title", category.Name );
                cblock.Set( "category.Id", category.Id );
                cblock.Set( "category.Url", to( new CategoryController().Show, category.Id ) );

                IBlock fblock = cblock.GetBlock( "list" );
                List<Subscription> feeds = subscribeService.GetByCategoryId( slist, category.Id );
                foreach (Subscription f in feeds) {
                    fblock.Set( "feed.Title", f.Name );
                    fblock.Set( "feed.Id", f.FeedSource.Id );
                    fblock.Set( "feed.ItemCount", f.FeedSource.EntryCount );
                    fblock.Set( "feed.Url", to( new SubscriptionController().Show, f.Id ) );
                    fblock.Next();
                }

                cblock.Next();
            }

        }

        private Boolean canAdmin() {
            return (ctx.viewer.Id == ctx.owner.Id && ctx.owner.obj is User);
        }



    }

}
