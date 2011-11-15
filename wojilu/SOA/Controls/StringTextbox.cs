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

namespace wojilu.SOA.Controls {


    internal class StringTextbox : ParamControl {

        public override String Html {
            get {
                return String.Format( "<span class=\"paramLabel\">{0}</span> <span class=\"paramControl\"><input name=\"{1}\" type=\"text\" value=\"{2}\" class=\"StringTextbox\"/></span>", base.Label, base.Name, base.Value );
            }
        }

        public override Type Type {
            get { return typeof( String ); }
        }

    }
}

