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
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using wojilu.Web;
using wojilu.Web.Mvc;
using wojilu.IO;
using System.Collections.Generic;

namespace wojilu {

    /// <summary>
    /// ��װ�� web �����³���·���� url �Ĳ���
    /// </summary>
    public class PathHelper {

        /// <summary>
        /// �����·��ת��Ϊ����·��
        /// </summary>
        /// <param name="path">���������·��</param>
        /// <returns>���ؾ���·��</returns>
        public static String Map( String relativePath ) {
            return PathTool.getInstance().Map( relativePath );
        }

        /// <summary>
        /// ������·��ƴ��Ϊ����·��(��һ��·�������Ǿ���·��)
        /// </summary>
        /// <param name="arrPath"></param>
        /// <returns></returns>
        public static String CombineAbs( String[] arrPath ) {
            return PathTool.getInstance().CombineAbs( arrPath );
        }

        private static Boolean IsFirstDomainEqual( String url1, String url2 ) {
            String host1 = new UriBuilder( url1 ).Host;
            String host2 = new UriBuilder( url2 ).Host;
            return host1.Equals( host2 );
        }

        /// <summary>
        /// ��ָ����path��ȥ����rootPath���֣�
        /// </summary>
        /// <param name="rootPath">��Ҫ�޳��ĸ�·��</param>
        /// <param name="pathFull">�������path</param>
        /// <returns>���ض��·���б�(���������ռ����ε��������ռ�)</returns>
        public static IList GetPathList( String rootPath, String pathFull ) {

            String mypath = strUtil.TrimStart( pathFull, rootPath );

            String[] arrPath;
            if (strUtil.HasText( mypath )) {
                mypath = mypath.TrimStart( '.' );
                arrPath = mypath.Split( '.' );
            }
            else
                arrPath = new String[] { };

            IList list = new ArrayList();
            list.Add( "" );
            String result = "";
            for (int i = 0; i < arrPath.Length; i++) {
                result = result + "." + arrPath[i];
                result = result.TrimStart( '.' );
                result = result.Replace( ".", "/" );
                list.Add( result );
            }

            IList results = new ArrayList();
            for (int i = list.Count - 1; i >= 0; i--) {
                results.Add( list[i].ToString() );
            }

            return results;
        }

        // ------------------------------ Url ---------------------------------------

        /// <summary>
        /// ���url�Ƿ�����(�Ƿ���http��ͷ������������ͷ)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Boolean IsFullUrl( String url ) {

            if (strUtil.IsNullOrEmpty( url )) return false;
            if (url.Trim().StartsWith( "/" )) return false;
            if (url.Trim().StartsWith( "http://" )) return true;

            String[] arrItem = url.Split( '/' );
            if (arrItem.Length < 1) return false;

            int dotIndex = arrItem[0].IndexOf( "." );
            if (dotIndex <= 0) return false;


            return hasCommonExt( arrItem[0] ) == false;

        }

        private static readonly List<String> extList = getExtList();

        private static List<String> getExtList() {
            String[] exts = { "htm", "html", "xhtml", "txt",
                                "jpg", "gif", "png", "jpg", "jpeg", "bmp", 
                                "doc", "docx", "ppt", "pptx", "xls", "xlsx", "chm", "pdf",
                                "zip", "7z", "rar", "exe", "dll", 
                                "mov", "wav", "mp3", "rm", "rmvb", "mkv", "avi",
                                "asp", "aspx", "php", "jsp"
                            };

            return new List<String>( exts );
        }

        /// <summary>
        /// �ж���ַ�Ƿ����������׺�������� .htm/.html/.aspx/.jpg/.doc/.avi ��
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool hasCommonExt( string str ) {

            int dotIndex = str.LastIndexOf( "." );
            String ext = str.Substring( dotIndex + 1, str.Length - dotIndex - 1 );
            return extList.Contains( ext );
        }

        /// <summary>
        /// �ж���ַ�Ƿ������׺�������� xyzz/ab.htm ������my/xyz/dfae3 �򲻰���
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool UrlHasExt( String url ) {


            if (strUtil.IsNullOrEmpty( url )) return false;
            String[] arrItem = url.Split( '/' );

            String lastPart = arrItem[arrItem.Length - 1];

            return lastPart.IndexOf( "." ) >= 0;
        }

        /// <summary>
        /// �Ƿ����ⲿ����
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Boolean IsOutUrl( String url ) {

            Boolean isFull = IsFullUrl( url );
            if (!isFull) return false;

            String targetHost = new UriBuilder( url ).Host;

            if (targetHost.Equals( SystemInfo.HostNoSubdomain )) return false;
            if (targetHost.IndexOf( SystemInfo.HostNoSubdomain ) >= 0) return false;
            return true;
        }

        private static Boolean IsHostSame( String url1, String url2 ) {
            String host1 = new UriBuilder( url1 ).Host;
            String host2 = new UriBuilder( url2 ).Host;
            return host1.Equals( host2 );
        }

        /// <summary>
        /// �޳��� url �ĺ�׺��
        /// </summary>
        /// <param name="rawUrl">ԭʼurl</param>
        /// <returns>���ر��޳�����׺���� url</returns>
        public static String TrimUrlExt( String rawUrl ) {
            if (strUtil.IsNullOrEmpty( rawUrl )) return rawUrl;
            int dotIndex = rawUrl.IndexOf( "." );
            if (dotIndex < 0) return rawUrl;

            String[] arrItem = rawUrl.Split( '.' );
            String ext = arrItem[arrItem.Length - 1];
            if (ext.IndexOf( '/' ) > 0) return rawUrl;
            return strUtil.TrimEnd( rawUrl, ext ).TrimEnd( '.' );
        }

        /// <summary>
        /// �ڲ����Ǻ�׺��������£��Ƚ�������ַ�Ƿ���ͬ
        /// </summary>
        /// <param name="url1"></param>
        /// <param name="url2"></param>
        /// <returns></returns>
        public static Boolean CompareUrlWithoutExt( String url1, String url2 ) {
            if (strUtil.IsNullOrEmpty( url1 ) && strUtil.IsNullOrEmpty( url2 )) return true;
            if (strUtil.IsNullOrEmpty( url1 ) || strUtil.IsNullOrEmpty( url2 )) return false;
            return TrimUrlExt( url1 ) == TrimUrlExt( url2 );
        }



    }
}

