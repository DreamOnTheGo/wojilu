/*
 * Copyright (c) 2010, www.wojilu.com. All rights reserved.
 */

using System;

namespace wojilu.Common.Skins {

    public interface ISkin {

        int Id { get; set; }

        /// <summary>
        /// �����˻��Զ�����
        /// </summary>
        int MemberId { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// �Զ�����ʽ������
        /// </summary>
        String Body { get; set; }

        /// <summary>
        /// ���
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// ��ʽ��·��(�洢�������·��������·����ʹ��GetSkinPath����)
        /// </summary>
        String StylePath { get; set; }

        /// <summary>
        /// ����Ч��������ͼ·��(�洢�������·��������·����ʹ��GetThumbPath����)
        /// </summary>
        String ThumbUrl { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        int Hits { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        int Replies { get; set; }

        /// <summary>
        /// ʹ������
        /// </summary>
        int MemberCount { get; set; }

        /// <summary>
        /// ״̬(Ԥ��������)
        /// </summary>
        int Status { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        DateTime CreateTime { get; set; }

        /// <summary>
        /// ����Ч��ͼƬ��·��
        /// </summary>
        /// <returns></returns>
        String GetScreenShotPath();

        /// <summary>
        /// ������ʽ�������·��
        /// </summary>
        /// <returns></returns>
        String GetSkinPath();

        /// <summary>
        /// ��������ͼ������·��
        /// </summary>
        /// <returns></returns>
        String GetThumbPath();

        /// <summary>
        /// ��ȡ��ʽ���ݣ�����Զ����ˣ������Զ������ݣ����򷵻���ʽ������
        /// </summary>
        /// <returns></returns>
        String GetSkinContent();

    }

}
