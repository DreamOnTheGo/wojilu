using System;
using System.Collections;
using System.Collections.Generic;
using wojilu.Test.Orm.Entities;
using wojilu.Test.Orm.Utils;
using NUnit.Framework;

namespace wojilu.Test.Orm {

    // ��̬�����Ͷ�̬��������
    [TestFixture]
    public class TestPolymorphism {


        // ����ֱ���Ҽ��˴���ʼ���ԣ������ÿ������ݿ��ʼ����





        [Test]
        public void testAbstractFind() {

            // ��������� TAbCategory

            List<TAbNewCategory> list = db.findAll<TAbNewCategory>();
            Assert.AreEqual( 9, list.Count );

            Assert.AreEqual( "���Ⱑ_1", list[0].Title );

            List<TAbNewCategory2> list2 = db.findAll<TAbNewCategory2>();
            Assert.AreEqual( 9, list2.Count );

            Assert.AreEqual( "���Ⱑ2_1", list2[0].Title2 );


            TAbNewCategory c = db.findById<TAbNewCategory>( 2 );
            Assert.IsNotNull( c );
            Assert.AreEqual( 4, c.Hits );

            List<TAbNewCategory> qlist = db.find<TAbNewCategory>( "Hits=6" ).list();
            Assert.AreEqual( 1, qlist.Count );
        }



        [Test]
        public void FindById() {
            ConsoleTitleUtil.ShowTestTitle( "FindById" );

            // ����TDataRoot������TPost��TTopic

            //����򵥲�ѯ
            TPost post = TPost.findById( 3 ) as TPost;
            Assert.IsNotNull( post );
            Assert.AreEqual( 3, post.Id );
            Console.WriteLine( "Id:{0}  Title:{1}", post.Id, post.Title );

            TPost post2 = TPost.findById( 11 ) as TPost;
            Assert.IsNotNull( post2 );


            //��̬��ѯ����ȷ�жϽ������Ӧ����
            TDataRoot root = TDataRoot.findById( 6 );
            Assert.IsNotNull( root );
            Assert.AreEqual( 6, root.Id );
            Assert.AreEqual( typeof( TTopic ), root.GetType() );

            //������ԵĶ�̬��ѯ
            Assert.AreEqual( typeof( TTopicCategory ), root.Category.GetType() );
        }

        [Test]
        public void FindById_FromRoot() {

            // 1������Ķ�̬
            //---------------------------------------

            Console.WriteLine( "---------- �����̬ -----------" );

            // Ҳ����ֱ�Ӵ�������
            TCategory category = new TCategory();
            category.Name = "����ķ���֮FindById_FromRoot";
            db.insert( category );

            Assert.Greater( category.Id, 0 );

            // ��̬��ѯ����ȷ�жϾ���Ľ���Ǹ���
            TCategory cat = TCategory.findById( category.Id );
            Assert.AreEqual( category.Name, cat.Name );
            Assert.AreEqual( typeof( TCategory ), cat.GetType() );

            Console.WriteLine( "---------- ��̬���� -----------" );


            // 2�� �������� ������ ��̬�ķ���
            //---------------------------------------
            TDataRoot root = new TDataRoot();
            root.Title = "���Ǹ���֮һ";
            root.Body = "���������֮һ";
            root.Category = category;
            db.insert( root );

            Assert.Greater( root.Id, 0 );
            TDataRoot data = TDataRoot.findById( root.Id );
            Assert.IsNotNull( data );
            Assert.AreEqual( root.Title, data.Title );
            Assert.AreEqual( category.Id, data.Category.Id );
            Assert.AreEqual( category.Name, data.Category.Name );
        }

