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

namespace wojilu {

    /// <summary>
    /// ���ݲ����Ⱥ��������� Hashtable
    /// </summary>
    public class Dictionary : Hashtable {

        private ArrayList _keys = new ArrayList();

        public Dictionary() {
        }

        /// <summary>
        /// ����ֵ�����ֵ�
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public override void Add( Object key, Object value ) {
            base.Add( key, value );
            _keys.Add( key );
        }

        /// <summary>
        /// �����ֵ�ļ�ֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set( Object key, Object value ) {
            base[key] = value;
            if (!_keys.Contains( key )) _keys.Add( key );
        }

        /// <summary>
        /// �����������
        /// </summary>
        public override void Clear() {
            base.Clear();
            _keys.Clear();
        }

        /// <summary>
        /// ��ȡ���е� key�����ղ����������
        /// </summary>
        public override ICollection Keys {
            get { return _keys; }
        }

        /// <summary>
        /// ɾ��ĳ��
        /// </summary>
        /// <param name="key"></param>
        public override void Remove( Object key ) {
            base.Remove( key );
            _keys.Remove( key );
        }

        /// <summary>
        /// ���� index ��ȡĳ������
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Object Get( int index ) {
            Object key = _keys[index];
            return base[key];
        }


    }
}
