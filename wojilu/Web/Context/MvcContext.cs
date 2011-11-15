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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

using wojilu.Data;
using wojilu.ORM;
using wojilu.Web.Mvc;
using wojilu.Web.Mvc.Routes;
using wojilu.Web.Utils;

namespace wojilu.Web.Context {

    /// <summary>
    /// mvc ���������ݣ�������ִ�������г��õ����ݷ�װ
    /// </summary>
    public class MvcContext {

        private IWebContext _context;
        private MvcContextUtils _thisUtils;

        public MvcContext( IWebContext context ) {
            _context = context;
            _thisUtils = new MvcContextUtils( this );
        }

        /// <summary>
        /// �߼����߷���MvcContextUtils
        /// </summary>
        public MvcContextUtils utils {
            get {
                if (_thisUtils != null) return _thisUtils;
                _thisUtils = new MvcContextUtils( this );
                return _thisUtils;
            }
        }

        /// <summary>
        /// web ԭʼ���ݺͷ�����װ
        /// </summary>
        public IWebContext web { get { return _context; } }

        private Result _errors = new Result();

        /// <summary>
        /// ��ȡ��ǰctx�еĴ�����Ϣ
        /// </summary>
        public Result errors { get { return _errors; } }

        /// <summary>
        /// ��ǰ·����Ϣ
        /// </summary>
        public Route route { get { return utils.getRoute(); } }

        /// <summary>
        /// ��ǰ������
        /// </summary>
        public ControllerBase controller { get { return utils.getController(); } }

        /// <summary>
        /// ��ǰ owner(�����ʵĶ�����Ϣ)
        /// </summary>
        public IOwnerContext owner { get { return utils.getOwnerContext(); } }

        /// <summary>
        /// �����ߵ���Ϣ
        /// </summary>
        public IViewerContext viewer { get { return utils.getViewerContext(); } }

        /// <summary>
        /// ��ǰ app
        /// </summary>
        public IAppContext app { get { return utils.getAppContext(); } }

        private PageMeta _pageMeta = new PageMeta();

        /// <summary>
        /// ҳ��Ԫ��Ϣ(����Title/Keywords/Description/RssLink)
        /// </summary>
        /// <returns></returns>
        public PageMeta GetPageMeta() {
            return _pageMeta;
        }


        //---------------------------------------------------------

        private Hashtable _contextItems = new Hashtable();

        /// <summary>
        /// ���� key ��ȡ�洢�� ctx ��ĳ���ֵ
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object GetItem( String key ) {
            return _contextItems[key];
        }

        /// <summary>
        /// ��ĳ������洢�� ctx �У����㲻ͬ�� controller �� action ֮�����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetItem( String key, Object val ) {
            _contextItems[key] = val;
        }

        /// <summary>
        /// �ж� ctx �Ĵ洢�����Ƿ����ĳ�� key ��
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean HasItem( string key ) {
            return _contextItems.ContainsKey( key );
        }

        //---------------------------------------------------------

        /// <summary>
        /// ��ȡ���Ӷ���
        /// </summary>
        /// <returns></returns>
        public Link GetLink() {
            return new Link( this );
        }

        private UrlInfo _url;

        /// <summary>
        /// ��ȡ������װ�� url ��Ϣ
        /// </summary>
        public UrlInfo url { get { return getUrl(); } }

        private UrlInfo getUrl() {

            if (_url == null) {
                _url = new UrlInfo( _context.Url, _context.PathApplication, _context.PathInfo );
            }
            return _url;
        }

        /// <summary>
        /// ���õ�ǰ��ַ�������Զ�����ַ
        /// </summary>
        /// <param name="url"></param>
        public void setUrl( String url ) {
            _url = new UrlInfo( url, _context.PathApplication, _context.PathInfo );
        }

        /// <summary>
        /// ��ǰ�ͻ����ϴ��������ļ�
        /// </summary>
        /// <returns></returns>
        public List<HttpFile> GetFiles() {
            HttpFileCollection files = _context.getUploadFiles();

            List<HttpFile> list = new List<HttpFile>();
            for (int i = 0; i < files.Count; i++) {
                list.Add( new HttpFile( files[i] ) );
            }
            return list;
        }