        public void FindAll() {
            ConsoleTitleUtil.ShowTestTitle( "FindAll" );

            Console.WriteLine( "__tpost__" );

            // ����ֱ�Ӳ�ѯ�������򵥣�û�ж��⿼��
            //IList list = TPost.findAll();
            IList list = db.findAll<TPost>();

            Assert.AreEqual( list.Count, 20 );
            foreach (TPost post in list) {
                Assert.AreEqual( typeof( TPostCategory ), post.Category.GetType() ); //����ʵ�����Ե�ʱ��ʹ����FindBy����
                Console.WriteLine( "Id:{0}  Title:{1}", post.Id, post.Title );
            }

            Console.WriteLine( "__ttopic__" );

            //IList topicList = TTopic.findAll();
            IList topicList = db.findAll<TTopic>();

            Assert.AreEqual( topicList.Count, 23 );
            foreach (TTopic post in topicList) {
                Assert.AreEqual( typeof( TTopicCategory ), post.Category.GetType() );
                Console.WriteLine( "Id:{0}  Title:{1}", post.Id, post.Title );
            }

            Console.WriteLine( "__findAll__" );

            //��̬��ѯ���������ѯ����ϲ������Ҹ��ݶ�̬��ѯ��������
            IList list2 = TDataRoot.findAll();
            Assert.AreEqual( 46, list2.Count );

            int rootDataCount = 0;

            foreach (TDataRoot root in list2) {
                if (root.GetType() == typeof( TPost )) {
                    Assert.AreEqual( typeof( TPostCategory ), root.Category.GetType() );
                }
                else if (root.GetType() == typeof( TTopic )) {
                    Assert.AreEqual( typeof( TTopicCategory ), root.Category.GetType() );
                }
                else if (root.GetType() == typeof( TDataRoot )) {
                    rootDataCount += 1;
                    Assert.AreEqual( typeof( TCategory ), root.Category.GetType() );
                }

                Console.WriteLine( "id:{1} [type]{0} [categoryType]:{3} title:{2}", Entity.GetInfo( root ).FullName, root.Id, root.Title, root.Category.GetType() );
            }
            Assert.AreEqual( 3, rootDataCount );

        }

        [Test]
        public void FindBy() {
            ConsoleTitleUtil.ShowTestTitle( "Find" );

            IList list = TDataRoot.find( "Category.Name=:cname" ).set( "cname", "post���ӷ���" ).select( "Id,Title,Body,Category.Name" ).list();
            Assert.AreEqual( 23, list.Count );
            Console.WriteLine( "���н����" + list.Count );

            int rootDataCount = 0;
            foreach (TDataRoot root in list) {
                Assert.AreEqual( "post���ӷ���", root.Category.Name );

                // ��Ϊ�Ƕ�̬���������������

                if (root.GetType() == typeof( TPost )) {
                    Assert.AreEqual( typeof( TPostCategory ), root.Category.GetType() ); //ÿ�����͵����ԣ������ࡱҲ�Ƕ�̬��ѯ�õ��ģ�������ͬ��������Ӧ�ĸ��������
                }
                else if (root.GetType() == typeof( TTopic )) {
                    Assert.AreEqual( typeof( TTopicCategory ), root.Category.GetType() );
                }
                else if (root.GetType() == typeof( TDataRoot )) {
                    rootDataCount += 1;
                    Assert.AreEqual( typeof( TCategory ), root.Category.GetType() );
                }


                Console.WriteLine( "id:{1} type:{0} title:{2}", Entity.GetInfo( root ).FullName, root.Id, root.Title );
            }
            Assert.AreEqual( 3, rootDataCount );
        }


        public static IList FindPage() {
            return null; //TODO
        }


