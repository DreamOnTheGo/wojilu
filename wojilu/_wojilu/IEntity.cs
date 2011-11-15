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
using System.Collections.Generic;
using System.Text;
using wojilu.ORM;
using wojilu.Reflection;

namespace wojilu {

    /// <summary>
    /// ���Ա� ORM �־û��Ķ��󣬶��Զ�ʵ���˱��ӿ�
    /// </summary>
    public interface IEntity {

        /// <summary>
        /// ÿһ���־û����󣬶�����һ�� Id ����
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// ��ȡ���Ե�ֵ(����ͨ�����䣬�ٶȽϿ�)
        /// </summary>
        /// <param name="propertyName">��������</param>
        /// <returns></returns>
        Object get( String propertyName );

        /// <summary>
        /// �������Ե�ֵ(����ͨ�����䣬�ٶȽϿ�)
        /// </summary>
        /// <param name="propertyName">��������</param>
        /// <param name="propertyValue">���Ե�ֵ</param>
        void set( String propertyName, Object propertyValue );

        /// <summary>
        /// ���������Ԫ���ݣ��Լ��ڶ����ѯ��ʱ����Ҫ�Ķ�����Ϣ��������
        /// </summary>
        //ObjectInfo state { get; set; }
    }



}
