using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RegistroEmpleados
{
    public partial class Form1 : Form
    {
        string rutaArchivo = "";

        public Form1()
        {
            InitializeComponent();

            btnGuardar.Click += btnGuardar_Click;
            btnAbrir.Click += btnAbrir_Click;
            btnSalir.Click += btnSalir_Click;

            txtId.TextChanged += Campos_TextChanged;
            txtNombre.TextChanged += Campos_TextChanged;
            txtApellido.TextChanged += Campos_TextChanged;
            txtDireccion.TextChanged += Campos_TextChanged;
            txtTelefono.TextChanged += Campos_TextChanged;
            txtEmail.TextChanged += Campos_TextChanged;
            txtCargo.TextChanged += Campos_TextChanged;
            txtSalario.TextChanged += Campos_TextChanged;

            cmbGenero.SelectedIndexChanged += cmbGenero_SelectedIndexChanged;

            txtId.KeyPress += txtId_KeyPress;
            txtTelefono.KeyPress += txtTelefono_KeyPress;
            txtSalario.KeyPress += txtSalario_KeyPress;

            cmbGenero.Items.Add("Masculino");
            cmbGenero.Items.Add("Femenino");
            cmbGenero.SelectedIndex = -1;

            dtIngreso.MinDate = new DateTime(2000, 1, 1);
            dtIngreso.MaxDate = DateTime.Now;

            btnGuardar.Enabled = false;
        }

        private bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private bool ValidarCampos()
        {
            if (!int.TryParse(txtId.Text, out int id) || id <= 0)
            {
                MessageBox.Show("El ID debe ser un número positivo");
                txtId.Focus();
                return false;
            }

            if (txtNombre.Text.Length < 2)
            {
                MessageBox.Show("Nombre inválido");
                txtNombre.Focus();
                return false;
            }

            if (txtApellido.Text.Length < 2)
            {
                MessageBox.Show("Apellido inválido");
                txtApellido.Focus();
                return false;
            }

            if (!long.TryParse(txtTelefono.Text, out _))
            {
                MessageBox.Show("Teléfono inválido");
                txtTelefono.Focus();
                return false;
            }

            if (!EsEmailValido(txtEmail.Text))
            {
                MessageBox.Show("Email inválido");
                txtEmail.Focus();
                return false;
            }

            if (!decimal.TryParse(txtSalario.Text, out decimal salario) || salario <= 0)
            {
                MessageBox.Show("Salario inválido");
                txtSalario.Focus();
                return false;
            }

            if (cmbGenero.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un género");
                cmbGenero.Focus();
                return false;
            }

            return true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            if (MessageBox.Show("¿Desea guardar los datos?", "Confirmar", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Archivo de texto (*.txt)|*.txt";

            if (save.ShowDialog() == DialogResult.OK)
            {
                rutaArchivo = save.FileName;

                string contenido =
                    $"ID: {txtId.Text}\n" +
                    $"Nombre: {txtNombre.Text}\n" +
                    $"Apellido: {txtApellido.Text}\n" +
                    $"Dirección: {txtDireccion.Text}\n" +
                    $"Teléfono: {txtTelefono.Text}\n" +
                    $"Email: {txtEmail.Text}\n" +
                    $"Cargo: {txtCargo.Text}\n" +
                    $"Salario: {txtSalario.Text}\n" +
                    $"Género: {cmbGenero.Text}\n" +
                    $"Fecha ingreso: {dtIngreso.Value:dd/MM/yyyy}\n";

                File.WriteAllText(rutaArchivo, contenido);

                MessageBox.Show("Archivo guardado correctamente");

                if (MessageBox.Show("¿Desea abrir el archivo?", "Abrir", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start("notepad.exe", rutaArchivo);
                }
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea borrar los datos?", "Confirmar", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            txtId.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtCargo.Clear();
            txtSalario.Clear();

            cmbGenero.SelectedIndex = -1;
            dtIngreso.Value = DateTime.Now;
            btnGuardar.Enabled = false;
            txtId.Focus();
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Archivos de texto (*.txt)|*.txt";

            if (open.ShowDialog() == DialogResult.OK)
            {
                Process.Start("notepad.exe", open.FileName);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea salir?", "Salir", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void Campos_TextChanged(object sender, EventArgs e)
        {
            HabilitarBotonGuardar();
        }

        private void cmbGenero_SelectedIndexChanged(object sender, EventArgs e)
        {
            HabilitarBotonGuardar();
        }

        private void HabilitarBotonGuardar()
        {
            btnGuardar.Enabled =
                !string.IsNullOrWhiteSpace(txtId.Text) &&
                !string.IsNullOrWhiteSpace(txtNombre.Text) &&
                !string.IsNullOrWhiteSpace(txtApellido.Text) &&
                cmbGenero.SelectedIndex != -1;
        }

        private void txtId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtSalario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
                e.Handled = true;
        }
    }
}
