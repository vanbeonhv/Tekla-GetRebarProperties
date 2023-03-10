using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tekla.Structures;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.Operations;

using TSM = Tekla.Structures.Model;
using TSMUI = Tekla.Structures.Model.UI;
using TSG = Tekla.Structures.Geometry3d;

using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Tekla.Structures.ModelInternal;
using Tekla.Structures.Solid;
using System.Threading;
using ComboBox = System.Windows.Forms.ComboBox;
using Microsoft.Office.Interop.Excel;
using Model = Tekla.Structures.Model.Model;

namespace Get_Rebar_Length
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private BindingList<RebarDetail> rebarDetails = new BindingList<RebarDetail> { };

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = rebarDetails;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Model model = new Model();
                if (!model.GetConnectionStatus())
                {
                    MessageBox.Show("Connect fail!");
                }
                else
                {
                    bool isPicking = true;
                    try
                    {
                        ArrayList propsS = new ArrayList();
                        ArrayList propsD = new ArrayList();
                        ArrayList propsI = new ArrayList();
                        propsS.Add("NAME");
                        propsD.Add("LENGTH");

                        do
                        {
                            Picker picker = new Picker();
                            ModelObject myEnum = picker.PickObject(Picker.PickObjectEnum.PICK_ONE_REINFORCEMENT, "pick something");
                            RebarGroup rebar = myEnum as RebarGroup;
                            Hashtable hashtable = new Hashtable();
                            rebar.GetAllReportProperties(propsS, propsD, propsI, ref hashtable);
                            foreach (string key in hashtable.Keys)
                            {
                                //Console.WriteLine(String.Format("{0}: {1}", key, hashtable[key]));
                                rebarDetails.Add(new RebarDetail() { rebarName = (string)hashtable["NAME"], rebarLength = (double)hashtable["LENGTH"] });
                                //(double)hashtable[key]
                            }
                            foreach (var value in hashtable.Values)
                            {
                                Console.WriteLine(value + "");
                            }
                            dataGridView1.Refresh();
                        } while (isPicking);
                    }
                    catch (Exception)
                    {
                        isPicking = false;
                    }
                    Console.WriteLine(isPicking.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }
    }
}