﻿using System;
using System.Collections.Generic;
using System.Text;
using wojilu.Common.Spider.Domain;
using wojilu.Net;
using System.Text.RegularExpressions;
using wojilu.Web.Utils;
using System.Net;
using System.IO;
using wojilu.Drawing;

namespace wojilu.Common.Spider.Service {

    // 还要负责分页的部分
    public class PagedDetailSpider : DetailSpider {

        public override String GetContent( String url, SpiderTemplate s, StringBuilder sb ) {
            this._template = s;
            this._url = url;
            this._log = sb;
            return this.getPageContent();
        }

        private string getPageContent() {

            // 1) 抓取网页内容
            string page = getDetailPageBody( this._url, this._template, this._log );
            if (string.IsNullOrEmpty( page )) return null;

            // 2) 获取匹配的部分
            String matchedPage = getMatchedBody( page, this._template, this._log );
            if (string.IsNullOrEmpty( matchedPage )) return null;

            // 3) 图片处理
            if (this._template.IsSavePic == 1) {
                matchedPage = processPic( matchedPage, this._template.SiteUrl );
            }

            // 4) 处理分页
            matchedPage += getPagedContent( page, _url, _template, _log );

            return matchedPage;
        }

        private string getPagedContent( string page, string url, SpiderTemplate s, StringBuilder sb ) {
            StringBuilder pList = new StringBuilder();
            List<String> urls = getPagedUrl( page, url );
            for (int i = 0; i < urls.Count; i++) {
                pList.AppendLine( "<hr>" );
                String pageContent = new DetailSpider().GetContent( urls[i], s, sb );
                pList.Append( pageContent );
            }
            return pList.ToString();
        }


        private static List<String> getPagedUrl( String page, String url ) {

            String urlWithouExt = getUrlWithouExt( url );

            List<String> list = new List<string>();

            MatchCollection matchs = Regex.Matches( page, "<a href=\"(" + urlWithouExt + "[^\"]*?)\".+?\">", RegexOptions.Singleline );
            foreach (Match m in matchs) {
                String u = m.Groups[1].Value;
                if (u.Equals( url )) continue;
                if (list.Contains( u )) continue;
                list.Add( u );
            }

            return list;
        }

        private static String getUrlWithouExt( String url ) {
            int lastDot = url.LastIndexOf( '.' );
            int lastSlash = url.LastIndexOf( '/' );
            if (lastDot > lastSlash)
                return url.Substring( 0, lastDot );
            else
                return url;
        }

    }

}
