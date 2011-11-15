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
using System.Text;

namespace wojilu.Web {

    /// <summary>
    /// ģ�������е�����
    /// </summary>
    public interface IBlock {

        /// <summary>
        /// ������һ��ѭ��
        /// </summary>
        void Next();

        /// <summary>
        /// ��ģ�������ֵ
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="lblValue"></param>
        void Set( String lbl, String lblValue );

        /// <summary>
        /// ��ģ�������ֵ
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="val"></param>
        void Set( String lbl, Object val );

        /// <summary>
        /// ������󶨵�ģ����
        /// </summary>
        /// <param name="obj"></param>
        void Bind( Object obj );

        /// <summary>
        /// ������󶨵�ģ���У���ָ��������ģ���еı�����
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="obj"></param>
        void Bind( String lbl, Object obj );

        /// <summary>
        /// �������б�󶨵�ģ����
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="lbl"></param>
        /// <param name="objList"></param>
        void BindList( String listName, String lbl, IList objList );

        /// <summary>
        /// ��ȡģ���е����飬���ڽ�һ���İ�
        /// </summary>
        /// <param name="blockName"></param>
        /// <returns></returns>
        IBlock GetBlock( String blockName );

        /// <summary>
        /// �󶨷�����ͨ�������Զ����
        /// </summary>
        bindFunction bindFunc { get; set; }

        /// <summary>
        /// �󶨷�����ͨ�������Զ����
        /// </summary>
        otherBindFunction bindOtherFunc { get; set; }



    }

}
