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
using System.Reflection;
using System.Web;

using wojilu.IO;
using wojilu.Log;
using wojilu.Web;

namespace wojilu {

    /// <summary>
    /// ��־�������ͨ�����ڻ�ȡ��־����
    /// </summary>
    /// <example>
    /// һ������ĵ�һ�ж���
    /// <code>
    /// private static readonly ILog logger = LogManager.GetLogger( typeof( ObjectBase ) );
    /// </code>
    /// Ȼ�����������������ʹ��
    /// <code>
    /// logger.Info( "your message" );
    /// </code>
    /// </example>
    public class LogManager {
        
        private LogManager() {
        }

        /// <summary>
        /// ��ȡһ����־����
        /// </summary>
        /// <param name="type">��������</param>
        /// <returns>������־����</returns>
        public static ILog GetLogger( Type type ) {
            return GetLogger( type.FullName );
        }

        /// <summary>
        /// ��ȡһ����־����
        /// </summary>
        /// <param name="typeName">��������</param>
        /// <returns>������־����</returns>
        public static ILog GetLogger( String typeName ) {
            ILog log = getLogger();
            log.TypeName = typeName;
            return log;
        }

        private static ILog getLogger() {

            if (LogConfig.Instance.Level == LogLevel.None)
                return new NullLogger();

            if (strUtil.IsNullOrEmpty( LogConfig.Instance.LoggerImpl ))
                return new FileLogger();

            ILog log = null;
            String loggerImpl = LogConfig.Instance.LoggerImpl;
            if (strUtil.HasText( loggerImpl )) {
                String[] strArray = loggerImpl.Split( new char[] { ',' } );
                if (strArray.Length == 1) {
                    Type type = Type.GetType( strArray[0].Trim() );
                    if (type != null) {
                        log = rft.GetInstance( type ) as ILog;
                    }
                    return log;
                }
                if (strArray.Length == 2) {
                    log = Assembly.Load( strArray[1].Trim() ).CreateInstance( strArray[0].Trim() ) as ILog;
                }
            }
            return log;
        }

        /// <summary>
        /// ��������־����д�����(�� web �У���־����ҳ�����������ʱ���һ��д�뵽���̵�)
        /// </summary>
        public static void Flush() {

            if (!SystemInfo.IsWeb) return;
            LoggerUtil.Flush();
      }


    }
}

