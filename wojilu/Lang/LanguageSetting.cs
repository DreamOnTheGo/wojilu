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
using System.IO;

using System.Web;

namespace wojilu {

    /// <summary>
    /// ĳ�����԰������ļ������ݣ�����һ�����ƺ�һ�����԰��� Dictionary
    /// </summary>
    public class LanguageSetting {

        private String name;
        private Dictionary<String, String> langMap;

        public LanguageSetting( String name, Dictionary<String, String> lang ) {
            this.name = name;
            this.langMap = lang;
        }

        /// <summary>
        /// ���� key ��ȡ����ֵ
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String get( String key ) {
            return langMap[key];
        }

        /// <summary>
        /// ��ȡ���Եļ�ֵ�� Dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, String> getLangMap() {
            return this.langMap;
        }


    }
}
