﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Attr;
using wojilu.Serialization;

namespace wojilu.Web.Controller.Admin {

    public class CacheController : ControllerBase {

        public override void CheckPermission() {

            if (ctx.viewer.IsAdministrator() == false) {
                echoRedirect( "没有权限" );
                return;
            }
        }        

        public void Index() {

            set( "clearLink", to( Clear ) );
            set( "addLink", to( Add ) );
            set( "deleteKeyLink", to( DeleteByKey ) );
            set( "veiwLink", to( ViewCache ) );

            IDictionaryEnumerator e = HttpRuntime.Cache.GetEnumerator();

            IBlock block = getBlock( "list" );

            set( "cacheCount", HttpRuntime.Cache.Count );

            int count = 0;
            while (e.MoveNext()) {

                if (count > 500) break;

                DictionaryEntry entry = e.Entry;

                block.Set( "key", entry.Key );
                block.Set( "value", "..." );
                block.Set( "removeLink", to( Remove ) + "?key=" + entry.Key.ToString() );
                block.Set( "viewLink", to( Read ) + "?key=" + entry.Key.ToString() );
                block.Next();

                count++;

            }

        }

        public void Add() {

            target( Create );

        }

        [HttpPost]
        public void Create() {

            String key = ctx.web.post( "cacheKey" );
            String value = ctx.PostHtmlAll( "cacheValue" );

            if (strUtil.IsNullOrEmpty( key )) errors.Add( "请填写cache key" );
            if (strUtil.IsNullOrEmpty( value )) errors.Add( "请填写cache value" );

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            HttpRuntime.Cache.Insert( key, value );
            echoToParentPart( lang( "opok" ) );
        }

        public void ViewCache() {
            target( Read );
        }

        public void Read() {

            String key = ctx.web.param( "key" );

            if (strUtil.IsNullOrEmpty( key )) {
                echoToParentPart( "cache key 不能为空" );
                return;
            }

            Object val = HttpRuntime.Cache.Get( key );
            String cacheValue = val == null ? "" : val.ToString();

            if (key.StartsWith( "__object_" )) {
                cacheValue = JsonString.ConvertEntity( val as IEntity );
            }

            set( "cacheValue", strUtil.EncodeTextarea( cacheValue ) );
            set( "cacheKey", key );

            target( Update );
        }

        [HttpPost]
        public void Update() {

            String key = ctx.web.post( "cacheKey" );
            String value = ctx.PostHtmlAll( "cacheValue" );

            if (strUtil.IsNullOrEmpty( key )) errors.Add( "请填写cache key" );
            if (strUtil.IsNullOrEmpty( value )) errors.Add( "请填写cache value" );

            if (strUtil.HasText( key ) && key.StartsWith( "__object" )) {
                errors.Add( "对象缓存只可以查看或删除，不可以直接更新" );
            }

            if (ctx.HasErrors) {
                echoError();
                return;
            }

            HttpRuntime.Cache.Insert( key, value );
            echoToParentPart( lang( "opok" ) );
        }

        public void Clear() {


            IDictionaryEnumerator e = HttpRuntime.Cache.GetEnumerator();

            while (e.MoveNext()) {
                DictionaryEntry entry = e.Entry;
                HttpRuntime.Cache.Remove( entry.Key.ToString() );
            }

            echoRedirect( lang( "opok" ) );

        }

        public void DeleteByKey() {
            target( Remove );
        }

        [HttpDelete]
        public void Remove() {
            String key = ctx.web.param( "key" );
            
            if (strUtil.IsNullOrEmpty( key )) {
                echoRedirect( "cache key 不能为空" );
                return;
            }
            HttpRuntime.Cache.Remove( key );
            echoRedirect( lang( "opok" ) );
        }

    }

}
