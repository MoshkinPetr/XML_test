using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace XML_test
{
    public partial class Form1 : Form
    {
        private static string fileName = "Files\\24_21_1003001_2017-05-29_kpt11.xml";
        private static string savedFileName = null;

        private XDocument doc = XDocument.Load(fileName);
                
        private List<string> name = new List<string>() { "Parcels", "ObjectRealty (build)", "ObjectRealty (construction)", "SpatialData", "Bound", "Zone" };
        private List<string> parent = new List<string>() { "land_record", "build_record", "construction_record", "spatial_data", "municipal_boundary_record", "zones_and_territories_record" };
        private List<string> child = new List<string>() { "common_data", "common_data", "common_data", "entity_spatial", "b_object", "b_object" };
        private List<string> id = new List<string> { "cad_number", "cad_number", "cad_number", "sk_id", "reg_numb_border", "reg_numb_border" };

        

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                treeView1.Nodes.Add(new TreeNode(name[i]));

                Search(i, parent[i], child[i], id[i]);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string test;
            if (e.Node.Parent != null)
            {
                savedFileName += e.Node.Parent.Text;
                int numb = name.IndexOf(e.Node.Parent.Text);

                foreach (XElement ele in doc.Descendants(parent[numb]))
                {
                    foreach (XElement ele2 in ele.Descendants(child[numb]))
                    {
                        test = ele2.Element(id[numb]).Value;
                        if (test == e.Node.Text)
                        {
                            richTextBox1.Text +=  ele.ToString() + "\n\n";
                            break;
                        }
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            savedFileName = null;
        }

        private void Search(int i, String name, String name2, String name3)
        {
            foreach (XElement ele in doc.Descendants(name))
            {
                foreach (XElement ele2 in ele.Descendants(name2))
                {
                    treeView1.Nodes[i].Nodes.Add((ele2.Element(name3).Value));
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            
            try
            {
                string path = $@"Files\\XML_{savedFileName}.xml";
                MessageBox.Show(path);
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" + "<saved_information>\r\n" + richTextBox1.Text + "</saved_information>\r\n");

                    fs.Write(info, 0, info.Length);
                } 

                richTextBox1.Clear();
                savedFileName= null;               
            }
            

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
