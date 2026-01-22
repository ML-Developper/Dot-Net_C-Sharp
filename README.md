

# Medical Clinic Management — Windows Forms (C# + MySQL)

## Description
This Windows Forms application (C#) allows the management of patients in a medical clinic.  
It uses a MySQL database to store patient information and generates text files for consultation history.

The project is compatible with **XAMPP** and includes a `patient.sql` file for database initialization.

---

## Features

### Patient Management
- Add a new patient
- Edit patient information
- Delete patient (with confirmation)
- Display all patients in a list
- Export the patient list to a `.txt` file

---

## Database Structure (Table: `Patient`)

| Field | Type | Details |
|-------|------|---------|
| ID_Client | INT | 8 digits max |
| Nom | VARCHAR(15) | Last name |
| Prenom | VARCHAR(15) | First name |
| Genre | ENUM('Homme', 'Femme') |
| Date_Naissance | DATE |
| Adresse | VARCHAR(255) |
| Tel | INT | 8 digits max |

You can import the structure using `patient.sql`.

---

## Consultation History
For each consultation, a text file is created or updated with the patient's ID.  
Example:
