using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProjectNet
{
    public partial class Form1 : Form
    {
        private PatientManager manager = new PatientManager();
        private int? selectedPatientId = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadPatients();
            timer1.Start();
        }

        private void LoadPatients()
        {
            try
            {
                var patients = manager.GetAllPatients();
                dgvPatients.DataSource = patients;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion à la base de données: " + ex.Message);
            }
        }

        private void ClearInputs()
        {
            txtNom.Clear();
            txtPrenom.Clear();
            txtTel.Clear();
            txtAdresse.Clear();
            cmbGenre.SelectedIndex = -1;
            dtpDob.Value = DateTime.Now;
            selectedPatientId = null;
            txtConsultation.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            Patient p = new Patient
            {
                Nom = txtNom.Text,
                Prenom = txtPrenom.Text,
                Tel = txtTel.Text,
                Genre = cmbGenre.SelectedItem.ToString(),
                DateNaissance = dtpDob.Value,
                Adresse = txtAdresse.Text
            };

            try
            {
                manager.AddPatient(p);
                LoadPatients();
                ClearInputs();
                MessageBox.Show("Patient ajouté avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedPatientId == null)
            {
                MessageBox.Show("Veuillez sélectionner un patient.");
                return;
            }
            if (!ValidateInputs()) return;

            Patient p = new Patient
            {
                Id = selectedPatientId.Value,
                Nom = txtNom.Text,
                Prenom = txtPrenom.Text,
                Tel = txtTel.Text,
                Genre = cmbGenre.SelectedItem.ToString(),
                DateNaissance = dtpDob.Value,
                Adresse = txtAdresse.Text
            };

            try
            {
                manager.UpdatePatient(p);
                LoadPatients();
                ClearInputs();
                MessageBox.Show("Patient modifié avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedPatientId == null)
            {
                MessageBox.Show("Veuillez sélectionner un patient.");
                return;
            }

            var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce patient ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    manager.DeletePatient(selectedPatientId.Value);
                    LoadPatients();
                    ClearInputs();
                    MessageBox.Show("Patient supprimé.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur: " + ex.Message);
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Fichiers Texte|*.txt";
            sfd.FileName = "Patients.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    manager.ExportPatientsToTextFile(sfd.FileName);
                    MessageBox.Show("Liste exportée avec succès.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur export: " + ex.Message);
                }
            }
        }

        private void btnSaveConsultation_Click(object sender, EventArgs e)
        {
            if (selectedPatientId == null)
            {
                MessageBox.Show("Veuillez sélectionner un patient pour ajouter une consultation.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtConsultation.Text))
            {
                MessageBox.Show("Veuillez saisir les détails de la consultation.");
                return;
            }

            try
            {
                manager.SaveConsultation(selectedPatientId.Value, txtConsultation.Text);
                MessageBox.Show("Consultation enregistrée dans le dossier du patient.");
                txtConsultation.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur hist: " + ex.Message);
            }
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
             if (selectedPatientId == null)
            {
                MessageBox.Show("Veuillez sélectionner un patient.");
                return;
            }
            string history = manager.GetConsultationHistory(selectedPatientId.Value);
            MessageBox.Show(history, "Historique Consultations Patient " + selectedPatientId.Value);
        }

        private void dgvPatients_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPatients.SelectedRows.Count > 0)
            {
                var row = dgvPatients.SelectedRows[0];
                var p = (Patient)row.DataBoundItem;
                if (p != null)
                {
                    selectedPatientId = p.Id;
                    txtNom.Text = p.Nom;
                    txtPrenom.Text = p.Prenom;
                    txtTel.Text = p.Tel;
                    txtAdresse.Text = p.Adresse;
                    dtpDob.Value = p.DateNaissance;
                    if (!string.IsNullOrEmpty(p.Genre) && cmbGenre.Items.Contains(p.Genre))
                        cmbGenre.SelectedItem = p.Genre;
                }
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text) || txtNom.Text.Length > 15)
            {
                MessageBox.Show("Nom invalide (Max 15 carac).");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPrenom.Text) || txtPrenom.Text.Length > 15)
            {
                MessageBox.Show("Prénom invalide (Max 15 carac).");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTel.Text) || txtTel.Text.Length > 8) 
            {
                MessageBox.Show("Téléphone invalide (Max 8 carac).");
                return false;
            }
            if (cmbGenre.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un genre.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtAdresse.Text) || txtAdresse.Text.Length > 255)
            {
                MessageBox.Show("Adresse invalide (Max 255 carac).");
                return false;
            }
            return true;
        }

        private void txtTel_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
             if (MessageBox.Show("Voulez-vous vraiment quitter l'application ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
