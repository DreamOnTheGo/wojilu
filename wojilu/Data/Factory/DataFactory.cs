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
using System.Data;
using System.Data.Common;

namespace wojilu.Data {

    /// <summary>
    /// ���ݹ��������Բ��ÿ������ݿ�������ȡ Connection, Command, DataAdapter
    /// </summary>
    public class DataFactory {

        public static IDbConnection GetConnection( String connectionString, DatabaseType dbtype ) {
            return DbFactoryBase.Instance( dbtype ).GetConnection( connectionString );
        }

        public static IDbCommand GetCommand( String CommandText, IDbConnection cn ) {
            return DbFactoryBase.Instance( cn ).GetCommand( CommandText );
        }

        internal static IDatabaseChecker GetDatabaseChecker( DatabaseType dbtype ) {
            return DbFactoryBase.Instance( dbtype ).GetDatabaseChecker();
        }

        public static IDatabaseDialect GetDialect( DatabaseType dbtype ) {
            return DbFactoryBase.Instance( dbtype ).GetDialect();
        }

        public static Object SetParameter( IDbCommand cmd, String parameterName, Object parameterValue ) {
            return DbFactoryBase.Instance( cmd ).SetParameter( cmd, parameterName, parameterValue );
        }

        public static DbDataAdapter GetAdapter( IDbCommand cmd ) {
            return DbFactoryBase.Instance( cmd ).GetAdapter();
        }

        public static DbDataAdapter GetAdapter( String CommandText, IDbConnection cn ) {
            return DbFactoryBase.Instance( cn ).GetAdapter( CommandText );
        }


    }


}
