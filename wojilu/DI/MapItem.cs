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

using wojilu.Data;
using wojilu.ORM;

namespace wojilu.DI {

    /// <summary>
    /// ����ע���е�������
    /// </summary>
    public class MapItem : CacheObject {

        private Boolean _singleton = true;
        private Dictionary<String, object> _maps = new Dictionary<String, object>();
        
        /// <summary>
        /// �������������ʱ���Ƿ��Ե���ģʽ����
        /// </summary>
        public Boolean Singleton {
            get { return _singleton; }
            set { _singleton = value; }
        }
        
        /// <summary>
        /// ��������ע���ϵ�� map
        /// </summary>
        public Dictionary<String, object> Map {
            get { return _maps; }
            set { _maps = value; }
        }

        /// <summary>
        /// ����� typeFullName
        /// </summary>
        public String Type { get; set; }


        internal void AddMap( String propertyName, MapItem item ) {
            AddMap( propertyName, item.Name );
        }

        internal void AddMap( String propertyName, String injectBy ) {
            this.Map.Add( propertyName, injectBy );
        }


        private Object _obj;

        [NotSave]
        internal Object TargetObject {
            get { return _obj; }
            set { _obj = value; }
        }

    }
}