        /// <summary>
        /// ��ǰ�ͻ����ϴ��ĵ�һ���ļ�
        /// </summary>
        /// <returns></returns>
        public HttpFile GetFileSingle() {
            return this.GetFiles().Count == 0 ? null : GetFiles()[0];
        }

        /// <summary>
        /// �ͻ����Ƿ��ϴ����ļ�
        /// </summary>
        public Boolean HasUploadFiles {
            get { return GetFiles().Count > 0 && GetFileSingle().ContentLength > 10; }
        }

        /// <summary>
        /// ��ǰ ctx ���Ƿ��д�����Ϣ
        /// </summary>
        public Boolean HasErrors {
            get { return errors.HasErrors; }
        }

        /// <summary>
        /// �ͻ����ύ�� HttpMethod������GET/POST/DELETE/PUT ��
        /// </summary>
        public String HttpMethod { get { return getMethod(); } }

        /// <summary>
        /// ��ǰ�ͻ����ύ�����Ƿ��� GET ����
        /// </summary>
        public Boolean IsGetMethod {
            get { return strUtil.EqualsIgnoreCase( "get", this.HttpMethod ); }
        }

        private String getMethod() {

            if ("POST".Equals( _context.post( "_httpmethod" ) )) return "POST";
            if ("DELETE".Equals( _context.post( "_httpmethod" ) )) return "DELETE";
            if ("PUT".Equals( _context.post( "_httpmethod" ) )) return "PUT";

            return _context.ClientHttpMethod;
        }

        private MethodInfo _actionMethodInfo;

        internal void setActionMethodInfo( MethodInfo mi ) {
            _actionMethodInfo = mi;
        }
        public MethodInfo ActionMethodInfo {
            get {
                if (_actionMethodInfo == null) {
                }
                return _actionMethodInfo;
            }
        }


        private List<Attribute> _attributes;
        public List<Attribute> ActionMethods {
            get {
                if (_attributes == null) {
                    Object[] attrs = this.controller.utils.getAttributesAll( this.ActionMethodInfo );
                    _attributes = new List<Attribute>();
                    foreach (Object obj in attrs) {
                        _attributes.Add( (Attribute)obj );
                    }
                }
                return _attributes;
            }
        }

