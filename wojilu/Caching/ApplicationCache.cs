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
using System.Web;
using System.Collections;

using wojilu.Data;
using wojilu.Caching;

namespace wojilu.Caching {


    /// <summary>
    /// Ӧ�ó���Χ�Ļ���(ORM�Ķ�������)
    /// </summary>
    public class ApplicationCache : IApplicationCache {

        /// <summary>
        /// �Ӷ��������л�ȡֵ
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object Get( String key ) {
            return SysCache.Get( key );
        }

        /// <summary>
        /// ���������������棬������������д�����滻
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Put( String key, Object val ) {
            SysCache.Put( key, val );
        }

        /// <summary>
        /// ��������뻺�棬���һ�η���֮��� minutes �����ڣ������û�з��ʣ������ڣ����Թ��ڣ�
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="minutes"></param>
        public void Put( String key, Object val, int minutes ) {
            SysCache.PutSliding( key, val, minutes * 60 );
        }

        /// <summary>
        /// �ӻ������Ƴ�ĳ��
        /// </summary>
        /// <param name="key"></param>
        public void Remove( String key ) {
            SysCache.Remove( key );
        }

    }
}