        // �������ʱ��һ�����ࡢ�������඼Ҫ������
        // 1�����ཨ���ʱ�򣬽�ID����������ȥ��������Int����
        [Test]
        public void A_Insert() {


            wojiluOrmTestInit.ClearLog();
            wojiluOrmTestInit.InitMetaData();

            ConsoleTitleUtil.ShowTestTitle( "Insert" );

            //wojilu.file.Delete( "log.txt" );

            // �˴�Ӧ����������������
            // Ȼ��������������ݣ�ͬʱ����insert sql������Id��ֵ��

            // �ܹ����46������
            for (int i = 0; i < 20; i++) {

                // �ڲ������ݵ�ʱ�򣬶�̬����û���ر���Ҫע���

                TPostCategory pcat = new TPostCategory();
                pcat.Name = "post���ӷ���";
                pcat.Hits = new Random().Next( 1, 100 );
                db.insert( pcat );
                Assert.Greater( pcat.Id, 0 );

                TTopicCategory tcat = new TTopicCategory();
                tcat.Name = "topic�������";
                tcat.ReplyCount = new Random().Next( 1, 200 );
                db.insert( tcat );
                Assert.Greater( tcat.Id, 0 );

                TPost post = new TPost();
                post.Title = "post_34��������Ա�����º�";
                post.Body = "ϣ����Ĺ�ȥ�ķ����ִ�Y��¯�ĒY���_��";
                post.Uid = "����";
                post.Category = pcat; // ��̬�������
                post.Hits = new Random().Next();
                db.insert( post );
                Assert.Greater( post.Id, 0 );

                TTopic topic = new TTopic();
                topic.Title = "topic_������������";
                topic.Body = "�����ƺ�����֣��������Ȼ�ǲ���˵�ġ����Ǻ�������˵�����ܡ������һ�����䡣";
                topic.Uid = "����";
                topic.Category = tcat;
                topic.Hits = new Random().Next( 34, 10039343 );
                topic.ReplyCount = 3;
                db.insert( topic );
                Assert.Greater( topic.Id, 0 );


            }

            for (int i = 0; i < 3; i++) {

                // ����������ӣ�����������ƺ����ӷ���������ͬ������������Ե�ʱ���Ƿ�Ҳ�ڶ�̬��ѯ�����
                TTopicCategory tcatfake = new TTopicCategory();
                tcatfake.Name = "zzTopic���ӷ���";
                tcatfake.ReplyCount = new Random().Next( 1, 200 );
                db.insert( tcatfake );
                Assert.Greater( tcatfake.Id, 0 );

                TTopic topicfake = new TTopic();
                topicfake.Title = "zzTopic������������";
                topicfake.Body = "�����ƺ�����֣��������Ȼ�ǲ���˵�ġ����Ǻ�������˵�����ܡ������һ�����䡣";
                topicfake.Uid = "����";
                topicfake.Category = tcatfake;
                topicfake.Hits = new Random().Next( 34, 10039343 );
                topicfake.ReplyCount = 3;
                db.insert( topicfake );
                Assert.Greater( topicfake.Id, 0 );

                // ֱ����Ӹ���ľ�������
                TCategory category = new TCategory();
                category.Name = "post���ӷ���";
                db.insert( category );

                TDataRoot root = new TDataRoot();
                root.Title = "zzParent���Ǹ���֮init��ʼ��";
                root.Body = "���������֮init��ʼ��";
                root.Category = category;
                db.insert( root );
            }

            insertAbstractTest();

        }


        private void insertAbstractTest() {

            for (int i = 1; i < 10; i++) {
                TAbNewCategory abCategory = new TAbNewCategory();
                abCategory.Name = "���Ǽ̳е�����_" + i;
                abCategory.Title = "���Ⱑ_" + i;
                abCategory.Hits = 2 + i;
                db.insert( abCategory );
                Assert.AreEqual( abCategory.Id, i );

                TAbNewCategory2 abCategory2 = new TAbNewCategory2();
                abCategory2.Name = "���Ǽ̳е�����2_" + i;
                abCategory2.Title2 = "���Ⱑ2_" + i;
                db.insert( abCategory2 );
                Assert.AreEqual( abCategory2.Id, i );
            }
        }

        [Test]
        public void X_UpdateAndDelete() {

            Update();
            Delete();

        }


        public void Delete() {
            ConsoleTitleUtil.ShowTestTitle( "Delete" );

            //����ɾ����ɾ�������ͬʱ��ҲҪɾ�������е�����
            TPost post = TPost.findById( 11 ) as TPost;
            Assert.IsNotNull( post );
            db.delete( post );

            post = TPost.findById( 11 ) as TPost;
            Assert.IsNull( post );

            //����ɾ����ͬʱɾ������
            TDataRoot.delete( 13 );
            Assert.IsNull( TDataRoot.findById( 13 ) );
        }

        public void Update() {
            ConsoleTitleUtil.ShowTestTitle( "Update" );

            //������£�������Ӧ����ҲҪ����
            TPost post = TPost.findById( 3 ) as TPost;
            post.Title = "**����֮�������";
            db.update( post );

            string sql = "select title from Tdataroot where id=3";
            string title = wojilu.Data.EasyDB.ExecuteScalar( sql, wojilu.Data.DbContext.getConnection(post.GetType()) ) as string;

            Assert.IsNotNull( title );
            Assert.AreEqual( post.Title, title );

            //������£���ʵ����ʵ�ĵײ�������£�ͬ��
        }


        [TestFixtureSetUp]
        public void Init() {
            wojiluOrmTestInit.ClearLog();
            wojiluOrmTestInit.InitMetaData();

        }

        [TestFixtureTearDown]
        public void clear() {


            wojiluOrmTestInit.ClearTables();

        }


    }
}
