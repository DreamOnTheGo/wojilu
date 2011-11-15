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
using System.Text;
using System.IO;
using System.Reflection;
using wojilu.Serialization;

namespace wojilu.Web.Mvc.Utils {

    /// <summary>
    /// mvc 常用工具
    /// </summary>
    public class MvcUtil {

        /// <summary>
        /// 根据相对路径(相对于view的根目录，不包括扩展名)，加载模板
        /// </summary>
        /// <param name="relativePath">相对路径不包括扩展名(如果带了扩展名，也会被移除，然后用默认扩展名代替)</param>
        /// <returns></returns>
        public static ITemplate LoadTemplate( String relativePath ) {

            String pathWithoutExt = Path.ChangeExtension( relativePath, null );

            String templatePath = strUtil.Join( MvcConfig.Instance.ViewDir, pathWithoutExt );
            templatePath = templatePath.TrimEnd( '/' ).TrimEnd( '\\' );
            templatePath = templatePath + MvcConfig.Instance.ViewExt;

            String absPath = PathHelper.Map( templatePath );

            return new Template( absPath );
        }

        internal static String getParentViewPath( MethodInfo actionMethod, String rootNamespace ) {

            String declaringTypeNamespace = strUtil.TrimStart( actionMethod.DeclaringType.Namespace, rootNamespace );
            if (strUtil.HasText( declaringTypeNamespace ))
                declaringTypeNamespace = declaringTypeNamespace.TrimStart( '.' );

            declaringTypeNamespace = declaringTypeNamespace.Replace( ".", "/" );

            String t = strUtil.GetTypeName( actionMethod.DeclaringType );
            declaringTypeNamespace = strUtil.Join( declaringTypeNamespace, strUtil.TrimEnd( t, "Controller" ) );

            String filePath = strUtil.Join( declaringTypeNamespace, actionMethod.Name );
            return filePath;

        }

        /// <summary>
        /// 向客户端呈现 json 信息，并指出是否 valid
        /// </summary>
        /// <param name="htmlMsg"></param>
        /// <param name="isValid"></param>
        /// <param name="otherInfo"></param>
        /// <returns></returns>
        public static String renderValidatorJson( String htmlMsg, Boolean isValid, String otherInfo ) {

            StringBuilder builder = new StringBuilder();
            builder.Append( "{\"IsValid\":" );
            builder.Append( isValid ? "true" : "false" );
            builder.Append( ", \"Msg\":\"" );

            builder.Append( JsonString.ClearNewLine( htmlMsg ) );

            builder.Append( "\", \"Info\":\"" );
            builder.Append( JsonString.ClearNewLine( otherInfo ) );
            builder.Append( "\"}" );

            return builder.ToString();
        }


        internal static String getNoLayoutContent( String body ) {

            return getNoLayoutTemplate() + body + "<div style=\"display:none\">#{elapseTime}</div></body></html>";
        }



        private static string getNoLayoutTemplate() {
            String jsEnv = getJs( "jquery" ) + getJs( "lang." + wojilu.lang.getLangString() ) + getJs( "wojilu.nolayout" ) + getJs( "wojilu.common" ) + getJs( "wojilu.common.admin" );
            String cssEnv = getCss( "wojilu.common" ) + getCss( "wojilu.common.admin" );

            return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" + Environment.NewLine
                + "<html xmlns=\"http://www.w3.org/1999/xhtml\">" + Environment.NewLine
                + "<head>" + cssEnv + jsEnv + "</head>" + Environment.NewLine
                + "<body id=\"boxBody\">";
        }


        internal static String getFrameContent( String body ) {

            return getFrameTemplate() + body + "</body></html>";

        }

        private static String getFrameTemplate() {

            String jsEnv = getJs( "jquery" ) + getJs( "lang." + wojilu.lang.getLangString() ) + getJs( "wojilu.frame" ) + getJs( "wojilu.common" ) + getJs( "wojilu.common.admin" );
            String cssEnv = getCss( "wojilu.common" ) + getCss( "wojilu.common.admin" );

            return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" + Environment.NewLine
                + "<html xmlns=\"http://www.w3.org/1999/xhtml\">" + Environment.NewLine
                + "<head>" + cssEnv + jsEnv + "</head>" + Environment.NewLine
                + "<body id=\"boxBody\">";

        }

        private static String getCss( String path ) {
            return "<link href=\"" + sys.Path.Css + path + ".css?v=" + MvcConfig.Instance.CssVersion + "\" rel=\"stylesheet\" type=\"text/css\" />" + Environment.NewLine;
        }

        private static String getJs( String path ) {
            return "<script src=\"" + sys.Path.Js + path + ".js?v=" + MvcConfig.Instance.JsVersion + "\" type=\"text/javascript\"></script>" + Environment.NewLine;
        }




    }

}
