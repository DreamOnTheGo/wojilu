/*
 * Copyright 2010 www.wojilu.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections;
using wojilu.Members.Interface;

namespace wojilu.Web.Context {

    /// <summary>
    /// ��ǰ�����ߵĽӿ�
    /// </summary>
    public interface IViewerContext {

        int Id { get; set; }

        /// <summary>
        /// ��ǰ������
        /// </summary>
        IUser obj { get; set; }

        /// <summary>
        /// ����վ��˽��
        /// </summary>
        /// <param name="ownerName"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Result SendMsg( String ownerName, String title, String body );

        /// <summary>
        /// ��Ϊ����
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="msg"></param>
        Result AddFriend( int ownerId, String msg );

        /// <summary>
        /// ������˽���ã���ǰ viewer �� owner ��ĳ��item�Ƿ���з���Ȩ��
        /// </summary>
        /// <param name="owner">��������</param>
        /// <param name="item"></param>
        /// <returns></returns>
        Boolean HasPrivacyPermission( IMember owner, String item );

        /// <summary>
        /// �Ƿ���վ����Ա
        /// </summary>
        /// <returns></returns>
        Boolean IsAdministrator();

        /// <summary>
        /// �Ƿ��ǵ�ǰ���ʵ�owner�Ĺ���Ա
        /// </summary>
        /// <returns></returns>
        Boolean IsOwnerAdministrator( IMember owner );

        /// <summary>
        /// �Ƿ��ע��ĳ��
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Boolean IsFollowing( int ownerId );

        /// <summary>
        /// ��ĳ���Ƿ�������
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Boolean IsFriend( int ownerId );

        /// <summary>
        /// �Ƿ��Ѿ���¼
        /// </summary>
        Boolean IsLogin { get; set; }

        IList Menus { get; set; }
    }

}
