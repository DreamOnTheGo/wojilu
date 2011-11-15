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
using System.Text;
using System.Collections.Specialized;
using System.Collections.Generic;

using wojilu.Web.Mvc;

namespace wojilu.Web.GlobalApp {

    /// <summary>
    /// 全局异常处理
    /// </summary>
    public abstract class AppGlobalHelper {

        protected HttpApplication app;
        protected Exception ex;

        public abstract void LogError( Boolean responseError );
        public abstract void MailError();
        public abstract void ClearError();

        public static AppGlobalHelper New( Object sender ) {

            HttpApplication app = sender as HttpApplication;
            if (app == null) return new AppGlobalNull();
            if (app.Server.GetLastError() == null) return new AppGlobalNull();

            return new AppGlobalLogger( app );
        }

        protected StringBuilder getErrorInfo( HttpApplication app ) {

            ex = app.Server.GetLastError().GetBaseException();
            ex = wrapStaticFileException( ex );

            if (app.Request == null) throw ex;


            StringBuilder sb = new StringBuilder();
            String rurl = app.Request.Url.ToString();
            sb.AppendLine( "url=" + rurl );
            sb.AppendLine( "ex.Message=" + ex.Message );
            appendPostValues( "ex.PostedValue=", app.Request.Form, sb );
            sb.AppendLine( "ex.Source=" + getExSource() );
            sb.AppendLine( "ex.StackTrace=" + getExStackTrace() );
            return sb;
        }

        private static Exception wrapStaticFileException( Exception ex ) {

            if (ex.TargetSite.DeclaringType.FullName.Equals( "System.Web.StaticFileHandler" ) == false) return ex;

            MvcException mvcEx = new MvcException( lang.get( "NotFound404" ), ex.InnerException == null ? ex : ex.InnerException );
            mvcEx.Status = HttpStatus.NotFound_404;
            return mvcEx;
        }

        private static void appendPostValues( String key, NameValueCollection postValue, StringBuilder sb ) {

            if (postValue.Count <= 0) return;

            sb.Append( key );
            for (int i = 0; i < postValue.Count; i++) {
                sb.Append( postValue.GetKey( i ) );
                sb.Append( "=" );
                sb.Append( postValue[i] );
                sb.Append( ";" );
            }
            sb.AppendLine();
        }

        private String getExSource() {
            return strUtil.HasText( ex.Source ) ? ex.Source : ex.InnerException.Source;
        }

        private String getExStackTrace() {
            return strUtil.HasText( ex.StackTrace ) ? ex.StackTrace : ex.InnerException.StackTrace;
        }

        protected String getXHtmlTemplate() {

            String exPath = PathHelper.Map( MvcConfig.Instance.GetErrorTemplatePath() );
            if (file.Exists( exPath )) return file.Read( exPath );

            return getDefaultXHtmlTemplate();
        }

        private static string getDefaultXHtmlTemplate() {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine( "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" );
            sb.AppendLine( "<html xmlns=\"http://www.w3.org/1999/xhtml\">" );
            sb.AppendLine( "<head>" );
            sb.AppendLine( "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />" );
            sb.AppendLine( "<title>#{pageTitle}</title>" );
            sb.AppendLine( "<style>" );
            sb.AppendLine( "body {font-size:14px;line-height:150%;font-family:verdana;}" );
            sb.AppendLine( "</style>" );
            sb.AppendLine( "</head>" );
            sb.AppendLine( "<body>" );
            sb.AppendLine( "<h2>#{exTitle}</h2>" );
            sb.AppendLine( "<div>#{navLink}</div>" );
            sb.AppendLine( "<div style=\"width:95%;border:1px #ccc solid;padding:15px;font-size:12px;color:#333;background:#f2f2f2;\">#{exDetail}</div>" );
            sb.AppendLine( "</body></html>" );

            return sb.ToString();
        }

        //----------------------------------------------------------------------------------------------

        protected Boolean isLogError() {

            int statusCode = getErrorStatus();
            if (statusCode <= 0) return true;

            List<int> noLogStatus = getNoLogStatusCode();
            if (noLogStatus.Contains( statusCode )) return false;
            return true;
        }

        private int getErrorStatus() {

            if (ex != null) {
                MvcException mvcEx = ex as MvcException;
                if (mvcEx != null && mvcEx.hasStatus()) {
                    return mvcEx.getStatusCode();
                }
            }

            return 0;
        }

        private List<int> getNoLogStatusCode() {
            return MvcConfig.Instance.getNoLogStatusCode();
        }

        //----------------------------------------------------------------------------------------------

        protected void addHttpStatus() {

            int customStatusCode = getErrorStatus();
            if (customStatusCode > 0) {
                app.Response.Status = HttpStatus.GetStatusString( customStatusCode );
                return;
            }

            // 其余统一使用400错误码
            app.Response.Status = HttpStatus.BadRequest_400;
        }


    }
}
