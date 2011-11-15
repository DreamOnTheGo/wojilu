using System;
using System.Collections;

using wojilu.ORM;

namespace wojilu.Test.Orm.Entities {

    public class TValidateData : ObjectBase<TValidateData> {
        [NotNull]
        public string Body { get; set; }
    }

    public class TValidateData2 : ObjectBase<TValidateData2> {
        [NotNull( "����д����" )]
        public string Body { get; set; }
    }

    public class TValidateData3 : ObjectBase<TValidateData3> {

        [Email]
        public string Email { get; set; }
    }

    public class TValidateData4 : ObjectBase<TValidateData4> {

        [Email( "����ȷ��д�����ʼ�" )]
        public string Email { get; set; }
    }

    public class TValidateData5 : ObjectBase<TValidateData5> {

        [Unique]
        public string Name { get; set; }
    }

    public class TValidateData6 : ObjectBase<TValidateData6> {

        [Unique( "�û����ظ�" )]
        public string Name { get; set; }
    }
}
