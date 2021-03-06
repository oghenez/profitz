﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using System.Collections;
using Profit.Server;

namespace Profit
{
    public partial class MarkDownSellingPriceForm : KryptonForm
    {
        Repository r_group = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_GROUP_REPOSITORY);
        Repository r_ccy = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY);
        Repository r_category = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_CATEGORY_REPOSITORY);
        Repository r_pricecategory = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PRICE_CATEGORY_REPOSITORY);
        Repository r_unit = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY);
        PartRepository r_part = (PartRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY);


        IList m_partGroupList = new ArrayList();
        IList m_currencyList = new ArrayList();
        IList m_partCategoryList = new ArrayList();
        IList m_priceCategoryList = new ArrayList();
        IList m_partList = new ArrayList();


        IDictionary<int, PartGroup> m_partGroupDic = new Dictionary<int, PartGroup>();
        IDictionary<int, Currency> m_ccyDic = new Dictionary<int, Currency>();
        IDictionary<int, PartCategory> m_partCategoryDic = new Dictionary<int, PartCategory>();
        IDictionary<int, PriceCategory> m_priceCategoryDic = new Dictionary<int, PriceCategory>();
        IDictionary<int, Unit> m_unitDic = new Dictionary<int, Unit>();

        MarkUpDownSellingPrice m_marksellingprice = new MarkUpDownSellingPrice();
        IList m_result = new ArrayList();

        public MarkDownSellingPriceForm()
        {
            InitializeComponent();
            initDataSource();
        }

        private void initDataSource()
        {
            m_partGroupList = r_group.GetAll();
            m_partGroupList.Add(new PartGroup(0, "ALL"));
            partGroupkryptonComboBox1.DataSource = m_partGroupList;
            m_partCategoryList = r_category.GetAll();
            m_partCategoryList.Add(new PartCategory(0, "ALL"));
            partCategorykryptonComboBox5.DataSource = m_partCategoryList;
            m_priceCategoryList = r_pricecategory.GetAll();
            m_priceCategoryList.Add(new PriceCategory(0, "ALL"));
            pricecategorykryptonComboBox4.DataSource = m_priceCategoryList;
            m_currencyList = r_ccy.GetAll();
            currencykryptonComboBox4.DataSource = m_currencyList;
            m_partList = r_part.GetAll();
            m_partList.Add(new Part(0, "ALL"));
            partkryptonComboBox1.DataSource = m_partList;



            //typekryptonComboBox1.DataSource = Enum.GetValues(typeof(MarkUpDownSellingPriceType));
            baseonkryptonComboBox2.DataSource = Enum.GetValues(typeof(MarkUpDownSellingPriceBaseOn));
            marktypekryptonComboBox3.DataSource = Enum.GetValues(typeof(MarkUpDownSellingPriceMarkType));
            roundTypekryptonComboBox4.DataSource = Enum.GetValues(typeof(RoundType));

            partGroupkryptonComboBox1.Text = "ALL";
            partCategorykryptonComboBox5.Text = "ALL";
            pricecategorykryptonComboBox4.Text = "ALL";
            currencykryptonComboBox4.Text = "ALL";
            partkryptonComboBox1.Text = "ALL";

            initDictionary();
        }

        private void initDictionary()
        {
            foreach (PartGroup pg in m_partGroupList)
            {
                m_partGroupDic.Add(pg.ID,pg);
            }
            foreach (Currency ccy in m_currencyList)
            {
                m_ccyDic.Add(ccy.ID,ccy);
            }
            foreach (PartCategory prtCat in m_partCategoryList)
            {
                m_partCategoryDic.Add(prtCat.ID,prtCat);
            }
            foreach (PriceCategory prcCat in m_priceCategoryList)
            {
                m_priceCategoryDic.Add(prcCat.ID,prcCat);
            }
            foreach (Unit u in r_unit.GetAll())
            {
                m_unitDic.Add(u.ID,u);
            }
        }

        private void findkryptonButton1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                updateEntity();
                IList result = r_part.FindAllMarkDownSellingPrice(m_marksellingprice);
                partDataGridView1.Rows.Clear();
                m_result = result;
                foreach (Part p in result)
                {
                    p.UNIT = m_unitDic[p.UNIT.ID];
                    p.PART_GROUP = m_partGroupDic[p.PART_GROUP.ID];
                    p.PART_CATEGORY = m_partCategoryDic[p.PART_CATEGORY.ID];
                    p.CURRENCY = m_ccyDic[p.CURRENCY.ID];

                    int r = partDataGridView1.Rows.Add();
                    partDataGridView1[codeColumn.Index, r].Value = p.CODE;
                    partDataGridView1[nameColumn.Index, r].Value = p.NAME;
                    partDataGridView1[codeColumn.Index, r].Value = p.CODE;
                    partDataGridView1[unitColumn.Index, r].Value = p.UNIT.CODE;
                    partDataGridView1[groupColumn.Index, r].Value = p.PART_GROUP.CODE;
                    partDataGridView1[categoryColumn.Index, r].Value = p.PART_CATEGORY.CODE;
                    partDataGridView1[costpriceColumn.Index, r].Value = p.COST_PRICE;
                    partDataGridView1[sellpriceColumn.Index, r].Value = p.SELL_PRICE;
                    partDataGridView1[newsellpriceColumn.Index, r].Value = p.NEW_SELL_PRICE;
                    partDataGridView1[ccyColumn.Index, r].Value = p.CURRENCY.CODE;
                    partDataGridView1.Rows[r].Tag = p;
                }
                foundkryptonLabel12.Text = "Found " + partDataGridView1.Rows.Count + " record(s)";
                UserSetting.AddNumberToGrid(partDataGridView1);
                this.Cursor = Cursors.Default;
            }
            catch (Exception x)
            {
                KryptonMessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void updateEntity()
        {
            m_marksellingprice.CURRENCY = (Currency)currencykryptonComboBox4.SelectedItem;
            m_marksellingprice.MARK_BASE_ON = (MarkUpDownSellingPriceBaseOn)baseonkryptonComboBox2.SelectedItem;
            m_marksellingprice.MARK_MARK_TYPE = (MarkUpDownSellingPriceMarkType)marktypekryptonComboBox3.SelectedItem;
            //m_marksellingprice.MARK_TYPE = (MarkUpDownSellingPriceType)typekryptonComboBox1.SelectedItem;
            m_marksellingprice.PART = (Part)partkryptonComboBox1.SelectedItem;
            m_marksellingprice.PART_CATEGORY = (PartCategory)partCategorykryptonComboBox5.SelectedItem;
            m_marksellingprice.PART_GROUP = (PartGroup)partGroupkryptonComboBox1.SelectedItem;
            m_marksellingprice.PERCENTAGE = Convert.ToDouble(percemtagekryptonNumericUpDown1.Value);
            m_marksellingprice.PRICE_CATEGORY = (PriceCategory)pricecategorykryptonComboBox4.SelectedItem;
            m_marksellingprice.ROUND_TYPE = (RoundType)roundTypekryptonComboBox4.SelectedItem;
            m_marksellingprice.ROUNDING = Convert.ToDouble(roundValuekryptonNumericUpDown3.Value);
            m_marksellingprice.VALUE = Convert.ToDouble(valuekryptonNumericUpDown2.Value);
        }

        private void MarkDownSellingPriceForm_Load(object sender, EventArgs e)
        {
            UserSetting.LoadSetting(partDataGridView1, 1, this.Name);
        }

        private void MarkDownSellingPriceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserSetting.SaveSetting(partDataGridView1, 1, this.Name);
        }

        private void partDataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
        }

        private void partDataGridView1_Sorted(object sender, EventArgs e)
        {
            UserSetting.AddNumberToGrid(partDataGridView1);
        }

        private void marktypekryptonComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            currencykryptonComboBox4.Enabled = ((MarkUpDownSellingPriceMarkType)marktypekryptonComboBox3.SelectedItem) == MarkUpDownSellingPriceMarkType.Value;
            valuekryptonNumericUpDown2.Enabled = currencykryptonComboBox4.Enabled;
            percemtagekryptonNumericUpDown1.Enabled = !currencykryptonComboBox4.Enabled;
        }

        private void updatekryptonButton1_Click(object sender, EventArgs e)
        {
            if (KryptonMessageBox.Show("Are you sure to update?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (m_result.Count > 0)
                    {
                        r_part.UpdateSellingPrice(m_result);
                        KryptonMessageBox.Show("Update completed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    this.Cursor = Cursors.Default;
                }
                catch (Exception x)
                {
                    KryptonMessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void buttonSpecHeaderGroup1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void partkryptonComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            partkryptonTextBox2.Text = "";
            if (partkryptonComboBox1.SelectedItem != null)
            {
                Part p = (Part)partkryptonComboBox1.SelectedItem;
                partkryptonTextBox2.Text = p.NAME;
            }
        }

        private void partGroupkryptonComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            partGroupkryptonTextBox.Text = "";
            if (partGroupkryptonComboBox1.SelectedItem != null)
            {
                PartGroup p = (PartGroup)partGroupkryptonComboBox1.SelectedItem;
                partGroupkryptonTextBox.Text = p.NAME;
            }
        }

        private void partCategorykryptonComboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            partCategorykryptonTextBox.Text = "";
            if (partCategorykryptonComboBox5.SelectedItem != null)
            {
                PartCategory p = (PartCategory)partCategorykryptonComboBox5.SelectedItem;
                partCategorykryptonTextBox.Text = p.NAME;
            }
        }

        private void pricecategorykryptonComboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            priceCategorykryptonTextBox1.Text = "";
            if (pricecategorykryptonComboBox4.SelectedItem != null)
            {
                PriceCategory p = (PriceCategory)pricecategorykryptonComboBox4.SelectedItem;
                priceCategorykryptonTextBox1.Text = p.NAME;
            }
        }

    }
}
