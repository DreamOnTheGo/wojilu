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

namespace wojilu.Web.Handler {

    /// <summary>
    /// ��֤������
    /// </summary>
    public class ValidationType {

        /// <summary>
        /// ����
        /// </summary>
        public static readonly int Digit = 0;

        /// <summary>
        /// Ӣ����ĸ
        /// </summary>
        public static readonly int Letter = 1;

        /// <summary>
        /// ���ֺ�Ӣ����ĸ
        /// </summary>
        public static readonly int DigitAndLetter = 2;

        /// <summary>
        /// ����
        /// </summary>
        public static readonly int Chinese = 3;

        
    }

    /// <summary>
    /// ��֤��Ĭ��ֵ
    /// </summary>
    public class ValidationDefault {

        /// <summary>
        /// Ӣ�Ļ���ĸ��֤���Ĭ�ϳ���
        /// </summary>
        public static readonly int Length = 6;

        /// <summary>
        /// ������֤���Ĭ�ϳ���
        /// </summary>
        public static readonly int ChineseLength = 2;

        /// <summary>
        /// Ĭ�ϵ���֤������
        /// </summary>
        public static readonly int Type = ValidationType.Digit;

    }

}
