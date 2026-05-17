using ProyectoFinalSistemaBancario.Fabricas;
using ProyectoFinalSistemaBancario.Repositorio;
using ProyectoFinalSistemaBancario.Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoFinalSistemaBancario
{
    public partial class Form1 : Form
    {
        // ── DEPENDENCIAS ────────────────────────────
        private IServicioBancario _servicio;

        // ── PALETA DE COLORES ───────────────────────
        private readonly Color CLR_FONDO_FORM = ColorTranslator.FromHtml("#1A1A2E");
        private readonly Color CLR_PANEL = ColorTranslator.FromHtml("#16213E");
        private readonly Color CLR_PANEL_CDT = ColorTranslator.FromHtml("#0F2A4A");
        private readonly Color CLR_ACENTO = ColorTranslator.FromHtml("#0F3460");
        private readonly Color CLR_TEXTO = ColorTranslator.FromHtml("#E0E0E0");
        private readonly Color CLR_TEXTO_SEC = ColorTranslator.FromHtml("#A0A0B0");
        private readonly Color CLR_INPUT_BACK = ColorTranslator.FromHtml("#0F172A");
        private readonly Color CLR_INPUT_FORE = ColorTranslator.FromHtml("#E2E8F0");

        private readonly Color CLR_BTN_AGREGAR = ColorTranslator.FromHtml("#0F3460");
        private readonly Color CLR_BTN_DEPOSITAR = ColorTranslator.FromHtml("#16A34A");
        private readonly Color CLR_BTN_RETIRAR = ColorTranslator.FromHtml("#DC2626");
        private readonly Color CLR_BTN_RESUMEN = ColorTranslator.FromHtml("#2563EB");
        private readonly Color CLR_BTN_MICUENTA = ColorTranslator.FromHtml("#7C3AED");
        private readonly Color CLR_BTN_DISABLED = ColorTranslator.FromHtml("#2D2D3A");

        private readonly Color CLR_EXITO_BACK = ColorTranslator.FromHtml("#14532D");
        private readonly Color CLR_EXITO_FORE = ColorTranslator.FromHtml("#86EFAC");
        private readonly Color CLR_ERROR_BACK = ColorTranslator.FromHtml("#7F1D1D");
        private readonly Color CLR_ERROR_FORE = ColorTranslator.FromHtml("#FCA5A5");
        private readonly Color CLR_WARN_BACK = ColorTranslator.FromHtml("#78350F");
        private readonly Color CLR_WARN_FORE = ColorTranslator.FromHtml("#FCD34D");
        private readonly Color CLR_INFO_BACK = ColorTranslator.FromHtml("#1E293B");
        private readonly Color CLR_INFO_FORE = ColorTranslator.FromHtml("#94A3B8");

        // ── CONTROLES CREAR CUENTA ──────────────────
        private ComboBox cmbTipoCuenta;
        private TextBox txtNumeroCuenta;
        private TextBox txtNombreTitular;
        private TextBox txtSaldoInicial;
        private TextBox txtPlazoMeses;
        private TextBox txtMesesTranscurridos;
        private Button btnAgregarCuenta;
        private Panel pnlCDT;

        // ── CONTROLES OPERACIONES ───────────────────
        private TextBox txtNumeroCuentaOp;
        private TextBox txtMonto;
        private Button btnDepositar;
        private Button btnRetirar;
        private Button btnVerResumen;
        private Button btnVerResumenCuenta;

        // ── RESULTADO ───────────────────────────────
        private TextBox txtResultado;

        // ── CONSTRUCTOR — NO MODIFICAR ──────────────
        public Form1()
        {
            InitializeComponent();
            ICuentaRepository repositorio = new InMemoryRepositoryCuenta();
            _servicio = new ServicioBancario(repositorio);
            InicializarUI();
            WireEvents();
            UpdateUIState();
        }

        // ════════════════════════════════════════════
        // INICIALIZAR UI — REESCRITO CON DISEÑO PRO
        // ════════════════════════════════════════════

        /// <summary>
        /// Permite solo números en un TextBox.
        /// Conectar con el evento KeyPress del control.
        /// </summary>
        private void SoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permite números, backspace y enter
            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // bloquea el carácter
            }
        }

        /// <summary>
        /// Permite solo letras y espacios en un TextBox.
        /// </summary>
        private void SoloLetras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) &&
                e.KeyChar != ' ' &&
                e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
        private void InicializarUI()
        {
            // ── FORM BASE ───────────────────────────
            this.Text = "Sistema Bancario — N-Layer + Strategy + Factory";
            this.Size = new Size(560, 820);
            this.BackColor = CLR_FONDO_FORM;
            this.ForeColor = CLR_TEXTO;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            int margen = 20;
            int y = margen;

            // ════════════════════════════════════════
            // NIVEL 1 — TÍTULO DISPLAY
            // ════════════════════════════════════════
            var lblBanco = new Label();
            lblBanco.Text = "🏦  SISTEMA BANCARIO";
            lblBanco.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblBanco.ForeColor = CLR_TEXTO;
            lblBanco.Location = new Point(margen, y);
            lblBanco.Size = new Size(500, 35);
            this.Controls.Add(lblBanco);
            y += 35;

            var lblSub = new Label();
            lblSub.Text = "N-Layer  ·  Strategy Pattern  ·  Factory Method";
            lblSub.Font = new Font("Segoe UI", 8, FontStyle.Regular);
            lblSub.ForeColor = CLR_TEXTO_SEC;
            lblSub.Location = new Point(margen, y);
            lblSub.Size = new Size(500, 18);
            this.Controls.Add(lblSub);
            y += 30;

            // ════════════════════════════════════════
            // PANEL — REGISTRAR CUENTA
            // ════════════════════════════════════════
            var pnlCrear = new Panel();
            pnlCrear.BackColor = CLR_PANEL;
            pnlCrear.Location = new Point(margen, y);
            pnlCrear.Size = new Size(500, 310);
            pnlCrear.Padding = new Padding(15);
            this.Controls.Add(pnlCrear);

            // NIVEL 2 — Título de sección
            var lblSecCrear = new Label();
            lblSecCrear.Text = "▌  REGISTRAR CUENTA";
            lblSecCrear.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblSecCrear.ForeColor = ColorTranslator.FromHtml("#60A5FA");
            lblSecCrear.Location = new Point(15, 12);
            lblSecCrear.Size = new Size(460, 22);
            pnlCrear.Controls.Add(lblSecCrear);

            int py = 44; // y interno del panel

            // ── TIPO DE CUENTA ──────────────────────
            AgregarLabel(pnlCrear, "Tipo de cuenta", 15, py);
            cmbTipoCuenta = new ComboBox();
            cmbTipoCuenta.Location = new Point(165, py - 1);
            cmbTipoCuenta.Size = new Size(180, 26);
            EstilizarInput(cmbTipoCuenta);
            cmbTipoCuenta.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var tipo in RegistroDeFabricas.ObtenerTiposDisponibles())
                cmbTipoCuenta.Items.Add(tipo);
            cmbTipoCuenta.SelectedIndex = 0;
            pnlCrear.Controls.Add(cmbTipoCuenta);
            py += 38;

            // ── NÚMERO CUENTA ───────────────────────
            AgregarLabel(pnlCrear, "Número de cuenta", 15, py);
            txtNumeroCuenta = new TextBox();
            txtNumeroCuenta.Location = new Point(165, py - 1);
            txtNumeroCuenta.Size = new Size(180, 26);
            EstilizarInput(txtNumeroCuenta);
            pnlCrear.Controls.Add(txtNumeroCuenta);
            py += 38;

            // ── TITULAR ─────────────────────────────
            AgregarLabel(pnlCrear, "Nombre titular", 15, py);
            txtNombreTitular = new TextBox();
            txtNombreTitular.Location = new Point(165, py - 1);
            txtNombreTitular.Size = new Size(180, 26);
            EstilizarInput(txtNombreTitular);
            pnlCrear.Controls.Add(txtNombreTitular);
            py += 38;

            // ── SALDO INICIAL ───────────────────────
            AgregarLabel(pnlCrear, "Saldo inicial ($)", 15, py);
            txtSaldoInicial = new TextBox();
            txtSaldoInicial.Location = new Point(165, py - 1);
            txtSaldoInicial.Size = new Size(180, 26);
            EstilizarInput(txtSaldoInicial);
            pnlCrear.Controls.Add(txtSaldoInicial);
            py += 38;

            // ── PANEL CDT (oculto por defecto) ──────
            pnlCDT = new Panel();
            pnlCDT.BackColor = ColorTranslator.FromHtml("#0F2A4A");
            pnlCDT.Location = new Point(10, py);
            pnlCDT.Size = new Size(468, 65); // ← más compacto
            pnlCDT.Visible = false;
            pnlCrear.Controls.Add(pnlCDT);

            var lblCDTTitulo = new Label();
            lblCDTTitulo.Text = "⏱  Parámetros CDT";
            lblCDTTitulo.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            lblCDTTitulo.ForeColor = ColorTranslator.FromHtml("#FCD34D");
            lblCDTTitulo.Location = new Point(10, 6);
            lblCDTTitulo.Size = new Size(200, 18);
            pnlCDT.Controls.Add(lblCDTTitulo);

            // ── PLAZO ───────────────────────────────
            var lblPlazoIn = new Label();
            lblPlazoIn.Text = "Plazo (meses)";
            lblPlazoIn.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblPlazoIn.ForeColor = ColorTranslator.FromHtml("#A0A0B0");
            lblPlazoIn.Location = new Point(10, 35);
            lblPlazoIn.Size = new Size(100, 20);
            pnlCDT.Controls.Add(lblPlazoIn);

            txtPlazoMeses = new TextBox();
            txtPlazoMeses.Location = new Point(115, 33);
            txtPlazoMeses.Size = new Size(70, 24);
            txtPlazoMeses.BackColor = ColorTranslator.FromHtml("#1E3A5F");
            txtPlazoMeses.ForeColor = Color.White;
            txtPlazoMeses.Font = new Font("Segoe UI", 9.5f);
            txtPlazoMeses.BorderStyle = BorderStyle.FixedSingle;
            pnlCDT.Controls.Add(txtPlazoMeses);

            // ── MESES TRANSCURRIDOS ─────────────────
            var lblMesesIn = new Label();
            lblMesesIn.Text = "Transcurridos";
            lblMesesIn.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lblMesesIn.ForeColor = ColorTranslator.FromHtml("#A0A0B0");
            lblMesesIn.Location = new Point(210, 35);
            lblMesesIn.Size = new Size(100, 20);
            pnlCDT.Controls.Add(lblMesesIn);

            txtMesesTranscurridos = new TextBox();
            txtMesesTranscurridos.Location = new Point(320, 33);
            txtMesesTranscurridos.Size = new Size(70, 24);
            txtMesesTranscurridos.BackColor = ColorTranslator.FromHtml("#1E3A5F");
            txtMesesTranscurridos.ForeColor = Color.White;
            txtMesesTranscurridos.Font = new Font("Segoe UI", 9.5f);
            txtMesesTranscurridos.BorderStyle = BorderStyle.FixedSingle;
            pnlCDT.Controls.Add(txtMesesTranscurridos);

            py += 75; // ← espacio después del panel CDT

            // ── BOTÓN REGISTRAR ─────────────────────
            btnAgregarCuenta = new Button();
            btnAgregarCuenta.Text = "＋  Registrar Cuenta";
            btnAgregarCuenta.Location = new Point(15, py);
            btnAgregarCuenta.Size = new Size(200, 36);
            EstilizarBoton(btnAgregarCuenta, CLR_BTN_AGREGAR);
            pnlCrear.Controls.Add(btnAgregarCuenta);

            // Ajustar altura del panel según contenido
            pnlCrear.Height = py + 50;
            y += pnlCrear.Height + 16;

            // ════════════════════════════════════════
            // PANEL — OPERACIONES
            // ════════════════════════════════════════
            var pnlOp = new Panel();
            pnlOp.BackColor = CLR_PANEL;
            pnlOp.Location = new Point(margen, y);
            pnlOp.Size = new Size(500, 195);
            this.Controls.Add(pnlOp);

            var lblSecOp = new Label();
            lblSecOp.Text = "▌  OPERACIONES";
            lblSecOp.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblSecOp.ForeColor = ColorTranslator.FromHtml("#60A5FA");
            lblSecOp.Location = new Point(15, 12);
            lblSecOp.Size = new Size(460, 22);
            pnlOp.Controls.Add(lblSecOp);

            int oy = 44;

            // ── NÚMERO CUENTA OP ────────────────────
            AgregarLabel(pnlOp, "Número de cuenta", 15, oy);
            txtNumeroCuentaOp = new TextBox();
            txtNumeroCuentaOp.Location = new Point(165, oy - 1);
            txtNumeroCuentaOp.Size = new Size(180, 26);
            EstilizarInput(txtNumeroCuentaOp);
            pnlOp.Controls.Add(txtNumeroCuentaOp);
            oy += 38;

            // ── MONTO ───────────────────────────────
            AgregarLabel(pnlOp, "Monto ($)", 15, oy);
            txtMonto = new TextBox();
            txtMonto.Location = new Point(165, oy - 1);
            txtMonto.Size = new Size(180, 26);
            EstilizarInput(txtMonto);
            pnlOp.Controls.Add(txtMonto);
            oy += 46;

            // ── BOTONES OPERACIONES ─────────────────
            btnDepositar = new Button();
            btnDepositar.Text = "↑  Depositar";
            btnDepositar.Location = new Point(15, oy);
            btnDepositar.Size = new Size(108, 36);
            EstilizarBoton(btnDepositar, CLR_BTN_DEPOSITAR);
            pnlOp.Controls.Add(btnDepositar);

            btnRetirar = new Button();
            btnRetirar.Text = "↓  Retirar";
            btnRetirar.Location = new Point(132, oy);
            btnRetirar.Size = new Size(108, 36);
            EstilizarBoton(btnRetirar, CLR_BTN_RETIRAR);
            pnlOp.Controls.Add(btnRetirar);

            btnVerResumen = new Button();
            btnVerResumen.Text = "≡  Ver Todas";
            btnVerResumen.Location = new Point(249, oy);
            btnVerResumen.Size = new Size(108, 36);
            EstilizarBoton(btnVerResumen, CLR_BTN_RESUMEN);
            pnlOp.Controls.Add(btnVerResumen);

            btnVerResumenCuenta = new Button();
            btnVerResumenCuenta.Text = "◎  Mi Cuenta";
            btnVerResumenCuenta.Location = new Point(366, oy);
            btnVerResumenCuenta.Size = new Size(108, 36);
            EstilizarBoton(btnVerResumenCuenta, CLR_BTN_MICUENTA);
            pnlOp.Controls.Add(btnVerResumenCuenta);

            y += pnlOp.Height + 16;

            // ════════════════════════════════════════
            // ÁREA DE RESULTADO INTELIGENTE
            // ════════════════════════════════════════
            var lblResultadoTitulo = new Label();
            lblResultadoTitulo.Text = "▌  RESULTADO";
            lblResultadoTitulo.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblResultadoTitulo.ForeColor = CLR_TEXTO_SEC;
            lblResultadoTitulo.Location = new Point(margen, y);
            lblResultadoTitulo.Size = new Size(200, 20);
            this.Controls.Add(lblResultadoTitulo);
            y += 24;

            txtResultado = new TextBox();
            txtResultado.Location = new Point(margen, y);
            txtResultado.Size = new Size(500, 120);
            txtResultado.Multiline = true;
            txtResultado.ReadOnly = true;
            txtResultado.ScrollBars = ScrollBars.Vertical;
            txtResultado.Font = new Font("Consolas", 9.5f);
            txtResultado.BackColor = CLR_INFO_BACK;
            txtResultado.ForeColor = CLR_INFO_FORE;
            txtResultado.BorderStyle = BorderStyle.FixedSingle;
            txtResultado.Text = "Sistema listo. Registre una cuenta para comenzar.";
            this.Controls.Add(txtResultado);

            // Ajustar tamaño del Form al contenido
            this.Height = y + 120 + 40;
        }

        // ════════════════════════════════════════════
        // WIRE EVENTS — NO MODIFICAR
        // ════════════════════════════════════════════
        private void WireEvents()
        {
            btnAgregarCuenta.Click += BtnAgregarCuenta_Click;
            btnDepositar.Click += BtnDepositar_Click;
            btnRetirar.Click += BtnRetirar_Click;
            btnVerResumen.Click += BtnVerResumen_Click;
            btnVerResumenCuenta.Click += BtnVerResumenCuenta_Click;

            cmbTipoCuenta.SelectedIndexChanged += CmbTipoCuenta_Changed;

            txtNumeroCuenta.TextChanged += (s, e) => UpdateUIState();
            txtNombreTitular.TextChanged += (s, e) => UpdateUIState();
            txtSaldoInicial.TextChanged += (s, e) => UpdateUIState();
            txtNumeroCuentaOp.TextChanged += (s, e) => UpdateUIState();
            txtMonto.TextChanged += (s, e) => UpdateUIState();

            btnAgregarCuenta.Click += BtnAgregarCuenta_Click;
            btnDepositar.Click += BtnDepositar_Click;
            btnRetirar.Click += BtnRetirar_Click;
            btnVerResumen.Click += BtnVerResumen_Click;
            btnVerResumenCuenta.Click += BtnVerResumenCuenta_Click;
            cmbTipoCuenta.SelectedIndexChanged += CmbTipoCuenta_Changed;
            txtNumeroCuenta.TextChanged += (s, e) => UpdateUIState();
            txtNombreTitular.TextChanged += (s, e) => UpdateUIState();
            txtSaldoInicial.TextChanged += (s, e) => UpdateUIState();
            txtNumeroCuentaOp.TextChanged += (s, e) => UpdateUIState();
            txtMonto.TextChanged += (s, e) => UpdateUIState();

            // ── VALIDACIONES DE ENTRADA ─────────────────
            // Solo números en campos numéricos
            txtNumeroCuenta.KeyPress += SoloNumeros_KeyPress;
            txtSaldoInicial.KeyPress += SoloNumeros_KeyPress;
            txtMonto.KeyPress += SoloNumeros_KeyPress;
            txtPlazoMeses.KeyPress += SoloNumeros_KeyPress;
            txtMesesTranscurridos.KeyPress += SoloNumeros_KeyPress;
            txtNumeroCuentaOp.KeyPress += SoloNumeros_KeyPress;

            // Solo letras en nombre del titular
            txtNombreTitular.KeyPress += SoloLetras_KeyPress;
        }

        // ════════════════════════════════════════════
        // UPDATE UI STATE — MEJORADO CON COLOR SEMÁNTICO
        // ════════════════════════════════════════════
        private void UpdateUIState()
        {
            bool camposCrear =
                !string.IsNullOrWhiteSpace(txtNumeroCuenta.Text) &&
                !string.IsNullOrWhiteSpace(txtNombreTitular.Text) &&
                !string.IsNullOrWhiteSpace(txtSaldoInicial.Text);

            AplicarEstiloBoton(btnAgregarCuenta, camposCrear, CLR_BTN_AGREGAR);

            bool camposOp =
                !string.IsNullOrWhiteSpace(txtNumeroCuentaOp.Text) &&
                !string.IsNullOrWhiteSpace(txtMonto.Text);

            AplicarEstiloBoton(btnDepositar, camposOp, CLR_BTN_DEPOSITAR);
            AplicarEstiloBoton(btnRetirar, camposOp, CLR_BTN_RETIRAR);

            bool tieneCuenta =
                !string.IsNullOrWhiteSpace(txtNumeroCuentaOp.Text);

            AplicarEstiloBoton(btnVerResumen, true, CLR_BTN_RESUMEN);
            AplicarEstiloBoton(btnVerResumenCuenta, tieneCuenta, CLR_BTN_MICUENTA);
        }

        // ════════════════════════════════════════════
        // MANEJADORES — NO MODIFICAR LÓGICA INTERNA
        // ════════════════════════════════════════════
        private void CmbTipoCuenta_Changed(object sender, EventArgs e)
        {
            bool esCDT = cmbTipoCuenta.SelectedItem?.ToString() == "CDT";
            pnlCDT.Visible = esCDT;
        }

        private void BtnAgregarCuenta_Click(object sender, EventArgs e)
        {
            try
            {
                string tipo = cmbTipoCuenta.SelectedItem.ToString();
                string numero = txtNumeroCuenta.Text.Trim();
                string nombre = txtNombreTitular.Text.Trim();
                double saldo = double.Parse(txtSaldoInicial.Text);

                var parametros = new Dictionary<string, object>();
                if (tipo == "CDT")
                {
                    parametros["plazoMeses"] =
                        int.Parse(txtPlazoMeses.Text);
                    parametros["mesesTranscurridos"] =
                        int.Parse(txtMesesTranscurridos.Text);
                }

                _servicio.AgregarCuenta(tipo, numero, nombre, saldo, parametros);
                string msg = $"Cuenta {tipo} registrada exitosamente.\n" +
                             $"Titular: {nombre}  |  Número: {numero}";
                MostrarResultado(msg, DetectarTipo(msg));
                LimpiarCamposCreacion();
            }
            catch (FormatException)
            {
                MostrarResultado(
                    "Error: verifique que los campos numéricos sean válidos.",
                    "error");
            }
            catch (Exception ex)
            {
                MostrarResultado($"Error: {ex.Message}", "error");
            }
        }

        private void BtnDepositar_Click(object sender, EventArgs e)
        {
            try
            {
                string numero = txtNumeroCuentaOp.Text.Trim();
                double monto = double.Parse(txtMonto.Text);
                string resultado = _servicio.Depositar(numero, monto);
                MostrarResultado(resultado, DetectarTipo(resultado));
            }
            catch (FormatException)
            {
                MostrarResultado(
                    "Error: el monto debe ser un número válido.", "error");
            }
            catch (Exception ex)
            {
                MostrarResultado($"Error: {ex.Message}", "error");
            }
        }

        private void BtnRetirar_Click(object sender, EventArgs e)
        {
            try
            {
                string numero = txtNumeroCuentaOp.Text.Trim();
                double monto = double.Parse(txtMonto.Text);
                string resultado = _servicio.Retirar(numero, monto);
                MostrarResultado(resultado, DetectarTipo(resultado));
            }
            catch (FormatException)
            {
                MostrarResultado(
                    "Error: el monto debe ser un número válido.", "error");
            }
            catch (Exception ex)
            {
                MostrarResultado($"Error: {ex.Message}", "error");
            }
        }

        private void BtnVerResumen_Click(object sender, EventArgs e)
        {
            string resultado = _servicio.GetResumenTodas();
            MostrarResultado(resultado, DetectarTipo(resultado));
        }

        private void BtnVerResumenCuenta_Click(object sender, EventArgs e)
        {
            try
            {
                string numero = txtNumeroCuentaOp.Text.Trim();
                string resultado = _servicio.GetResumenCuenta(numero);
                MostrarResultado(resultado, DetectarTipo(resultado));
            }
            catch (Exception ex)
            {
                MostrarResultado($"Error: {ex.Message}", "error");
            }
        }

        // ════════════════════════════════════════════
        // HELPERS — RESULTADO INTELIGENTE
        // ════════════════════════════════════════════

        /// <summary>
        /// Muestra el resultado con color semántico según el tipo.
        /// exito → verde | error → rojo | advertencia → ámbar | info → gris
        /// </summary>
        private void MostrarResultado(string mensaje, string tipo)
        {
            txtResultado.Text = mensaje;
            switch (tipo)
            {
                case "exito":
                    txtResultado.BackColor = CLR_EXITO_BACK;
                    txtResultado.ForeColor = CLR_EXITO_FORE;
                    break;
                case "error":
                    txtResultado.BackColor = CLR_ERROR_BACK;
                    txtResultado.ForeColor = CLR_ERROR_FORE;
                    break;
                case "advertencia":
                    txtResultado.BackColor = CLR_WARN_BACK;
                    txtResultado.ForeColor = CLR_WARN_FORE;
                    break;
                default:
                    txtResultado.BackColor = CLR_INFO_BACK;
                    txtResultado.ForeColor = CLR_INFO_FORE;
                    break;
            }
        }

        /// <summary>
        /// Analiza el texto del resultado y determina su tipo semántico.
        /// </summary>
        private string DetectarTipo(string resultado)
        {
            if (resultado == null) return "info";

            string lower = resultado.ToLower();

            if (lower.Contains("exitoso") ||
                lower.Contains("exitosa") ||
                lower.Contains("creada") ||
                lower.Contains("registrada"))
                return "exito";

            if (lower.Contains("insuficiente") ||
                lower.Contains("no encontrada") ||
                lower.Contains("error") ||
                lower.Contains("inválido"))
                return "error";

            if (lower.Contains("bloqueado") ||
                lower.Contains("plazo") ||
                lower.Contains("cdt") ||
                lower.Contains("faltan"))
                return "advertencia";

            return "info";
        }

        /// <summary>
        /// Aplica color semántico a un botón según su estado habilitado.
        /// Habilitado → color activo | Deshabilitado → gris oscuro
        /// </summary>
        private void AplicarEstiloBoton(Button btn, bool habilitado, Color colorActivo)
        {
            btn.Enabled = habilitado;
            btn.BackColor = habilitado ? colorActivo : CLR_BTN_DISABLED;
            btn.ForeColor = habilitado
                ? Color.White
                : ColorTranslator.FromHtml("#555566");
        }

        // ════════════════════════════════════════════
        // HELPERS — CONSTRUCCIÓN DE CONTROLES
        // ════════════════════════════════════════════

        /// <summary>
        /// Crea y agrega un Label estilizado al panel dado.
        /// </summary>
        private void AgregarLabel(Control parent, string texto, int x, int y)
        {
            var lbl = new Label();
            lbl.Text = texto;
            lbl.Font = new Font("Segoe UI", 9, FontStyle.Regular);
            lbl.ForeColor = CLR_TEXTO_SEC;
            lbl.Location = new Point(x, y + 3);
            lbl.Size = new Size(145, 20);
            parent.Controls.Add(lbl);
        }

        /// <summary>
        /// Aplica el estilo oscuro profesional a un TextBox o ComboBox.
        /// </summary>
        private void EstilizarInput(Control ctrl)
        {
            ctrl.BackColor = CLR_INPUT_BACK;
            ctrl.ForeColor = CLR_INPUT_FORE;
            ctrl.Font = new Font("Segoe UI", 9.5f);

            if (ctrl is TextBox txt)
                txt.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// Aplica el estilo base a un botón con color semántico.
        /// </summary>
        private void EstilizarBoton(Button btn, Color colorFondo)
        {
            btn.BackColor = colorFondo;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor =
                ControlPaint.Light(colorFondo, 0.2f);
        }

        // ════════════════════════════════════════════
        // LIMPIAR CAMPOS — NO MODIFICAR
        // ════════════════════════════════════════════
        private void LimpiarCamposCreacion()
        {
            txtNumeroCuenta.Text = "";
            txtNombreTitular.Text = "";
            txtSaldoInicial.Text = "";
            txtPlazoMeses.Text = "";
            txtMesesTranscurridos.Text = "";
        }
    }
}