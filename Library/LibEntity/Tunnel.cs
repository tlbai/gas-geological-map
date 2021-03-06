﻿using System;
using System.Linq;
using Castle.ActiveRecord;
using NHibernate.Criterion;

namespace LibEntity
{
    [ActiveRecord("tunnels")]
    public class Tunnel : ActiveRecordBase<Tunnel>
    {
        /// <summary>
        ///     巷道编号
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Identity)]
        public int id { get; set; }

        /// <summary>
        ///     巷道名称
        /// </summary>
        [Property]
        public string name { get; set; }

        /// <summary>
        ///     支护方式
        /// </summary>
        [Property]
        public string support_pattern { get; set; }

        /// <summary>
        ///     围岩类型
        /// </summary>
        [Property]
        public string lithology { get; set; }

        /// <summary>
        ///     断面类型
        /// </summary>
        [Property]
        public string section_type { get; set; }

        /// <summary>
        ///     断面参数
        /// </summary>
        [Property]
        public string @params { get; set; }

        /// <summary>
        ///     设计长度
        /// </summary>
        [Property]
        public double design_length { get; set; }

        /// <summary>
        ///     设计面积
        /// </summary>
        [Property]
        public double design_area { get; set; }

        /// <summary>
        ///     巷道类型
        /// </summary>
        [Property]
        public TunnelTypeEnum type { get; set; }

        /// <summary>
        ///     工作面
        /// </summary>
        [BelongsTo("workingface_id")]
        public Workingface workingface { get; set; }

        /// <summary>
        ///     煤巷岩巷
        /// </summary>
        [Property]
        public string coal_or_stone { get; set; }

        [Property]
        public string coal_seam { get; set; }

        /// <summary>
        ///     BID
        /// </summary>
        [Property]
        public string bid { get; set; }


        [Property]
        public string rule_ids { get; set; }

        [Property]
        public string early_warning_params { get; set; }

        [Property]
        public DateTime created_at { get; set; } = DateTime.Now;

        [Property]
        public DateTime updated_at { get; set; } = DateTime.Now;


        /// <summary>
        ///     巷道宽度
        /// </summary>
        [Property]
        public double width { get; set; }

        //public static Tunnel[] find_all_by_working_face_id(int workingfaceId)
        //{
        //    var criterion = new ICriterion[]
        //    {
        //        Restrictions.Eq("Workingface.WorkingFaceId", workingfaceId)
        //    };
        //    return FindAll(criterion);
        //}


        public static bool exists_by_tunnel_name_and_working_face_id(string tunnelName, int workingfaceId)
        {
            var criterion = new ICriterion[]
            {
                Restrictions.Eq("name", tunnelName),
                Restrictions.Eq("workingface.id", workingfaceId)
            };
            return Exists(criterion);
        }

        public override void Delete()
        {
            var wire = Wire.FindAllByProperty("tunnel.id", id).FirstOrDefault();
            if (wire != null)
                wire.Delete();
            base.Delete();
        }
    }
}