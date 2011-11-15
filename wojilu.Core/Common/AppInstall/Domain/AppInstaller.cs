/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.Data;
using wojilu.ORM;
using wojilu.Members.Interface;

namespace wojilu.Common.AppInstall {


    public class AppInstaller : CacheObject {

        public AppInstaller() {
            this.Singleton = true;
        }

        public AppInstaller( int id ) {
            this.Id = id;
            this.Singleton = true;
        }

        /// <summary>
        /// ��������
        /// </summary>
        public int CatId { get; set; }

        /// <summary>
        /// app�����߻򴴽���˾
        /// </summary>
        public String Creator { get; set; }

        /// <summary>
        /// ���
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// logo
        /// </summary>
        public String Logo { get; set; }

        /// <summary>
        /// ��Ӧapp��������type����
        /// </summary>
        public String TypeFullName { get; set; }

        /// <summary>
        /// app״̬��Ĭ��0��ʾ���ã�1��ʾ���á�
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// �Ƿ����û���������(UGC)������У�Ӧ���ṩ���û��������ݵĺ�̨������档
        /// </summary>
        public Boolean HasUserData { get; set; }

        /// <summary>
        /// �Ƿ���(��װ��ʱ���Ƿ�ֻ�ܰ�װһ�����У����ǿ��԰�װ�������)
        /// </summary>
        public Boolean Singleton { get; set; }

        /// <summary>
        /// �������ID(ĳЩ�����ں���ͬ��������ͳ�ʼ�����ݲ�һ����ͨ���ƶ�������ID�����Թ��ൽͬһ��)
        /// </summary>
        public int ParentId { get; set; }


        /// <summary>
        /// �رշ�ʽ��Ĭ��0��ʾ��ֹ�û���װ��������Ѱ�װ�ˣ���������У�1��ʾ������ֹ��װ���Ѿ���װ��Ҳ��ֹ���С�
        /// </summary>
        public int CloseMode { get; set; }

        //-------------------------------------------------------------------------------------

        [NotSave]
        public String TypeName {
            get { return strUtil.GetTypeName( this.TypeFullName ); }
        }

        [NotSave]
        public String CatName {
            get { return AppCategory.GetNameById( this.CatId ); }
        }

        [NotSave]
        public String LogoImg {
            get { return this.Logo.Replace( "~img/", sys.Path.Img ); }
        }

        [NotSave]
        public String StatusName {

            get {

                if (this.Status == AppInstallerStatus.Stop.Id) return AppInstallerStatus.Stop.Name;

                //if (this.Status == AppInstallerStatus.Run.Id) return AppInstallerStatus.Run.Name;

                if (this.Status == AppInstallerStatus.Run.Id) {

                    if (this.CatId == AppCategory.General) {
                        return AppCategory.GetAllNameWithoutGeneral();
                    }
                    else {
                        return AppCategory.GetByCatId( this.CatId ).Name;
                    }

                }

                return AppMemberShip.GetStatusName( this.Id );

            }
        }

        [NotSave]
        public String StatusValue {

            get {

                if (this.Status == AppInstallerStatus.Stop.Id) return null;

                if (this.Status == AppInstallerStatus.Run.Id) {

                    if (this.CatId == AppCategory.General) {
                        return AppCategory.GetAllTypeNameWithoutGeneral();
                    }
                    else {
                        return AppCategory.GetByCatId( this.CatId ).TypeFullName;
                    }

                }

                return AppMemberShip.GetStatusTypeValue( this.Id );

            }

        }

        [NotSave]
        public String CloseModeName {
            get { return AppCloseMode.GetCloseModeName( this.CloseMode ); }
        }

        //-------------------------------------------------------------------------------------------------

        public Boolean IsInstanceClose( Type ownerType ) {

            if (this.CloseMode == AppCloseMode.CloseInstall.Id) return false; // ������ֹ��װ�����appʵ����������

            return this.IsClose( ownerType );
        }

        public Boolean IsClose( Type ownerType ) {


            if (this.Status == AppInstallerStatus.Stop.Id) return true;

            if (this.Status == AppInstallerStatus.Run.Id) { // Ĭ��״̬
                return !(this.CatId == AppCategory.General || AppCategory.GetByCatId( this.CatId ).TypeFullName.Equals( ownerType.FullName ));
            }

            // �Զ���
            return AppMemberShip.IsAppStop( this.Id, ownerType );
        }


    }


}

