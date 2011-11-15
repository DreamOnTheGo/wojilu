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
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Text;

using wojilu;
using wojilu.IO;
using wojilu.ORM;
using wojilu.Web;

namespace wojilu.Log {

    /// <summary>
    /// ��־������
    /// </summary>
    public class LoggerUtil {

        private static Object objLock = new object();

        /// <summary>
        /// sql ��־��ǰ׺
        /// </summary>
        public static readonly String SqlPrefix = "sql=";

        /// <summary>
        /// �� web ϵͳ�У���¼ sql ִ�еĴ���
        /// </summary>
        public static void LogSqlCount() {

            if (CurrentRequest.getItem( "sqlcount" ) == null) {
                CurrentRequest.setItem( "sqlcount", 1 );
            }
            else {
                CurrentRequest.setItem( "sqlcount", ((int)CurrentRequest.getItem( "sqlcount" )) + 1 );
            }
        }

        /// <summary>
        /// ����־д�����
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteFile( ILogMsg msg ) {

            if (SystemInfo.IsWeb == false) {
                writeFilePrivate( msg );
                return;
            }

            StringBuilder sb = CurrentRequest.getItem( "currentLogList" ) as StringBuilder;
            if (sb == null) {
                sb = new StringBuilder();
                CurrentRequest.setItem( "currentLogList", sb );
            }

            sb.AppendFormat( "{0} {1} {2} - {3} \r\n", msg.LogTime, msg.LogLevel, msg.TypeName, msg.Message );

        }

        private static void writeFilePrivate( ILogMsg msg ) {

            String formatMsg = GetFormatMsg( msg );
            writeContentToFile( formatMsg );
        }

        private static void writeContentToFile( String formatMsg ) {

            String logFilePath = LogConfig.Instance.FilePath;
            lock (objLock) {
                if (wojilu.IO.File.Exists( logFilePath )) {
                    DateTime lastAccessTime = System.IO.File.GetLastWriteTime( logFilePath );
                    DateTime now = DateTime.Now;

                    if (cvt.IsDayEqual( lastAccessTime, now )) {
                        wojilu.IO.File.Append( logFilePath, formatMsg );
                    }
                    else {
                        String destFileName = getDestFileName( logFilePath );
                        wojilu.IO.File.Move( logFilePath, destFileName );
                        wojilu.IO.File.Write( logFilePath, formatMsg );
                    }
                }
                else {
                    if (Directory.Exists( Path.GetDirectoryName( logFilePath ) ) == false) {
                        Directory.CreateDirectory( Path.GetDirectoryName( logFilePath ) );
                    }
                    wojilu.IO.File.Write( logFilePath, formatMsg );
                }
            }
        }

        private static String getDestFileName( string logFilePath ) {
            String ext = Path.GetExtension( logFilePath );
            String pathWithoutExt = strUtil.TrimEnd( logFilePath, ext );
            return pathWithoutExt + "_" + DateTime.Now.Subtract( TimeSpan.FromDays( 1 ) ).ToString( "yyyy.MM.dd" ) + ext;
        }

        public static String GetFormatMsg( ILogMsg logMsg ) {
            return String.Format( "{0} {1} {2} - {3} \r\n", logMsg.LogTime, logMsg.LogLevel, logMsg.TypeName, logMsg.Message );
        }

        /// <summary>
        /// ��������־����д�����
        /// </summary>
        internal static void Flush() {

            StringBuilder sb = CurrentRequest.getItem( "currentLogList" ) as StringBuilder;

            if (sb != null)
                writeContentToFile( sb.ToString() );
        }


    }
}

