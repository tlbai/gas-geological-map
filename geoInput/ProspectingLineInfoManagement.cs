﻿using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using GIS;
using GIS.Common;
using LibCommon;
using LibEntity;

namespace geoInput
{
    public partial class ProspectingLineInfoManagement : Form
    {
        // 构造方法
        public ProspectingLineInfoManagement()
        {
            InitializeComponent();
        }

        private void RefreshData()
        {
            gcProspectingLine.DataSource = ProspectingLine.FindAll();
        }

        /// <summary>
        ///     添加（必须实装）
        /// </summary>
        /// <params name="sender"></params>
        /// <params name="e"></params>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            var prospectingLineInfoEnteringForm = new ProspectingLineInfoEntering();
            if (DialogResult.OK == prospectingLineInfoEnteringForm.ShowDialog())
            {
                RefreshData();
            }
        }

        /// <summary>
        ///     修改（必须实装）
        /// </summary>
        /// <params name="sender"></params>
        /// <params name="e"></params>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var prospectingLineInfoEnteringForm =
                new ProspectingLineInfoEntering((ProspectingLine)gridView1.GetFocusedRow());
            if (DialogResult.OK == prospectingLineInfoEnteringForm.ShowDialog())
            {
                RefreshData();
            }
        }

        /// <summary>
        ///     删除按钮（必须实装）
        /// </summary>
        /// <params name="sender"></params>
        /// <params name="e"></params>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!Alert.Confirm("确定要删除勘探线吗？")) return;
            var prospectingLine = (ProspectingLine)gridView1.GetFocusedRow();
            DeleteJLDCByBID(new[] { prospectingLine.binding_id });
            prospectingLine.Delete();
            RefreshData();
        }

        #region 删除勘探线图元

        /// <summary>
        ///     根据勘探线层绑定ID删除勘探线层图元
        /// </summary>
        /// <params name="sfpFaultageBidArray">要删除勘探线层的绑定ID</params>
        private void DeleteJLDCByBID(string[] sfpFaultageBidArray)
        {
            if (sfpFaultageBidArray.Length == 0) return;

            //1.获得当前编辑图层
            var drawspecial = new DrawSpecialCommon();
            const string sLayerAliasName = LayerNames.DEFALUT_KANTANXIAN; //“勘探线层”图层
            var featureLayer = drawspecial.GetFeatureLayerByName(sLayerAliasName);
            if (featureLayer == null)
            {
                MessageBox.Show(@"未找到" + sLayerAliasName + @"图层,无法删除揭露断层图元。");
                return;
            }

            //2.删除勘探线层图元
            foreach (var sfpFaultageBid in sfpFaultageBidArray)
            {
                DataEditCommon.DeleteFeatureByBId(featureLayer, sfpFaultageBid);
            }
        }

        #endregion

        /// <summary>
        ///     退出
        /// </summary>
        /// <params name="sender"></params>
        /// <params name="e"></params>
        private void btnExit_Click(object sender, EventArgs e)
        {
            // 关闭窗口
            Close();
        }

        /// <summary>
        ///     导出
        /// </summary>
        /// <param name="sender"></params>
        /// <params name="e"></params>
        private void tsBtnExport_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                gcProspectingLine.ExportToXls(saveFileDialog1.FileName);
            }
        }

        /// <summary>
        ///     打印
        /// </summary>
        /// <params name="sender"></params>
        /// <params name="e"></params>
        private void tsBtnPrint_Click(object sender, EventArgs e)
        {
            DevUtil.DevPrint(gcProspectingLine, "勘探线信息报表");
        }

        /// <summary>
        ///     刷新
        /// </summary>
        /// <params name="sender"></params>
        /// <params name="e"></params>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        ///     图显按钮事件
        /// </summary>
        /// <params name="sender"></params>
        /// <params name="e"></params>
        private void btnMap_Click(object sender, EventArgs e)
        {
            var bid = ((ProspectingLine)gridView1.GetFocusedRow()).binding_id;
            var pLayer = DataEditCommon.GetLayerByName(DataEditCommon.g_pMap, LayerNames.DEFALUT_KANTANXIAN);
            if (pLayer == null)
            {
                MessageBox.Show(@"未发现勘探线图层！");
                return;
            }
            var pFeatureLayer = (IFeatureLayer)pLayer;
            var str = "";
            //for (int i = 0; i < iSelIdxsArr.Length; i++)
            //{

            if (bid != "")
            {
                if (true)
                    str = "bid='" + bid + "'";
                //else
                //    str += " or bid='" + bid + "'";
            }
            //}
            var list = MyMapHelp.FindFeatureListByWhereClause(pFeatureLayer, str);
            if (list.Count > 0)
            {
                MyMapHelp.Jump(MyMapHelp.GetGeoFromFeature(list));
                DataEditCommon.g_pMap.ClearSelection();
                foreach (var t in list)
                {
                    DataEditCommon.g_pMap.SelectFeature(pLayer, t);
                }
                WindowState = FormWindowState.Normal;
                Location = DataEditCommon.g_axTocControl.Location;
                Width = DataEditCommon.g_axTocControl.Width;
                Height = DataEditCommon.g_axTocControl.Height;
                DataEditCommon.g_pMyMapCtrl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null,
                    DataEditCommon.g_pAxMapControl.Extent);
            }
            else
            {
                Alert.AlertMsg("图元丢失");
            }
        }

        private void ProspectingLineInfoManagement_Load(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}