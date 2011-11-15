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
using System.Web;
using System.Text.RegularExpressions;
using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Web;

namespace wojilu {

    /// <summary>
    /// ��װ�� ORM ��ҳ��ѯ�Ľ����
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    [Serializable]
    public class DataPage<T> : IPageList {

        public DataPage() {
        }

        public DataPage( IPageList list ) {
            this.Current = list.Current;
            this.Size = list.Size;
            this.PageCount = list.PageCount;
            this.PageBar = list.PageBar;
            this.RecordCount = list.RecordCount;
            this.Results = db.getResults<T>( list.Results );
        }

        public void CopyStats( IPageList list ) {
            this.Current = list.Current;
            this.Size = list.Size;
            this.PageCount = list.PageCount;
            this.PageBar = list.PageBar;
            this.RecordCount = list.RecordCount;
        }

        private int _current;
        private String _pageBar;
        private int _pageCount;
        private int _recordCount;
        private List<T> _results;
        private int _size;

        /// <summary>
        /// ��ǰҳ��
        /// </summary>
        public int Current {
            get { return _current; }
            set { _current = value; }
        }

        /// <summary>
        /// ÿҳ������
        /// </summary>
        public int Size {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// ��ת���� html ��ҳ��(Ҳ�����Զ���)
        /// </summary>
        public String PageBar {
            get { return _pageBar; }
            set { _pageBar = value; }
        }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        public int PageCount {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        /// <summary>
        /// �ܼ�¼��
        /// </summary>
        public int RecordCount {
            get { return _recordCount; }
            set { _recordCount = value; }
        }

        /// <summary>
        /// ��ѯ�����������б�
        /// </summary>
        public List<T> Results {
            get { return _results; }
            set { _results = value; }
        }

        /// <summary>
        /// ���ؿյķ�ҳ�����
        /// </summary>
        /// <returns></returns>
        public static DataPage<T> GetEmpty() {
            DataPage<T> p = new DataPage<T>();
            p.Results = new List<T>();
            p.Current = 1;
            p.RecordCount = 0;
            return p;
        }


        System.Collections.IList IPageList.Results {
            get { return new System.Collections.ArrayList(); }
            set {
            }
        }

        /// <summary>
        /// ��������б�ķ�ҳ��
        /// </summary>
        /// <param name="recentLink">��������б���ַ(����ҳ��)</param>
        /// <param name="archiveLink">�浵�����б���ַ(����ҳ��)</param>
        /// <param name="recentPageCount">��������б���Ҫչʾ��ҳ��</param>
        /// <returns></returns>
        public String GetRecentPage( String recentLink, String archiveLink, int recentPageCount ) {
            return GetRecentPage( recentLink, archiveLink, recentPageCount, recentPageCount );
        }

        /// <summary>
        /// ��������б�ķ�ҳ��
        /// </summary>
        /// <param name="recentLink">��������б���ַ(����ҳ��)</param>
        /// <param name="archiveLink">�浵�����б���ַ(����ҳ��)</param>
        /// <param name="recentPageCount">��������б���Ҫչʾ��ҳ��</param>
        /// <param name="pageWidth">��ҳ�����</param>
        /// <returns></returns>
        public String GetRecentPage( String recentLink, String archiveLink, int recentPageCount, int pageWidth ) {

            StringBuilder sb = new StringBuilder();
            sb.Append( "<div class=\"turnpage\">" );

            // prev
            if (this.Current > 1) {
                sb.AppendFormat( "<a href=\"{0}\">&lt;{1}</a>&nbsp;", Link.AppendPage( recentLink, this.Current - 1 ), lang.get( "prevPage" ) );
            }

            // page number
            int startNo;
            int pp = this.Current / pageWidth;
            if (this.Current % pageWidth == 0) {
                startNo = (pp - 1) * pageWidth + 1;
            }
            else {
                startNo = pp * pageWidth + 1;
            }

            if (startNo > this.PageCount) startNo = 1;

            int endNo = startNo + pageWidth - 1;
            if (endNo >= this.PageCount) endNo = this.PageCount;
            if (endNo > recentPageCount) endNo = recentPageCount;

            for (int i = startNo; i <= endNo; i++) {
                String pstyle = this.Current == i ? "currentPageNo" : "pageNo";
                sb.AppendFormat( "<a href=\"{0}\" class=\"{1}\">{2}</a>&nbsp;", Link.AppendPage( recentLink, i ), pstyle, i );
            }

            // next
            if (this.Current + 1 > this.PageCount) {
            }
            else if (this.Current + 1 > recentPageCount) {
                int nextPage = this.PageCount - recentPageCount;
                sb.AppendFormat( "<a href=\"{0}\">{1}&gt;</a>&nbsp;", Link.AppendPage( archiveLink, nextPage ), lang.get( "nextPage" ) );
            }
            else {
                sb.AppendFormat( "<a href=\"{0}\">{1}&gt;</a>&nbsp;", Link.AppendPage( recentLink, this.Current + 1 ), lang.get( "nextPage" ) );
            }

            // last
            //if (this.Current >= this.PageCount) {
            //}
            //else if (recentPageCount > this.PageCount) {
            //}
            //else {
            //    sb.AppendFormat( "<a href=\"{0}\">{1}&raquo;</a>&nbsp;", Link.AppendPage( archiveLink, 1 ), lang.get( "lastPage" ) );
            //}
            sb.AppendFormat( "<span class=\"pageCount\">" + lang.get( "pageCount" ) + "</span>", this.PageCount );

            sb.Append( "</div>" );

            return sb.ToString();
        }

        /// <summary>
        /// �浵�����б�ķ�ҳ��
        /// </summary>
        /// <param name="recentLink">��������б���ַ(����ҳ��)</param>
        /// <param name="archiveLink">�浵�����б���ַ(����ҳ��)</param>
        /// <param name="recentPageCount">��������б���Ҫչʾ��ҳ��</param>
        /// <returns></returns>
        public String GetArchivePage( String recentLink, String archiveLink, int recentPageCount ) {

            StringBuilder sb = new StringBuilder();
            sb.Append( "<div class=\"turnpage\">" );

            sb.AppendFormat( "<a href=\"{0}\">&laquo;{1}</a>", recentLink, lang.get( "firstPage" ) );
            sb.Append( "<span class=\"\">&nbsp;</span>" );


            if (this.PageCount - this.Current <= recentPageCount) {
                int prevPage = this.PageCount - this.Current;
                sb.AppendFormat( "<a href=\"{0}\">&lt;{1}</a>", Link.AppendPage( recentLink, prevPage ), lang.get( "prevPage" ) );
                sb.Append( "<span class=\"\">&nbsp;</span>" );
            }
            else {
                sb.AppendFormat( "<a href=\"{0}\">&lt;{1}</a>", Link.AppendPage( archiveLink, this.Current + 1 ), lang.get( "prevPage" ) );
                sb.Append( "<span class=\"\">&nbsp;</span>" );
            }

            if (this.Current > 1) {

                int nextPage = this.Current - 1;

                if (nextPage > 1) {
                    sb.AppendFormat( "<a href=\"{0}\">{1}&gt;</a>", Link.AppendPage( archiveLink, this.Current - 1 ), lang.get( "nextPage" ) );
                    sb.Append( "<span class=\"\">&nbsp;</span>" );
                    sb.AppendFormat( "<a href=\"{0}\">{1}&raquo;</a>", Link.AppendPage( archiveLink, 1 ), lang.get( "lastPage" ) );
                }
                else {
                    sb.AppendFormat( "<a href=\"{0}\">{1}&gt;</a>", Link.AppendPage( archiveLink, this.Current - 1 ), lang.get( "nextPage" ) );
                }
            }
            sb.Append( "</div>" );

            return sb.ToString();
        }



        public static DataPage<T> GetPage( List<T> list, int pageSize ) {

            ObjectPage op = new ObjectPage();
            if (pageSize <= 0) pageSize = 20;
            op.setSize( pageSize );
            op.RecordCount = list.Count;
            // ����ҳ��
            op.computePageCount();

            // ������ǰҳ��
            int currentPageNumber = CurrentRequest.getCurrentPage();
            op.setCurrent( currentPageNumber );
            op.resetCurrent();
            currentPageNumber = op.getCurrent();

            // �õ������
            List<T> results = new List<T>();
            int start = (currentPageNumber - 1) * pageSize;
            int count = 1;
            for (int i = start; i < list.Count; i++) {
                if (count > pageSize) break;
                results.Add( list[i] );
                count++;
            }

            // ����ҳ����
            DataPage<T> page = new DataPage<T>();
            page.Results = results;
            page.Current = currentPageNumber;
            page.Size = pageSize;
            page.RecordCount = list.Count;

            page.PageCount = op.PageCount;
            page.PageBar = op.PageBar;

            return page;
        }


    }


}
