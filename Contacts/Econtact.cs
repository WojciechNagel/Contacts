using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Contacts.EcontactClasses;

namespace Contacts
{
    public partial class Econtact : Form
    {
        public Econtact()
        {
            InitializeComponent();
        }
        contactClass c = new contactClass();


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (textBoxFirstName.Text != String.Empty && textBoxLastName.Text != String.Empty && textBoxContactNumber.Text != String.Empty 
                && textBoxAddress.Text != String.Empty && cmbGender.Text != String.Empty)
            { 
                //Get the value from the input fields
                c.FirstName = textBoxFirstName.Text;
                c.LastName = textBoxLastName.Text;
                c.ContactNo = textBoxContactNumber.Text;
                c.Address = textBoxAddress.Text;
                c.Gender = cmbGender.Text;

                //Inserting Data into Database using the method we created in contactClass.cs

                bool success = c.Insert(c);
                if(success==true)
                {
                    //Successfully inserted
                    MessageBox.Show("New contact successfully inserted");
                    //Call the clear method here
                    Clear();
                }
                else
                {
                    //Failed to add contact
                    MessageBox.Show("Failed to add new contact. Try again.");
                }
                //Load Data on DataGridView
                DataTable dt = c.Select();
                dgvContactList.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Complete the fields");
            }
        }

        private void Econtact_Load(object sender, EventArgs e)
        {
            //Load Data on DataGridView
            DataTable dt = c.Select();
            dgvContactList.DataSource = dt;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //MEthod to Clear Fields
        public void Clear()
        {
            textBoxFirstName.Text = "";
            textBoxLastName.Text = "";
            textBoxContactNumber.Text = "";
            textBoxAddress.Text = "";
            cmbGender.Text = "";
            textBoxContactID.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (textBoxFirstName.Text != String.Empty && textBoxLastName.Text != String.Empty && textBoxContactNumber.Text != String.Empty
                && textBoxAddress.Text != String.Empty && cmbGender.Text != String.Empty && textBoxContactID.Text != String.Empty)
            {
                //Get the data from textboxes
                c.ContactID = int.Parse(textBoxContactID.Text);
                c.FirstName = textBoxFirstName.Text;
                c.LastName = textBoxLastName.Text;
                c.ContactNo = textBoxContactNumber.Text;
                c.Address = textBoxAddress.Text;
                c.Gender = cmbGender.Text;
                //update data in datebase
                bool success = c.Update(c);
                if (success == true)
                {
                    //Updated Successfully
                    MessageBox.Show("Contact has been successfully updated.");
                    //Load data on Data Grid View
                    DataTable dt = c.Select();
                    dgvContactList.DataSource = dt;
                    //Call clear method
                    Clear();
                }
                else
                {
                    //Failed to update
                    MessageBox.Show("Failed to update contact. Try again.");
                }
            }
            else
            {
                MessageBox.Show("Complete the fields");
            }

        }

        private void dgvContactList_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Get the data from data grid view and load it to the textboxes respectively
            //identify the row on which moise is clicked
            int rowIndex = e.RowIndex;
            textBoxContactID.Text = dgvContactList.Rows[rowIndex].Cells[0].Value.ToString();
            textBoxFirstName.Text = dgvContactList.Rows[rowIndex].Cells[1].Value.ToString();
            textBoxLastName.Text = dgvContactList.Rows[rowIndex].Cells[2].Value.ToString();
            textBoxContactNumber.Text = dgvContactList.Rows[rowIndex].Cells[3].Value.ToString();
            textBoxAddress.Text = dgvContactList.Rows[rowIndex].Cells[4].Value.ToString();
            cmbGender.Text = dgvContactList.Rows[rowIndex].Cells[5].Value.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Call the clear method
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Get data from the text boxes
            c.ContactID = Convert.ToInt32(textBoxContactID.Text);
            bool success = c.Delete(c);
            if(success == true)
            {
                //Successfully deleted
                MessageBox.Show("Contact successfully deleted.");
                //Load Data on DataGridView
                DataTable dt = c.Select();
                dgvContactList.DataSource = dt;
                //Call the clear method
                Clear();
            }
            else
            {
                //Failed deleted
                MessageBox.Show("Failed to delete contact. TryAgain.");

            }
        }
        static string myconnstr = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            // Get the value from textbox
            string keyword = textBoxSearch.Text;
            SqlConnection conn = new SqlConnection(myconnstr);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tbl_contact WHERE  FirstName LIKE '%"+keyword+"%' OR LastName LIKE '%"+keyword+"%' ",conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dgvContactList.DataSource = dt;
        }

        private void Econtact_Load_1(object sender, EventArgs e)
        {
            this.Left = Properties.Settings.Default.X;
            this.Top = Properties.Settings.Default.Y;
            this.Width = Properties.Settings.Default.W;
            this.Height = Properties.Settings.Default.H;
            this.Text = Properties.Settings.Default.Title;

            //Load Data on DataGridView
            DataTable dt = c.Select();
            dgvContactList.DataSource = dt;
        }

        private void Econtact_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.X = this.Left;
            Properties.Settings.Default.Y = this.Top;
            Properties.Settings.Default.W = this.Width;
            Properties.Settings.Default.H = this.Height;
            Properties.Settings.Default.Save();
        }
    }

}
