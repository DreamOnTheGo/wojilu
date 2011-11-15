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
using System.Text;
using wojilu.Data;

namespace wojilu {

    /// <summary>
    /// ��ͬ����֮����ֵת��
    /// </summary>
    public class cvt {

        /// <summary>
        /// �ж��ַ����Ƿ���С��������
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsDecimal( String str ) {

            if (strUtil.IsNullOrEmpty( str )) {
                return false;
            }

            if (str.StartsWith( "-" )) {
                return isDecimal_private( str.TrimStart( '-' ) );
            }
            else
                return isDecimal_private( str );

        }

        private static Boolean isDecimal_private( String str ) {
            foreach (char ch in str.ToCharArray()) {
                if (!(char.IsDigit( ch ) || (ch == '.'))) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// �ж��ַ����Ƿ��Ƕ���������б�����֮�����ͨ��Ӣ�Ķ��ŷָ�
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static Boolean IsIdListValid( String ids ) {

            if (strUtil.IsNullOrEmpty( ids )) {
                return false;
            }
            String[] strArray = ids.Split( new char[] { ',' } );
            foreach (String str in strArray) {
                if (!IsInt( str )) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// �ж��ַ����Ƿ�������������
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Boolean IsInt( String str ) {

            if (strUtil.IsNullOrEmpty( str )) {
                return false;
            }

            if (str.StartsWith( "-" )) {
                str = str.Substring( 1, str.Length - 1 );
            }

            if (str.Length > 10) {
                return false;
            }

            char[] chArray = str.ToCharArray();
            foreach (char ch in chArray) {
                if (!char.IsDigit( ch )) {
                    return false;
                }
            }
            if (chArray.Length == 10) {

                int charInt;
                Int32.TryParse( chArray[0].ToString(), out charInt );
                if (charInt > 2) {
                    return false;
                }

                int charInt2;
                Int32.TryParse( chArray[1].ToString(), out charInt2 );

                if ((charInt == 2) && (charInt2 > 0)) {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        /// �ж��ַ����Ƿ���"true"��"false"(�����ִ�Сд)
        /// </summary>
        /// <param name="str"></param>
        /// <returns>ֻ���ַ�����"true"��"false"(�����ִ�Сд)ʱ���ŷ���true</returns>
        public static Boolean IsBool( String str ) {
            if (str == null) return false;
            if (strUtil.EqualsIgnoreCase( str, "true" ) || strUtil.EqualsIgnoreCase( str, "false" )) return true;

            return false;
        }

        /// <summary>
        /// ������ת����Ŀ������
        /// </summary>
        /// <param name="val"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public static Object To( Object val, Type destinationType ) {
            return Convert.ChangeType( val, destinationType );
        }

        /// <summary>
        /// ������ת���� Boolean ���͡�ֻ�в�������1ʱ���ŷ���true
        /// </summary>
        /// <param name="integer"></param>
        /// <returns>ֻ�в�������1ʱ���ŷ���true</returns>
        public static Boolean ToBool( int integer ) {

            return (integer == 1);
        }

        /// <summary>
        /// ������ת���� Boolean ���͡�ֻ�ж�����ַ�����ʽ����1����true(�����ִ�Сд)ʱ���ŷ���true
        /// </summary>
        /// <param name="objBool"></param>
        /// <returns>ֻ�ж�����ַ�����ʽ����1����true(�����ִ�Сд)ʱ���ŷ���true</returns>
        public static Boolean ToBool( Object objBool ) {

            if (objBool == null) {
                return false;
            }
            String str = objBool.ToString();
            return (str.Equals( "1" ) || str.ToUpper().Equals( "TRUE" ));
        }

        /// <summary>
        /// ���ַ���(�����ִ�Сд)ת���� Boolean ���͡�ֻ���ַ�������1����trueʱ���ŷ���true
        /// </summary>
        /// <param name="str"></param>
        /// <returns>ֻ���ַ�������1����trueʱ���ŷ���true</returns>
        public static Boolean ToBool( String str ) {

            if (str == null) {
                return false;
            }
            if (str.ToUpper().Equals( "TRUE" )) {
                return true;
            }
            if (str.ToUpper().Equals( "FALSE" )) {
                return false;
            }
            return (str.Equals( "1" ) || str.ToUpper().Equals( "TRUE" ));
        }

        /// <summary>
        /// ���ַ���ת���� System.Decimal ���͡����str����������С��������0
        /// </summary>
        /// <param name="str"></param>
        /// <returns>���str����������С��������0</returns>
        public static decimal ToDecimal( String str ) {

            if (!IsDecimal( str )) {
                return 0;
            }
            return Convert.ToDecimal( str );
        }

        /// <summary>
        /// ���ַ���ת���� System.Double ���͡����str����������С��������0
        /// </summary>
        /// <param name="str"></param>
        /// <returns>���str����������С��������0</returns>
        public static Double ToDouble( String str ) {

            if (!IsDecimal( str )) {
                return 0;
            }
            return Convert.ToDouble( str );
        }

        /// <summary>
        /// ���ַ���ת���� System.Decimal ���͡����str����������С�������ز��� defaultValue ָ����ֵ
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal( String str, decimal defaultValue ) {

            if (!IsDecimal( str )) {
                return defaultValue;
            }
            return Convert.ToDecimal( str );
        }

        /// <summary>
        /// ������ת������������������������򷵻�0
        /// </summary>
        /// <param name="objInt"></param>
        /// <returns>��������������򷵻�0</returns>
        public static int ToInt( Object objInt ) {

            if ((objInt != null) && IsInt( objInt.ToString() )) {
                int result;
                Int32.TryParse( objInt.ToString(), out result );
                return result;
            }
            return 0;
        }

        /// <summary>
        /// �� decimal ת��������
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int ToInt( decimal number ) {
            return Convert.ToInt32( number );
        }

        /// <summary>
        /// ������ת���ɷ�Null��ʽ���������Ĳ����� null���򷵻ؿ��ַ���(��""��Ҳ��string.Empty)
        /// </summary>
        /// <param name="str"></param>
        /// <returns>���Ϊnull���򷵻ؿ��ַ���(��""��Ҳ��string.Empty)</returns>
        public static String ToNotNull( Object str ) {

            if (str == null) {
                return "";
            }
            return str.ToString();
        }

        /// <summary>
        /// ������ת���� DateTime ��ʽ����������ϸ�ʽ���򷵻ص�ǰʱ��
        /// </summary>
        /// <param name="objTime"></param>
        /// <returns>��������ϸ�ʽ���򷵻ص�ǰʱ��</returns>
        public static DateTime ToTime( Object objTime ) {

            return ToTime( objTime, DateTime.Now );
        }

        /// <summary>
        /// ������ת���� DateTime ��ʽ����������ϸ�ʽ���򷵻صڶ�������ָ����ʱ��
        /// </summary>
        /// <param name="objTime"></param>
        /// <param name="targetTime"></param>
        /// <returns></returns>
        public static DateTime ToTime( Object objTime, DateTime targetTime ) {

            if (objTime == null) {
                return targetTime;
            }
            try {
                return Convert.ToDateTime( objTime );
            }
            catch {
                return targetTime;
            }
        }

        /// <summary>
        /// �ж�����ʱ��������Ƿ���ͬ(Ҫ��ͬ��ͬ��ͬ��)
        /// </summary>
        /// <param name="day1"></param>
        /// <param name="day2"></param>
        /// <returns></returns>
        public static Boolean IsDayEqual( DateTime day1, DateTime day2 ) {
            return (day1.Year == day2.Year && day1.Month == day2.Month && day1.Day == day2.Day);
        }

        /// <summary>
        /// ��ȡ���ڵ��ճ������ʽ��Ҫ��������������� {���죬���죬ǰ��} ��ʾ
        /// </summary>
        /// <param name="day"></param>
        /// <returns>Ҫ��������������� {���졢���졢ǰ��} ��ʾ</returns>
        public static String ToDayString( DateTime day ) {

            DateTime today = DateTime.Now;

            if (IsDayEqual( day, today )) return lang.get( "today" );
            if (IsDayEqual( day, today.AddDays( -1 ) )) return lang.get( "yesterday" );
            if (IsDayEqual( day, today.AddDays( -2 ) )) return lang.get( "thedaybeforeyesterday" );

            return day.ToShortDateString();
        }

        /// <summary>
        /// ��ȡʱ����ճ������ʽ����ʽΪ {**Сʱǰ��**����ǰ��**��ǰ}���Լ� {���죬ǰ��}
        /// </summary>
        /// <param name="t"></param>
        /// <returns>��ʽΪ {**Сʱǰ��**����ǰ��**��ǰ}���Լ� {���죬ǰ��}</returns>
        public static String ToTimeString( DateTime t ) {

            DateTime now = DateTime.Now;
            TimeSpan span = now.Subtract( t );

            if (cvt.IsDayEqual( t, now )) {

                if (span.Hours > 0)
                    return span.Hours + lang.get( "houresAgo" );
                else {
                    if (span.Minutes == 0)
                        if (span.Seconds == 0)
                            return lang.get( "justNow" );
                        else
                            return span.Seconds + lang.get( "secondAgo" );
                    else
                        return span.Minutes + lang.get( "minuteAgo" );
                }
            }

            if (cvt.IsDayEqual( t, now.AddDays( -1 ) )) return lang.get( "yesterday" );
            if (cvt.IsDayEqual( t, now.AddDays( -2 ) )) return lang.get( "thedaybeforeyesterday" );

            return t.ToShortDateString();
        }

        /// <summary>
        /// ���������л�Ϊ xml (�ڲ����� .net ����Դ��� XmlSerializer)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String ToXML( Object obj ) {
            return EasyDB.SaveToString( obj );
        }

        /// <summary>
        /// ������ת�����ַ�����ʽ���������֮����Ӣ�Ķ��ŷָ�
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static String ToString( int[] ids ) {

            if (ids == null || ids.Length == 0) return "";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < ids.Length; i++) {
                builder.Append( ids[i] );
                if (i < ids.Length - 1) builder.Append( ',' );
            }
            return builder.ToString();
        }

        /// <summary>
        /// ���ַ�����ʽ�� id �б�ת������������
        /// </summary>
        /// <param name="myids"></param>
        /// <returns></returns>
        public static int[] ToIntArray( String myids ) {

            if (strUtil.IsNullOrEmpty( myids )) return new int[] { };

            String[] arrIds = myids.Split( ',' );
            int[] Ids = new int[arrIds.Length];
            for (int i = 0; i < arrIds.Length; i++) {
                int oneID = ToInt( arrIds[i].Trim() );
                Ids[i] = oneID;
            }

            return Ids;
        }

        /// <summary>
        /// ���ַ���ת�����Ծ��ſ�ͷ�ı����ʽ�����������Ч����ɫֵ���򷵻�null
        /// </summary>
        /// <param name="val"></param>
        /// <returns>���ַ���ת�����Ծ��ſ�ͷ�ı����ʽ�����������Ч����ɫֵ���򷵻�null</returns>
        public static String ToColorValue( String val ) {
            if (strUtil.IsColorValue( val ) == false) return null;
            if (val.StartsWith( "#" )) return val;
            return "#" + val;
        }

        /// <summary>
        /// ��10��������ת��Ϊ62����
        /// </summary>
        /// <param name="inputNum"></param>
        /// <returns>62������</returns>
        public static String ToBase62( Int64 inputNum ) {
            String chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return ToBase( inputNum, chars );
        }

        /// <summary>
        /// ��10��������ת��Ϊn����
        /// </summary>
        /// <param name="inputNum">10��������</param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static String ToBase( Int64 inputNum, String chars ) {
            int cbase = chars.Length;
            int imod;
            String result = "";

            while (inputNum >= cbase) {
                imod = (int)(inputNum % cbase);
                result = chars[imod] + result;
                inputNum = inputNum / cbase;
            }

            return chars[(int)inputNum] + result;
        }

        /// <summary>
        /// ��62����ת��Ϊ10��������
        /// </summary>
        /// <param name="str">62������</param>
        /// <returns>10��������</returns>
        public static Int64 DeBase62( String str ) {

            String chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return DeBase( str, chars );
        }

        /// <summary>
        /// ��n����ת��Ϊ10��������
        /// </summary>
        /// <param name="str">��Ҫת����n������</param>
        /// <param name="chars"></param>
        /// <returns>10��������</returns>
        public static Int64 DeBase( String str, String chars ) {
            int cbase = chars.Length;

            Int64 result = 0;
            for (int i = 0; i < str.Length; i++) {

                int index = chars.IndexOf( str[i] );
                result += index * (Int64)Math.Pow( cbase, (str.Length - i - 1) );
            }

            return result;
        }


    }
}

