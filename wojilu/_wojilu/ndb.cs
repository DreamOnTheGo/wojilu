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
using System.Text;
using wojilu.ORM;
using wojilu.ORM.Caching;

namespace wojilu {

    /// <summary>
    /// �����˶���ĳ��� CRUD (��ȡ/����/����/ɾ��) �����������Ƿ���ʵ�֡���Ҫ����ĳЩ����ʹ�÷��͵ĳ��ϣ���̫���á�
    /// </summary>
    public class ndb {

        /// <summary>
        /// ���� id ��ѯ����
        /// </summary>
        /// <param name="t">���������</param>
        /// <param name="id">����� id</param>
        /// <returns></returns>
        public static IEntity findById( Type t, int id ) {

            if (id < 0) return null;

            IEntity objCache = ObjectPool.FindOne( t, id );
            if (objCache == null) {
                ObjectInfo state = new ObjectInfo( t );
                objCache = ObjectDB.FindById( id, state );
                ObjectPool.Add( objCache );
            }
            return objCache;
        }

        /// <summary>
        /// ��ѯ t ���Ͷ������������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IList findAll( Type t ) {
            ObjectInfo state = new ObjectInfo( t );
            state.includeAll();
            IList objList = ObjectPool.FindAll( t );
            if (objList == null) {
                objList = ObjectDB.FindAll( state );
                ObjectPool.AddAll( t, objList );
            }
            return objList;
        }

        /// <summary>
        /// ����������ѯ
        /// </summary>
        /// <param name="t"></param>
        /// <param name="condition">��ѯ����</param>
        /// <returns>���ز�ѯ����Query�����Խ�һ����������ֵ�����õ����</returns>
        public static Query find( Type t, String condition ) {
            ObjectInfo state = new ObjectInfo( t );
            return ObjectDB.Find( state, condition );
        }

        /// <summary>
        /// ���ݲ�ѯ���������ط�ҳ���ݼ���
        /// </summary>
        /// <param name="t"></param>
        /// <param name="condition">��ѯ����</param>
        /// <returns>��ҳ�����б�������ǰҳ���ܼ�¼������ҳ����</returns>
        public static IPageList findPage( Type t, String condition ) {
            return findPage( t, condition, -1 );
        }

        /// <summary>
        /// ���ݲ�ѯ���������ط�ҳ���ݼ���
        /// </summary>
        /// <param name="t"></param>
        /// <param name="condition">��ѯ����</param>
        /// <param name="pageSize">ÿҳ����</param>
        /// <returns>��ҳ�����б�������ǰҳ���ܼ�¼������ҳ����</returns>
        public static IPageList findPage( Type t, String condition, int pageSize ) {

            ObjectInfo state = new ObjectInfo( t );
            state.includeAll();
            if (pageSize > 0) state.Pager.setSize( pageSize );

            IList list = ObjectDB.FindPage( state, condition );
            IPageList result = new PageList();
            result.Results = list;
            result.PageCount = state.Pager.PageCount;
            result.RecordCount = state.Pager.RecordCount;
            result.Size = pageSize>0 ? pageSize: state.Pager.getSize();
            result.PageBar = state.Pager.PageBar;
            result.Current = state.Pager.getCurrent();
            return result;
        }

        /// <summary>
        /// ���� sql ��䣬��ѯ����
        /// </summary>
        /// <param name="t"></param>
        /// <param name="sql"></param>
        /// <returns>���ض����б�</returns>
        public static Object findBySql( Type t, String sql ) {

            IList objList = ObjectPool.FindBySql( sql, t );
            if (objList == null) {
                objList = ObjectDB.FindBySql( sql, t );
                ObjectPool.AddSqlList( sql, objList );
            }
            return objList;
        }

        /// <summary>
        /// ͳ�� t ���Ͷ��������������
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int count( Type t ) {
            return ObjectDB.Count( t );
        }

        /// <summary>
        /// ��������ͳ��������
        /// </summary>
        /// <param name="t"></param>
        /// <param name="condition">ͳ������</param>
        /// <returns></returns>
        public static int count( Type t, String condition ) {
            return ObjectDB.Count( t, condition );
        }

        /// <summary>
        /// ���� id ɾ������
        /// </summary>
        /// <param name="t"></param>
        /// <param name="objId">���� id</param>
        /// <returns>������Ӱ�������</returns>
        public static int delete( Type t, int objId ) {
            int num = ObjectDB.Delete( t, objId );
            ObjectPool.Delete( t, objId );
            return num;
        }
    }

}
