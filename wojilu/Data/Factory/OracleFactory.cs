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
using System.Data;
using System.Data.OracleClient;
using System.Data.Common;

namespace wojilu.Data {

    /// <summary>
    /// oracle ���ݹ�������ȡ Connection, Command, DataAdapter
    /// </summary>
    public class OracleFactory : DbFactoryBase {

        public override IDbConnection GetConnection( String connectionString ) {
            return new OracleConnection( connectionString );
        }

        public override IDbCommand GetCommand( String CommandText ) {
            IDbCommand cmd = new OracleCommand();
            cmd.Connection = cn;
            cmd.CommandText = CommandText;
            setTransaction( cmd );
            return cmd;
        }

        // TODO
        internal override IDatabaseChecker GetDatabaseChecker() {
            throw new Exception( lang.get( "dbNotSupport" ) );
        }

        // TODO
        public override IDatabaseDialect GetDialect() {
            throw new Exception( lang.get( "dbNotSupport" ) );
        }

        public override Object SetParameter( IDbCommand cmd, String parameterName, Object parameterValue ) {

            parameterValue = base.processValue( parameterValue );

            // TODO
            //parameterName = new SQLServerDialect().GetParameterAdder( parameterName );

            IDbDataParameter parameter = new OracleParameter( parameterName, parameterValue );
            cmd.Parameters.Add( parameter );

            return parameterValue;
        }

        public override DbDataAdapter GetAdapter() {
            return new OracleDataAdapter( (OracleCommand)cmd );
        }

        public override DbDataAdapter GetAdapter( String CommandText ) {
            return new OracleDataAdapter( (OracleCommand)GetCommand( CommandText ) );
        }

    }

}
