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
using System.Data;
using System.Threading;

using wojilu.Web;
using wojilu.ORM;
using wojilu.ORM.Caching;

namespace wojilu.Data {

    /// <summary>
    /// ���ݿ������ģ���Ҫ���ڻ�ȡ���ݿ�����
    /// </summary>
    public class DbContext {

        /// <summary>
        /// ��ȡ���ݿ����ӣ����ص������Ѿ���(open)���� mvc ����в��ùرգ���ܻ��Զ��ر����ӡ�
        /// ֮����Ҫ���� Type����Ϊ ORM ֧�ֶ�����ݿ⣬��ͬ�������п���ӳ�䵽��ͬ�����ݿ⡣
        /// </summary>
        /// <param name="t">ʵ�������</param>
        /// <returns></returns>
        public static IDbConnection getConnection( Type t ) {
            return getConnection( Entity.GetInfo( t ) );
        }

        /// <summary>
        /// ��ȡ���ݿ����ӣ����ص������Ѿ���(open)���� mvc ����в��ùرգ���ܻ��Զ��ر����ӡ�
        /// ֮����Ҫ���� EntityInfo����Ϊ ORM ֧�ֶ�����ݿ⣬��ͬ�������п���ӳ�䵽��ͬ�����ݿ⡣
        /// </summary>
        /// <param name="et"></param>
        /// <returns></returns>
        public static IDbConnection getConnection( EntityInfo et ) {

            String db = et.Database;
            String connectionString = DbConfig.GetConnectionString( db );

            IDbConnection connection;
            getConnectionAll().TryGetValue( db, out connection );

            if (connection == null) {
                connection = DataFactory.GetConnection( connectionString, et.DbType );

                connection.Open();
                setConnection( db, connection );

                if (shouldTransaction()) {
                    IDbTransaction trans = connection.BeginTransaction();
                    setTransaction( db, trans );
                }

                return connection;
            }
            if (connection.State == ConnectionState.Closed) {
                connection.ConnectionString = connectionString;
                connection.Open();
            }
            return connection;
        }

        /// <summary>
        /// �ر����ݿ����ӡ���ΪORM֧�ֶ�����ݿ⣬�������п��ܵ����ݿ����Ӷ���һ��رա�
        /// </summary>
        public static void closeConnectionAll() {

            ContextCache.Clear();

            Dictionary<String, IDbConnection> dic = getConnectionAll();
            foreach (KeyValuePair<String, IDbConnection> kv in dic) {

                IDbConnection connection = kv.Value;
                if ((connection != null) && (connection.State == ConnectionState.Open)) {
                    connection.Close();
                    connection.Dispose();
                }
            }

            freeItem( _connectionKey );
        }

        //------------------------------------------------------------------------------

        /// <summary>
        /// ��ȡ���е����ݿ�����
        /// </summary>
        /// <returns></returns>
        public static Dictionary<String, IDbConnection> getConnectionAll() {

            Dictionary<String, IDbConnection> dic;

            dic = CurrentRequest.getItem( _connectionKey ) as Dictionary<String, IDbConnection>;
            if (dic == null) {
                dic = new Dictionary<String, IDbConnection>();
                CurrentRequest.setItem( _connectionKey, dic );
            }
            return dic;
        }

        private static Dictionary<String, IDbTransaction> getTransactionAll() {

            Dictionary<String, IDbTransaction> dic;
            dic = CurrentRequest.getItem( _transactionKey ) as Dictionary<String, IDbTransaction>;
            if (dic == null) {
                dic = new Dictionary<String, IDbTransaction>();
                CurrentRequest.setItem( _transactionKey, dic );
            }
            return dic;
        }

        private static void setConnection( String key, IDbConnection cn ) {
            getConnectionAll()[key] = cn;
        }

        private static void setTransaction( String key, IDbTransaction trans ) {
            getTransactionAll()[key] = trans;
        }

        //------------------------------------------------------------------------------

        public static void beginAndMarkTransactionAll() {
            CurrentRequest.setItem( _beginTransactionAll, true ); // �����ǰû�д򿪵����ݿ����ӣ�����ϱ�ǣ��������򿪵�ʱ����������
            beginTransactionAll();
        }


        private static bool shouldTransaction() {
            Object trans = CurrentRequest.getItem( _beginTransactionAll );
            if (trans == null) return false;
            return (Boolean)trans;
        }

        /// <summary>
        /// ����������ݿ����ӣ��������ݿ�����
        /// </summary>
        public static void beginTransactionAll() {
            Dictionary<String, IDbConnection> list = getConnectionAll();
            foreach (KeyValuePair<String, IDbConnection> kv in list) {
                IDbTransaction trans = kv.Value.BeginTransaction();
                setTransaction( kv.Key, trans );
            }
        }

        public static void setTransaction( IDbCommand cmd ) {
            Dictionary<String, IDbTransaction> transTable = getTransactionAll();
            foreach (KeyValuePair<String, IDbTransaction> kv in transTable) {
                IDbTransaction trans = kv.Value;

                if (cmd.Connection == trans.Connection) {
                    cmd.Transaction = trans;
                    return;
                }
            }
        }

        private static void clearTransactionAll() {
            Dictionary<String, IDbTransaction> dic = CurrentRequest.getItem( _transactionKey ) as Dictionary<String, IDbTransaction>;
            if (dic == null) return;
            dic.Clear();
            CurrentRequest.setItem( _transactionKey, dic );
        }

        /// <summary>
        /// �ύȫ�������ݿ�����
        /// </summary>
        public static void commitAll() {
            Dictionary<String, IDbTransaction> transTable = getTransactionAll();
            foreach (KeyValuePair<String, IDbTransaction> kv in transTable) {
                IDbTransaction trans = kv.Value;
                if (trans != null && trans.Connection != null ) trans.Commit();
            }
            clearTransactionAll();
        }

        /// <summary>
        /// �ع��������ݿ�����
        /// </summary>
        public static void rollbackAll() {
            Dictionary<String, IDbTransaction> transTable = getTransactionAll();
            foreach (KeyValuePair<String, IDbTransaction> kv in transTable) {
                IDbTransaction trans = kv.Value;
                if (trans != null && trans.Connection != null) trans.Rollback();
            }
            clearTransactionAll();
        }

        //------------------------------------------------------------------------------

        private static void freeItem( String key ) {
            if (!SystemInfo.IsWeb) {
                Thread.FreeNamedDataSlot( key );
            }
        }


        internal static IDictionary getContextCache() {

            Object dic = CurrentRequest.getItem( _contextCacheKey );
            if (dic == null) {
                dic = new Hashtable();
                CurrentRequest.setItem( _contextCacheKey, dic );
            }
            return dic as IDictionary;
        }

        /// <summary>
        /// ��ȡ�洢���������е� sql ִ�д���
        /// </summary>
        /// <returns></returns>
        public static int getSqlCount() {

            Object count = CurrentRequest.getItem( "sqlcount" );
            if (count == null) return 0;
            return cvt.ToInt( count );
        }

        private static readonly String _beginTransactionAll = "__beginTransactionAll";
        private static readonly String _connectionKey = "__wojiluConnection";
        private static readonly String _transactionKey = "__wojiluDbTransaction";
        private static readonly String _contextCacheKey = "__contextCacheDictionary";


    }
}

