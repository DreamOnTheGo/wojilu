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
using System.Web;
using System.Web.Caching;

namespace wojilu.Caching {

    /// <summary>
    /// .net �Դ��� InMemory ����
    /// </summary>
    public class SysCache {

        /// <summary>
        /// �ӻ����л�ȡֵ
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Object Get( String key ) {
            return HttpRuntime.Cache[key];
        }

        /// <summary>
        /// ��������뻺�棬������������д�����滻��a)�������ڣ�b)���ȼ�Ϊ Normal��c)û�л���������
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void Put( String key, Object val ) {
            HttpRuntime.Cache[key] = val;
        }

        /// <summary>
        /// ��������뻺�棬�ڲ��� seconds ָ��������֮�����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="seconds"></param>
        public static void Put( String key, Object val, int seconds ) {
            HttpRuntime.Cache.Insert( key, val, null, DateTime.UtcNow.AddSeconds( (double)seconds ), Cache.NoSlidingExpiration );
        }

        /// <summary>
        /// ��������뻺�棬�����һ�η���֮��� seconds ����֮����ڣ����Թ��ڣ�
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="seconds"></param>
        public static void PutSliding( String key, Object val, int seconds ) {
            HttpRuntime.Cache.Insert( key, val, null, Cache.NoAbsoluteExpiration, new TimeSpan( 0, 0, seconds ) );
        }

        /// <summary>
        /// �ӻ������Ƴ�ĳ��
        /// </summary>
        /// <param name="key"></param>
        public static void Remove( String key ) {
            if (strUtil.HasText( key )) {
                HttpRuntime.Cache.Remove( key );
            }
        }


    }
}
