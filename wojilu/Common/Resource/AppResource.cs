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

namespace wojilu.Common.Resource {

    /// <summary>
    /// ���ೣ�û��������б�(ʡ�ݡ��Ա�ʱ�䡢��ߡ�������Ѫ�͡�������)
    /// </summary>
    public class AppResource {

        /// <summary>
        /// ����
        /// </summary>
        public static PropertyCollection Body = GetPropertyList( "member_body" );

        /// <summary>
        /// �����û���ϵ�ҵ�����
        /// </summary>
        public static PropertyCollection ContactCondition = GetPropertyList( "member_contactcondition" );

        /// <summary>
        /// email֪ͨ״̬(����/����)
        /// </summary>
        public static PropertyCollection EmailNotify = GetPropertyList( "member_emailnotify" );

        /// <summary>
        /// �Ա�(����/��/Ů)
        /// </summary>
        public static PropertyCollection Gender = GetPropertyList( "member_gender" );

        /// <summary>
        /// ͷ����ɫ
        /// </summary>
        public static PropertyCollection Hair = GetPropertyList( "member_hair" );

        /// <summary>
        /// ���ѡ��
        /// </summary>
        public static PropertyCollection Height = getHeightPropertyList();

        /// <summary>
        /// ʡ��
        /// </summary>
        public static PropertyCollection Province = GetPropertyList( "province" );

        /// <summary>
        /// ע��Ŀ��
        /// </summary>
        public static PropertyCollection Purpose = GetPropertyList( "member_regpurpose" );

        /// <summary>
        /// ����״��
        /// </summary>
        public static PropertyCollection Relationship = GetPropertyList( "member_relationship" );

        /// <summary>
        /// ��ȡ��
        /// </summary>
        public static PropertyCollection Sexuality = GetPropertyList( "member_sexuality" );

        /// <summary>
        /// ˯��ϰ��
        /// </summary>
        public static PropertyCollection Sleeping = GetPropertyList( "member_sleeping" );

        /// <summary>
        /// ���̰���
        /// </summary>
        public static PropertyCollection Smoking = GetPropertyList( "member_smoking" );

        /// <summary>
        /// ����
        /// </summary>
        public static PropertyCollection Weight = getWeightPropertyList();

        /// <summary>
        /// Ѫ��
        /// </summary>
        public static PropertyCollection Blood = GetPropertyList( "member_blood" );

        /// <summary>
        /// ����
        /// </summary>
        public static PropertyCollection Zodiac = GetPropertyList( "member_zodiac" );

        /// <summary>
        /// ʱ���б�
        /// </summary>
        public static String[] Time = getOneDayTime();

        /// <summary>
        /// ѧ��
        /// </summary>
        public static PropertyCollection Degree = GetPropertyList( "degree_list" );

        private static PropertyCollection getHeightPropertyList() {
            PropertyCollection propertys = new PropertyCollection();
            propertys.Add( new PropertyItem( lang.get( "plsSelect" ), 0 ) );
            for (int i = 120; i < 221; i++) {
                propertys.Add( new PropertyItem( i + " cm", i ) );
            }
            return propertys;
        }

        /// <summary>
        /// ��ȡ���԰���洢�ļ�ֵ���б�(�����Զ�����չ)
        /// </summary>
        /// <param name="langItemName">����key</param>
        /// <returns></returns>
        public static PropertyCollection GetPropertyList( String langItemName ) {

            PropertyCollection propertys = new PropertyCollection();
            String str = lang.get( langItemName );
            if (strUtil.IsNullOrEmpty( str )) return propertys;

            String[] strArray = str.Split( new char[] { '/' } );
            foreach (String item in strArray) {
                if (strUtil.IsNullOrEmpty( item )) continue;
                String[] arrPair = item.Split( new char[] { '-' } );
                if (arrPair.Length != 2) continue;
                String name = arrPair[0].Trim();
                int val = cvt.ToInt( arrPair[1] );
                propertys.Add( new PropertyItem( name, val ) );
            }

            return propertys;
        }

        /// <summary>
        /// ����ֵ��ȡʡ������
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns></returns>
        public static PropertyItem GetProvince( int provinceId ) {
            PropertyCollection province = Province;
            foreach (PropertyItem item in province) {
                if (item.Value == provinceId) return item;
            }
            return new PropertyItem( "", provinceId );
        }

        /// <summary>
        /// ������ֵ����ȡ�������
        /// </summary>
        /// <param name="langKey">���԰��е�key</param>
        /// <param name="itemId">��ֵ</param>
        /// <returns>�������</returns>
        public static String GetItemName( String langKey, int itemId ) {
            PropertyCollection list = GetPropertyList( langKey );
            foreach (PropertyItem item in list) {
                if (item.Value == itemId) return item.Name;
            }
            return null;
        }

        private static PropertyCollection getWeightPropertyList() {
            PropertyCollection propertys = new PropertyCollection();
            propertys.Add( new PropertyItem( lang.get( "plsSelect" ), 0 ) );
            for (int i = 30; i < 131; i++) {
                propertys.Add( new PropertyItem( i + " kg", i ) );
            }
            return propertys;
        }

        private static String[] getOneDayTime() {

            String[] result = new string[48];

            result[0] = "0:00";
            result[1] = "0:30";

            result[2] = "1:00";
            result[3] = "1:30";

            result[4] = "2:00";
            result[5] = "2:30";

            result[6] = "3:00";
            result[7] = "3:30";

            result[8] = "4:00";
            result[9] = "4:30";

            result[10] = "5:00";
            result[11] = "5:30";

            result[12] = "6:00";
            result[13] = "6:30";

            result[14] = "7:00";
            result[15] = "7:30";

            result[16] = "8:00";
            result[17] = "8:30";

            result[18] = "9:00";
            result[19] = "9:30";

            result[20] = "10:00";
            result[21] = "10:30";

            result[22] = "11:00";
            result[23] = "11:30";

            result[24] = "12:00";
            result[25] = "12:30";

            result[26] = "13:00";
            result[27] = "13:30";

            result[28] = "14:00";
            result[29] = "14:30";

            result[30] = "15:00";
            result[31] = "15:30";

            result[32] = "16:00";
            result[33] = "16:30";

            result[34] = "17:00";
            result[35] = "17:30";

            result[36] = "18:00";
            result[37] = "18:30";

            result[38] = "19:00";
            result[39] = "19:30";

            result[40] = "20:00";
            result[41] = "20:30";

            result[42] = "21:00";
            result[43] = "21:30";

            result[44] = "22:00";
            result[45] = "22:30";

            result[46] = "23:00";
            result[47] = "23:30";

            return result;
        }

        /// <summary>
        /// ��ȡ��ֵ�б���������ѡ��Զ��ڵ�һ��ǰ�����ӡ���ѡ�����ֵΪ0
        /// </summary>
        /// <param name="intFrom">��ʼֵ</param>
        /// <param name="intTo">��ֵֹ</param>
        /// <returns>��ֵ�б�</returns>
        public static PropertyCollection GetInts( int intFrom, int intTo ) {

            PropertyCollection propertys = new PropertyCollection();
            propertys.Add( new PropertyItem( lang.get( "plsSelect" ), 0 ) );

            for (int i = intFrom; i <= intTo; i++) {
                propertys.Add( new PropertyItem( i.ToString(), i ) );
            }

            return propertys;
        }

    }
}