        /// <summary>
        /// ����������Դ��׼���׳��쳣
        /// </summary>
        /// <param name="httpStatus">���ͻ��˵� httpStatus ״̬��Ϣ</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public MvcException ex( String httpStatus, String msg ) {
            utils.clearResource();
            return new MvcException( httpStatus, msg );
        }

        /// <summary>
        /// ���� json ���ͻ���
        /// </summary>
        /// <param name="jsonContent"></param>
        public void RenderJson( String jsonContent ) {
            _context.RenderJson( jsonContent );
        }

        /// <summary>
        /// ���� xml ���ͻ���
        /// </summary>
        /// <param name="xmlContent"></param>
        public void RenderXml( String xmlContent ) {
            _context.RenderXml( xmlContent );
        }


        //---------------------------------------- Get ---------------------------------------------

        /// <summary>
        /// ��ȡ url �е�ĳ��ֵ������ѱ�����(������html)
        /// </summary>
        /// <param name="queryItem"></param>
        /// <returns></returns>
        public String Get( String queryItem ) {
            String val = _context.get( queryItem );
            return checkClientValue( val );
        }

        /// <summary>
        /// ��� url ���Ƿ����ĳ�� key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean GetHas( String key ) {
            return _context.getHas( key );
        }

        /// <summary>
        /// �� url �Ĳ�ѯ��Ϣ (query string) �л�ȡ id �б������������֤�������Ͱ�ȫ�ġ�������Ϸ����򷵻�null
        /// </summary>
        /// <param name="idname"></param>
        /// <returns></returns>
        public String GetIdList( String idname ) {
            String ids = Get( idname );
            if (!cvt.IsIdListValid( ids )) {
                return null;
            }
            return ids;
        }

        /// <summary>
        /// �� url �л�ȡĳ���ֵ����ת��������
        /// </summary>
        /// <param name="queryItemName"></param>
        /// <returns></returns>
        public int GetInt( String queryItemName ) {
            if ((_context.get( queryItemName ) != null) && cvt.IsInt( _context.get( queryItemName ) )) {
                return int.Parse( _context.get( queryItemName ) );
            }
            return 0;
        }

        /// <summary>
        /// �� url �л�ȡĳ���ֵ����ת���� Decimal
        /// </summary>
        /// <param name="queryItemName"></param>
        /// <returns></returns>
        public Decimal GetDecimal( String queryItemName ) {
            if ((_context.get( queryItemName ) != null)) {
                return cvt.ToDecimal( _context.get( queryItemName ) );
            }
            return 0;
        }

        /// <summary>
        /// �� url �л�ȡĳ���ֵ����ת���� Double
        /// </summary>
        /// <param name="queryItemName"></param>
        /// <returns></returns>
        public Double GetDouble( String queryItemName ) {
            if ((_context.get( queryItemName ) != null)) {
                return cvt.ToDouble( _context.get( queryItemName ) );
            }
            return 0;
        }

        /// <summary>
        /// ��ȡ�ͻ��� ip ��ַ
        /// </summary>
        public String Ip { get { return getIp(); } }

        private String getIp() {

            String ip;
            if (_context.ClientVar( "HTTP_VIA" ) != null)
                ip = _context.ClientVar( "HTTP_X_FORWARDED_FOR" );
            else
                ip = _context.ClientVar( "REMOTE_ADDR" );

            return checkIp( ip );
        }

        private String checkIp( String ip ) {

            int maxLength = 3 * 15 + 2;
            String unknow = "unknow";

            if (strUtil.IsNullOrEmpty( ip ) || ip.Length > maxLength || ip.Length < 7) return unknow;

            char[] arr = ip.ToCharArray();
            foreach (char c in arr) {
                if (!char.IsDigit( c ) && c != '.' && c != ',') return unknow;
            }

            return ip;
        }

        //------------------------------------------- POST ------------------------------------------

        /// <summary>
        /// ��ȡ�ͻ��� post ��ֵ������ѱ�����(������html)
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public String Post( String postItem ) {
            String val = _context.post( postItem );
            return checkClientValue( val );
        }

        /// <summary>
        /// ���ͻ��� post ���������Ƿ���ĳ�� key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean PostHas( String key ) {
            return _context.postHas( key );
        }

        /// <summary>
        /// �ӿͻ��� post �������л�ȡĳ���ֵ����ת���� decimal
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public Decimal PostDecimal( String postItem ) {
            if (_context.post( postItem ) != null) {
                return cvt.ToDecimal( _context.post( postItem ) );
            }
            return 0;
        }

        /// <summary>
        /// �ӿͻ��� post �������л�ȡĳ���ֵ����ת���� Double
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public Double PostDouble( String postItem ) {
            if (_context.post( postItem ) != null) {
                return cvt.ToDouble( _context.post( postItem ) );
            }
            return 0;
        }

        /// <summary>
        /// �ӿͻ��� post �������л�ȡ id �б������������֤�������Ͱ�ȫ��
        /// </summary>
        /// <param name="idname"></param>
        /// <returns></returns>
        public String PostIdList( String idname ) {
            String ids = Post( idname );
            if (!cvt.IsIdListValid( ids )) {
                return null;
            }
            return ids;
        }

        /// <summary>
        /// �ӿͻ��� post �������л�ȡĳ���ֵ����ת��������
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public int PostInt( String postItem ) {
            if ((_context.post( postItem ) != null) && cvt.IsInt( _context.post( postItem ) )) {
                return int.Parse( _context.post( postItem ) );
            }
            return 0;
        }

        /// <summary>
        /// ���ͻ����Ƿ��Ѿ���ѡ�˶�ѡ�������ѡ����1�����򷵻�0
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns>�����ѡ����1�����򷵻�0</returns>
        public int PostIsCheck( String postItem ) {
            String target = Post( postItem );
            if (strUtil.HasText( target ) && target.Equals( "on" )) {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// �ӿͻ��� post �������л�ȡĳ���ֵ����ת����ʱ�����͡�������ύֵ���ʽ�����򷵻ص�ǰʱ��DateTime.Now
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public DateTime PostTime( String postItem ) {
            if (_context.post( postItem ) != null) {
                return cvt.ToTime( _context.post( postItem ) );
            }
            return DateTime.Now;
        }

        /// <summary>
        /// ��ȡ�ͻ��� post �� html������ѱ����ˣ�ֻ���ڰ������е� tag �ű�����
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public String PostHtml( String postItem ) {
            String val = _context.post( postItem );
            if (val != null) {

                if (this.viewer != null && this.viewer.IsAdministrator()) return val;
                val = strUtil.TrimHtml( val );
                val = HtmlFilter.Filter( val );
            }
            return val;
        }

        /// <summary>
        /// ��ȡ�ͻ��� post �� html������ѱ����ˣ�ֻ���� allowedTags ��ָ���� tag
        /// </summary>
        /// <param name="postItem"></param>
        /// <param name="allowedTags"></param>
        /// <returns></returns>
        public String PostHtml( String postItem, String allowedTags ) {
            String val = _context.post( postItem );
            if (val != null) {
                val = strUtil.TrimHtml( val );
                val = HtmlFilter.Filter( val, allowedTags );
            }
            return val;
        }

        /// <summary>
        /// ��Ĭ�ϰ������Ļ����ϣ����� allowedTags ��ָ����tag
        /// </summary>
        /// <param name="postItem"></param>
        /// <param name="allowedTags"></param>
        /// <returns></returns>
        public String PostHtmlAppendTags( String postItem, String allowedTags ) {

            String val = _context.post( postItem );
            if (val != null) {
                val = strUtil.TrimHtml( val );
                val = HtmlFilter.FilterAppendTags( val, allowedTags );
            }
            return val;
        }

        /// <summary>
        /// ������տͻ��������ַ��������ʹ��
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public String PostHtmlAll( String postItem ) {
            return _context.post( postItem );
        }

        //------------------------------------------- PARAMS ------------------------------------------

        /// <summary>
        /// ��ȡ�ͻ����ύ������(����get��post)������ѱ�����(������html)
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public String Params( String itemName ) {
            String val = _context.param( itemName );
            return checkClientValue( val );
        }

        /// <summary>
        /// �ӿͻ����ύ�������л�ȡĳ���ֵ����ת��������
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public int ParamInt( String postItem ) {
            if ((_context.param( postItem ) != null) && cvt.IsInt( _context.param( postItem ) )) {
                return int.Parse( _context.param( postItem ) );
            }
            return 0;
        }

        /// <summary>
        /// �ӿͻ����ύ�������л�ȡĳ���ֵ����ת���� Decimal
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public Decimal ParamDecimal( String postItem ) {
            if (_context.param( postItem ) != null) {
                return cvt.ToDecimal( _context.param( postItem ) );
            }
            return 0;
        }

        /// <summary>
        /// �ӿͻ����ύ�������л�ȡĳ���ֵ����ת���� Double
        /// </summary>
        /// <param name="postItem"></param>
        /// <returns></returns>
        public Double ParamDouble( String postItem ) {
            if (_context.param( postItem ) != null) {
                return cvt.ToDouble( _context.param( postItem ) );
            }
            return 0;
        }

        //-------------------------------------------------------------------------------------

        /// <summary>
        /// ��֤����ĸ��������Ƿ�Ϸ�
        /// </summary>
        /// <param name="target">��Ҫ����֤�Ķ���</param>
        /// <returns>������֤���</returns>
        public Result Validate( IEntity target ) {
            return Validator.Validate( target );
        }

        /// <summary>
        /// ��ȡ�ͻ���post�����ݣ����Զ���ֵ����������ԣ���������֤
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T PostValue<T>() {

            EntityInfo entityInfo = Entity.GetInfo( typeof( T ) );
            Type t = typeof( T );
            T obj = (T)rft.GetInstance( t );

            setObjectProperties( entityInfo, t, obj );

            IEntity entity = obj as IEntity;
            if (entity != null) {
                Result result = Validate( entity );
                if (result.HasErrors) errors.Join( result );
            }

            return obj;
        }

        /// <summary>
        /// ��ȡ�ͻ���post�����ݣ����Զ���ֵ����������ԣ���������֤
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Object PostValue( Object obj ) {

            EntityInfo entityInfo = Entity.GetInfo( obj );
            Type t = obj.GetType();
            setObjectProperties( entityInfo, t, obj );

            IEntity entity = obj as IEntity;
            if (entity != null) {
                Result result = Validate( entity );
                if (result.HasErrors) errors.Join( result );
            }

            return obj;
        }

        private void setObjectProperties( EntityInfo entityInfo, Type t, Object obj ) {
            String camelType = strUtil.GetCamelCase( t.Name );
            String prefix = camelType + ".";

            NameValueCollection posts = _context.postValueAll();
            foreach (String key in posts.Keys) {

                if (key.StartsWith( prefix ) == false) continue;

                String propertyName = strUtil.TrimStart( key, prefix );
                PropertyInfo p = t.GetProperty( propertyName );
                if (p == null) continue;

                if (entityInfo == null)
                    setPropertyValue( obj, p, posts[key] );
                else {
                    EntityPropertyInfo ep = entityInfo.GetProperty( propertyName );
                    setEntityPropertyValue( obj, ep, posts[key] );
                }
            }
        }

        private String checkClientValue( String val ) {

            if (val != null) {
                val = val.Trim();
                val = HttpUtility.HtmlEncode( val );
            }

            return val;
        }

        private void setPropertyValue( Object obj, PropertyInfo p, String postValue ) {

            if (p.PropertyType == typeof( int )) {
                p.SetValue( obj, cvt.ToInt( postValue ), null );
            }
            else if (p.PropertyType == typeof( String )) {
                p.SetValue( obj, getAutoPostString( p, postValue ), null );
            }
            else if (p.PropertyType == typeof( Decimal )) {
                p.SetValue( obj, cvt.ToDecimal( postValue ), null );
            }
            else if (p.PropertyType == typeof( Double )) {
                p.SetValue( obj, cvt.ToDouble( postValue ), null );
            }
            else if (p.PropertyType == typeof( DateTime )) {
                p.SetValue( obj, cvt.ToTime( postValue ), null );
            }
        }

        private void setEntityPropertyValue( Object obj, EntityPropertyInfo p, String postValue ) {

            if (p.Type == typeof( int )) {
                p.SetValue( obj, cvt.ToInt( postValue ) );
            }
            else if (p.Type == typeof( String )) {

                p.SetValue( obj, getAutoPostString( p.Property, postValue ) );

            }
            else if (p.Type == typeof( Decimal )) {
                p.SetValue( obj, cvt.ToDecimal( postValue ) );
            }
            else if (p.Type == typeof( Double )) {
                p.SetValue( obj, cvt.ToDouble( postValue ) );
            }
            else if (p.Type == typeof( DateTime )) {
                p.SetValue( obj, cvt.ToTime( postValue ) );
            }
            else if (p.IsEntity) {
                IEntity objProperty = Entity.New( p.EntityInfo.FullName );
                objProperty.Id = cvt.ToInt( postValue );
                p.SetValue( obj, objProperty );

            }
        }

        private String getAutoPostString( PropertyInfo p, String postValue ) {

            Attribute htmlAttr = rft.GetAttribute( p, typeof( HtmlTextAttribute ) );
            if (htmlAttr == null) postValue = checkClientValue( postValue );
            return postValue;
        }

        private Boolean _isRunAction = true;

        internal void isRunAction( Boolean isRun ) {
            _isRunAction = isRun;
        }

        internal Boolean isRunAction() {
            return _isRunAction;
        }

        //------------------------------------------------------------------------------------

        private Link getLink() {
            return new Link( this );
        }

        /// <summary>
        /// ���ӵ�ĳ�� action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public String to( aAction action ) {
            return getLink().To( action );
        }

        /// <summary>
        /// ���ӵ�ĳ�� action
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public String to( aActionWithId action, int id ) {
            return getLink().To( action, id );
        }

        /// <summary>
        /// ���ӵ�ĳ�� action����ַ�в����� appId ��Ϣ
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public String t2( aAction action ) {
            return getLink().T2( action );
        }

        /// <summary>
        /// ���ӵ�ĳ�� action����ַ�в����� appId ��Ϣ
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public String t2( aActionWithId action, int id ) {
            return getLink().T2( action, id );
        }


    }
}