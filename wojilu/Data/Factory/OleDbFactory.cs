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
using System.Data.OleDb;
using System.Data.Common;

namespace wojilu.Data {

    /// <summary>
    /// access ���ݹ�������ȡ Connection, Command, DataAdapter
    /// </summary>
    public class AccessFactory : DbFactoryBase {


        public override IDbConnection GetConnection( String connectionString ) {
            return new OleDbConnection( connectionString );
        }

        public override IDbCommand GetCommand( String CommandText ) {
            IDbCommand cmd = new OleDbCommand();
            cmd.Connection = cn;
            cmd.CommandText = CommandText;
            setTransaction( cmd );
            return cmd;
        }

        internal override IDatabaseChecker GetDatabaseChecker() {
            return new AccessDatabaseChecker();
        }

        public override IDatabaseDialect GetDialect() {
            return new AccessDialect();
        }

        public override Object SetParameter( IDbCommand cmd, String parameterName, Object parameterValue ) {

            parameterValue = base.processValue( parameterValue );
            parameterName = new AccessDialect().GetParameterAdder( parameterName );

            IDbDataParameter parameter;
            if (parameterValue is DateTime) {
                parameter = new OleDbParameter( parameterName, parameterValue.ToString() );
            }
            else {
                parameter = new OleDbParameter( parameterName, parameterValue );
            }
            cmd.Parameters.Add( parameter );

            return parameterValue;

        }

        public override DbDataAdapter GetAdapter() {
            return new OleDbDataAdapter( (OleDbCommand)cmd );
        }

        public override DbDataAdapter GetAdapter( String CommandText ) {
            return new OleDbDataAdapter( (OleDbCommand)GetCommand( CommandText ) );
        }
    }

}
