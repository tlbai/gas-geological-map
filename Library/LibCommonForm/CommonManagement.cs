﻿using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Castle.ActiveRecord;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using LibBusiness;
using LibCommon;
using LibEntity;

namespace LibCommonForm
{
    public partial class CommonManagement : Form
    {
        // 功能识别位

        private const int FlagManangingMineName = 1;
        private const int FlagManangingHorizontal = 2;
        private const int FlagManangingMiningArea = 3;
        private const int FlagManangingWorkingFace = 4;
        private const int FlagManangingCoalSeam = 5;
        private static int _typeFlag;
        // id

        /// <summary>
        ///     构造方法
        /// </summary>
        public CommonManagement()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     带参数的构造方法
        /// </summary>
        /// <params name="typeFlag"></params>
        /// <params name="id"></params>
        public CommonManagement(int typeFlag, int id)
        {
            InitializeComponent();

            Id = id;
            _typeFlag = typeFlag;

            switch (typeFlag)
            {
                case FlagManangingMineName:
                    {
                        // 窗口标题
                        Text = @"矿井名称管理";
                        AddIdColumn("编号", "id");

                        // 矿井名称
                        gridView1.Columns.Add(new GridColumn
                        {
                            Caption = @"矿井名称",
                            FieldName = "name",
                            VisibleIndex = gridView1.Columns.Count
                        });

                        AddDeleteButton();

                        gridControl1.DataSource = CollectionHelper.ConvertTo(Mine.FindAll());
                    }
                    break;
                case FlagManangingHorizontal:
                    {
                        Text = @"水平名称管理";
                        AddIdColumn("编号", "id");

                        // 矿井名称
                        gridView1.Columns.Add(new GridColumn
                        {
                            Caption = @"水平名称",
                            FieldName = "name",
                            VisibleIndex = gridView1.Columns.Count
                        });

                        gridView1.Columns.Add(new GridColumn
                        {
                            Caption = @"所在矿区",
                            FieldName = "mine",
                            ColumnEdit = lueMine,
                            VisibleIndex = gridView1.Columns.Count,
                            FilterMode = ColumnFilterMode.DisplayText
                        });

                        AddDeleteButton();
                        lueMine.DataSource = Mine.FindAll();
                        gridControl1.DataSource = CollectionHelper.ConvertTo(Horizontal.FindAll());
                    }
                    break;
                case FlagManangingMiningArea:
                    {
                        Text = @"采区名称管理";
                        AddIdColumn("编号", "id");

                        // 矿井名称
                        gridView1.Columns.Add(new GridColumn
                        {
                            Caption = @"采区名称",
                            FieldName = "name",
                            VisibleIndex = gridView1.Columns.Count
                        });


                        // 所属水平
                        gridView1.Columns.Add(new GridColumn
                        {
                            Caption = @"所在水平",
                            FieldName = "horizontal",
                            ColumnEdit = lueHorizontal,
                            VisibleIndex = gridView1.Columns.Count,
                            FilterMode = ColumnFilterMode.DisplayText
                        });

                        AddDeleteButton();
                        lueHorizontal.DataSource = Horizontal.FindAll();
                        gridControl1.DataSource = CollectionHelper.ConvertTo(MiningArea.FindAll());

                    }
                    break;
                case FlagManangingWorkingFace:
                    {
                        Text = @"工作面名称管理";
                        AddIdColumn("编号", "id");

                        // 矿井名称
                        gridView1.Columns.Add(new GridColumn
                        {
                            Caption = @"工作面名称",
                            FieldName = "name",
                            VisibleIndex = gridView1.Columns.Count
                        });

                        gridView1.Columns.Add(new GridColumn
                        {
                            Caption = @"所在采区",
                            FieldName = "mining_area",
                            ColumnEdit = lueMiningArea,
                            VisibleIndex = gridView1.Columns.Count,
                            FilterMode = ColumnFilterMode.DisplayText
                        });

                        AddDeleteButton();
                        lueMiningArea.DataSource = MiningArea.FindAll();
                        gridControl1.DataSource = CollectionHelper.ConvertTo(Workingface.FindAll());
                    }
                    break;   
            }
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public static int Id { get; set; }

        /// <summary>
        ///     提交
        /// </summary>
        /// <params name="sender"></params>
        /// <params name="e"></params>
        private void btnSubmit_Click(object sender, EventArgs e)
        {

            //gridView1.UpdateCurrentRow();
            switch (_typeFlag)
            {
                case FlagManangingMineName:
                    // 矿井名称管理
                    UpdateInfo<Mine>();
                    break;
                case FlagManangingHorizontal:
                    // 水平名称管理
                    UpdateInfo<Horizontal>();
                    break;
                case FlagManangingMiningArea:
                    // 采区名称管理
                    UpdateInfo<MiningArea>();
                    break;
                case FlagManangingWorkingFace:
                    // 工作面名称管理
                    UpdateInfo<Workingface>();
                    break;
            }
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        ///     更新矿井信息
        /// </summary>
        private void UpdateInfo<T>() where T : ActiveRecordBase
        {
            //TODO:此处需要优化强力优化！！！
            var dt = (DataTable)gridControl1.DataSource;
            var list = CollectionHelper.ConvertTo<T>(dt);
            try
            {
                foreach (var t in list.Where(t => t != null))
                {
                    t.Save();
                }
            }
            catch (Exception)
            {
                Alert.AlertMsg("输入信息有误，请检查信息是否输入正确");
                return;
            }

            Alert.AlertMsg("修改成功！");
        }

        /// <summary>
        ///     更新矿井信息
        /// </summary>
        private void DeleteInfo<T>() where T : ActiveRecordBase
        {
            var obj = (DataRowView)gridView1.GetFocusedRow();
            var list = CollectionHelper.ConvertTo<T>(new[] { obj.Row });
            list.First().Delete();
        }

        /// <summary>
        ///     取消
        /// </summary>
        /// <params name="sender"></params>
        /// <params name="e"></params>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 窗口关闭
            Close();
        }

        private void AddDeleteButton()
        {
            //delButton.Buttons.Add(new EditorButton(ButtonPredefines.Delete));
            gridView1.Columns.Add(new GridColumn
            {
                Caption = @"删除",
                MaxWidth = 60,
                VisibleIndex = gridView1.Columns.Count,
                ColumnEdit = beDelete
            });
        }

        private void AddIdColumn(string caption, string fieldName)
        {
            var col = new GridColumn
            {
                Caption = caption,
                FieldName = fieldName,
                MaxWidth = 60,
                VisibleIndex = gridView1.Columns.Count,
            };
            col.OptionsColumn.AllowEdit = false;
            gridView1.Columns.Add(col);
        }

        private void gridView1_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "mining_area":
                    {
                        if (e.Value == null) return;
                        var miningArea = (MiningArea)e.Value;
                        e.DisplayText = miningArea.name;
                        break;
                    }
                case "horizontal":
                    {
                        if (e.Value == null) return;
                        var horizontal = (Horizontal)e.Value;
                        e.DisplayText = horizontal.name;
                        break;
                    }
                case "mine":
                    {
                        if (e.Value == null) return;
                        var mine = (Mine)e.Value;
                        e.DisplayText = mine.name;
                        break;
                    }
            }
        }

        private void beDelete_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (!Alert.Confirm("确认删除数据？")) return;
            switch (_typeFlag)
            {
                case FlagManangingMineName:
                    // 矿井名称管理
                    DeleteInfo<Mine>();
                    gridControl1.DataSource = CollectionHelper.ConvertTo(Mine.FindAll());
                    break;
                case FlagManangingHorizontal:
                    // 水平名称管理
                    DeleteInfo<Horizontal>();
                    gridControl1.DataSource = CollectionHelper.ConvertTo(Horizontal.FindAll());
                    break;
                case FlagManangingMiningArea:
                    // 采区名称管理
                    DeleteInfo<MiningArea>();
                    gridControl1.DataSource = CollectionHelper.ConvertTo(MiningArea.FindAll());
                    break;
                case FlagManangingWorkingFace:
                    // 工作面名称管理
                    DeleteInfo<Workingface>();
                    gridControl1.DataSource = CollectionHelper.ConvertTo(Workingface.FindAll());
                    break;
            }
        }
    }
}