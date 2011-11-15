﻿using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Members.Users.Domain;
using wojilu.Web.Context;
using wojilu.Web.Mvc.Utils;
using wojilu.Caching;
using wojilu.Apps.Blog.Domain;
using wojilu.Members.Sites.Domain;

namespace wojilu.Apps.Blog.Caching {

    public class MainCacher {

        public static void Update( int appId ) {


            String key = GetCacheKey( "Index" );

            String content = getMainCache( appId );

            CacheManager.GetApplicationCache().Put( key, content );

        }

        public static String GetCacheKey( String actionName ) {
            return typeof( BlogApp ).FullName + "_main_"+ actionName;
        }

        private static string getMainCache( int appId) {
            MvcContext ctx = MockContext.GetOne( Site.Instance, appId );
            String content = ControllerRunner.Run( "wojilu.Web.Controller.Blog.MainController", "Index", ctx );

            return content;
        }

    }

}
