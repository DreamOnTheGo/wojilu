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
using System.Web;
using wojilu.Reflection;
using System.Collections.Generic;
using wojilu.Web.Context;

namespace wojilu.Web.Mvc {

    /// <summary>
    /// �ṩ���õ� html �ؼ������絥ѡ�б���ѡ�б������б��
    /// </summary>
    public class Html {

        /// <summary>
        /// ��֤��ؼ�(����һ��input  + �Ҳ��һ����֤�� + ���ˢ�»���)
        /// </summary>
        public static Captcha Captcha {
            get { return new wojilu.Web.Mvc.Captcha(); }
        }


        /// <summary>
        /// ��ѡ��(���������)
        /// </summary>
        /// <param name="items">����б���ַ�����</param>
        /// <param name="chkName">�ؼ�����</param>
        /// <param name="sValue">ѡ����ֵ�����ѡֵ֮����Ӣ�Ķ��ŷֿ������� "2, 6, 13"</param>
        /// <returns></returns>
        public static String CheckBoxList( String[] items, String chkName, Object sValue ) {
            String selectValue = cvt.ToNotNull( sValue );
            String[] arrSelectValue = getSelectValueArray( chkName, selectValue );
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < items.Length; i++) {
                Boolean isChk = IsValueChecked( arrSelectValue, items[i] );
                builder.AppendFormat( "<label><input type=\"checkbox\" name=\"{0}\" value=\"{1}\" {2}/>{1}</label> ", chkName, items[i], isChk ? "checked" : "" );
            }
            return builder.ToString();
        }

        /// <summary>
        /// ��ѡ��(�� Dictionary ���)
        /// </summary>
        /// <param name="dic">����б�� Dictionary</param>
        /// <param name="chkName">�ؼ�����</param>
        /// <param name="sValue">ѡ����ֵ�����ѡֵ֮����Ӣ�Ķ��ŷֿ������� "2, 6, 13"</param>
        /// <returns></returns>
        public static String CheckBoxList( Dictionary<String, string> dic, String chkName, Object sValue ) {

            String selectValue = cvt.ToNotNull( sValue );
            String[] arrSelectValue = getSelectValueArray( chkName, selectValue );
            StringBuilder builder = new StringBuilder();

            foreach (KeyValuePair<String, String> kv in dic) {
                Boolean isChk = IsValueChecked( arrSelectValue, kv.Value );
                String strchk = isChk ? "checked" : "";
                builder.AppendFormat( "<label><input type=\"checkbox\" name=\"{0}\" value=\"{1}\" {2}/>{3}</label> ", chkName, kv.Value, strchk, kv.Key );
            }

            return builder.ToString();
        }

