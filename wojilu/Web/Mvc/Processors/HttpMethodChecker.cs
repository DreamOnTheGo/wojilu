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
using System.Reflection;
using wojilu.Web.Mvc.Attr;

namespace wojilu.Web.Mvc.Processors {

    internal class HttpMethodChecker : ProcessorBase {

        public override void Process( ProcessContext context ) {

            MvcEventPublisher.Instance.BeginCheckHttpMethod( context.ctx );
            if (context.ctx.utils.isSkipCurrentProcessor()) return;

            MethodInfo actionMethod = context.ctx.ActionMethodInfo; // context.getActionMethod();
            String httpMethod = context.ctx.HttpMethod;
            Boolean isMethodError = isHttpMethodError( context.getController().utils.getHttpMethodAttributes( actionMethod ), httpMethod );
            if (isMethodError) {
                context.endMsg( lang.get( "exHttpMethodError" ), HttpStatus.MethodNotAllowed_405 );
            }

        }


        // 如果在定义的方法范围内
        private static Boolean isHttpMethodError( object[] attrList, String method ) {

            if (attrList == null || attrList.Length == 0) return false;

            if (strUtil.IsNullOrEmpty( method )) return true;

            foreach (IHttpMethod attr in attrList) {
                if (method.Equals( attr.GetString() )) return false;
            }
            return true;
        }


    }

}
