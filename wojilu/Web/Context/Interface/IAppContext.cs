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
using wojilu.Common.Security;


namespace wojilu.Web.Context {

    /// <summary>
    /// Ӧ�ó��������Ľӿ�
    /// </summary>
    public interface IAppContext {

        Type getAppType();
        int Id { get; set; }

        /// <summary>
        /// ��ǰ app ������
        /// </summary>
        String Name { get; }
        ISecurity SecurityObject { get; set; }
        void setAppType( Type t );
        void setContext( MvcContext wctx );
        String Url { get; }

        /// <summary>
        /// ��ǰ app ����
        /// </summary>
        Object obj { get; set; }

    }

}
