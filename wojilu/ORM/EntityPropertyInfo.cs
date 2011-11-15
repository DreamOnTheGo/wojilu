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
using System.Collections.Generic;
using System.Reflection;
using wojilu.Reflection;

namespace wojilu.ORM {

    /// <summary>
    /// ʵ����ĳ�����Ե�Ԫ������Ϣ
    /// </summary>
    [Serializable]
    public class EntityPropertyInfo {

        public EntityPropertyInfo() {
            this.IsList = false;
            this.IsAbstractEntity = false;
            this.IsEntity = false;
            this.ValidationAttributes = new List<ValidationAttribute>();
        }

        /// <summary>
        /// ��������
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// ��Ӧ����������
        /// </summary>
        public String ColumnName { get; set; }

        /// <summary>
        /// ���Ե����ͣ�������int������string�ȵ�
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// ������Ϣ(ϵͳ�Դ���Ԫ����)
        /// </summary>
        public PropertyInfo Property { get; set; }

        /// <summary>
        /// ������������ʵ������Ϣ������Blog��һ������Title����Title������Ե�ParentEntityInfo����Blog
        /// </summary>
        public EntityInfo ParentEntityInfo { get; set; }

        /// <summary>
        /// ����������ʵ������ʱ����ʵ�����Ե���Ϣ������Blog��ʵ������Category��EntityInfo���������ʵ�����ԣ���Ϊnull
        /// </summary>
        public EntityInfo EntityInfo { get; set; }

        /// <summary>
        /// �Ƿ񱣴浽���ݿ�(�Ƿ������NotSave��ע)
        /// </summary>
        public Boolean SaveToDB { get; set; }

        /// <summary>
        /// �Ƿ����б����ͣ��б����Ͳ��ᱣ�浽���ݿ�
        /// </summary>
        public Boolean IsList { get; set; }

        /// <summary>
        /// �Ƿ���ʵ��������
        /// </summary>
        public Boolean IsEntity { get; set; }

        /// <summary>
        /// �Ƿ��ǳ�������ʵ��
        /// </summary>
        public Boolean IsAbstractEntity { get; set; }

        /// <summary>
        /// ��ǰ���Ե� ColumnAttribute
        /// </summary>
        public ColumnAttribute SaveAttribute { get; set; }

        /// <summary>
        /// ��ǰ���Ե� LongTextAttribute
        /// </summary>
        public LongTextAttribute LongTextAttribute { get; set; }

        /// <summary>
        /// ��ǰ���Ե� MoneyAttribute
        /// </summary>
        public MoneyAttribute MoneyAttribute { get; set; }

        /// <summary>
        /// ��ǰ���Ե� DecimalAttribute
        /// </summary>
        public DecimalAttribute DecimalAttribute { get; set; }

        /// <summary>
        /// ��ǰ���Ե� DefaultAttribute
        /// </summary>
        public DefaultAttribute DefaultAttribute { get; set; }

        /// <summary>
        /// ��ǰ���Ե� ValidationAttribute ���б�
        /// </summary>
        public List<ValidationAttribute> ValidationAttributes { get; set; }

        /// <summary>
        /// ��ǰ���Եĸ�ֵ/ȡֵ�������Ա��ⷴ��ĵ�Ч
        /// </summary>
        internal IPropertyAccessor PropertyAccessor { get; set; }

        internal static EntityPropertyInfo Get( PropertyInfo property ) {

            EntityPropertyInfo ep = new EntityPropertyInfo();

            object[] arrAttr = property.GetCustomAttributes( typeof( ValidationAttribute ), true );
            foreach (Object at in arrAttr) {
                ep.ValidationAttributes.Add( at as ValidationAttribute );
            }

            ep.SaveAttribute = ReflectionUtil.GetAttribute( property, typeof( ColumnAttribute ) ) as ColumnAttribute;
            ep.LongTextAttribute = ReflectionUtil.GetAttribute( property, typeof( LongTextAttribute ) ) as LongTextAttribute;
            ep.MoneyAttribute = ReflectionUtil.GetAttribute( property, typeof( MoneyAttribute ) ) as MoneyAttribute;
            ep.DecimalAttribute = ReflectionUtil.GetAttribute( property, typeof( DecimalAttribute ) ) as DecimalAttribute;
            ep.DefaultAttribute = ReflectionUtil.GetAttribute( property, typeof( DefaultAttribute ) ) as DefaultAttribute;

            ep.Property = property;
            ep.Name = property.Name;
            ep.Type = property.PropertyType;
            ep.SaveToDB = !property.IsDefined( typeof( NotSaveAttribute ), false );

            if (property.PropertyType is IList) {
                ep.IsList = true;
                ep.SaveToDB = false;
            }


            return ep;
        }

        /// <summary>
        /// ��ȡobj�ĵ�ǰ���Ե�ֵ
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Object GetValue( Object target ) {
            return PropertyAccessor.Get( target );
        }

        /// <summary>
        /// ��obj�ĵ�ǰ���Ը�ֵ
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public void SetValue( Object target, Object value ) {
            PropertyAccessor.Set( target, value );
        }

        /// <summary>
        /// �Ƿ��ǳ��ı�
        /// </summary>
        public Boolean IsLongText {
            get {
                if (Type != typeof( String )) return false;
                return LongTextAttribute != null || ((SaveAttribute != null) && (SaveAttribute.Length > 255));
            }
        }

        /// <summary>
        /// ��ȡ���Ե�label(���ڱ���)
        /// </summary>
        public String Label {
            get {
                if (SaveAttribute == null) return Name;
                if (strUtil.IsNullOrEmpty( SaveAttribute.Label )) return Name;
                return SaveAttribute.Label;
            }
        }

    }
}

