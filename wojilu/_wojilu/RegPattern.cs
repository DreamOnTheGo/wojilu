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
using System.Text.RegularExpressions;

namespace wojilu {

    /// <summary>
    /// ��װ�˼������õ�������ʽ
    /// </summary>
    public class RegPattern {

        //public static readonly String Email = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        /// <summary>
        /// ��ַ��������ʽ
        /// </summary>
        //public static readonly String Url = @"^http\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$";
        public static readonly string Url = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";

        // http://msdn.microsoft.com/en-us/library/ms998267.aspx

        /// <summary>
        /// email ������ʽ
        /// </summary>
        public static readonly String Email = @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$";

        //public static readonly String Url = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";

        /// <summary>
        /// ����ֵ(С��)��������ʽ
        /// </summary>
        public static readonly String Currency = @"^\d+(\.\d\d)?$"; //1.00

        /// <summary>
        /// (����)����ֵ(С��)��������ʽ
        /// </summary>
        public static readonly String NegativeCurrency = @"^(-)?\d+(\.\d\d)?$"; //-1.20

        /// <summary>
        /// html ҳ����ͼƬ��������ʽ����ȡ&lt;img src="" /&gt; ��src����
        /// </summary>
        public static readonly String Img = "(?<=<img.+?src\\s*?=\\s*?\"?)([^\\s\"]+?)(?=[\\s\"])";

        /// <summary>
        /// ��� input �ַ����Ƿ��ָ����������ʽƥ��
        /// </summary>
        /// <param name="input">��Ҫ�����ַ���</param>
        /// <param name="pattern">������ʽ</param>
        /// <returns></returns>
        public static Boolean IsMatch( String input, String pattern ) {
            return Regex.IsMatch( input, pattern);
        }

    }

}

