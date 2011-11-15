/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;
using System.Collections.Generic;

using wojilu.ORM;
using wojilu.Web;
using wojilu.Serialization;
using wojilu.Common.Resource;
using wojilu.Common.AppBase.Interface;
using wojilu.Common.AppBase;

namespace wojilu.Apps.Content.Domain {

    // TODO 每个区块可以选择风格

    [Serializable]
    public class ContentApp : ObjectBase<ContentApp>, IApp, IAccessStatus {

        public ContentApp() {
            this.Style = "#row1_column1 {width:48%;margin:5px 5px 5px 10px;}" + Environment.NewLine
                 + "#row1_column2 {width:48%;margin:5px;}";
        }

        public int OwnerId { get; set; }
        public String OwnerUrl { get; set; }
        public String OwnerType { get; set; }

        // 可视化修改css
        [LongText]
        public String Style { get; set; }

        // 手动定义css样式
        [LongText]
        public String SkinStyle { get; set; }

        private int _skinId;
        // 皮肤
        public int SkinId {
            get { 
                if (_skinId == 0) return 1; 
                return _skinId; 
            }
            set { _skinId = value; }
        } 

        public String Layout { get; set; }

        [TinyInt]
        public int AccessStatus { get; set; }
        public DateTime Created { get; set; }

        [NotSave]
        public string[] RowList {
            get {
                if (strUtil.IsNullOrEmpty( Layout )) {
                    initLayoutString();
                }
                return Layout.Split( new char[] { ',', '/', '|' } );
            }
        }

        public String SubmitterRole { get; set; }

        public ContentSubmitterRole GetSubmitterRoleObj() {
            if (strUtil.IsNullOrEmpty( this.SubmitterRole )) return new ContentSubmitterRole();
            ContentSubmitterRole s = JSON.ToObject<ContentSubmitterRole>( this.SubmitterRole );
            return s;
        }

        public String Settings { get; set; }

        public ContentSetting GetSettingsObj() {
            if (strUtil.IsNullOrEmpty( this.Settings )) return new ContentSetting();
            ContentSetting s = JSON.ToObject<ContentSetting>( this.Settings );
            s.SetDefaultValue();
            return s;
        }

        private void initLayoutString() {
            this.Layout = "2";
            this.update( "Layout" );
        }



    }
}

