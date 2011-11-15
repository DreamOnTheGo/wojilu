/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

using wojilu.ORM;
using wojilu.Members.Users.Domain;
using System.Collections.Generic;
using wojilu.Drawing;
using wojilu.Common.AppBase.Interface;

namespace wojilu.Common.Microblogs.Domain {


    [Serializable]
    public class Microblog : ObjectBase<Microblog>, IAppData {


        public User User { get; set; }

        public int ParentId { get; set; } // ת��΢��

        [LongText]
        public String Content { get; set; }

        [Column( Length = 40 )]
        public String Ip { get; set; }

        public int Replies { get; set; }
        public int Reposts { get; set; } // ת������

        public DateTime Created { get; set; }

        //-------------------------------------------------------------------

        public String PageUrl { get; set; } // ���ݵ���Դ��ַ��������Ƶ�Ĳ���ҳ��
        public String FlashUrl { get; set; }
        public String PicUrl { get; set; } // ��վ��ͼƬ��������Ƶ��ͼ


        //-------------------------------------------------------------------

        public String Pic { get; set; } // �洢�ڷ������ϵ��ϴ���ͼƬ


        [NotSave]
        public String PicMedium {
            get {
                if (isUserAvatar()) {
                    return sys.Path.GetAvatarOriginal( this.Pic );
                }
                else {
                    return sys.Path.GetPhotoThumb( this.Pic, ThumbnailType.Medium );
                }
            }
        }

        [NotSave]
        public String PicBig {
            get {
                if (isUserAvatar()) {
                    return sys.Path.GetAvatarThumb( this.Pic, ThumbnailType.Big );
                }
                else {
                    return sys.Path.GetPhotoThumb( this.Pic, ThumbnailType.Big );
                }
            }
        }

        [NotSave]
        public String PicOriginal {
            get {
                if (isUserAvatar()) {
                    return sys.Path.GetAvatarOriginal( this.Pic );
                }
                else {

                    return sys.Path.GetPhotoOriginal( this.Pic );
                }
            }
        }

        [NotSave]
        public String PicSmall {
            get {
                if (isUserAvatar()) {
                    return sys.Path.GetAvatarThumb( this.Pic, ThumbnailType.Medium );
                }
                else {
                    return sys.Path.GetPhotoThumb( this.Pic, ThumbnailType.Small );
                }
            }
        }

        private Boolean isUserAvatar() {
            if (this.Pic == null) return false;
            return this.Pic.IndexOf( "face/" ) > 0;
        }


        //-------------------------------------------------------------------------

        #region IAppData ��Ա

        [NotSave]
        public int AppId { get { return 0; } set { } }

        [NotSave]
        public User Creator { get { return this.User; } set { this.User = value; } }

        [NotSave]
        public string CreatorUrl { get { return this.User.Url; } set { } }

        [NotSave]
        public int OwnerId { get { return this.User.Id; } set { } }

        [NotSave]
        public string OwnerType { get { return typeof( User ).FullName; } set { } }

        [NotSave]
        public string OwnerUrl { get { return this.User.Url; } set { } }

        [NotSave]
        public string Title { get { return "΢��: " + strUtil.ParseHtml( this.Content, 50 ); } set { } }

        [NotSave]
        public int AccessStatus { get { return 0; } set { } }

        #endregion
    }

}
