﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicGP
{
    public partial class ResultsForm : Form
    {
        public ResultsForm()
        {
            InitializeComponent();
        }
        private void ResultsForm_Load(object sender, EventArgs e)
        {

        }

        private void picLogo_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            this.Visible = false;
            dashboard.Visible = true;
        }

        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            string[] data;
            DataSet dataSet;
            DataTable table;
            if (rdbNHNumber.Checked)
            {
                // TODO: Send this all to backend
                //If the return key is pressed, sent a login button click event
                if (e.KeyChar == (char)13)
                {
                    data = new string[1];
                    // Define a dataSet from DBAccess with the SQL statement
                    dataSet = DBAccess.getData("findPatient", "id", txtInput.Text);
                    //Define a datatable with the tables from the dataset return
                    table = dataSet.Tables[0];

                    Console.WriteLine(table.Rows.Count);

                    if (table.Rows.Count > 0)
                    {
                        dgvPatients.DataSource = table;
                    }
                    else
                    {
                        MessageBox.Show("No data was found");
                    }
                }
            } else if (radioButton2.Checked) {
                if (e.KeyChar == (char)13)
                {
                    data = new string[1];
                    // Define a dataSet from DBAccess with the SQL statement
                    dataSet = DBAccess.getData("findPatient", "name&dob", txtInput.Text, dtpDOB.Text);
                    //Define a datatable with the tables from the dataset return
                    table = dataSet.Tables[0];

                    Console.WriteLine(table.Rows.Count);

                    if (table.Rows.Count > 0)
                    {
                        dgvPatients.DataSource = table;
                    }
                    else
                    {
                        MessageBox.Show("No data was found");
                    }
                }
            }
        }

        int NHNumber;

        /// <summary>
        /// when a cell is double clicked on a dataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatientSelect(object sender, DataGridViewCellEventArgs e)
        {
            dgvPatients.Visible = false;
            tcResults.Visible = true;

            NHNumber = e.RowIndex;

            string[] data = new string[1];
            //https://stackoverflow.com/questions/5571963/how-to-get-datagridview-cell-value-in-messagebox
            //finds the NHNumber of whichever row was clicked on
            DataSet dataSetAppointments = DBAccess.getData("patientAppointments", dgvPatients.Rows[NHNumber].Cells[0].Value.ToString());
            DataTable tableAppointments = dataSetAppointments.Tables[0];
            CheckForResults(tableAppointments, dgvAppointments);
            
            Console.WriteLine(tableAppointments.Rows.Count);

        }
        //seperating this into a method allows it to work on all three dgvs without writing it over and over again
        private void CheckForResults(DataTable dt, DataGridView dgv)
        {
            if (dt.Rows.Count > 0)
            {
                dgv.DataSource = dt;
            }
            else
            {
                MessageBox.Show("No data was found");
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            lblPrompt.Text = "Full Name: ";
            lblDOB.Visible = true;
            dtpDOB.Visible = true;
        }

        private void rdbNHNumber_CheckedChanged(object sender, EventArgs e)
        {
            lblPrompt.Text = "National Health Number: ";
            lblDOB.Visible = false;
            dtpDOB.Visible = false;
        }

        private void tcResults_Selecting(object sender, TabControlCancelEventArgs e)
        {
            switch(e.TabPageIndex)
            {
                case 0:
                    DataSet dataSetAppointments = DBAccess.getData("patientAppointments", dgvPatients.Rows[NHNumber].Cells[0].Value.ToString());
                    DataTable tableAppointments = dataSetAppointments.Tables[0];
                    CheckForResults(tableAppointments, dgvAppointments);
                    break;
                case 1:
                    DataSet dataSetPrescriptions = DBAccess.getData("patientPresciptions", dgvPatients.Rows[NHNumber].Cells[0].Value.ToString());
                    DataTable tablePrescriptions = dataSetPrescriptions.Tables[0];
                    CheckForResults(tablePrescriptions, dgvPrescriptions);
                    break;
                case 2:
                    DataSet dataSetResults = DBAccess.getData("testResults", dgvPatients.Rows[NHNumber].Cells[0].Value.ToString());
                    DataTable tableResults = dataSetResults.Tables[0];
                    CheckForResults(tableResults, dgvResults);
                    break;
                default:
                    break;
            }
        }

        private void PrescriptionClick(object sender, DataGridViewCellEventArgs e)
        {
            //these will be found from the DB
            int prescriptionID = 3;
            string presciptionName = "Medication Placeholder";
            string prescriptionDuration = "12";
            //https://stackoverflow.com/questions/3036829/how-do-i-create-a-message-box-with-yes-no-choices-and-a-dialogresult
            DialogResult result = MessageBox.Show("Would you like to extend: " + Environment.NewLine + presciptionName + " for another " + prescriptionDuration + " days.","Extend Prescription",MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                ExtendPrescription(prescriptionID);
            }
        }
        private void ExtendPrescription(int prescriptionID)
        {

        }
    }
}
