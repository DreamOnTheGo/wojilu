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

namespace wojilu.Web.Mvc.Routes {

    /// <summary>
    /// ·�����ó������
    /// </summary>
    public abstract class RouteConfig {

        //public abstract String getRootNamespace();
        public abstract Boolean isParseAppId();
        public abstract String urlExt();
        public abstract String getConfigPath();

        public static RouteConfig Instance = getConfig();

        private static RouteConfig getConfig() {
            return new RouteConfigFile();
        }

    }

    /// <summary>
    /// �û����Ե�·������
    /// </summary>
    public class RouteConfigTest : RouteConfig {

        //public override String getRootNamespace() {
        //    return "wojilu.Web.Controller";
        //}

        public override Boolean isParseAppId() {
            return true;
        }

        public override String urlExt() {
            return "aspx";
        }

        public override String getConfigPath() {
            return "";
        }

    }

    /// <summary>
    /// ·������
    /// </summary>
    public class RouteConfigFile : RouteConfig {

        //public override String getRootNamespace() {
        //    return MvcConfig.Instance.RootNamespace;
        //}

        public override Boolean isParseAppId() {
            return MvcConfig.Instance.IsParseAppId;
        }

        public override String urlExt() {
            return MvcConfig.Instance.UrlExt;
        }

        public override String getConfigPath() {
            return MvcConfig.Instance.RouteConfigPath;
        }
    }

}