        /// <summary>
        /// ��ѡ��(�ö����б����)
        /// </summary>
        /// <param name="list">����ѡ�б�Ķ����б�</param>
        /// <param name="chkName">�ؼ�����</param>
        /// <param name="textField">�������������(����ѡ�����ı�����)</param>
        /// <param name="valueField">�������������(����ѡ����ֵ)</param>
        /// <param name="sValue">ѡ����ֵ�����ѡֵ֮����Ӣ�Ķ��ŷֿ������� "2, 6, 13"</param>
        /// <returns></returns>
        public static String CheckBoxList( IList list, String chkName, String textField, String valueField, Object sValue ) {
            String selectValue = cvt.ToNotNull( sValue );
            String[] arrSelectValue = getSelectValueArray( chkName, selectValue );
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < list.Count; i++) {
                String txt = ReflectionUtil.GetPropertyValue( list[i], textField ).ToString();
                String val = ReflectionUtil.GetPropertyValue( list[i], valueField ).ToString();
                Boolean ischk = IsValueChecked( arrSelectValue, val );
                builder.AppendFormat( "<label><input type=\"checkbox\" name=\"{0}\" value=\"{1}\" {3}/>{2}</label> ", chkName, val, txt, ischk ? "checked" : "" );
            }
            return builder.ToString();
        }


        //-----------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// �����ؼ�(���������)
        /// </summary>
        /// <param name="items">�����������ַ�����</param>
        /// <param name="dropName">�ؼ�����</param>
        /// <param name="val">ѡ����ֵ</param>
        /// <returns></returns>
        public static String DropList( String[] items, String dropName, Object val ) {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat( "<select name=\"{0}\" id=\"{0}\">", dropName );
            String selVal = getSelectedValue( dropName, val );
            for (int i = 0; i < items.Length; i++) {
                String strchk = String.Empty;
                if (string.Compare( items[i], selVal, true ) == 0) {
                    strchk = "selected";
                }
                builder.AppendFormat( "<option value=\"{0}\" {1}>{0}</option>", items[i], strchk );
            }
            builder.Append( "</select>" );
            return builder.ToString();
        }

        /// <summary>
        /// �����ؼ�(�� Dictionary ���)
        /// </summary>
        /// <param name="dic">����������Dictionary</param>
        /// <param name="dropName">�ؼ�����</param>
        /// <param name="val">ѡ����ֵ</param>
        /// <returns></returns>
        public static String DropList( Dictionary<String, string> dic, String dropName, Object val ) {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat( "<select name=\"{0}\" id=\"{0}\">", dropName );
            String selval = getSelectedValue( dropName, val );

            foreach (KeyValuePair<String, string> kv in dic) {
                String strchk = kv.Value.Equals( selval ) ? "selected" : String.Empty;
                builder.AppendFormat( "<option value=\"{0}\" {1}>{2}</option>", kv.Value, strchk, kv.Key );
            }
            builder.Append( "</select>" );
            return builder.ToString();
        }

        /// <summary>
        /// �����ؼ�(�ö����б����)
        /// </summary>
        /// <param name="list">���������Ķ����б�</param>
        /// <param name="dropName">�ؼ�����</param>
        /// <param name="textField">�������������(����ѡ�����ı�����)</param>
        /// <param name="valueField">�������������(����ѡ����ֵ)</param>
        /// <param name="val">ѡ����ֵ</param>
        /// <returns></returns>
        public static String DropList( IList list, String dropName, String textField, String valueField, Object val ) {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat( "<select name=\"{0}\" id=\"{0}\">", dropName );
            String selval = getSelectedValue( dropName, val );
            for (int i = 0; i < list.Count; i++) {
                String txt = ReflectionUtil.GetPropertyValue( list[i], textField ).ToString();
                String fval = ReflectionUtil.GetPropertyValue( list[i], valueField ).ToString();
                String strchk = String.Empty;
                if (string.Compare( fval, selval, true ) == 0) {
                    strchk = "selected";
                }
                builder.AppendFormat( "<option value=\"{0}\" {1}>{2}</option>", fval, strchk, txt );
            }
            builder.Append( "</select>" );
            return builder.ToString();
        }


        //-----------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// �����ѡ���б�(���ַ��������)
        /// </summary>
        /// <param name="items">����б���ַ�����</param>
        /// <param name="radioName">�ؼ�����</param>
        /// <param name="val">ѡ����ֵ</param>
        /// <returns></returns>
        public static String RadioList( String[] items, String radioName, Object val ) {
            StringBuilder builder = new StringBuilder();
            String str = getSelectedValue( radioName, val );
            for (int i = 0; i < items.Length; i++) {
                String strchk = String.Empty;
                if (string.Compare( items[i], str.ToString(), true ) == 0) {
                    strchk = "checked=\"checked\"";
                }
                builder.AppendFormat( "<label><input type=\"radio\" id=\"{0}{3}\" name=\"{0}\" value=\"{1}\" {2}/>{1}</label> ", radioName, items[i], strchk, i );
            }
            return builder.ToString();
        }

        /// <summary>
        /// �����ѡ���б�(�� Dictionary ���)
        /// </summary>
        /// <param name="dic">����б�� Dictionary</param>
        /// <param name="radioName">�ؼ�����</param>
        /// <param name="val">ѡ����ֵ</param>
        /// <returns></returns>
        public static String RadioList( Dictionary<String, String> dic, String radioName, Object val ) {
            StringBuilder builder = new StringBuilder();
            String selval = getSelectedValue( radioName, val );
            int i = 0;
            foreach (KeyValuePair<String, String> kv in dic) {
                String strchk = kv.Value.Equals( selval ) ? "checked=\"checked\"" : String.Empty;
                builder.AppendFormat( "<label><input type=\"radio\" id=\"{0}{4}\" name=\"{0}\" value=\"{1}\" {2}/>{3}</label> ", radioName, kv.Value, strchk, kv.Key, i );
                i++;
            }
            return builder.ToString();
        }

        /// <summary>
        /// �����ѡ���б�(�ö����б����)
        /// </summary>
        /// <param name="list">��䵥ѡ�б�Ķ����б�</param>
        /// <param name="radioName">�ؼ�����</param>
        /// <param name="textField">�������������(����ѡ�����ı�����)</param>
        /// <param name="valueField">�������������(����ѡ����ֵ)</param>
        /// <param name="val">ѡ����ֵ</param>
        /// <returns></returns>
        public static String RadioList( IList list, String radioName, String textField, String valueField, Object val ) {
            StringBuilder builder = new StringBuilder();
            String str = getSelectedValue( radioName, val );
            for (int i = 0; i < list.Count; i++) {
                String txt = ReflectionUtil.GetPropertyValue( list[i], textField ).ToString();
                String fval = ReflectionUtil.GetPropertyValue( list[i], valueField ).ToString();
                String strchk = String.Empty;
                if (string.Compare( fval, str.ToString(), true ) == 0) {
                    strchk = "checked=\"checked\"";
                }
                builder.AppendFormat( "<label><input type=\"radio\" id=\"{0}{4}\" name=\"{0}\" value=\"{1}\" {3}/>{2}</label> ", radioName, fval, txt, strchk, i );
            }
            return builder.ToString();
        }


        //-----------------------------------------------------------------------------------------------------------------


        private StringBuilder sb = new StringBuilder();

        public void FormBegin( String actionUrl ) {
            this.sb.Append( GetFormBegin( actionUrl ) );
        }

        public void FormEnd() {
            this.sb.Append( GetFormEnd() );
        }

        public void Button( String id, String text ) {
            this.sb.Append( GetButton( id, text ) );
        }

        public void Code( String htmlCode ) {
            this.sb.Append( htmlCode );
        }

        public void CheckBox( String name, String val, String text ) {
            this.sb.Append( GetCheckBox( name, val, text ) );
        }

        public void HiddenInput( String name, String val ) {
            this.sb.Append( InputHidden( name, val ) );
        }

        public void Radio( String name, String val, String text ) {
            this.sb.Append( GetRadio( name, val, text ) );
        }

        public void Submit( String text ) {
            this.sb.Append( GetSubmit( text ) );
        }

        public override String ToString() {
            return this.sb.ToString();
        }

        //-----------------------------------------------------------------------------------------------------------------


        public static String CheckBox( String name, String text, String val, Boolean isChecked ) {
            String str = "";
            if (isChecked) {
                str = "checked";
            }
            return String.Format( "<label><input type=\"checkbox\" name=\"{0}\" value=\"{1}\" {2} /> {3}</label>", name, val, str, text );
        }

        private static String GetButton( String id, String text ) {
            return String.Format( "<input type=\"button\" id=\"{0}\" value=\"{1}\" />", id, text );
        }

        private static String GetCheckBox( String name, String val, String text ) {
            return String.Format( "<label><input type=\"checkbox\" name=\"{0}\" id=\"{3}\" value=\"{1}\" /> {2}</label>", name, val, text, name + val );
        }

        private static String GetFormBegin( String actionUrl ) {
            return String.Format( "<form method=\"post\" action=\"{0}\">", actionUrl );
        }

        private static String GetFormEnd() {
            return "</form>";
        }

        private static String GetRadio( String name, String val, String text ) {
            return String.Format( "<label><input type=\"radio\" name=\"{0}\" id=\"{3}\" value=\"{1}\" /> {2}</label> ", name, val, text, name + val );
        }

        private static String getSelectedValue( String dropName, Object val ) {
            String str = cvt.ToNotNull( val );
            if (strUtil.IsNullOrEmpty( str ) && CurrentRequest.getHttpMethod().Equals( "POST" )) {
                str = CurrentRequest.getForm( dropName );
            }
            str = cvt.ToNotNull( str );
            return str;
        }

        private static String[] getSelectValueArray( String chkName, String selectValue ) {

            if (CurrentRequest.getHttpMethod().Equals( "POST" )) {
                selectValue = CurrentRequest.getForm( chkName );
            }
            if (strUtil.HasText( selectValue )) {
                return selectValue.Split( new char[] { ',' } );
            }
            return null;
        }

        private static String GetSubmit( String text ) {
            return String.Format( "<input type=\"submit\" value=\"{0}\" />", text );
        }


        public static String InputHidden( String name, String value ) {
            return String.Format( "<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", name, value );
        }

        private static Boolean IsValueChecked( String[] arrSelectValue, String val ) {
            if (arrSelectValue == null) return false;
            foreach (String str in arrSelectValue) {
                if (strUtil.EqualsIgnoreCase( val.Trim(), str.Trim() )) return true;
            }
            return false;
        }



        public static String TextArea( String name, String value, String style ) {
            return String.Format( "<textarea name=\"{0}\" style=\"{2}\">{1}</textarea>", name, value, style );
        }

        public static String TextInput( String name, String value ) {
            return String.Format( "<input type=\"text\" name=\"{0}\" value=\"{1}\" />", name, value );
        }

        public static String TextInput( String name, String value, String style ) {
            return String.Format( "<input type=\"text\" name=\"{0}\" value=\"{1}\" style=\"{2}\" />", name, value, style );
        }

        public static String Tree( IList list, GetNodeString getItemString ) {
            return getChildren( list, 0, 0, getItemString );
        }

        private static String getChildren( IList list, int parentId, int depth, GetNodeString getItemString ) {
            String result = "";

            foreach (INode node in list) {
                if (node.ParentId != parentId) continue;

                String thisItemString = getItemString( node, depth );
                String childrenString = getChildren( list, node.Id, (depth + 1), getItemString );

                result += thisItemString + Environment.NewLine;
                if (strUtil.HasText( childrenString )) result += childrenString + Environment.NewLine;
            }

            return result;
        }

    }

    public delegate String GetNodeString( INode node, int depth );




}

