/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.Members.Interface;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.MemberApp.Interface;
using wojilu.Common.AppInstall;

namespace wojilu.Web.Context.Initor {

    public class AppInit : IContextInit {

        public void Init( MvcContext ctx ) {

            if (ctx.utils.isEnd()) return;

            initPrivate( ctx );

            if (ctx.app.obj != null) {

                // ���app�Ƿ�ͣ��
                Type appType = ctx.app.obj.GetType();
                AppInstaller installer = new AppInstallerService().GetByType( appType );
                if (installer == null || installer.IsInstanceClose( ctx.owner.obj.GetType() )) {
                    ctx.utils.endMsg( "�Բ��𣬱�app�Ѿ�ͣ��", HttpStatus.NotFound_404 );
                    return;
                }


                InitHelperFactory.GetHelper( ctx ).IsAppRunning( ctx ); // ���app�Ƿ�������ͣ״̬
            }

        }


        private void initPrivate( MvcContext ctx ) {

            IAppContext context = new AppContext();

            int appId = ctx.route.appId;
            context.Id = appId; // ID

            Type appType = ctx.controller.utils.getAppType();
            if (appType == null) {
                ctx.utils.setAppContext( context );
                return;
            }



            if (appId <= 0) {

                context.setAppType( appType );
                ctx.utils.setAppContext( context );
                return;
            }


            IApp app = getAppById( appType, appId, ctx.owner.obj );
            if (app == null) {
                ctx.utils.setAppContext( context );
                ctx.utils.endMsg( lang.get( "exAppNotFound" ) + ", appType=" + appType, HttpStatus.NotFound_404 );
            }
            else {

                context.obj = app; // objApp
                context.setAppType( app.GetType() ); // type
                ctx.utils.setAppContext( context ); 

                IAppStats stats = app as IAppStats;
                if (stats != null) {
                    refreshStats( stats );
                }

            }
        }

        private static IApp getAppById( Type appType, int appId, IMember owner ) {

            IApp app = ndb.findById( appType, appId ) as IApp;

            if (app == null) return null;
            if (app.OwnerId != owner.Id || owner.GetType().FullName.Equals( app.OwnerType ) == false) return null;

            return app;
        }

        private static void refreshStats( IAppStats app ) {

            if (DateTime.Now.Subtract( app.TodayTime ).Days > 0) {
                int num = app.TodayTopicCount + app.TodayPostCount;
                if (num > app.PeakPostCount) {
                    app.PeakPostCount = num;
                }
                app.TodayPostCount = 0;
                app.TodayTopicCount = 0;
                app.TodayVisitCount = 0;
                app.TodayTime = DateTime.Now;
                db.update( app );
            }

        }

    }

}
