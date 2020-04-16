using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudPractice
{
    public partial class Form1 : Form
    {
        Customer model = new Customer();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
        void Clear() {
            txtFirstName.Text = txtLastName.Text = txtCity.Text = txtAddress.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.CustomerId = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            Populate();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtLastName.Text.Trim();
            model.City = txtCity.Text.Trim();
            model.Address = txtAddress.Text.Trim();

            using (EfDBEntities db=new EfDBEntities ())
            {
                if (model.CustomerId == 0)
                    db.Customers.Add(model);
                else
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                
            }
            Clear();
            Populate();
            MessageBox.Show("Submitted Successfuly");
        }
        void Populate()
        {
            dataGridView1.AutoGenerateColumns = false;          
            using (EfDBEntities db = new EfDBEntities())
            {
                dataGridView1.DataSource = db.Customers.ToList<Customer>();
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                model.CustomerId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["CustomerId"].Value);
                using (EfDBEntities db=new EfDBEntities())
                { 
                model=db.Customers.Where(x=>x.CustomerId==model.CustomerId ).FirstOrDefault();
                    txtFirstName.Text = model.FirstName;
                    txtLastName.Text = model.LastName;
                    txtCity.Text = model.City;
                    txtAddress.Text = model.Address;
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are u sure u want to delete this Record?","Form1",MessageBoxButtons.YesNo)==DialogResult.Yes)
                    using(EfDBEntities db=new EfDBEntities())
            {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.Customers.Attach(model);
                    db.Customers.Remove(model);
                    db.SaveChanges();
                    Populate();
                    Clear();
                    MessageBox.Show("Deleted succsssfuly");
            }
        }

     
    }
}
