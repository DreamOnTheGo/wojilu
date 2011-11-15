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
using System.Collections;

namespace wojilu {

    /// <summary>
    /// ��ҳ��Ľ����
    /// </summary>
    public class PageList : IPageList {

        public PageList() {
        }

        public PageList( IList list ) {
            this.Results = list;
        }

        private int _current;
        private String _pageBar;
        private int _pageCount;
        private int _recordCount;
        private IList _results;
        private int _size;

        /// <summary>
        /// ��ǰҳ��
        /// </summary>
        public int Current {
            get { return _current; }
            set { _current = value; }
        }

        /// <summary>
        /// ÿҳ����
        /// </summary>
        public int Size {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// �Ѿ���װ�õ�html��ҳ��
        /// </summary>
        public String PageBar {
            get { return _pageBar; }
            set { _pageBar = value; }
        }

        /// <summary>
        /// �ܹ�ҳ��
        /// </summary>
        public int PageCount {
            get { return _pageCount; }
            set { _pageCount = value; }
        }

        /// <summary>
        /// ���м�¼��
        /// </summary>
        public int RecordCount {
            get { return _recordCount; }
            set { _recordCount = value; }
        }

        /// <summary>
        /// ��ǰҳ�������б�
        /// </summary>
        public IList Results {
            get { return _results; }
            set { _results = value; }
        }

        /// <summary>
        /// ����һ���յķ�ҳ�����
        /// </summary>
        /// <returns></returns>
        public static PageList GetEmpty() {
            PageList p = new PageList();
            p.Results = new ArrayList();
            p.Current = 1;
            p.RecordCount = 0;
            return p;
        }


    }
}

