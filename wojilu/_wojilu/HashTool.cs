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
using System.Security.Cryptography;

namespace wojilu {

    /// <summary>
    /// ���� hash �㷨����
    /// </summary>
    public enum HashType {
        MD5,
        MD5_16,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

    /// <summary>
    /// ��װ�˳��� hash �㷨
    /// </summary>
    public class HashTool : IHashTool {

        /// <summary>
        /// ����ָ���� hash �㷨�������ַ���(��������)
        /// </summary>
        /// <param name="pwd">��Ҫ hash ���ַ���</param>
        /// <param name="ht">hash �㷨����</param>
        /// <returns></returns>
        public virtual String Get( String pwd, HashType ht ) {

            HashAlgorithm algorithm;

            if (ht == HashType.MD5 || ht == HashType.MD5_16)
                algorithm = MD5.Create();
            else if (ht == HashType.SHA1)
                algorithm = SHA1CryptoServiceProvider.Create();
            else if (ht == HashType.SHA256)
                algorithm = SHA256Managed.Create();
            else if (ht == HashType.SHA384)
                algorithm = SHA384Managed.Create();
            else if (ht == HashType.SHA512)
                algorithm = SHA512Managed.Create();
            else
                algorithm = MD5.Create();

            byte[] buffer = Encoding.UTF8.GetBytes( pwd );
            String result = BitConverter.ToString( algorithm.ComputeHash( buffer ) ).Replace( "-", "" );
            if (ht == HashType.MD5_16) return result.Substring( 8, 16 ).ToLower();
            return result;
        }

        /// <summary>
        /// ���� hash �㷨��ָ���� salt�������ַ���
        /// </summary>
        /// <param name="pwd"></param>
        /// <param name="salt">ָ���� salt</param>
        /// <param name="ht">hash �㷨����</param>
        /// <returns></returns>
        public virtual String GetBySalt( String pwd, String salt, HashType ht ) {
            return Get( pwd + salt, ht );
        }

        /// <summary>
        /// ��ȡ�������(��Ӣ����ĸ�����ֹ���)
        /// </summary>
        /// <param name="passwordLength">���볤��</param>
        /// <returns></returns>
        public virtual String GetRandomPassword( int passwordLength ) {
            return GetRandomPassword( passwordLength, true );
        }

        /// <summary>
        /// ��ȡ�������(��Ӣ����ĸ�����ֹ���)
        /// </summary>
        /// <param name="passwordLength">���볤��</param>
        /// <param name="isLower">����Ƿ�Сд</param>
        /// <returns></returns>
        public virtual String GetRandomPassword( int passwordLength, Boolean isLower ) {

            String charList = isLower ? "abcdefghijklmnopqrstuvwxyz0123456789" : "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            byte[] buffer = new byte[passwordLength];

            RNGCryptoServiceProvider.Create().GetBytes( buffer );

            char[] chars = new char[passwordLength];
            int charCount = charList.Length;
            for (int i = 0; i < passwordLength; i++) {
                chars[i] = charList[(int)buffer[i] % charCount];
            }

            return new string( chars );
        }

        /// <summary>
        /// ����ָ�����Ȼ�ȡsalt
        /// </summary>
        /// <param name="size">salt�ĳ���</param>
        /// <returns></returns>
        public virtual String GetSalt( int size ) {
            byte[] buffer = new byte[size];
            RNGCryptoServiceProvider.Create().GetBytes( buffer );
            return BitConverter.ToString( buffer ).Replace( "-", "" );
        }

    }

}
