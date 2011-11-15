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
    public interface IPageList {

        /// <summary>
        /// ��ǰҳ�������б�
        /// </summary>
        IList Results { get; set; }

        /// <summary>
        /// ���м�¼��
        /// </summary>
        int RecordCount { get; set; }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        int PageCount { get; set; }

        /// <summary>
        /// ÿҳ��
        /// </summary>
        int Size { get; set; }

        /// <summary>
        /// ��ǰҳ��
        /// </summary>
        int Current { get; set; }

        /// <summary>
        /// �Ѿ���װ�õ�html��ҳ��
        /// </summary>
        String PageBar { get; set; }


    }

}

