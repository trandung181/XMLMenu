using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace XMLMenu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string path = "../../data.xml";

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear(); //Xóa các item hiện tại để load lại listView

            DataSet dataSet = new DataSet();
            dataSet.ReadXml(path);
            DataTable dt = new DataTable();
            dt = dataSet.Tables["food"];

            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                listView1.Items.Add(dr["ID"].ToString());
                listView1.Items[i].SubItems.Add(dr["name"].ToString());
                listView1.Items[i].SubItems.Add(dr["price"].ToString());
                listView1.Items[i].SubItems.Add(dr["description"].ToString());
                listView1.Items[i].SubItems.Add(dr["calories"].ToString());
                i++;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            //Phương thức tự sinh id
            string day = DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();
            string hour = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            string second = DateTime.Now.Second.ToString();

            long id = long.Parse(day + month + year + hour + minute + second);

            try
            {
                XDocument testXML = XDocument.Load(path);

                XElement newFood = new XElement("food",
                    new XElement("name", txtName.Text),
                    new XElement("price", txtPrice.Text),
                    new XElement("description", txtDescription.Text),
                    new XElement("calories", txtCalories.Text));

                var lastFood = testXML.Descendants("food").Last();
                long newID = Convert.ToInt64(lastFood.Attribute("ID").Value);
                newFood.SetAttributeValue("ID", id);
                testXML.Element("breakfast_menu").Add(newFood);
                testXML.Save(path);
                Form1_Load(sender, e);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                lblID.Text = item.SubItems[0].Text;
                txtName.Text = item.SubItems[1].Text;
                txtPrice.Text = item.SubItems[2].Text;
                txtDescription.Text = item.SubItems[3].Text;
                txtCalories.Text = item.SubItems[4].Text;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                XDocument testXML = XDocument.Load(path);
                XElement cFood = testXML.Descendants("food").Where(c => c.Attribute("ID").Value.Equals(lblID.Text)).FirstOrDefault();
                cFood.Remove();
                testXML.Save(path);
                Form1_Load(sender, e);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                XDocument testXML = XDocument.Load(path);
                XElement cFood = testXML.Descendants("food").Where(c => c.Attribute("ID").Value.Equals(lblID.Text)).FirstOrDefault();
                cFood.Element("name").Value = txtName.Text;
                cFood.Element("price").Value = txtPrice.Text;
                cFood.Element("description").Value = txtDescription.Text;
                cFood.Element("calories").Value = txtCalories.Text;
                testXML.Save(path);
                Form1_Load(sender, e);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
