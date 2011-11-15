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
using System.Collections.Generic;
using System.Text;

namespace wojilu.ORM {

    /// <summary>
    /// ���Ͳ�ѯ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class xQuery<T> {

        private Query _q;

        public xQuery( Query query ) {
            _q = query;
        }

        /// <summary>
        /// ����ѯ�����еĲ�����ֵ
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="val">����ֵ</param>
        /// <returns></returns>
        public xQuery<T> set( String name, Object val ) {
            _q.set( name, val );
            return this;
        }

        /// <summary>
        /// ���ز�ѯ�����н��
        /// </summary>
        /// <returns></returns>
        public List<T> list() {
            return this.list( -1 );
        }

        /// <summary>
        /// ���ط��ϲ�ѯ������ǰ n �����
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T> list( int count ) {
            IList list = _q.list( count );
            return db.getResults<T>( list );
        }

        /// <summary>
        /// ���ط��ϲ�ѯ�����ĵ�һ�����
        /// </summary>
        /// <returns></returns>
        public T first() {
            Object obj = _q.first();
            return (T)obj;
        }

        /// <summary>
        /// ͳ�Ʒ��ϲ�ѯ�����Ľ������
        /// </summary>
        /// <returns></returns>
        public int count() {
            return _q.count();
        }

        /// <summary>
        /// (������������ʹ��)ֻ��ѯָ�������ԣ���������������ܣ����ͻ�������ͻ��
        /// </summary>
        /// <param name="propertyString"></param>
        /// <returns></returns>
        public xQuery<T> select( String propertyString ) {
            _q.select( propertyString );
            return this;
        }

        /// <summary>
        /// �����н�������ĳ��ʵ�����Է�װ�ɼ��Ϸ���
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public List<T> listChildren<T>( String propertyName ) {
            IList list = _q.listChildren( propertyName );
            return db.getResults<T>( list );
        }

        /// <summary>
        /// ��ȡ���н����ĳ�����Ե��ַ������ϣ����� get( "Id" ) ���� "2, 7, 16, 25"
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public String get( String propertyName ) {
            return _q.get( propertyName );
        }

    }

}
