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
namespace wojilu {

    /// <summary>
    /// ��װ�˳��� hash �㷨
    /// </summary>
    public interface IHashTool {

        /// <summary>
        /// ����ָ���� hash �㷨�������ַ���(��������)
        /// </summary>
        /// <param name="pwd">��Ҫ hash ���ַ���</param>
        /// <param name="ht">hash �㷨����</param>
        /// <returns></returns>
        String Get( String pwd, HashType ht );


        /// <summary>
        /// ���� hash �㷨��ָ���� salt�������ַ���
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="salt">ָ���� salt</param>
        /// <param name="ht">hash �㷨����</param>
        /// <returns></returns>
        String GetBySalt( String pwd, String salt, HashType ht );

        /// <summary>
        /// ��ȡ�������(��Ӣ����ĸ�����ֹ���)
        /// </summary>
        /// <param name="passwordLength">���볤��</param>
        /// <returns></returns>
        String GetRandomPassword( int passwordLength );


        /// <summary>
        /// ��ȡ�������(��Ӣ����ĸ�����ֹ���)
        /// </summary>
        /// <param name="passwordLength">���볤��</param>
        /// <param name="isLower">����Ƿ�Сд</param>
        /// <returns></returns>
        String GetRandomPassword( int passwordLength, Boolean isLower );

        /// <summary>
        /// ����ָ�����Ȼ�ȡsalt
        /// </summary>
        /// <param name="size">salt�ĳ���</param>
        /// <returns></returns>
        String GetSalt( int size );

    }

}
